using System.Collections;
using UnityEngine;

namespace Gun
{
    public class MuzzleFlash : MonoBehaviour
    {
        public Renderer flashRenderer;
        public float flashDuration = 0.05f;
        public Vector2 scaleJitter = new Vector2(0.9f, 1.15f);

        Material _material;
        Vector3 _baseScale;
        Coroutine _routine;

        void Awake()
        {
            _baseScale = transform.localScale;

            if (flashRenderer != null)
            {
                _material = flashRenderer.material;
                flashRenderer.enabled = false;
            }
        }

        public void Show(Texture2D texture)
        {
            if (flashRenderer == null) return;

            if (texture != null && _material != null)
                _material.mainTexture = texture;

            transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            transform.localScale = _baseScale * Random.Range(scaleJitter.x, scaleJitter.y);
            flashRenderer.enabled = true;

            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(_HideAfter());
        }

        IEnumerator _HideAfter()
        {
            yield return new WaitForSeconds(flashDuration);
            flashRenderer.enabled = false;
            _routine = null;
        }
    }
}
