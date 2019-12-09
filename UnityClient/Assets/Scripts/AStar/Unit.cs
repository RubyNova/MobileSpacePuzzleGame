using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    private Transform target;
    [FormerlySerializedAs("speed")] [SerializeField] private float _speed = 5;
    Vector3[] path;
    private int targetIndex;

    private GameObject targetP;

    private WaveSpawner _ws;
    void Start()
    {
        targetP = _ws.CurrentTarget;
        target = targetP.transform;
        PathRequestManager.RequestPath(transform.position,target.position, OnPathFound);
    }

    public void Init(WaveSpawner ws)
    {
        _ws = ws;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    // yield is how you exit out a coroutine;
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, _speed * Time.deltaTime);
            yield return null;
        }
    }
    
    public void OnDrawGizmos() {
        if (path != null) {
            for (int i = targetIndex; i < path.Length; i ++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.3f, 0.3f,0.3f));

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else {
                    Gizmos.DrawLine(path[i-1],path[i]);
                }
            }
        }
    }
}
