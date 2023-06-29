using Newtonsoft.Json.Linq;
using UnityEngine;

namespace TheEscapists2
{
    internal class PlayerClass : MonoBehaviour
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

        internal static void Render()
        {
            if (GUI.Button(new Rect(20, 20, 100, 20), "Max Strength"))
                Strength = 100f;
            Strength = GUI.HorizontalSlider(new Rect(20, 40, 100, 20), Strength, 0f, 100f);
        }
    }
}
