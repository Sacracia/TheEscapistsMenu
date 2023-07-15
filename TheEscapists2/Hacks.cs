using UnityEngine;
using HarmonyLib;

namespace TheEscapists2
{
    internal class Hacks : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                GeneralMenu.visible = !GeneralMenu.visible;
                PlayerMenu.visible = !PlayerMenu.visible;
                PrisonMenu.visible = !PrisonMenu.visible;
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                Loader.Unload();
            }
        }
    }
}
