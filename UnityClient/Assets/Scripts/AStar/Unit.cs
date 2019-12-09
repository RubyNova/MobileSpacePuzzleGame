using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Unit : MonoBehaviour
{
    private Transform _target;
    private float _speed;
    private Vector3[] _path;
    private int _targetIndex;
    private GameObject _targetP;
    private Coroutine _followPathRoutine;

    private WaveSpawner _ws;
    private void Start()
    {
        //Speed
        GameObject enemyInstance = GameObject.FindGameObjectWithTag("Enemy");
        Enemy enemy = enemyInstance.GetComponent<Enemy>();
        _speed = enemy.speed;
        
        
        _targetP = _ws.CurrentTarget;
        _target = _targetP.transform;
        PathRequestManager.RequestPath(transform.position,_target.position, OnPathFound);
    }

    public void Init(WaveSpawner ws)
    {
        _ws = ws;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = newPath;
            _targetIndex = 0;

            if (_followPathRoutine != null)
            {
                StopCoroutine(_followPathRoutine);
            }

            _followPathRoutine = StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = _path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    // yield is how you exit out a coroutine;
                    yield break;
                }

                currentWaypoint = _path[_targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, _speed * Time.deltaTime);
            yield return null;
        }
    }
    
    public void OnDrawGizmos() {
        if (_path != null) {
            for (int i = _targetIndex; i < _path.Length; i ++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(_path[i], new Vector3(0.3f, 0.3f,0.3f));

                if (i == _targetIndex) {
                    Gizmos.DrawLine(transform.position, _path[i]);
                }
                else {
                    Gizmos.DrawLine(_path[i-1],_path[i]);
                }
            }
        }
    }
}
