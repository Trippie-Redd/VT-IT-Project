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
        AlreadyReloading,
        CanReload
    }

    public interface IGun
    {
        CanReloadResult CanReload();
        void TryReload();

        CanShootResult CanShoot();
        public static void Shoot(Transform origin, GunData data, string[] targetTags)
        {
            Vector3 direction = origin.forward;
            if (!Physics.Raycast(origin.position, direction, out RaycastHit hit, data.range))
            {
                Debug.DrawRay(origin.position, direction * data.range, Color.red, 0.1f);
                return;
            }

            Debug.DrawRay(origin.position, direction * hit.distance, Color.yellow, 0.1f);

            if (targetTags == null) return;

            bool isTarget = false;
            foreach (string tag in targetTags)
            {
                if (hit.transform.CompareTag(tag)) { isTarget = true; break; }
            }
            if (!isTarget) return;

            if (hit.transform.TryGetComponent<Damageable>(out var damageable))
                damageable.OnHit(data.damage);
            else
                Debug.LogWarning($"Target '{hit.transform.name}' matches a target tag but has no Damageable component.", hit.transform);
        }
    }
}



