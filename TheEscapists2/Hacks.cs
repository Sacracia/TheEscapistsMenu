using System.Reflection;
using UnityEngine;

namespace TheEscapists2
{
    internal class Hacks : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                GeneralMenu.visible = !GeneralMenu.visible;
                PlayerMenu.visible = !PlayerMenu.visible;
                PrisonMenu.visible = !PrisonMenu.visible;
                OutfitMenu.visible = !OutfitMenu.visible;
                WeaponMenu.visible = !WeaponMenu.visible;
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                Loader.Unload();
            }
        }

        internal static bool SendChatMessage(string message)
        {
            try
            {
                var chat = ChatFeedManager.GetInstance();
                if (chat == null)
                    return false;
                chat.GetType().GetMethod("SendMessageToHUD", BindingFlags.NonPublic | BindingFlags.Instance)
                    .Invoke(ChatFeedManager.GetInstance(), new object[1] { message });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
