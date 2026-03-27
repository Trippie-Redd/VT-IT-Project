using UnityEngine;
using Input;

namespace Player
{
    public class PlayerCam : MonoBehaviour
    {
        public float xSens;
        public float ySens;

        public Transform orientation;

        public InputReader inputReader;

        float xRotation;
        float yRotation;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Assert(inputReader != null);

        }

        void _OnLook(Vector2 direction, bool idfk)
        {
            var sensitivity = new Vector2(xSens, ySens);
            sensitivity *= Time.deltaTime;

            var mouse = direction * sensitivity;

            yRotation += mouse.x;

            xRotation -= mouse.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            transform.rotation = Quaternion.Euler(inputReader.Direction);

        }
    }
}