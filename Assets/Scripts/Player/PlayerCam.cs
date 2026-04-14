using UnityEngine;
using Input;

namespace Player
{
    public class PlayerCam : MonoBehaviour
    {
        public float xSens = 100f;
        public float ySens = 100f;

        public Transform orientation;
        public InputReader inputReader;

        float xRotation;
        float yRotation;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Assert(inputReader != null);
            inputReader.Look += _OnLook;
        }

        void OnDestroy()
        {
            if (inputReader != null)
                inputReader.Look -= _OnLook;
        }

        void _OnLook(Vector2 delta)
        {
            yRotation += delta.x * xSens * Time.deltaTime;

            xRotation -= delta.y * ySens * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            if (orientation != null)
                orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }
}
