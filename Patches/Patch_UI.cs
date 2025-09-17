using HarmonyLib;
using UnityEngine;

namespace Secret_Style_Things.Patches
{
    [HarmonyPatch]
    internal static class Patch_UI
    {
        [HarmonyPatch(typeof(StorageSlotUI), nameof(StorageSlotUI.Awake)), HarmonyPostfix]
        public static void Awake(StorageSlotUI __instance)
        {
            MessageDirector.BundlesListener method = null;
            method = x =>
            {
                if (__instance)
                    __instance.currentlyStoredId = null;
                else
                    GameContext.Instance.MessageDirector.bundlesListeners -= method;
            };
            GameContext.Instance.MessageDirector.bundlesListeners += method;
        }
        
        [HarmonyPatch(typeof(MarketUI), nameof(MarketUI.Start)), HarmonyPostfix]
        public static void Start(MarketUI __instance)
        {
            MessageDirector.BundlesListener method = null;
            method = x =>
            {
                if (__instance)
                {
                    if (__instance.amountMap != null)
                        foreach (var entry in __instance.amountMap)
                            entry.Value.GetComponent<PriceEntry>().itemIcon.sprite = __instance.lookupDir.GetIcon(entry.Key.id); ;
                }
                else
                    GameContext.Instance.MessageDirector.bundlesListeners -= method;
            };
            GameContext.Instance.MessageDirector.bundlesListeners += method;
        }

        [HarmonyPatch(typeof(MapUI), nameof(MapUI.ScaleMarkersOnZoom)), HarmonyPrefix]
        public static void ScaleMarkersOnZoom(MapUI __instance)
        {
            foreach (DisplayOnMap mappableObject in __instance.mappableObjects)
                if (!mappableObject.marker.rect)
                    mappableObject.marker.rect = mappableObject.marker.GetComponent<RectTransform>();
        }
        
    }
}