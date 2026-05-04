using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class HUD : MonoBehaviour
    {
        VisualElement _root;

        VisualElement _healthBarSlider;
        Label _magAmmoCount;
        Label _backupAmmoCount;
        Label _gunMessage;
        Coroutine _hideMessageRoutine;

        public Damageable playerDamageable;
        public PlayerGun playerGun;

        void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _healthBarSlider = _root.Q<VisualElement>("health-bar-slider");
            _magAmmoCount = _root.Q<Label>("mag-ammo-count");
            _backupAmmoCount = _root.Q<Label>("backup-ammo-count");
            _gunMessage = _root.Q<Label>("gun-message");

            playerGun.OnGunMessage += ShowGunMessage;
        }

        void OnDisable()
        {
            playerGun.OnGunMessage -= ShowGunMessage;
        }

        void Update()
        {
            float healthPercent = playerDamageable.currentHealth / playerDamageable.maxHealth * 100f;
            _healthBarSlider.style.width = new Length(healthPercent, LengthUnit.Percent);

            _magAmmoCount.text = playerGun.currentAmmoMag.ToString();
            _backupAmmoCount.text = playerGun.currentAmmoBackup.ToString();
        }

        void ShowGunMessage(string message)
        {
            _gunMessage.text = message;
            _gunMessage.style.display = DisplayStyle.Flex;

            if (_hideMessageRoutine != null) StopCoroutine(_hideMessageRoutine);
            _hideMessageRoutine = StartCoroutine(HideMessageAfterDelay(2f));
        }

        IEnumerator HideMessageAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _gunMessage.style.display = DisplayStyle.None;
            _hideMessageRoutine = null;
        }
    }
}