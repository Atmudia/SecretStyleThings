using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
//using DebuggingMod;
using DLCPackage;
using HarmonyLib;
using MonomiPark.SlimeRancher.DataModel;
using Secret_Style_Things.Assets;
using Secret_Style_Things.Utils;
using SRML;
using SRML.Config.Attributes;
using SRML.Console;
using SRML.SR;
using SRML.SR.Patches;
using SRML.SR.Templates.Misc;
using SRML.Utils.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Console = SRML.Console.Console;
using Object = UnityEngine.Object;

namespace Secret_Style_Things
{
    public class Main : ModEntryPoint
    {
        internal static readonly List<string> GordoMapList = new(new[]
        {
            "zoneRUINS/cellRuins_Overpass/Sector/Slimes/gordoBoom",
            "zoneQUARRY/cellQuarry_AshIsle/Sector/Slimes/gordoCrystal",
            "zoneDESERT/cellDesert_ScorchedPlainsNorthWest/Sector/Slimes/gordoDervish",
            "zoneMOSS/cellMoss_HoneyPerch/Sector/Slimes/gordoHoney",
            "zoneMOSS/cellMoss_Archipelago/Sector/Slimes/gordoHunter",
            "zoneDESERT/cellDesert_RuinsEntrance/Sector/Slimes/gordoMosaic",
            "zoneREEF/cellReef_SandyPass/Sector/Slimes/gordoPhosphor",
            "zoneREEF/cellReef_GordoIsland/Sector/Slimes/gordoPink",
            "zoneREEF/cellReef_RingIsland/Sector/Slimes/gordoPink",
            "zoneRUINS/cellRuins_BeaconTower/Sector/Slimes/gordoQuantum",
            "zoneQUARRY/cellQuarry_RadGordoCave/Sector/Slimes/gordoRad",
            "zoneQUARRY/cellQuarry_CaveHub/Sector/Slimes/gordoRock",
            "zoneQUARRY/cellQuarry_MirrorIsland/Sector/Slimes/gordoRock",
            "zoneREEF/cellReef_Beach/Sector/Slimes/gordoTabby",
            "zoneREEF/cellReef_BigTree/Sector/Slimes/gordoTabby",
            "zoneQUARRY/cellQuarry_CaveHub/Sector/Slimes/gordoPinkParty",
            "zoneDESERT/cellDesert_SunkenTunnelExit/Sector/Slimes/gordoPinkParty",
            "zoneREEF/cellReef_SandTrap/Sector/Slimes/gordoPinkParty",
            "zoneQUARRY/cellQuarry_AshIsle/Sector/Slimes/gordoPinkParty",
            "zoneWILDS/cellWilds_ForestBluffs/Sector/Slimes/gordoPinkParty",
            "zoneRUINSTransition/cellReef_RuinsGardenOfTranquility/Sector/Slimes/gordoPinkParty",
            "zoneQUARRY/cellQuarry_CrystalVolcano/Sector/Slimes/gordoPinkParty",
            "zoneMOSS/cellMoss_NorthPass/Sector/Slimes/gordoPinkParty",
            "zoneSEA/cellSea_MustacheIsland/Sector/Slimes/gordoPinkParty",
            "zoneRUINSTransition/cellReef_RuinsPassage/Sector/Slimes/gordoPinkParty",
            "zoneDESERT/cellDesert_AshSink/Sector/Slimes/gordoPinkParty",
            "zoneMOSS/cellMoss_Forest/Sector/Slimes/gordoPinkParty",
            "zoneMOSS/cellMoss_Entrance/Sector/Slimes/gordoPinkParty",
            "zoneREEF/cellReef_FeralSink/Sector/Slimes/gordoPinkParty",
            "zoneREEF/cellReef_CliffCaves/Sector/Slimes/gordoPinkParty",
            "zoneRUINS/cellRuins_CollapsedPlazaGarden/Sector/Slimes/gordoPinkParty",
            "zoneREEF/cellReef_RingIsland/Sector/Slimes/gordoPinkParty",
            "zoneQUARRY/cellQuarry_Onsen/Sector/Slimes/gordoPinkParty",
            "zoneRUINS/cellRuins_WaterHall/Sector/Slimes/gordoPinkParty",
            "zoneRUINS/cellRuins_Canal/Sector/Slimes/gordoPinkParty",
            "zoneDESERT/cellDesert_Canyon/Sector/Slimes/gordoPinkParty",
            "zoneMOSS/cellMoss_Coast/Sector/Slimes/gordoPinkParty",
            "zoneDESERT/cellDesert_RuinsEnd/Sector/Slimes/gordoPinkParty",
            "zoneMOSS/cellMoss_FlowerField/Sector/Slimes/gordoPinkParty",
            "zoneSEA/cellSea_SeaSteps/Sector/Slimes/gordoPinkParty",
            "zoneDESERT/cellDesert_Tower/Sector/Slimes/gordoPinkParty",
            "zoneDESERT/cellDesert_EndSteps/Sector/Slimes/gordoPinkParty",
            "zoneRUINS/cellRuins_ArchTower/Sector/Slimes/gordoPinkParty",
            "zoneWILDS/cellWilds_SeaIsles/Sector/Slimes/gordoPinkParty"
        });
        


