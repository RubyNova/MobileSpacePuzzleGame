
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Unit : MonoBehaviour
{
    // Movement variables
    public GameObject _target;
    public GameObject _healthBar;
    
    public Vector3 currentWaypoint;
    private Vector3 relativePos;
    private Vector3[] _path;
    private int _targetIndex;

    // Enemy Attributes /state handlers variables
    private float _speed;
    private float _health;
    private float _startHealth;
    private bool _isDead;
    private bool _currentSpeed;

    // Testings Mats
    public Material mat;
    public Material mat2;
    
    // Script instances
    private Graph _graph;
    private Enemy _enemy;
    
    // Reference Enemies on spawn
    public void InvokeEnemy(WaveManager enemy, GameObject target)
    {
        _waveManager = enemy;
        _target = target;
    }
    private WaveManager _waveManager;

    private void OnMouseDown()
    {
        Die();
    }

    void Start()
    {
        _enemy = GetComponent<Enemy>();
        // Initiate enemy attributes
        _speed = _enemy.Speed;
        _startHealth = _enemy.StartHealth;
        _health = _startHealth;

        _healthBar = transform.GetChild(0).gameObject;

        var transformPosition = _target.transform.position;
        PathRequestManager.RequestPath(transform.position, transformPosition, OnPathFound);
    }
    
    private void FixedUpdate()
    {
        // Adjust speed of unit
        if (_enemy.Speed != _speed) _speed = _enemy.Speed;

        if (relativePos == currentWaypoint - transform.position) return;

        // Keep health bar facing same direction
       // _healthBar.transform.rotation = Quaternion.LookRotation(new Vector3(30f, 180f, 0f));
        
        // Keep enemy unit facing forward
        relativePos = currentWaypoint - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }


    public void TakeDamage(float amount)
    {
        _health -= amount;

        _enemy.HealthBar.fillAmount = _health / _startHealth;

        if (_health <= 0 && !_isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        _isDead = true;

        GameObject effect = Instantiate(_enemy.DeathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        
        // Update enemy counter
        _waveManager.EnemiesAlive--;

        Destroy(gameObject);

        // Drop objects when enemy is killed
        foreach (GameObject drop in _enemy.Drops)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }
    
    public void LocateTarget()
    {
        _graph = GameObject.Find("pathfinder").GetComponent<Graph>();
        
        var activeUnits = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject unit in activeUnits)
        {
            var position = new Vector3(Random.Range(-25, _graph.gridWorldSize.x/2), 0, Random.Range(-25, _graph.gridWorldSize.y/2));
            var instance = unit.GetComponent<Unit>();
            instance.gameObject.GetComponent<Renderer>().material = mat;

            while (true)
            {
                if (!_graph.WalkableCheck(position))
                {
                    instance.gameObject.GetComponent<Renderer>().material = mat2;
                    // Get new position if the new position chosen has a UnWalkable region in it.
                    print("Invalid path. requesting new Path...");
                    position = new Vector3(Random.Range(-25, _graph.gridWorldSize.x / 2), 0,
                        Random.Range(-25, _graph.gridWorldSize.y / 2));
                }
                else
                {
                    break;
                }
            }
            instance.Requestpath(position);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        print(" COLLISION ");
        transform.position = transform.position;
    }
    
    public void ShortCut(GameObject newTarget)
    {
        Requestpath(newTarget.transform.position);
    }
    
    public void ShortCut(Vector3 newTarget)
    {
        Requestpath(newTarget);
    }

    private void Requestpath(Vector3 pos)
    {
        PathRequestManager.RequestPath(transform.position,pos, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = newPath;
            _targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        try
        {
            currentWaypoint = _path[0];
        }
        catch
        {
            print("ERROR");
            yield break;
        }

        currentWaypoint += new Vector3(0, transform.position.y);
        
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
                currentWaypoint += new Vector3(0, transform.localPosition.y, 0);
            }
            
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, _speed * Time.deltaTime);
            yield return null;
        }
    }
    
    [ContextMenu("Get New Target")]
    private void PrintIndexes()
    {
        LocateTarget();
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
