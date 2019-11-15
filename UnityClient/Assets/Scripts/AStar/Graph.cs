using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public bool displayGridGizmos;
    //public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    public TerrainType[] walkableRegions;
    private LayerMask walkableMask;
    
    // Alternative to going through the TerrainType[] each time. This dictionary has a int as its key and value.
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
    //2 Dimensional Array of Nodes
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

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

    void CreateGrid()
    {
        grid = new Node[gridSizeX,gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x ++) {
            for (int y = 0; y < gridSizeY; y ++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                // Collision check of each node - this bool is true if we do not collide with anything
                // CheckSphere returns true if there is a collision, else it will return false
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                // Movement penalty is 0 unless otherwise specified
                int movementPenalty = 0;
                
                // RayCast here
                if (walkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    // Shoot a ray from the corner of the world origin, with a max distance of 100 and return the LayerMasks hit
                    if (Physics.Raycast(ray, out hit, 100, walkableMask))
                    {
                        // Assign to the movement penalty
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }
                
                grid[x,y] = new Node(walkable, worldPoint, x, y, movementPenalty);
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
        Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

        
            // Check if the grid is working by checking if it is null
            if (grid != null && displayGridGizmos)
            {
                foreach (Node n in grid)
                {
                    // If walkable set node to white, otherwise set it to red
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
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
