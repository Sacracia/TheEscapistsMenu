using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapists2
{
    [HarmonyPatch]
    internal class Patches
    {
        //[HarmonyPatch(typeof(Player), "TakeDamage")]
        //[HarmonyPrefix]
        public static bool Godmode(ref bool __result)
        {
            __result = true;
            return false;
        }

        //[HarmonyPatch(typeof(AICharacter_Guard), "MissingKeyCheck")]
        //[HarmonyPrefix]
        public static bool NoMissingKeyCheck()
        {
            return false;
        }

        //[HarmonyPatch(typeof(Character), "DamageCharacterEvent")]
        //[HarmonyPrefix]
        public static bool OneHitKill(Character __instance, Character targetCharacter, ref float damage)
        {
            Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
            if (__instance == player)
            {
                damage = 9000f;
                targetCharacter.m_bIsBlocking = false;
            }
            return true;
        }

        //[HarmonyPatch(typeof(BrowseGamesFrontendMenu), "SetSelectedLobbyInfo")]
        //[HarmonyPrefix]
        public static bool JoinWithoutPassword(LobbyRoomInfoObject lobby)
        {
            T17NetRoomListManager.NetPhotonRoom m_Room = Traverse.Create(lobby).Field("m_Room").GetValue() as T17NetRoomListManager.NetPhotonRoom;
            var deleg = System.Delegate.CreateDelegate(typeof(NetJoinRoomHelper.JoinRoomHandler), lobby, "OnJoinedRoomResult") as NetJoinRoomHelper.JoinRoomHandler;
            NetJoinRoomHelper.JoinRoom(m_Room.Name, false, deleg, true, true);
            return false;
        }

        //[HarmonyPatch(typeof(ContrabandDetector), "DetectContraband")]
        //[HarmonyPrefix]
        public static bool DisableContrabandDetectors()
        {
            return false;
        }

        //[HarmonyPatch(typeof(RoutineManager), "ShouldSpeedUpForSleeping")]
        //[HarmonyPrefix]
        public static bool SpeedUpTime(ref bool __result)
        {
            __result = true;
            return false;
        }

        //[HarmonyPatch(typeof(CameraManager), "CalculatePixelPerfectOffset")]
        //[HarmonyPostfix]
        public static void FOV(ref float __result)
        {
            __result *= PlayerMenu.fov;
        }
    }
}
