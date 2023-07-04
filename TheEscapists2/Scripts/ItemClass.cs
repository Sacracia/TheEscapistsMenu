using HarmonyLib;
using UnityEngine;

namespace TheEscapists2
{
    internal class ItemClass : MonoBehaviour
    {
        private static bool _freeAccess = false;
        public static void Render()
        {
            bool _flag = GUILayout.Toggle(_freeAccess, "Open All Doors", new GUILayoutOption[0]);

            if (_flag != _freeAccess)
            {
                _freeAccess = _flag;
                if (_flag)
                {
                    Door door = null;
                    Collider collider = null;
                    var original = AccessTools.Method(typeof(Door), "OnTriggerStay");
                    var mPostfix = SymbolExtensions.GetMethodInfo(() => Patches.PassThroughAnyDoor(door, collider));
                    Loader.harmony.Patch(original, postfix : new HarmonyMethod(mPostfix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(Door), "OnTriggerStay");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Postfix);
                }
            }
        }
    }
}
