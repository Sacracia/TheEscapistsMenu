using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TheEscapists2
{
    internal class OutfitMenu : MonoBehaviour
    {
        internal static bool visible = true;
        private Rect window = new Rect(10f, 430f, 200f, 150f);

        public void OnGUI()
        {
            if (!visible)
                return;
            window = GUILayout.Window(4, window, OnWindow, "Set outfit", new GUILayoutOption[0]);
        }

        void OnWindow(int windowID)
        {
            DrawElements();
            GUI.DragWindow();
        }

        void DrawElements()
        {
            if (GUILayout.Button("Inmate", new GUILayoutOption[0]))
                GiveOutfit(Item_Outfit.OutFitType.Inmate, CustomisationData.Outfit.NULL);
            if (GUILayout.Button("Guard", new GUILayoutOption[0]))
                GiveOutfit(Item_Outfit.OutFitType.Guard, CustomisationData.Outfit.GUARD_01);
            if (GUILayout.Button("Medic", new GUILayoutOption[0]))
                GiveOutfit(Item_Outfit.OutFitType.Medic, CustomisationData.Outfit.NULL);
            if (GUILayout.Button("Warden", new GUILayoutOption[0]))
                GiveOutfit(Item_Outfit.OutFitType.Warden,CustomisationData.Outfit.RIOTGUARD_01);
            if (GUILayout.Button("Civilian", new GUILayoutOption[0]))
                GiveOutfit(Item_Outfit.OutFitType.None, CustomisationData.Outfit.NULL);
        }

        void GiveOutfit(Item_Outfit.OutFitType type, CustomisationData.Outfit outfit = CustomisationData.Outfit.NULL)
        {
            Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
            ItemManager itemManager = ItemManager.GetInstance();
            if (itemManager == null || player == null)
                return;
            List<ItemData> items = itemManager.GetAllowedList();
            if (player == null || items == null)
                return;
            foreach (ItemData itemData in items)
            {
                if (itemData == null || itemData.m_OutfitData == null || ((int)outfit != -1 && itemData.m_OutfitData.m_OutfitAppearance != outfit) 
                    || ((int)outfit == -1 && itemData.m_OutfitData.m_Type != type)) continue;
                Item newItem = typeof(ItemManager).GetMethod("CreateNewItem_Internal", BindingFlags.NonPublic | BindingFlags.Instance)
                    .Invoke(itemManager, new object[2] { "", 0 }) as Item;
                if (newItem == null)
                    return;
                newItem.m_ItemData = ScriptableObject.CreateInstance<ItemData>();
                newItem.m_ItemData.CopyData(itemData);
                newItem.m_ItemData.SetParentItem(newItem);
                newItem.MeshRendererProp.material = newItem.m_ItemData.m_ItemWorldMaterial;
                player.SetOutFit(newItem, true, false);
                break;
            }
        }
    }
}
