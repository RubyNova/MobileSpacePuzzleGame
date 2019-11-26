using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    
    [Header("Attributes")]
    
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountDown = 1f;
    private float turnSpeed = 10f;
    
    [Header("Unity Setup fields")]

    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public Transform turretGun;

    public GameObject bulletPrefab;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void UpdateTarget()
    {
        // Search through marked enemies and attack the closest one, then if the closest one is in range then attack
        // Rather than do it every frame, it is run around 2 times a second saving processing power.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
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

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    public void Update()
    {
        if (target == null)
        {
            return;
        }
        // Target Lock on
        // Getting the direction in which the turret should look : n(end) - n(start) = dir
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        // The Lerp smoothes the transition from current rotation to target rotation over time * turnSpeed of turret
        // Convert Quaternion in euler angles | to rotate around the y axis only it needs to be specified
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
        
        //Draw Ray from turret to enemy (Purely visualisation)
        Debug.DrawRay(turretGun.position, dir, Color.green);

        if (fireCountDown <= 0)
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }

        fireCountDown -= Time.deltaTime;
    }

    void Shoot()
    {
        // Instantiation of bullets and destruction on target. Cast returned GameObject into a GameObject to store instantiated objects.
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            // Call the seek method in the bullet script along with the targets position
            bullet.Seek(target);
        }

    }

    private void OnDrawGizmosSelected()
    {
        // Visualise the range of the turrets
        // Current position and the range of the turret
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
