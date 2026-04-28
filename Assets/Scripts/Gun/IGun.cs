using NUnit.Framework;
using UnityEngine;

namespace Gun
{
    public enum CanShootResult
    {
        NoAmmo,
        Underwater,
        CanShoot
    }

    public enum CanReloadResult
    {
        NoAmmo,
        MagFull,
        Underwater,
        Shooting,
        CanReload
    }

    public interface IGun
    {
        CanReloadResult CanReload();
        void TryReload();

        CanShootResult CanShoot();
        public static void Shoot(Transform origin, GunData data, string[] targetTags)
        {
            if (!Physics.Raycast(origin.position, origin.TransformDirection(Vector3.forward), out RaycastHit hit, data.range))
            {
                Debug.DrawRay(origin.position, origin.TransformDirection(Vector3.forward) * 10.0f, Color.red);
                return;
            }

            Debug.DrawRay(origin.position, origin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            foreach (string tag in targetTags)
            {
                if (!hit.transform.CompareTag(tag)) continue;

                if (!hit.transform.TryGetComponent<Damageable>(out var damageableComponent))
                {
                    damageableComponent.OnHit(data.damage);
                }
                else
                {
                    Debug.LogWarning("Tag of object that has no damageable component passed in to RaycastShoot");
                }
            }
        }
    }
}



