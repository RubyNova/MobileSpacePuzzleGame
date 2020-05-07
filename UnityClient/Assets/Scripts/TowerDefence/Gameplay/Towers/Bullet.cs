using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float speed = 70f;
    [SerializeField] private float explosionRadius = 0f;
    [SerializeField] private GameObject ImpactEffect;
    [SerializeField] private int damage = 20;

    public int BulletDamage => damage;

    private bool hitWall = false;
    
    public void Seek(Transform _target)
    {
        // Effect in bullet
        target = _target;
    }

    // Update is called once per frame
    private void Update()
    {
        // Make sure there is a target
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        
        // IF the length of the direction vector is <= distance move this frame then bullet has hit something
        if (dir.sqrMagnitude <= distanceThisFrame)
        {
            // Hit Target
            HitTarget();
        }
        
        // However close you are to object does not affect speed so normalize direction
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Wall")) return;
        print("WALL");
        hitWall = true;
        HitTarget();
    }

    private void HitTarget()
    {
        GameObject effectInstance = Instantiate(ImpactEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 3f);
        
        // If explosion radius is greater than 0, damage enemies in a radius
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else if (!hitWall)
        {
            Damage(target);
        }

        hitWall = false;
        
        Destroy(gameObject);
    }
    
    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            { // If they are an enemy damage them 
                Damage(collider.transform);
            }
        }
    }
    
    private void Damage(Transform enemy)
    {
        Unit u = enemy.GetComponent<Unit>();

        if (u != null)
        {
            // Apply Physical Resistance of Enemy -- Rocket and Bullet
            u.TakeDamage(damage - u.GetPhysicalResistance() * damage);
        }
        else
        {
            print("Object does not have a Unit Component");
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
