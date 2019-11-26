using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public int damage = 50;
    public GameObject impactEffect;
    
    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            // Can be a delay to destroy gameObject so return helps wait
            Destroy(obj: gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        
        // If the length of the dir vector <= distanceThisFrame then it has hit something
        if (dir.magnitude <= distanceThisFrame)
        {
            // When hit the target
            HitTarget();
            return;
        }
        // Normalized so the direction to the object does not affect the speed of the bullet and move relative to world space
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        
    }

    void HitTarget()
    {
        // Destroy bullet on hit
        GameObject _effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(_effectIns, 0.8f);
       
        Damage(target);
        Destroy(gameObject);
    }
    
    void Damage (Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }
}
