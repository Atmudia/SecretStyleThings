using System.Collections.Generic;
using System.Reflection;
using DLCPackage;
using Secret_Style_Things.Utils;
using SRML;
using SRML.Config.Attributes;
using UnityEngine;

namespace Secret_Style_Things
{
  public class Main : ModEntryPoint
  {
    internal static bool activate;
    internal static bool autosave = false;
    internal static Assembly execAssembly = Assembly.GetExecutingAssembly();
    internal static AssetBundle assetBundle;
    internal static List<string> change = new List<string>();
    internal static List<SecretStyleTranslation> activesecrets = new List<SecretStyleTranslation>();
    internal static string nameSuffix = " Slime";
    internal static string namePrefix = "";

    public override void PreLoad()
    {
      HarmonyInstance.PatchAll(execAssembly);
      assetBundle =
        AssetBundle.LoadFromStream(execAssembly.GetManifestResourceStream("Secret_Style_Things.secretstylethings"));
    }

    public override void Load()
    {
      foreach (Identifiable.Id id in Identifiable.PLORT_CLASS)
      {
        Sprite secretIconPlort = SlimeUtils.GetSecretIconPlort(id);
        if (secretIconPlort)
          SlimeUtils.SecretStyleData.Add(id, new SecretStyleData(secretIconPlort));
      }

      foreach (Identifiable.Id id in Identifiable.GORDO_CLASS)
      {
        Sprite secretIconGordo = SlimeUtils.GetSecretIconGordo(id);
        if (secretIconGordo)
          SlimeUtils.SecretStyleData.Add(id, new SecretStyleData(secretIconGordo));
      }

      SRSingleton<GameContext>.Instance.DLCDirector.onPackageInstalled += s =>
      {
        if (s != Id.SECRET_STYLE)
          return;
        activate = true;
      };
      SRSingleton<SceneContext>.Instance.SlimeAppearanceDirector.onSlimeAppearanceChanged += (x, y) =>
      {
        if (!Identifiable.SLIME_CLASS.Contains(x.IdentifiableId))
          return;
        string i = x.IdentifiableId.BasicName();
        bool flag = y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE;
        if (!activate || activesecrets.Exists(j => j.basicName == i) == flag)
          return;
        if (flag)
          new SecretStyleTranslation(x.IdentifiableId, y).TryAdd();
        else
          activesecrets.RemoveAll(j => j.basicName == i);
        SlimeUtils.UpdateGordoStyles(x.IdentifiableId);
        SlimeUtils.UpdatePlortStyles(i);
        if (!SRSingleton<GameContext>.Instance ||
            !SRSingleton<GameContext>.Instance.MessageDirector)
          return;
        SRSingleton<GameContext>.Instance.MessageDirector.bundlesListeners(SRSingleton<GameContext>.Instance
          .MessageDirector);
      };
    }

    public override void PostLoad()
    {
      foreach (Identifiable.Id id in Identifiable.SLIME_CLASS)
        change.Add("l." + id.ToString().ToLowerInvariant());
      foreach (Identifiable.Id id in Identifiable.LARGO_CLASS)
        change.Add("l." + id.ToString().ToLowerInvariant());
      foreach (Identifiable.Id id in Identifiable.GORDO_CLASS)
        change.Add("t." + id.ToString().ToLowerInvariant());
      foreach (Identifiable.Id id in Identifiable.PLORT_CLASS)
        change.Add("l." + id.ToString().ToLowerInvariant());
    }

    public class Config
    {
      [ConfigFile("config", "SETTINGS")]
      internal class Values
      {
        public static readonly bool DISABLE_TRANSLATION_SYSTEM;
      }
    }
  }
}
