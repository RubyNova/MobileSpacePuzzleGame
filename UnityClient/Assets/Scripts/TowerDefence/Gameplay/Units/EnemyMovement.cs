using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    #region Movement

    // Movement variables
    private GameObject _target;
    
    // Path Finder Variables
    private Vector3 currentWayPoint;
    private Vector3[] _path;
    private int _targetIndex;
    
    // Script References
    private WaveManager _waveManager;
    private Enemy _enemy;
    private Graph _graph;

    #endregion
    
    // AvoidTarget
    private Transform targetToAvoid;

    // If speed is less than a units original speed they are slowed
    private bool isSlowed = false;
    private bool isMoving = false;
    
    private RaycastHit hit;
    private Rigidbody rb;
    
    public bool Slowed()
    {
        return isSlowed;
    }
    
    // Enemy Instantiation
    public void InvokeEnemy(WaveManager enemy, GameObject target)
    {
        _waveManager = enemy;
        _target = target;
    }
    
    // Get Enemy Target
    public GameObject GetTarget()
    {
        return _target;
    }

    public WaveManager GetWaveManager()
    {
        return _waveManager;
    }
    
    private void Start()
    {
        // Get Enemy Component
        _enemy = GetComponent<Enemy>();

        // Get Target transform position
        var targetPos = _target.transform.position;
        // Make Path Request to Pathfinder 
        PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
    }
    
    private void Update()
    {
        if (isSlowed) return;
        _enemy.Speed = _enemy.StartSpeed;
    }
    
    #region MovementDebuffs

    public void RemoveMovementDebuffs()
    {
        // Remove slow de-buff
        isSlowed = false;
    }
    
    public void Slow(float slowAmount)
    { // Slow between 0% and 100%
        isSlowed = true;
        _enemy.Speed = _enemy.StartSpeed * (1f - slowAmount);
    }    

    #endregion
    
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (!pathSuccessful) return;
        _path = newPath;
        _targetIndex = 0;
        StopCoroutine(nameof(FollowPath));
        StartCoroutine(nameof(FollowPath));
    }
    
    private IEnumerator FollowPath()
    {
        try
        {
            currentWayPoint = _path[0];
        }
        catch
        {
            print("ERROR");
            yield break;
        }

        currentWayPoint += new Vector3(0, transform.position.y - 3);
        
        while (true)
        {
            if (transform.position == currentWayPoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    // yield is how you exit out a coroutine;
                    yield break;
                }
                currentWayPoint = _path[_targetIndex];
                currentWayPoint += new Vector3(0, transform.localPosition.y - 3, 0);
            }
            
            UnitLookDirection();
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, _enemy.Speed * Time.deltaTime);
            yield return null;
        }
    }

    private void UnitLookDirection()
    {
        var partToRotate = gameObject.transform;
        
        // Rotate Unit
        Vector3 dir = currentWayPoint - transform.position;
        
        // Smooth transition
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
        
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); 
    }

    // Shortcut Functionality with 2 Overrides 
    public void ShortCut(GameObject newTarget) => RequestPath(newTarget.transform.position);
    public void ShortCut(Vector3 newTarget) => RequestPath(newTarget);
    private void RequestPath(Vector3 pos) => PathRequestManager.RequestPath(transform.position,pos, OnPathFound);
    
    public void OnDrawGizmos()
    {
        if (_path == null) return;
        for (int i = _targetIndex; i < _path.Length; i ++) {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(_path[i], new Vector3(0.3f, 0.3f,0.3f));

            Gizmos.DrawLine(i == _targetIndex ? transform.position : _path[i - 1], _path[i]);
        }
    }

    [ContextMenu("DELETE PATH")]
    private void RemovePath()
    {
        StopCoroutine(nameof(FollowPath));
        _path = null;
    }
}
