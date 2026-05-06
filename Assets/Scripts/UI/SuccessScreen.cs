using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    // this is like basically the same as DeathScreen, DRY
    [RequireComponent(typeof(UIDocument))]
    public class SuccessScreen : MonoBehaviour
    {
        public Scene mainMenu;
        public Scene map;

        [SerializeField] float charDelay = 0.05f;
        [SerializeField] float lineDelay = 0.4f;

        VisualElement _screen;
        Label _output;
        Label _cursor;

        bool _typingDone;
        float _cursorTimer;
        bool _cursorVisible = true;

        static readonly string[] Lines =
        {
            "Targets eliminated.",
            "Mission successful.",
            "Fine work agent.",
            "",
            "Continue? [Y/n]"
        };

        void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _screen = root.Q<VisualElement>("death-screen");
            _output = root.Q<Label>("terminal-output");
            _cursor = root.Q<Label>("terminal-cursor");
            _cursor.style.visibility = Visibility.Hidden;

            _screen.style.display = DisplayStyle.Flex;

            root.Q<VisualElement>("death-background").style.visibility = Visibility.Hidden;

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            StartCoroutine(_TypeSequence());
        }

        void Update()
        {
            if (!_typingDone) return;

            _cursorTimer += Time.unscaledDeltaTime;
            if (_cursorTimer >= 0.5f)
            {
                _cursorVisible = !_cursorVisible;
                _cursorTimer = 0f;
                _cursor.style.visibility = _cursorVisible 
                    ? Visibility.Visible 
                    : Visibility.Hidden;
            }

            var kb = Keyboard.current;
            if (kb == null) return;
            if (kb.yKey.wasPressedThisFrame || kb.enterKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene((int)Utils.SceneEnum.MainMenu);
            }
            else if (kb.nKey.wasPressedThisFrame)
            {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }

        IEnumerator _TypeSequence()
        {
            string built = "";
            foreach (var line in Lines)
            {
                foreach (char c in line)
                {
                    built += c;
                    _output.text = built;
                    yield return new WaitForSecondsRealtime(charDelay);
                }
                built += "\n";
                _output.text = built;
                yield return new WaitForSecondsRealtime(lineDelay);
            }

            _cursor.style.visibility = Visibility.Visible;
            _typingDone = true;
        }
    }
}
