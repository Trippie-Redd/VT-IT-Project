using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Input;
using Gun;
using UnityEngine;

namespace Player
{
    public class PlayerGun : MonoBehaviour, IGun
    {
        public GunData gunData;

        public string[] targetTags;

        public InputReader inputReader;


        int _currentAmmoMag;
        int _currentAmmoBackup;

        void OnEnable()
        {
            inputReader.Attack += TryShoot;
            inputReader.Reload += TryReload;
        }

        void OnDisable()
        {
            inputReader.Attack += TryShoot;
            inputReader.Reload += TryReload;
        }


        public void TryShoot()
        {
            switch (CanShoot())
            {
                case CanShootResult.NoAmmo:
                    // Play clicking sound
                    // HUD message: "Out Of Ammo"
                    break;
                case CanShootResult.Underwater:
                    // Play muffled clicking sound
                    // HUD message: "Can't Shoot Underwater"
                    break;
                case CanShootResult.CanShoot:
                    // Shoot();
                    break;
            }
        }

        public CanShootResult CanShoot()
        {
            if (_currentAmmoMag == 0)
                return CanShootResult.NoAmmo;

            if (gameObject.GetComponent<PlayerController>().IsSubmerged)
                return CanShootResult.Underwater;

            return CanShootResult.CanShoot;
        }

        public void TryReload()
        {
            switch (CanReload())
            {
                case CanReloadResult.NoAmmo:
                    // Play some sound
                    // HUD message: "Out Of Ammo"
                    break;
                case CanReloadResult.MagFull:
                    // Play some sound
                    // HUD message: "Mag Already Full"
                    break;
                case CanReloadResult.Underwater:
                    // Play NoAmmo sound but muffled
                    // HUD message: "Cant Reload Underwater"
                    break;
                case CanReloadResult.Shooting:
                    // Wait until shoot anim is finished then reload
                    break;
                case CanReloadResult.CanReload:
                    // Play reload sound
                    // Reloadfunction();
                    break;
            }
        }

        public CanReloadResult CanReload()
        {
            if (_currentAmmoBackup == 0)
                return CanReloadResult.NoAmmo;

            if (_currentAmmoMag >= gunData.maxMagAmmo)
                return CanReloadResult.MagFull;

            if (gameObject.GetComponent<PlayerController>().IsSubmerged)
                return CanReloadResult.Underwater;

            if (gunData.fireMode != FireMode.SemiAuto)
            {
                // TODO - Implement CanReloadResult.Shooting   
            }

            return CanReloadResult.CanReload;
        }

        public void Reload()
        {
            // Begin reload animation
            // await finish
            // Use either a callback
            // Or make this function async

            // if successful:
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