        internal static bool activate = false;
        internal static bool autosave = false;

        internal static Assembly execAssembly = Assembly.GetExecutingAssembly();
        internal static AssetBundle assetBundle;
        internal static List<string> change = new List<string>();
        internal static List<SecretStyleTranslation> activesecrets = new List<SecretStyleTranslation>();
        static string nameSuffix = " Slime";
        static string namePrefix = "";

        public override void PreLoad()
        {
            HarmonyInstance.PatchAll(execAssembly);
            assetBundle = AssetBundle.LoadFromStream(execAssembly.GetManifestResourceStream("Secret_Style_Things.secretstylethings"));
        }

        public class Config
        {
            [ConfigFile("config", "SETTINGS")] //With this, you are indicating SRML that all the `public static readonly` variables are Configs. The "config" and "SETTINGS" strings can be changed to anything you want.
            internal class Values
            {
                
                public static readonly bool DISABLE_TRANSLATION_SYSTEM  = false; //You created your first config! A config can be pretty much anything. Players can change it to what they want it to be. 

            }
        }

        public override void Load()
        {
            //SlimeAppearance

            foreach (var id in Identifiable.PLORT_CLASS)
            {
                var s = SlimeUtils.GetSecretIconPlort(id);
                if (s)
                    SlimeUtils.SecretStyleData.Add(id, new SecretStyleData(s));
            }
            foreach (var id in Identifiable.GORDO_CLASS)
            {
                var s = SlimeUtils.GetSecretIconGordo(id);
                if (s)
                    SlimeUtils.SecretStyleData.Add(id, new SecretStyleData(s));
            }
            
            SRSingleton<GameContext>.Instance.DLCDirector.onPackageInstalled += s =>
            {
                if (s == Id.SECRET_STYLE)
                    activate = true;
            };
            
            SceneContext.Instance.SlimeAppearanceDirector.onSlimeAppearanceChanged += (x,y) => {
                if (!Identifiable.SLIME_CLASS.Contains(x.IdentifiableId))
                    return; 
                var i = x.IdentifiableId.BasicName();
                bool flag = y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE;
                if (!activate || (activesecrets.Exists((j) => j.basicName == i) == flag))
                    return;
                if (flag)
                {
                    //Patch_Translation.disabled = true;
                    var a = Identifiable.GetName(x.IdentifiableId, false).RemoveBefore(namePrefix).RemoveAfterLast(nameSuffix);
                    var b = GameContext.Instance.MessageDirector.Get("actor", y.NameXlateKey);
                    if (!string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b))
                    {
                        var ind = activesecrets.IndexOf((j) => j.original.Length < a.Length);
                        if (ind < 0)
                            ind = activesecrets.Count;
                        activesecrets.Insert(ind, new SecretStyleTranslation() { basicName = i, original = a, replacement = b });
                    }
                    //Patch_Translation.disabled = false;
                }
                else
                    activesecrets.RemoveAll((j) => j.basicName == i);
                SlimeUtils.UpdateGordoStyles(i);
                SlimeUtils.UpdatePlortStyles(i);
                SlimeUtils.UpdatePuzzleSlotStyles(i);
                if (GameContext.Instance && GameContext.Instance.MessageDirector)
                    GameContext.Instance.MessageDirector.bundlesListeners(GameContext.Instance.MessageDirector);
            };

        }

