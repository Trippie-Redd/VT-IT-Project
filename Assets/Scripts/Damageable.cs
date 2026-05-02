using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth;
    float _currentHealth;

    public bool Dead { private set; get; }

    void Start()
    {
        _currentHealth = maxHealth;
    }

    public void OnHit(float damage)
    {
        if (Dead) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0) 
            Dead = true;
    }
}
