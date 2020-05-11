using System;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Unit : MonoBehaviour
{
    //spawn this when it dies
    [SerializeField] private GameObject _explosion;
    
    // Fire Effect
    private bool _isBurning;
    private float _countdown = 0;
    private int burnPerSecond = 0;
    private GameObject fireEffect;
        
    private bool _isDead;
    private bool reachedCore;

    public bool ReachedCore
    {
        set => reachedCore = value;
    }

    private Enemy _enemy;
    private EnemyMovement _movement;
    
    public float GetPhysicalResistance()
    { // Helper Function for getting units resistance
        return _enemy.PhysicalResist;
    }

    public float GetMagicResistance()
    {
        return _enemy.MagicResist;
    }

    void Start()
    {
        _enemy = GetComponent<Enemy>();
        _movement = GetComponent<EnemyMovement>();
    }
    
    private void Update()
    {
        if (!_isBurning) return;
        if (_countdown <= 0)
        {
            _isBurning = false;
            Destroy(fireEffect);
            return;
        }
        //Burning - Apply Units fire resistance
        TakeDamage((burnPerSecond - _enemy.FireResist * burnPerSecond) * Time.deltaTime);
       
        // Play Burn Effect
        fireEffect.transform.position = transform.position + Vector3.up;

        // Reduce countdown by 1 every second
        _countdown -= Time.deltaTime;
        // Make sure countdown is never less than 0
        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);

    }
    
    private void OnParticleCollision(GameObject other)
    { // Fire Turret Damage
        if (_isDead) return;
        var turret = other.GetComponentInParent<Turret>();
        
        // Apply Units fire resistance
        TakeDamage(turret.damagePerHit - _enemy.FireResist * turret.damagePerHit);
        _countdown = turret.burnDuration;
        burnPerSecond = turret.burnDamage;

        if (_isBurning) return;
        _isBurning = true;
        fireEffect = Instantiate(turret.burnEffect, transform.position, transform.rotation);
    }
    
    public void TakeDamage(float amount)
    {
        _enemy.Health -= amount;

        _enemy.HealthBar.fillAmount = _enemy.Health / _enemy.StartHealth;

        if (_enemy.Health <= 0 && !_isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        _isDead = true;
        
        if(_isBurning) Destroy(fireEffect);

        GameObject effect = Instantiate(_enemy.DeathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        
        // Update enemy counter
        _movement.GetWaveManager().EnemiesAlive--;
        
        // Give player Points
        if(!reachedCore) PlayerStats.Money += _enemy.Worth;

        // Destroy Enemy
        Destroy(gameObject);
    }

    private void OnDestroy() => Instantiate(_explosion, transform.position, transform.rotation);
}
