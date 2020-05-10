using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Transform target;
    private Turret targetTurret;
    private bool attacked;
    private float countdown;
    
    [Header("Attributes")]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool noRotate;
    
    [Header("Attack Beam")] 
    [SerializeField] private LineRenderer lineRender;
    [SerializeField] private int damage = 10;
    [SerializeField] [Range(0f, 1f)] private float chanceToAttack = 0;
    [SerializeField] [Range(1f, 10f)] private int maxAttacks = 0;
    [SerializeField] private ParticleSystem lineImpact;
    [SerializeField] private Light lineLight;
    [SerializeField] private float range = 5;
    [SerializeField] private float timeBetweenAttacks = 10f;
    
    [Header("Set-Up Fields")]
    [SerializeField] private string turretTag = "Turret";
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform partToRotate;
    [SerializeField] private float turnSpeed = 10f;
    
    private void Start()
    {
        if (!canAttack)
        {
            lineRender.enabled = false;
            return;
        }
        countdown = timeBetweenAttacks;
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);

        lineRender.enabled = false;
        lineImpact.Stop();
        lineLight.enabled = false;
    }

    private void UpdateTarget()
    {
        if (!canAttack) return;

        GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);
        // No Turrets found
        if (turrets == null) return;
        
        // Enemy shortest Distance away
        float shortestDistance = Mathf.Infinity; // If there is no enemies found it is an infinite distance
        // Store nearest enemy so Far
        GameObject nearestTurret = null;

        foreach (var turret in turrets)
        {
            float distanceToTurret = Vector3.Distance(transform.position, turret.transform.position);

            if (!(distanceToTurret < shortestDistance) || !(distanceToTurret <= range)) continue;
            // Turret is in range
            // Check if wall is in the way 
            if (CheckForWall(turret)) continue;
            if (CheckIfAlreadyDestroyed(turret)) continue;
            shortestDistance = distanceToTurret;
            nearestTurret = turret;
        }

        if (nearestTurret != null && shortestDistance <= range)
        {
            target = nearestTurret.transform;
            targetTurret = nearestTurret.GetComponent<Turret>();
        }
        else
        {
            target = null;
        }
    }
    
    private bool CheckForWall(GameObject turret)
    {
        var enemyPos = turret.transform.position;
        // Does the ray intersect any objects excluding the player layer
        if (!Physics.Raycast(firePoint.position, (enemyPos - firePoint.position), out var hit, range))
            return false; // No Wall in way
        Debug.DrawRay(firePoint.position, (enemyPos - firePoint.position), Color.green);
        return hit.transform.gameObject.layer == 8;
    }
    
    private bool CheckIfAlreadyDestroyed(GameObject turret)
    { // Check if object is destroyed in its turret script
        return turret.GetComponent<Turret>().Destroyed;
    }
    
    private void Update()
    {
        if (!canAttack || !target) return;

        if (maxAttacks == 0) return;

        // If attack cooldown has reached 0 and there is a target attempt to attack
        if (countdown <= 0)
            if (target) 
                AttemptAttack();

        if (noRotate)
        {
            // Lock On To Target
            LockOnTarget();
        }

        countdown -= Time.deltaTime;
    }
    
    private void AttemptAttack()
    {
        float randomNumber = Random.Range(1, 100);
        randomNumber /= 100;
        print(randomNumber);

        if (randomNumber <= chanceToAttack) // Attacks
        {
            targetTurret.TakeDamage(damage);
            maxAttacks--;
            
            if (!lineRender.enabled)
            {
                lineRender.enabled = true;
                lineImpact.Play();
                lineLight.enabled = true;
            }
            
            // Laser Beam
            lineRender.SetPosition(0, firePoint.position);
            lineRender.SetPosition(1, target.position);

            // Stop impact Effect and Light
            lineImpact.Stop();
            lineLight.enabled = false;
            
            StartCoroutine(nameof(LaserFade));
            
            // Handle particle System
            Vector3 targetToEnemy = firePoint.position - target.position;
            // Create Effect at collision point of Laser and Enemy ( impact effect offset - *offset)
            lineImpact.transform.position = target.position + targetToEnemy.normalized; //* .5f;
            // Point towards turret
            lineImpact.transform.rotation = Quaternion.LookRotation(targetToEnemy);
        }

        ResetCountdown();
    }
    
    IEnumerator LaserFade()
    {
        yield return new WaitForSeconds(0.1f);
        if (!lineRender.enabled) yield break;
        lineRender.enabled = false;
    }
    
    private void LockOnTarget()
    {
        // Rotate Turret head -- Vector 3 points in enemies direction
        Vector3 dir = target.position - firePoint.position;
        
        // Smooth Transition of rotation with Lerp
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles; // To rotate only around Y axis

        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        
    }
    
    private void ResetCountdown()
    {
        // Reset countdown
        countdown = timeBetweenAttacks;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
