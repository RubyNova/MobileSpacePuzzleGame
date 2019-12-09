using UnityEngine;
using UnityEngine.Serialization;

public class Turret : MonoBehaviour
{
    private Transform _target;
    
    [Header("Attributes")]
    [FormerlySerializedAs("range")] [SerializeField] private float _range = 15f;
    [FormerlySerializedAs("fireRate")] [SerializeField] private float _fireRate = 1f;
    private float _fireCountDown = 1f;
    private float _turnSpeed = 10f;
    
    [Header("Unity Setup fields")]
    [FormerlySerializedAs("enemyTag")] [SerializeField] private string _enemyTag = "Enemy";
    [FormerlySerializedAs("partToRotate")] [SerializeField] private Transform _partToRotate;
    [FormerlySerializedAs("turretGun")] [SerializeField] private Transform _turretGun;
    [FormerlySerializedAs("firePoint")] [SerializeField] private Transform _firePoint;
    [FormerlySerializedAs("bulletPrefab")] [SerializeField] private GameObject _bulletPrefab;


    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    private void UpdateTarget()
    {
        // Search through marked enemies and attack the closest one, then if the closest one is in range then attack
        // Rather than do it every frame, it is run around 2 times a second saving processing power.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
        // find the shortest distance, if none found then it can be infinity
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        foreach (GameObject enemy in enemies)
        {
            // Get Distance to enemy
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                // Set target to the nearest enemy
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= _range)
        {
            _target = nearestEnemy.transform;
        }
        else
        {
            _target = null;
        }
    }

    public void Update()
    {
        if (_target == null)
        {
            return;
        }
        // Target Lock on
        // Getting the direction in which the turret should look : n(end) - n(start) = dir
        Vector3 dir = _target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        // The Lerp smoothes the transition from current rotation to target rotation over time * turnSpeed of turret
        // Convert Quaternion in euler angles | to rotate around the y axis only it needs to be specified
        Vector3 rotation = Quaternion.Lerp(_partToRotate.rotation, lookRotation, Time.deltaTime * _turnSpeed).eulerAngles;
        _partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
        
        //Draw Ray from turret to enemy (Purely visualisation)
        Debug.DrawRay(_turretGun.position, dir, Color.green);

        if (_fireCountDown <= 0)
        {
            Shoot();
            _fireCountDown = 1f / _fireRate;
        }

        _fireCountDown -= Time.deltaTime;
    }

    private void Shoot()
    {
        // Instantiation of bullets and destruction on target. Cast returned GameObject into a GameObject to store instantiated objects.
        GameObject bulletGO = (GameObject)Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            // Call the seek method in the bullet script along with the targets position
            bullet.Seek(_target);
        }

    }

    private void OnDrawGizmosSelected()
    {
        // Visualise the range of the turrets
        // Current position and the range of the turret
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