        static void BundleChange(MessageDirector director)
        {
            //Patch_Translation.disabled = true;
            var a = new string[]
            {
                Identifiable.GetName(Identifiable.Id.BOOM_SLIME),
                Identifiable.GetName(Identifiable.Id.CRYSTAL_SLIME),
                Identifiable.GetName(Identifiable.Id.DERVISH_SLIME),
                Identifiable.GetName(Identifiable.Id.FIRE_SLIME),
                Identifiable.GetName(Identifiable.Id.GOLD_SLIME),
                Identifiable.GetName(Identifiable.Id.HONEY_SLIME),
                Identifiable.GetName(Identifiable.Id.HUNTER_SLIME),
                Identifiable.GetName(Identifiable.Id.LUCKY_SLIME),
                Identifiable.GetName(Identifiable.Id.MOSAIC_SLIME),
                Identifiable.GetName(Identifiable.Id.PHOSPHOR_SLIME),
                Identifiable.GetName(Identifiable.Id.PINK_SLIME),
                Identifiable.GetName(Identifiable.Id.PUDDLE_SLIME),
                Identifiable.GetName(Identifiable.Id.QUANTUM_SLIME),
                Identifiable.GetName(Identifiable.Id.RAD_SLIME),
                Identifiable.GetName(Identifiable.Id.ROCK_SLIME),
                Identifiable.GetName(Identifiable.Id.TABBY_SLIME),
                Identifiable.GetName(Identifiable.Id.TANGLE_SLIME)
            };
            //Patch_Translation.disabled = false;
            var m = a.Min((x) => x.Length);
            var c = "";
            while (c.Length < m)
            {
                var e = a[0].Remove(0, a[0].Length - c.Length - 1);
                if (a.All((x) => e == x.Remove(0, x.Length - c.Length - 1)))
                    c = e;
                else
                    break;
            }

            nameSuffix = c;
            c = "";
            while (c.Length < m)
            {
                var e = a[0].Remove(c.Length + 1);
                if (a.All((x) => e == x.Remove(c.Length + 1)))
                    c = e;
                else
                    break;
            }

            namePrefix = c;
        }

        public override void PostLoad()
        {
            GameContext.Instance.MessageDirector.RegisterBundlesListener(BundleChange);
            foreach (var i in Identifiable.SLIME_CLASS)
                change.Add($"l.{i.ToString().ToLowerInvariant()}");
            foreach (var i in Identifiable.LARGO_CLASS)
                change.Add($"l.{i.ToString().ToLowerInvariant()}");
            foreach (var i in Identifiable.GORDO_CLASS)
                change.Add($"t.{i.ToString().ToLowerInvariant()}");
            foreach (var i in Identifiable.PLORT_CLASS)
                change.Add($"l.{i.ToString().ToLowerInvariant()}");
        }

        [HarmonyPatch(typeof(MessageBundle), "GetResourceString", typeof(string), typeof(bool))]
        class Patch_Translation
        {
            static void Postfix(string key,ref string __result)
            {
                if (!activate || Config.Values.DISABLE_TRANSLATION_SYSTEM || string.IsNullOrEmpty(__result) || string.IsNullOrEmpty(key))
                    return;
                if (activesecrets.Count > 0 && change.Contains(key))
                {
                    var parts1 = new List<string> { __result };
                    for (int l = 0; l < activesecrets.Count; l++)
                    {
                        for (int i = parts1.Count - 1; i >= 0; i-=2)
                        {
                            var split = parts1[i].Split(new string[] { activesecrets[l].original },StringSplitOptions.None).ToList();
                            for (int j = split.Count - 1; j > 0; j--)
                                split.Insert(j, activesecrets[l].replacement);
                            if (split.Count > 1)
                            {
                                parts1.RemoveAt(i);
                                parts1.InsertRange(i, split);
                            }
                        }
                    }
                    __result = parts1.Join(null, "");
                }
            }
        }

        [HarmonyPatch(typeof(LookupDirector), "GetIcon")]
        class Patch_LookupIcon
        {
            static void Postfix(Identifiable.Id id, ref Sprite __result)
            {
                if (activate && activesecrets.Exists((j) => j.basicName == id.BasicName()) && SlimeUtils.SecretStyleData.TryGetValue(id, out var data) && data.sprite != null)
                    __result = data.sprite;
            }
        }

        [HarmonyPatch(typeof(LookupDirector), "GetColor")]
        class Patch_LookupColor
        {
            static void Postfix(Identifiable.Id id, ref Color __result)
            {
                if (activate && Identifiable.IsPlort(id) && activesecrets.Exists((j) => j.basicName == id.BasicName()))
                    __result = SlimeUtils.GetSecretColorPlort(id);
            }
        }
    }

    internal class SecretStyleTranslation
    {
        public string basicName;
        public string original;
        public string replacement;
    }
}