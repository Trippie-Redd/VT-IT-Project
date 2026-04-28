using NUnit.Framework;
using UnityEngine;

[ExecuteInEditMode]
public abstract class Gun : MonoBehaviour
{
    public GunData gunData;

    public Transform firePoint;


    // returns true if a gameobject of target tag is hit
    public void RaycastShoot(string[] targetTags)
    {
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, transform.TransformDirection(Vector3.forward), out hit, gunData.range))
        {
            Debug.DrawRay(firePoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            foreach (string tag in targetTags)
            {
                if (hit.transform.CompareTag(tag))
                {
                    if (!hit.transform.TryGetComponent<Damageable>(out var damageableComponent))
                    {
                        damageableComponent.OnHit(gunData.damage);
                    }
                    else
                    {
                        Debug.LogWarning("Tag of object that has no damageable component passed in to RaycastShoot");
                    }
                }
            }
        }
        else
        {
            Debug.DrawRay(firePoint.position, transform.TransformDirection(Vector3.forward) * 10.0f, Color.red);
        }
    }

    public void Shooting()
    {
        RaycastHit hit;

        if (Physics.Raycast(FirePoint.position, transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            Debug.DrawRay(FirePoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().OnHit();
            }
        }
        else
        {
            Debug.DrawRay(FirePoint.position, transform.TransformDirection(Vector3.forward) * 10.0f, Color.red);
        }
    }
    
    
}
