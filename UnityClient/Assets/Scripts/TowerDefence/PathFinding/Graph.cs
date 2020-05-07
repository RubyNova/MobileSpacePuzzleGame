using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public bool displayGridGizmos;
    
    //public Transform player;
    private LayerMask walkableMask;
    public LayerMask unwalkableMask; 
    public Vector2 gridWorldSize; // Define area in world coordinates the grid will cover
    public float nodeRadius; // Define how much space each individual node covers

    public TerrainType[] walkableRegions;
    public  int ObstacleProximityPenalty = 10;

    // Alternative to going through the TerrainType[] each time. This dictionary has a int as its key and value.
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
    //2 Dimensional Array of Nodes
    Node[,] grid; 

    // How many nodes can fit in a grid
    float nodeDiameter; 
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    void Awake()
    {
        nodeDiameter = nodeRadius*2;
        
        // Returns how many nodes can fit into the world size(X)
        // Round the nodes up to whole numbers by converting into integers
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);

        foreach (TerrainType region in walkableRegions)
        {
            // Iterate through terrain types in walkableRegions and add each of them to walkableMask 
            // Here the bitwise OR operator is used to work out the accumulative value of the layer masks.
            walkableMask.value |= region.terrainMask.value; 
            // Add the value of layerMasks to a dictionary to avoid having to keep going through the loop
            // Pass in the value of the layer located by turning the binary value back into the layer number : 9 ECT
            // Use log to do this. (int) casts the returned float to an integer.
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }
    
    public bool WalkableCheck(Vector3 worldPoint)
    {
        bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

        return walkable;
    }

    void CreateGrid()
    {
        // Create a new 2D array of nodes with values contained in gridX and gridY
        grid = new Node[gridSizeX,gridSizeY];
        // Get the bottom left position of the world, using transform.position as the center of the selection.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2; // Get left edge and bottom edge of the world

        for (int x = 0; x < gridSizeX; x ++) {
            for (int y = 0; y < gridSizeY; y ++) {
                // This gets each point a node is going to occupy in the world
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                // Collision check of each node - this bool is true if we do not collide with anything
                // CheckSphere returns true if there is a collision, else it will return false
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                // Movement penalty is 0 unless otherwise specified
                int movementPenalty = 0;
                
                    // RayCast here
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    // Shoot a ray from the corner of the world origin, with a max distance of 100 and return the LayerMasks hit
                    if (Physics.Raycast(ray, out hit, 100, walkableMask))
                    {
                        // Assign to the movement penalty
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }

                    if (!walkable)
                    {
                        movementPenalty += ObstacleProximityPenalty;
                    }
                    // Create new node and populate grid with node
                    grid[x,y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        BlurPenaltyMap(3);
    }
    
    // Weighted path smoothing - using the blur algorithm called the box blur.
    void BlurPenaltyMap(int blurSize)
    {
        // A kernelSize of 1 will give us a Kernel size of 3
        int kernelSize = blurSize * 2 + 1;
        // How many squares are there between the central square and the edge of the kernel
        // for a 3 x 3 kernel that would be 1 square 
        int kernelExtents = (kernelSize - 1) / 2;
        
        // Two arrays to store vertical and horizontal values of the graphs in
        int[,] penaltiesHorizontalPass = new int[gridSizeX,gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX,gridSizeY];

        // Horizontal Pass
        for (int y = 0; y < gridSizeY; y++) 
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                // When X is negative, we clamp it to 0, so it gets the value from the first node instead of going out of bounds
                int sampleX = Mathf.Clamp (x, 0, kernelExtents);
                // Add the nodes value to the horizontal pass, incremented by the movement of our grid 
                penaltiesHorizontalPass [0, y] += grid [sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                // Calculate the index of the node no longer inside the kernel when it moves along
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                // This is for the node that has just entered the kernel
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX-1);

                penaltiesHorizontalPass [x, y] = penaltiesHorizontalPass [x - 1, y] - 
                                                 grid[removeIndex, y].movementPenalty + 
                                                 grid[addIndex, y].movementPenalty;
            }
        }
        // Vertical Pass
        for (int x = 0; x < gridSizeX; x++) 
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                // When X is negative, we clamp it to 0, so it gets the value from the first node instead of going out of bounds
                int sampleY = Mathf.Clamp (y, 0, kernelExtents);
                // Add the nodes value to the horizontal pass, incremented by the movement of our grid 
                penaltiesVerticalPass [x, 0] += penaltiesHorizontalPass [x, sampleY];
            }
            
            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, 0] / (kernelSize * kernelSize));
            grid [x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                // Calculate the index of the node no longer inside the kernel when it moves along
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                // This is for the node that has just entered the kernel
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY-1);

                penaltiesVerticalPass [x, y] = penaltiesVerticalPass [x, y-1] - 
                                               penaltiesHorizontalPass [x,removeIndex] + 
                                               penaltiesHorizontalPass [x, addIndex];

                // Get final blurred penalty for each node
                // Average the penaltiesVerticalPass, instead of rounding down in integer, we round to nearest integer
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, y] / (kernelSize * kernelSize));
                grid [x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax) 
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin) 
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        // Where is the node in the array of nodes
        List<Node> neighbours = new List<Node>();

        // Search for the node in a 3x3 block
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Is the node that was given
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        
        
        // If the character is outside the grid, this stops invalid coordinates
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Convert float to int 
        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
        return grid[x,y];
    }
    
    private void OnDrawGizmos()
    {
        // the y axis represents the z axis in 3D-space
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        
            // Check if the grid is working by checking if it is null
            if (grid != null && displayGridGizmos)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.red,
                        Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                    
                    // If walkable set node to Gizmos current colour, otherwise set it to red
                    Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
                    Gizmos.DrawCube(n.worldPosition, new Vector3(0.3f,0.3f,0.3f ) * (nodeDiameter -.1f));
                }
            }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
