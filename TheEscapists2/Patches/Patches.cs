using HarmonyLib;
using System;

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

        //[HarmonyPatch(typeof(Player), "DamageCharacter")]
        //[HarmonyPrefix]
        public static bool OneHitKill(Player __instance, Character target, ref float damage)
        {
            Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
            if (__instance == player && target != player)
            {
                damage = 9000f;
                target.CombatBlock(false);
            }
            return true;
        }

        //[HarmonyPatch(typeof(BrowseGamesFrontendMenu), "SetSelectedLobbyInfo")]
        //[HarmonyPrefix]
        public static bool JoinWithoutPassword(LobbyRoomInfoObject lobby)
        {
            string passwordDecrypted = Encryption.Decrypt(lobby.m_Password, "default", "of all the flavours you choose to be salty", "SHA1", 2, 256);
            GeneralMenu.room.Set(lobby.m_RoomName.text, lobby.m_LevelName.text, passwordDecrypted);
            T17NetRoomListManager.NetPhotonRoom m_Room = Traverse.Create(lobby).Field("m_Room").GetValue() as T17NetRoomListManager.NetPhotonRoom;
            var deleg = Delegate.CreateDelegate(typeof(NetJoinRoomHelper.JoinRoomHandler), lobby, "OnJoinedRoomResult") as NetJoinRoomHelper.JoinRoomHandler;
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

        //[HarmonyPatch(typeof(NetUserManager), "RPC_HandleKicked")]
        //[HarmonyPrefix]
        public static bool AntiKick()
        {
            Hacks.SendChatMessage("Athena defends me from kicking!", 
                ChatFeedManager.MessageTag.System);
            return false;
        }

        //[HarmonyPatch(typeof(Character), "GetItemCombat")]
        //[HarmonyPrefix]
        public static bool FistsOfFury(Character __instance, ref Item_Combat __result)
        {
            Item equippedItem = __instance.GetEquippedItem();
            if (equippedItem == null || equippedItem.CombatData == null)
            {
                Item_Combat item_Combat = new Item_Combat();
                Item_Combat m_UnarmedCombatConfig = ConfigManager.GetInstance().combatConfig.m_UnarmedCombatConfig;
                item_Combat.m_CombatConfig = m_UnarmedCombatConfig.m_CombatConfig;
                item_Combat.m_CombatAnimation = m_UnarmedCombatConfig.m_CombatAnimation;
                item_Combat.m_fAttackAngle = 350f;
                item_Combat.m_fAttackRange = 10f;
                item_Combat.m_fRecoveryTime = 0.1f;
                __result = item_Combat;
                return false;
            }
            return true;
        }

        //[HarmonyPatch(typeof(PhotonNetwork), "get_isMasterClient")]
        //[HarmonyPrefix]
        public static bool BeHost(ref bool __result)
        {
            __result = true;
            return false;
        }

        //[HarmonyPatch(typeof(DeskIteraction), "UpdateInteraction")]
        //[HarmonyPrefix]
        public static bool FastOpenDesk(DeskInteraction __instance)
        {
            bool? m_bOpening = Traverse.Create(__instance).Field("m_bOpening").GetValue() as bool?;
            float? m_DeskOpeningTime = Traverse.Create(__instance).Field("m_DeskOpeningTime").GetValue() as float?;
            if (m_bOpening == true && m_DeskOpeningTime != null)
                Traverse.Create(__instance).Field("m_ElapsedOpeningTime").SetValue(m_DeskOpeningTime);
            return true;
        }

        //[HarmonyPatch(typeof(Character), "RPC_RoutineMissedAlertness")]
        //[HarmonyPrefix]
        public static bool AlertnessPenalties()
        {
            return false;
        }

        //[HarmonyPatch(typeof(Item), "DecreaseHealth")]
        //[HarmonyPrefix]
        public static bool DecreaseHealth(Item __instance)
        {
            __instance.m_ItemData.ResetHealth();
            return false;
        }
    }
}
