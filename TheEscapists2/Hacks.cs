using UnityEngine;
using HarmonyLib;

namespace TheEscapists2
{
    internal class Hacks : MonoBehaviour
    {
        private bool _show = true;
        Rect _windowPlayer = new Rect(10f, 10f, 250f, 400f);
        Rect _windowOther = new Rect(270f, 10f, 250f, 400f);
        Rect _windowTrash = new Rect(530f, 10f, 200f, 200f);

        public void OnGUI()
        {
            if (_show)
            {
                _windowPlayer = GUILayout.Window(0, _windowPlayer, ShowWindowPlayer, "Player", new GUILayoutOption[0]);
                _windowOther = GUILayout.Window(1, _windowOther, ShowWindowItem, "Other", new GUILayoutOption[0]);
                _windowTrash = GUILayout.Window(2, _windowTrash, ShowWindowTrash, "Trash", new GUILayoutOption[0]);
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

        void ShowWindowTrash(int windowId)
        {
            Trash.Render();
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
