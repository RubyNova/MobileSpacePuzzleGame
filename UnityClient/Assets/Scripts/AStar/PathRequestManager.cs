using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    // Queue being used to store all the requests of the agents
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    
    // Store current Path request
    PathRequest currentPathRequest;

    // Allows access to the static methods
    static PathRequestManager instance;
    Pathfinding pathfinding;
    
	bool isProcessingPath;
    
    
    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }
    
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
        PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        // if we are not processing a path and the path request queue is not empty, then current path request will be
        // taken out of the queue with Dequeue.
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        // Called by path-finding script once it has finished finding a path 
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
