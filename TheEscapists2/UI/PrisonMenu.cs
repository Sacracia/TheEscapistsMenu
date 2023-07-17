using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace TheEscapists2
{
    internal class PrisonMenu : MonoBehaviour
    {
        internal static bool visible = true;
        private bool _detectorActive = false;
        private bool _checkMissingKey = false;
        private bool _freezeTimer = false;
        private bool _speedUpTime = false;
        private Rect window = new Rect(270f, 10f, 250f, 400f);

        public void OnGUI()
        {
            if (!visible)
                return;
            window = GUILayout.Window(1, window, OnWindow, "Prisons", new GUILayoutOption[0]);
        }

        void OnWindow(int windowID)
        {
            DrawElements();
            GUI.DragWindow();
        }

        private void DrawElements()
        {
            if (GUILayout.Button("Knock out everyone", new GUILayoutOption[0]))
                KnockoutEveryone();
            if (GUILayout.Button("Set opinions to max", new GUILayoutOption[0]))
                SetOpinions();
            if (GUILayout.Button("Unlock doors for player", new GUILayoutOption[0]) && PlayerMenu.player)
                UnlockDoors();
            if (GUILayout.Button("Teleport to current routine", new GUILayoutOption[0]))
                TeleportToRoutine();
            if (GUILayout.Button("Complete Job", new GUILayoutOption[0]))
                Gamer.GetPrimaryGamer().m_PlayerObject?.SetJobComplete(true);
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-", new GUILayoutOption[0]))
                PrisonAlertnessManager.GetInstance()?.DecrementAlertnessBy(1);
            if (GUILayout.Button("+", new GUILayoutOption[0]))
                PrisonAlertnessManager.GetInstance()?.IncrementAlertnessBy(1, PrisonAlertnessManager.AlertnessReason.Escaping);
            GUILayout.Label("Alertness", new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Turn off generators", new GUILayoutOption[0]))
                TurnPowerOff();
            
            //Turn off detectors
            bool _flag = GUILayout.Toggle(_detectorActive, "Detectors do not work", new GUILayoutOption[0]);
            if (_flag != _detectorActive)
            {
                _detectorActive = _flag;
                if (_flag)
                {
                    var original = AccessTools.Method(typeof(ContrabandDetector), "DetectContraband");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.DisableContrabandDetectors());
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(ContrabandDetector), "DetectContraband");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }
            
            if (GUILayout.Button("Escape", new GUILayoutOption[0]))
                EscapePrisonFunctionality.GetInstance().TriggerEscape();
            if (GUILayout.Button("Free Craft", new GUILayoutOption[0]))
            {
                CraftManager craftManager = CraftManager.GetInstance();
                if (craftManager != null)
                {
                    List<CraftManager.Recipe> currentRecipes = craftManager.GetCurrentRecipes();
                    for (int num2 = 0; num2 < currentRecipes.Count; num2++)
                        currentRecipes.ElementAt(num2).m_Ingredients = new ItemData[3];
                }
            }
            
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
            
            _flag = GUILayout.Toggle(_freezeTimer, "Freeze Timer", new GUILayoutOption[0]);
            if (_flag != _freezeTimer)
            {
                _freezeTimer = _flag;
                RoutineManager.GetInstance().SetTimeFrozenRPC(_flag);
            }

            _flag = GUILayout.Toggle(_speedUpTime, "Speed up time", new GUILayoutOption[0]);
            if (_flag != _speedUpTime)
            {
                _speedUpTime = _flag;
                if (_flag)
                {
                    bool res = false;
                    var original = AccessTools.Method(typeof(RoutineManager), "ShouldSpeedUpForSleeping");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.SpeedUpTime(ref res));
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(RoutineManager), "ShouldSpeedUpForSleeping");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }

            if (GUILayout.Button("Calm dogs", new GUILayoutOption[0]))
                DogsForgetEverything();
            if (GUILayout.Button("Apply for a job >>", new GUILayoutOption[0]))
            {
                JobMenu.visible = !JobMenu.visible;
                JobMenu.window.x = window.x + 260f;
                JobMenu.window.y = window.y + 310f;
            }
        }

        private void KnockoutEveryone()
        {
            if (!PlayerMenu.player)
                return;
            T17NetManager.IsMasterClient = true;
            List<Character> characters = Character.GetAllCharacters();
            foreach (Character character in characters)
            {
                if (character != PlayerMenu.player && (character.m_CharacterRole == CharacterRole.Inmate || character.m_CharacterRole == CharacterRole.Guard
                    || character.m_CharacterRole == CharacterRole.Warden || character.m_CharacterRole == CharacterRole.Dog))
                    character.SetIsKnockedOut(true, PlayerMenu.player);
            }
        }

        private void SetOpinions()
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

        private void TeleportToRoutine()
        {
            Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
            if (player)
            {
                RoomBlob room = Traverse.Create(player).Field("_m_RoutineTargetLocation").GetValue() as RoomBlob;
                player.Teleport(room.position);
            }
        }

        private void TurnPowerOff()
        {
            foreach (var gen in PrisonPowerManager.GetInstance().m_Generators)
                gen?.m_Generator.DisableGenerator();
        }
    
        private void UnlockDoors()
        {
            if (PlayerMenu.player == null)
                return;
            DoorManager doorManager = DoorManager.GetInstance();
            if (doorManager == null)
                return;
            List<Door> doors = typeof(DoorManager).GetField("m_AllDoors", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(doorManager)
                as List<Door>;
            foreach (Door door in doors)
            {
                PlayerMenu.player.AddAllowedDoor(door, null);
                door.SetForceOpen(true);
                DoorManager.GetInstance().SetUpCharacterKeys(PlayerMenu.player);
            }
        }

        private void DogsForgetEverything()
        {
            if (PlayerMenu.player == null)
                return;
            foreach (AICharacter character in NPCManager.GetInstance().m_Doggies)
                character?.ForgetEverything();
        }
    }
}
