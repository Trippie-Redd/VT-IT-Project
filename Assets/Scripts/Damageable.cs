using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth;
    float currentHealth;
    bool dead;

    public bool Dead { private set; get; }

    public void OnHit(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) 
            Dead = true;
    }
}
