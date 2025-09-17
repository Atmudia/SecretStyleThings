using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MonomiPark.SlimeRancher.DataModel;

namespace Secret_Style_Things.Patches
{
    [HarmonyPatch]
    internal static class Patch_Message
    {
        [HarmonyPatch(typeof(MessageBundle), nameof(MessageBundle.GetResourceString), typeof (string), typeof (bool)), HarmonyPostfix]
        static void GetResourceString(string key, ref string __result)
        {
          if (!Main.activate || Main.Config.Values.DISABLE_TRANSLATION_SYSTEM || string.IsNullOrEmpty(__result) || string.IsNullOrEmpty(key) || Main.activesecrets.Count <= 0 || !Main.change.Contains(key))
                  return;
          string delimiter = " ";
          if (__result.Count(x => x == ' ') < __result.Count(x => x == '-'))
            delimiter = "-";
          List<string> list = __result.Split(new char[2]
          {
            ' ',
            '-'
          }, StringSplitOptions.None).ToList();
          List<int> intList = new List<int>();
          for (int index1 = 0; index1 < Main.activesecrets.Count; ++index1)
          {
            string[] strArray = Main.activesecrets[index1].original.Split(new char[2]
            {
              ' ',
              '-'
            }, StringSplitOptions.None);
            string[] o = Main.activesecrets[index1].replacement.Split(new char[2]
            {
              ' ',
              '-'
            }, StringSplitOptions.None);
            for (int index2 = list.Count - strArray.Length; index2 >= 0; --index2)
            {
              bool flag = false;
              int index3 = 0;
              while (index3 < strArray.Length && !(flag = intList.Contains(index2 + index3) || list[index2 + index3].ToLowerInvariant() != strArray[index3].ToLowerInvariant()))
                ++index3;
              if (!flag)
              {
                string[] collection = o.MatchCase(list.GetRange(index2, strArray.Length).ToArray());
                list.RemoveRange(index2, strArray.Length);
                list.InsertRange(index2, collection);
                for (int index4 = 0; index4 < o.Length; ++index4)
                  intList.Add(index2 + index4);
              }
            }
          }
          __result = list.Join(delimiter: delimiter);
        }

        [HarmonyPatch(typeof(MessageDirector), nameof(MessageDirector.LoadBundle)), HarmonyPostfix] 
        public static void LoadBundle(MessageDirector __instance, string path, ref ResourceBundle __result)
        {
          if (path != "actor")
            return;
          Dictionary<string, string> d = __result.dict;
          string[] source = new string[17]
          {
            Get(Identifiable.Id.BOOM_SLIME),
            Get(Identifiable.Id.CRYSTAL_SLIME),
            Get(Identifiable.Id.DERVISH_SLIME),
            Get(Identifiable.Id.FIRE_SLIME),
            Get(Identifiable.Id.GOLD_SLIME),
            Get(Identifiable.Id.HONEY_SLIME),
            Get(Identifiable.Id.HUNTER_SLIME),
            Get(Identifiable.Id.LUCKY_SLIME),
            Get(Identifiable.Id.MOSAIC_SLIME),
            Get(Identifiable.Id.PHOSPHOR_SLIME),
            Get(Identifiable.Id.PINK_SLIME),
            Get(Identifiable.Id.PUDDLE_SLIME),
            Get(Identifiable.Id.QUANTUM_SLIME),
            Get(Identifiable.Id.RAD_SLIME),
            Get(Identifiable.Id.ROCK_SLIME),
            Get(Identifiable.Id.TABBY_SLIME),
            Get(Identifiable.Id.TANGLE_SLIME)
          };
          int num = source.Min(x => x.Length);
          string e1;
          string c;
          for (c = ""; c.Length < num; c = e1)
          {
            e1 = source[0].Remove(0, source[0].Length - c.Length - 1);
            if (!source.All(x => e1 == x.Remove(0, x.Length - c.Length - 1)))
              break;
          }
          Main.nameSuffix = c;
          string e2;
          for (c = ""; c.Length < num; c = e2)
          {
            e2 = source[0].Remove(c.Length + 1);
            if (!source.All(x => e2 == x.Remove(c.Length + 1)))
              break;
          }
          Main.namePrefix = c;
          Main.activesecrets.Clear();
          foreach (KeyValuePair<Identifiable.Id, SlimeAppearance> selection in SRSingleton<SceneContext>.Instance.SlimeAppearanceDirector.appearanceSelections.selections)
          {
            if (AppearancesModel.ShouldPersistSlimeAppearanceInfo(selection.Key) && selection.Value.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
            {
              new SecretStyleTranslation(selection.Key, Get(selection.Key), d.TryGetValue(selection.Value.NameXlateKey, out var str) ? str : null).TryAdd();
            }
          }

          string Get(Identifiable.Id i)
          {
            return !d.TryGetValue("l." + i.ToString().ToLowerInvariant(), out var str) ? null : str;
          }
      }
    }
}