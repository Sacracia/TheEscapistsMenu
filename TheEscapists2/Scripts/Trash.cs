using HarmonyLib;
using UnityEngine;

namespace TheEscapists2
{
    internal class Trash
    {
        private static bool _throPassword = false;
        private static bool _showLobbies = false;
        internal static void Render()
        {
            bool flag = GUILayout.Toggle(_throPassword, "Enter lobbies through password");
            if (flag != _throPassword)
            {
                _throPassword = flag;
                if (flag)
                {
                    var original = AccessTools.Method(typeof(BrowseGamesFrontendMenu), "SetSelectedLobbyInfo");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.JoinWithoutPassword(null));
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(BrowseGamesFrontendMenu), "SetSelectedLobbyInfo");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }

            flag = GUILayout.Toggle(_showLobbies, "Show all lobbies");
            if (flag != _showLobbies)
            {
                _showLobbies = flag;
                if (flag)
                {
                    var original = AccessTools.Method(typeof(BrowseGamesFrontendMenu), "SetRoomInfo");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.ShowAllLobbies(null));
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(BrowseGamesFrontendMenu), "SetRoomInfo");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }
        }
    }
}
