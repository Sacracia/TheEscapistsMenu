using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapists2
{
    internal class ItemClass : MonoBehaviour
    {
        private static bool _noCollide = false;
        private static bool _checkMissingKey = false;
        private static bool _oneHitKill = false;
        private static bool _freezeTimer = false;
        private static void KnockoutEveryone()
        {
            if (!PlayerClass.player)
                return;
            List<Character> characters = Character.GetAllCharacters();
            foreach (Character character in characters)
            {
                if (character != PlayerClass.player && (character.m_CharacterRole == CharacterRole.Guard 
                    || character.m_CharacterRole == CharacterRole.Warden || character.m_CharacterRole == CharacterRole.Dog))
                    character.SetIsKnockedOut(true, PlayerClass.player);
            }
        }

        private static void SetKeysFound()
        {
            SolitaryManager solitaryManager = SolitaryManager.GetInstance();
            if (!solitaryManager)
                return;
            List<int> m_MissingKeys = Traverse.Create(solitaryManager).Field("m_MissingKeys").GetValue() as List<int>;
            T17NetView m_NetView = Traverse.Create(solitaryManager).Field("m_NetView").GetValue() as T17NetView;
            foreach (int id in m_MissingKeys) {
                m_NetView.RPC("RPC_SetKeyMissing", NetTargets.All, new object[]
                {
                    id,
                    false
                });
            }
            m_MissingKeys.Clear();
        }

        private static void SetOpinions()
        {
            Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
            if (!player)
                return;
            foreach (Character character in Character.GetAllCharacters())
            {
                if (character && (character.m_CharacterRole == CharacterRole.Inmate || character.m_CharacterRole == CharacterRole.Guard
                    || character.m_CharacterRole == CharacterRole.Warden || character.m_CharacterRole == CharacterRole.Dog))
                {
                    CharacterOpinions opinions = character.m_CharacterOpinions;
                    opinions?.SetOpinionOf(player, 100);
                }
            }
        }

        private static void TeleportToRoutine()
        {
            Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
            if (player)
            {
                RoomBlob room = Traverse.Create(player).Field("_m_RoutineTargetLocation").GetValue() as RoomBlob;
                player.Teleport(room.position);
            }
            
        }

        public static void Render()
        {
            if (GUILayout.Button("Knock out everyone", new GUILayoutOption[0]))
                KnockoutEveryone();
            if (GUILayout.Button("Set all keys found", new GUILayoutOption[0]))
                SetKeysFound();
            if (GUILayout.Button("Set opinions to max", new GUILayoutOption[0]))
                SetOpinions();


            //Set Collision
            bool _flag = GUILayout.Toggle(_noCollide, "No Collision", new GUILayoutOption[0]);
            if (_flag != _noCollide && PlayerClass.player)
            {
                _noCollide = _flag;
                PlayerClass.player.m_PhysicsSphereCol.enabled = !_flag;
            }

            //Set guards check missing items
            _flag = GUILayout.Toggle(_checkMissingKey, "Guards do not check missing keys", new GUILayoutOption[0]);
            if (_flag != _checkMissingKey)
            {
                _checkMissingKey = _flag;
                if (_flag)
                {
                    var original = AccessTools.Method(typeof(AICharacter_Guard), "MissingKeyCheck");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.NoMissingKeyCheck());
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(AICharacter_Guard), "MissingKeyCheck");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }

            //One hit kills
            _flag = GUILayout.Toggle(_oneHitKill, "One Hit Kills", new GUILayoutOption[0]);
            if (_flag != _oneHitKill)
            {
                _oneHitKill = _flag;
                if (_flag)
                {
                    float dmg = 0f;
                    var original = AccessTools.Method(typeof(Character), "DamageCharacterEvent");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.OneHitKill(null, null, ref dmg));
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(Character), "DamageCharacterEvent");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }

            //Freeze Clock
            _flag = GUILayout.Toggle(_freezeTimer, "Freeze Timer", new GUILayoutOption[0]);
            if (_flag != _freezeTimer)
            {
                _freezeTimer = _flag;
                RoutineManager.GetInstance().SetTimeFrozenRPC(_flag);
            }

            if (GUILayout.Button("Complete Job", new GUILayoutOption[0]))
                Gamer.GetPrimaryGamer().m_PlayerObject?.SetJobComplete(true);

            if (GUILayout.Button("Teleport to current routine", new GUILayoutOption[0]))
                TeleportToRoutine();
        }
    }
}
