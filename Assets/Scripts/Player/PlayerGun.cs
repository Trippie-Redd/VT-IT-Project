using System.Collections;
using Input;
using Gun;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerGun : MonoBehaviour, IGun
    {
        public GunData gunData;
        public string[] targetTags;
        public InputReader inputReader;

        public Transform muzzleOrigin;
        public AudioSource audioSource;
        public MuzzleFlash muzzleFlash;

        int _currentAmmoMag;
        int _currentAmmoBackup;
        float _nextShotTime;
        bool _isReloading;
        Coroutine _burstRoutine;

        PlayerController _player;

        void Awake()
        {
            _player = GetComponent<PlayerController>();
            _currentAmmoMag = gunData.maxMagAmmo;
            _currentAmmoBackup = gunData.maxBackupAmmo;

            Debug.Assert(muzzleOrigin != null, "PlayerGun: muzzleOrigin is not assigned.", this);
            Debug.Assert(gunData != null, "PlayerGun: gunData is not assigned.", this);
        }

        void OnEnable()
        {
            inputReader.Attack += _OnAttackPressed;
            inputReader.Reload += TryReload;
        }

        void OnDisable()
        {
            inputReader.Attack -= _OnAttackPressed;
            inputReader.Reload -= TryReload;

            _burstRoutine = null;
            _isReloading = false;
        }

        void Update()
        {
            if (gunData.fireMode == FireMode.FullAuto && inputReader.IsAttackHeld)
                TryShoot();
        }

        void _OnAttackPressed()
        {
            if (gunData.fireMode == FireMode.FullAuto) return;
            TryShoot();
        }

        public void TryShoot()
        {
            if (_isReloading) return;
            if (Time.time < _nextShotTime) return;

            switch (CanShoot())
            {
                case CanShootResult.NoAmmo:
                    audioSource.PlayOneShot(gunData.magEmptyFireSound);
                    // TODO: HUD "Out Of Ammo"
                    break;
                case CanShootResult.Underwater:
                    // TODO: muffled-click sfx + HUD "Can't Shoot Underwater"
                    break;
                case CanShootResult.CanShoot:
                    if (gunData.fireMode == FireMode.Burst)
                    {
                        _burstRoutine ??= StartCoroutine(_BurstFire());
                    }
                    else
                    {
                        _Shoot();
                    }
                    break;
            }
        }

        public CanShootResult CanShoot()
        {
            if (_player.IsSubmerged) return CanShootResult.Underwater;
            if (_currentAmmoMag <= 0) return CanShootResult.NoAmmo;
            return CanShootResult.CanShoot;
        }

        IEnumerator _BurstFire()
        {
            int shots = Mathf.Max(1, gunData.burstCount);
            float interval = 1f / Mathf.Max(0.01f, gunData.fireRate);
            for (int i = 0; i < shots; i++)
            {
                if (CanShoot() != CanShootResult.CanShoot) break;
                _Shoot();
                if (i < shots - 1) yield return new WaitForSeconds(interval);
            }
            _burstRoutine = null;
        }

        void _Shoot()
        {
            _currentAmmoMag--;
            _nextShotTime = Time.time + 1f / Mathf.Max(0.01f, gunData.fireRate);

            IGun.Shoot(muzzleOrigin, gunData, targetTags);

            if (audioSource != null && gunData.fireSound != null)
                audioSource.PlayOneShot(gunData.fireSound);

            if (muzzleFlash != null)
                muzzleFlash.Show(gunData.muzzleFlash);

            Noise.EmitNoise(transform.position, gunData.noiseStrength);
        }

        public void TryReload()
        {
            switch (CanReload())
            {
                case CanReloadResult.NoAmmo:
                    // TODO: HUD "Out Of Ammo"
                case CanReloadResult.MagFull:
                    // TODO: HUD "Mag Already Full"
                case CanReloadResult.Underwater:
                    // TODO: muffled empty-click sfx + HUD "Can't Reload Underwater"
                    audioSource.PlayOneShot(gunData.cantReloadSound);
                    break;
                case CanReloadResult.Shooting:
                    break;
                case CanReloadResult.AlreadyReloading:
                    break;
                case CanReloadResult.CanReload:
                    StartCoroutine(_ReloadRoutine());
                    break;
            }
        }

        public CanReloadResult CanReload()
        {
            if (_isReloading) return CanReloadResult.AlreadyReloading;
            if (_burstRoutine != null) return CanReloadResult.Shooting;
            if (_currentAmmoBackup == 0) return CanReloadResult.NoAmmo;
            if (_currentAmmoMag >= gunData.maxMagAmmo) return CanReloadResult.MagFull;
            if (_player.IsSubmerged) return CanReloadResult.Underwater;
            return CanReloadResult.CanReload;
        }

        IEnumerator _ReloadRoutine()
        {
            _isReloading = true;
            if (audioSource != null && gunData.reloadSound != null)
                audioSource.PlayOneShot(gunData.reloadSound);
            yield return new WaitForSeconds(gunData.reloadTime);
            Reload();
            _isReloading = false;
        }

        public void Reload()
        {
            int magDifference = gunData.maxMagAmmo - _currentAmmoMag;
            if (_currentAmmoBackup >= magDifference)
            {
                _currentAmmoMag += magDifference;
                _currentAmmoBackup -= magDifference;
            }
            else
            {
                _currentAmmoMag += _currentAmmoBackup;
                _currentAmmoBackup = 0;
            }
        }
    }
}
