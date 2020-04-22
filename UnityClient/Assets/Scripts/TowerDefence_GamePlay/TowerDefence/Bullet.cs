using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    private string _identifier;

    [FormerlySerializedAs("speed")] [SerializeField] private float _speed = 70f;
    [FormerlySerializedAs("damage")] [SerializeField] private int _damage = 25;
    [FormerlySerializedAs("impactEffect")] [SerializeField] private GameObject _impactEffect;
    
    public void Seek(Transform target, string targetName)
    {
        _identifier = targetName;
        _target = target;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_target == null)
        {
            // Can be a delay to destroy gameObject so return helps wait
            Destroy(obj: gameObject);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceThisFrame = _speed * Time.deltaTime;
        
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

    private void HitTarget()
    {
        // Destroy bullet on hit
        GameObject effectIns = Instantiate(_impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 0.8f);
       
        Damage(_target);
        Destroy(gameObject);
    }
    
    private void Damage (Transform bulletTarget)
    {
        if (_identifier == "enemy" && bulletTarget != null)
        {
            Unit e = bulletTarget.GetComponent<Unit>();
            e.TakeDamage(_damage);
            
        }
        else if (_identifier == "turret" && bulletTarget != null)
        {
            Turret t = bulletTarget.GetComponent<Turret>();
            if (bulletTarget != null)
            {
                t.TakeDamage(_damage);
            }
        }
        else
        {
            print("The bulletTarget (Bullet)");
        }
    }
}
