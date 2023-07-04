using UnityEngine;
using HarmonyLib;

namespace TheEscapists2
{
    internal class Hacks : MonoBehaviour
    {
        private bool _show = true;
        Rect _windowPlayer = new Rect(10f, 10f, 250f, 400f);
        Rect _windowItem = new Rect(270f, 10f, 250f, 400f);

        public void OnGUI()
        {
            if (_show)
            {
                _windowPlayer = GUILayout.Window(0, _windowPlayer, ShowWindowPlayer, "Player", new GUILayoutOption[0]);
                _windowItem = GUILayout.Window(1, _windowItem, ShowWindowItem, "Inventory", new GUILayoutOption[0]);
            }
        }

        void ShowWindowPlayer(int windowId)
        {
            PlayerClass.Render();
            GUI.DragWindow();
        }

        void ShowWindowItem(int windowId)
        {
            ItemClass.Render();
            GUI.DragWindow();
        }

        public void Start() {}

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
