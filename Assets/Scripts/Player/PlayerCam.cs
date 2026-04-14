using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{

    // Yo this fuckahh camera to annoying it keeps feeling bad and snappy and jittery
    // especially when i tried to use the new input system
    // if there is time i will fix/change this to the new input system but for rn this is fine
    public class PlayerCam : MonoBehaviour
    {
        public float xSens = 0.1f;
        public float ySens = 0.1f;

        public Transform orientation;

        float xRotation;
        float yRotation;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void LateUpdate()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            Vector2 delta = mouse.delta.ReadValue();

            yRotation += delta.x * xSens * Time.deltaTime;
            xRotation -= delta.y * ySens * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Set player yaw first so camera inherits it via the parent transform
            if (orientation != null)
                orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);

            // Camera only handles pitch via local rotation; yaw comes from the parent
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
