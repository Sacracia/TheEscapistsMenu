using HarmonyLib;
using UnityEngine;

namespace TheEscapists2
{
    internal class GeneralMenu : MonoBehaviour
    {
        internal static bool _visible = true;
        private bool _throPassword = false;
        private Rect window = new Rect(530f, 10f, 200f, 200f);

        public void OnGUI()
        {
            if (!_visible)
                return;
            window = GUILayout.Window(2, window, OnWindow, "General", new GUILayoutOption[0]);
        }

        void OnWindow(int windowID)
        {
            DrawElements();
            GUI.DragWindow();
        }

        void DrawElements()
        {
            bool flag = GUILayout.Toggle(_throPassword, "Enter lobbies through password");
            if (flag != _throPassword)
            {
                _throPassword = flag;
                if (flag)
                {
                    var original = AccessTools.DeclaredPropertyGetter(typeof(BrowseGamesFrontendMenu), "SetSelectedLobbyInfo");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.JoinWithoutPassword(null));
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(BrowseGamesFrontendMenu), "SetSelectedLobbyInfo");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }

            if (GUILayout.Button("Unlock all maps", new GUILayoutOption[0]))
            {
                ProgressManager progressManager = ProgressManager.GetInstance();
                if (progressManager == null)
                    return;
                foreach (ProgressMilestone progressMilestone in progressManager.m_Milestones)
                    progressManager.SetMilestoneAchieved(progressMilestone.id, true);
            }
        }
    }
}
