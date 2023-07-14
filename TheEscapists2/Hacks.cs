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
                GeneralMenu._visible = !GeneralMenu._visible;
                PlayerMenu._visible = !PlayerMenu._visible;
                PrisonMenu._visible = !PrisonMenu._visible;
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                Loader.Unload();
            }
        }
    }
}
