using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    private Transform _target;

    [FormerlySerializedAs("speed")] [SerializeField] private float _speed = 70f;
    [FormerlySerializedAs("damage")] [SerializeField] private int _damage = 50;
    [FormerlySerializedAs("impactEffect")] [SerializeField] private GameObject _impactEffect;
    
    public void Seek(Transform target)
    {
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
    
    private void Damage (Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(_damage);
        }
    }
}
