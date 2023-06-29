using UnityEngine;

namespace TheEscapists2
{
    internal class Hacks : MonoBehaviour
    {
        private bool _show = true;
        Rect windowRect = new Rect(10f, 10f, 500f, 400f);

        public void OnGUI()
        {
            if (_show)
            {
                windowRect = GUI.Window(0, windowRect, ShowWindow, "Cheat Menu");
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
