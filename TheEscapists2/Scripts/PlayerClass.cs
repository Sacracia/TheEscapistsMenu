using HarmonyLib;
using UnityEngine;

namespace TheEscapists2
{
    internal class PlayerClass
    {

        internal static float Strength
        {
            get
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return 0f;
                return player.m_CharacterStats.Strength;
            }
            set
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return;
                player.m_CharacterStats.Strength = value;
            }
        }

        internal static float Cardio
        {
            get
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return 0f;
                return player.m_CharacterStats.Cardio;
            }
            set
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return;
                player.m_CharacterStats.Cardio = value;
            }
        }

        internal static float Intellect
        {
            get
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return 0f;
                return player.m_CharacterStats.Intellect;
            }
            set
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return;
                player.m_CharacterStats.Intellect = value;
            }
        }

        internal static void Render()
        {
            if (GUI.Button(new Rect(20, 20, 100, 20), "Max Strength"))
                Strength = CharacterStats.MaxStrength;
            Strength = GUI.HorizontalSlider(new Rect(20, 50, 50, 20), Strength, 0f, CharacterStats.MaxStrength);
            Cardio = GUI.HorizontalSlider(new Rect(20, 80, 50, 20), Cardio, 0f, CharacterStats.MaxCardio);
            Intellect = GUI.HorizontalSlider(new Rect(20, 110, 50, 20), Intellect, 0f, CharacterStats.MaxIntellect);
        }

        [HarmonyPatch(typeof(CharacterStats), "DecreaseEnergyRPC")]
        public class Patch1
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __result)
            {
                __result = true;
                return false;
            }
        }
    }
}
