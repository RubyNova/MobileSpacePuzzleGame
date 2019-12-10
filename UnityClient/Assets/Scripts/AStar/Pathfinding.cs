using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Pathfinding : MonoBehaviour
{
    private PathRequestManager requestManager;
    Graph graph;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        graph = GetComponent<Graph>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        // This class will start the FindPath Coroutine
        StartCoroutine(FindPath(startPos, targetPos));
    }

    private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Debug.Log($"START: {startPos}, END: {targetPos}");
        // Stopwatch to see the performance gain through heap optimization
        Stopwatch sw = new Stopwatch();
        // Start stopwatch
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        
        Node startNode = graph.NodeFromWorldPoint(startPos);
        Node targetNode = graph.NodeFromWorldPoint(targetPos);
        
        // If the start or end nodes are not walkable do not bother finding a path
        if (startNode.walkable && targetNode.walkable)
        {
            // Implementing the Heap Optimization into the former lists.
            Heap<Node> openSet = new Heap<Node>(graph.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                // This is equal to the first node in the open set
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    // Stop stopwatch
                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in graph.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour)
                        + neighbour.movementPenalty;
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        // wait one frame before returning
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
        
    }
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        
        //Trace path backwards
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplyfyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplyfyPath(List<Node> path)
    {
        // Store path of the nodes used to get to location
        List<Vector3> waypoints = new List<Vector3>();
        // Store direction 
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            // If this is true then we know the path has changed direction
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i-1].worldPosition);
            }

            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    int GetDistance(Node NodeA, Node NodeB)
    {
        int dstX = Mathf.Abs(NodeA.gridX - NodeB.gridX);
        int dstY = Mathf.Abs(NodeA.gridY - NodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
    
    
}
