using UnityEngine;

namespace Secret_Style_Things.Harmony
{
    [HarmonyLib.HarmonyPatch(typeof(MapUI), nameof(MapUI.ScaleMarkersOnZoom))]
    internal static class MapUIPatch
    {
        internal static void Prefix(MapUI __instance)
        {
            foreach (DisplayOnMap mappableObject in __instance.mappableObjects)
                if (!mappableObject.marker.rect)
                    mappableObject.marker.rect = mappableObject.marker.GetComponent<RectTransform>();
        }
        
    }
}