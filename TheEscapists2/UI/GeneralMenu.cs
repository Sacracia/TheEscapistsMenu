﻿using HarmonyLib;
using UnityEngine;

namespace TheEscapists2
{
    internal class GeneralMenu : MonoBehaviour
    {
        internal struct RoomInfo
        {
            public string Name;
            public string LevelName;
            public string Password;

            public void Set(string name, string levelname, string password)
            {
                Name = name;
                LevelName = levelname;
                Password = password;
            }
        }

        internal static RoomInfo room = new RoomInfo();
        internal static bool visible = true;
        private bool _throPassword = false;
        private bool _antikick = false;
        private Rect window = new Rect(530f, 10f, 400f, 300f);

        public void OnGUI()
        {
            if (!visible)
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

            flag = GUILayout.Toggle(_antikick, "Antikick");
            if (flag != _antikick)
            {
                _antikick = flag;
                if (flag)
                {
                    var original = AccessTools.Method(typeof(NetUserManager), "RPC_HandleKicked");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.AntiKick());
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(NetUserManager), "RPC_HandleKicked");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }

            if (_throPassword)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Last Server Info:", new GUILayoutOption[0]);
                GUILayout.Label($"Name: {room.Name}", new GUILayoutOption[0]);
                GUILayout.Label($"Level: {room.LevelName}", new GUILayoutOption[0]);
                GUILayout.Label($"Password: {room.Password}", new GUILayoutOption[0]);
                if (GUILayout.Button("Reconnect", new GUILayoutOption[0]))
                {
                    NetConnectAndJoinRoom.Init_OnlineMode_JoinSpecific(room.Name);
                }
                GUILayout.EndVertical();
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
