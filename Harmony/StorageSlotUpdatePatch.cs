using HarmonyLib;

namespace Secret_Style_Things.Harmony
{
    [HarmonyPatch(typeof(StorageSlotUI), "Awake")]
    class Patch_StorageSlot
    {
        static void Postfix(StorageSlotUI __instance)
        {
            MessageDirector.BundlesListener method = null;
            method = (x) =>
            {
                if (__instance)
                    __instance.currentlyStoredId = null;
                else
                    GameContext.Instance.MessageDirector.bundlesListeners -= method;
            };
            GameContext.Instance.MessageDirector.bundlesListeners += method;
        }
    }

    [HarmonyPatch(typeof(MarketUI), "Start")]
    class Patch_Market
    {
        static void Postfix(MarketUI __instance)
        {
            MessageDirector.BundlesListener method = null;
            method = (x) =>
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
    }
}