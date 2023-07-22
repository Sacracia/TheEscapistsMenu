using UnityEngine;

namespace TheEscapists2
{
    internal class Hacks : MonoBehaviour
    {
        private float _lastCacheTime = Time.time + 60f;

        public void Update()
        {
            //PhotonNetwork.offlineMode = true;
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                GeneralMenu.visible = !GeneralMenu.visible;
                PlayerMenu.visible = !PlayerMenu.visible;
                PrisonMenu.visible = !PrisonMenu.visible;
                OutfitMenu.visible = !OutfitMenu.visible;
                WeaponMenu.visible = !WeaponMenu.visible;
            }
        }

        internal static void SendChatMessage(string message, ChatFeedManager.MessageTag tag)
        {
            try
            {
                Gamer gamer = Gamer.GetPrimaryGamer();
                ChatFeedManager chatFeedManager = ChatFeedManager.GetInstance();
                chatFeedManager?.SendChatMessage_RPC(gamer, message, tag, false);
            }
            catch
            {
                
            }
        }
    }
}
