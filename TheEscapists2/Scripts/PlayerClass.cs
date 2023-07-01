using HarmonyLib;
using UnityEngine;

namespace TheEscapists2
{
    internal class PlayerClass : MonoBehaviour
    {
        static bool _godmode = false;

        internal static int Strength
        {
            get
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return 0;
                return Mathf.RoundToInt(player.m_CharacterStats.Strength);
            }
            set
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return;
                player.m_CharacterStats.Strength = value;
            }
        }

        internal static int Cardio
        {
            get
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return 0;
                return Mathf.RoundToInt(player.m_CharacterStats.Cardio);
            }
            set
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return;
                player.m_CharacterStats.Cardio = value;
            }
        }

        internal static int Intellect
        {
            get
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return 0;
                return Mathf.RoundToInt(player.m_CharacterStats.Intellect);
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
            _godmode = GUILayout.Toggle(_godmode, "Godmode", new GUILayoutOption[0]);
            GUILayout.Label($"Strength {Strength}", new GUILayoutOption[0]);
            Strength = Mathf.RoundToInt(GUILayout.HorizontalSlider(Strength, 0f, CharacterStats.MaxStrength, new GUILayoutOption[0]));
            GUILayout.Label($"Cardio {Cardio}", new GUILayoutOption[0]);
            Cardio = Mathf.RoundToInt(GUILayout.HorizontalSlider(Cardio, 0f, CharacterStats.MaxCardio, new GUILayoutOption[0]));
            GUILayout.Label($"Intellect {Intellect}", new GUILayoutOption[0]);
            Intellect = Mathf.RoundToInt(GUILayout.HorizontalSlider(Intellect, 0f, CharacterStats.MaxIntellect, new GUILayoutOption[0]));
        }

        public static void Update()
        {
            if (_godmode)
            {
                Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
                if (!player)
                    return;
                Gamer.GetPrimaryGamer().m_PlayerObject.m_CharacterStats.Health = CharacterStats.MaxHealth;
            }
        }
    }
}
