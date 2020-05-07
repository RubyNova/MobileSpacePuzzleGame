using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    // Temporary Variables
    private bool gotTarget = false;
    
    // ++ Helper Function ++
    private Transform target;
    public Transform Target => target;
    
    private Unit targetEnemy;
    private EnemyMovement targetEnemyMovement;
    private Animator anim;

    [Header("General")]
    [SerializeField] private int range = 15;
    [SerializeField] private float startHealth = 100f;

    public float StartHealth => startHealth;

    // ++ Helper Function ++
    private float health;
    public float Health => health;

    // ++ Helper Function ++
    private bool destroyed;
    public bool Destroyed => destroyed;
    private bool damaged;
    public bool Damaged => damaged;
    
    [Header("Use Bullets (default)")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Use Laser")] 
    [SerializeField] private bool useLaser = false;
    [SerializeField] private LineRenderer lineRender;
    [SerializeField] private int damageOverTime = 30;
    [Range(0.0f, 1.0f)][SerializeField] private float slowPercentage = .5f;
    [SerializeField] private ParticleSystem lineImpact;
    [SerializeField] private Light lineLight;

    [Header("Use Fire")] 
    [SerializeField] private bool useFire = false;
    [SerializeField] private ParticleSystem fireImpact;
    [SerializeField] public int damagePerHit = 4;
    [SerializeField] public int burnDamage = 2;
    [SerializeField] public int burnDuration = 1;
    [SerializeField] public GameObject burnEffect;
    
    [Header("Set-Up Fields")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private Transform partToRotate;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Transform firePoint;
    
    private void Start()
    {// Set Turrets Health
        health = startHealth;
        
        anim = GetComponent<Animator>();
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }
    
    // ++ Helper Function ++
    public string GetDamage()
    { // Return Damage
        if (useLaser)
            return damageOverTime + "/s";
        if (useFire)
            return damagePerHit + " Per Hit";

        return bulletPrefab.GetComponent<Bullet>().BulletDamage.ToString();
    }
    
    // ++ Helper Function ++
    public int GetRange()
    { // Return Range
        return range;
    }

    // Update is called once per frame
    private void UpdateTarget()
    {
        if (destroyed)
        {
            target = null;
            return;
        }
        
        if (targetEnemyMovement != null)
        {
            RemoveDebuffs();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        // Enemy shortest Distance away
        float shortestDistance = Mathf.Infinity; // If there is no enemies found it is an infinite distance
        // Store nearest enemy so Far
        GameObject nearestEnemy = null;
        
        foreach (var enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            { // Found enemy closer than any previously
                // Check if wall is in the way
                if (CheckForWall(enemy)) continue;
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        
        // If we have find an enemy and within range
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            // Get Enemy component whenever a new target is found instead of every frame
            targetEnemy = nearestEnemy.GetComponent<Unit>();
            targetEnemyMovement = nearestEnemy.GetComponent<EnemyMovement>();
            
            // Set State of turret in state machine
            anim.SetBool(nameof(gotTarget), true);
        } // If enemy has left range
        else
        {
            target = null;
            
            // Set State of turret in state machine
            anim.SetBool(nameof(gotTarget), false);
        }
    }
    
    private bool CheckForWall(GameObject enemy)
    {
        var enemyPos = enemy.transform.position;
        // Does the ray intersect any objects excluding the player layer
        if (!Physics.Raycast(firePoint.position, (enemyPos - firePoint.position), out var hit, range))
            return false; // No Wall in way
        Debug.DrawRay(firePoint.position, (enemyPos - firePoint.position), Color.green);
        return hit.transform.gameObject.layer == 8;
    }
    
    private void RemoveDebuffs()
    {
        // If enemy is slowed remove it
        if (targetEnemyMovement.Slowed())
        {
            targetEnemyMovement.RemoveMovementDebuffs();
        }
    }
    

    private void Update()
    {
        // Check health of turret
        if (health >= 0) Destroy();
        
        // If health is less than startHealth turret is damaged
        damaged = health < startHealth;
        
        // If there is no target found do nothing
        if (target == null)
        { // If laser being used
            if (useLaser)
            {
                if (lineRender.enabled)
                {
                    lineRender.enabled = false;
                    lineImpact.Stop();
                    lineLight.enabled = false;
                }
            }
            else if (useFire)
            {
                fireImpact.Stop();
            }
            return;
        }

        LockOnTarget();

        if (useLaser)
        { // Do laser functionality
            Laser();
        }
        else if (useFire)
        {
            Fire();
        }
        else
        { // Do bullet functionality
            // If fireCountdown is 0 then shoot
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }
    
    private void LockOnTarget()
    {
        // Rotate Turret head -- Vector 3 points in enemies direction
        Vector3 dir = target.position - transform.position;
        
        // Smooth Transition of rotation with Lerp
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles; // To rotate only around Y axis

        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
    
    private void Fire()
    {
        fireImpact.Play();

        Vector3 dir = target.position - transform.position;

        fireImpact.transform.rotation = Quaternion.LookRotation(dir);
    }
    
    private void Laser()
    {
        // Damage enemy - over time (Apply Resistance)
        targetEnemy.TakeDamage((damageOverTime - targetEnemy.GetMagicResistance() * damageOverTime) * Time.deltaTime);
        // Apply Slow to Enemy
        targetEnemyMovement.Slow(slowPercentage);
        
        if (!lineRender.enabled)
        {
            lineRender.enabled = true;
            lineImpact.Play();
            lineLight.enabled = true;
        }

        lineRender.SetPosition(0, firePoint.position);
        lineRender.SetPosition(1, target.position);

        // Handle particle System
        Vector3 targetToTurret = firePoint.position - target.position;
        // Create Effect at collision point of Laser and Enemy ( impact effect offset - *offset)
        lineImpact.transform.position = target.position + targetToTurret.normalized; //* .5f;
        // Point towards turret
        lineImpact.transform.rotation = Quaternion.LookRotation(targetToTurret);
    }
    
    private void Destroy()
    { // Turret has been destroyed
        if (health > 0) return; // Not destroyed
        destroyed = true;
        
        // Set to destroyed state in FSM
        anim.SetBool(nameof(destroyed), true);
    }

    public void TakeDamage(int damage)
    { // Turret has taken damage
        health -= damage;
        print(name + " has taken " + damage + " damage");
    }

    public void RepairTurret()
    { // Helper function to repair turret
        health = startHealth;
        destroyed = false;
        
        // Set to NOT destroyed state in FSM
        anim.SetBool(nameof(destroyed), false);
    }
    
    private void Shoot()
    {
        GameObject currentBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = currentBullet.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(target);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualise the range of the turrets
        // Current position and the range of the turret
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
