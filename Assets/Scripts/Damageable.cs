using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public bool Dead => currentHealth <= 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void OnHit(float damage)
    {
        if (Dead) return;

        currentHealth -= damage;
    }
}
