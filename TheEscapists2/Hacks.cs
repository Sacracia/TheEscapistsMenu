using UnityEngine;
using HarmonyLib;

namespace TheEscapists2
{
    internal class Hacks : MonoBehaviour
    {
        private bool _show = true;
        Rect _windowPlayer = new Rect(10f, 10f, 250f, 400f);

        public void OnGUI()
        {
            if (_show)
            {
                _windowPlayer = GUILayout.Window(0, _windowPlayer, ShowWindow, "Player", new GUILayoutOption[0]);
            }
        }

        void ShowWindow(int windowId)
        {
            PlayerClass.Render();
            GUI.DragWindow();
        }

        public void Start()
        {
        }

        public void Update()
        {
            PlayerClass.Update();
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                _show = !_show;
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                Loader.Unload();
            }
        }
    }
}
