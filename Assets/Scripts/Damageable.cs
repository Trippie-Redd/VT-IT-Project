using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    bool _dead = false;

    public bool IsDead => _dead;

    public event UnityAction Dead = delegate { };

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void OnHit(float damage)
    {
        if (_dead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Dead.Invoke();
            _dead = true;
        }

    }
}
