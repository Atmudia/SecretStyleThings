using HarmonyLib;
using Secret_Style_Things.Utils;
using UnityEngine;

namespace Secret_Style_Things.Patches
{
    [HarmonyPatch(typeof(LookupDirector))]
    internal static class Patch_LookupDirector
    {
        [HarmonyPatch(nameof(LookupDirector.GetIcon)), HarmonyPostfix]
        public static void GetIcon(Identifiable.Id id, ref Sprite __result)
        {
            if (!Main.activate || !Main.activesecrets.Exists(j => j.basicName == id.BasicName()) || !SlimeUtils.SecretStyleData.TryGetValue(id, out var secretStyleData) || !(secretStyleData.sprite != null))
              return;
            __result = secretStyleData.sprite;
        }

        [HarmonyPatch(nameof(LookupDirector.GetColor)), HarmonyPostfix]
        public static void GetColor(Identifiable.Id id, ref Color __result)
        {
            if (!Main.activate || !Identifiable.IsPlort(id) || !Main.activesecrets.Exists(j => j.basicName == id.BasicName()))
                return;
            __result = SlimeUtils.GetSecretColorPlort(id);
        }
    }
}