using System;
using System.Collections.Generic;
using System.Linq;
using Secret_Style_Things.Assets;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Console = SRML.Console.Console;

namespace Secret_Style_Things.Utils
{
    public class SlimeUtils
    {
        private static readonly int Texcoord = Shader.PropertyToID("_texcoord");
        private static readonly int Texcoord2 = Shader.PropertyToID("_texcoord2");
        private static Material TabbyResize;
        private static Mesh HunterResize;
        private static Mesh HoneyRepos;
        private static Material DervishGordoResize;
        private static Material DervishPlortResize;
        internal static Sprite GetSecretIconGordo(Identifiable.Id gordo)
        {
            try
            {
                return Main.assetBundle.LoadAsset<Sprite>($"iconGordo{gordo.ToString().Replace("_GORDO", "").ToTitle()}Exotic");
            } catch
            {
                return null;
            }
        }
        internal static Sprite GetSecretIconPlort(Identifiable.Id plort)
        {
            string plortStrings = plort.ToString().Replace("_PLORT", "").ToLower().ToTitle();
            string plortstrings = $"iconPlort{plortStrings}Exotic";
            try
            {
                return Main.assetBundle.LoadAsset<Sprite>(plortstrings);
            }
            catch
            {
                return null;
            }
        }
        internal static Color GetSecretColorPlort(Identifiable.Id plort)
        {
            Identifiable.Id o = (Identifiable.Id)Enum.Parse(typeof(Identifiable.Id),
                plort.ToString().Replace("_PLORT", "_SLIME"));
            return AmmoSlotUI.Instance.GetCurrentColor(o);
        }
        

        public static Dictionary<Identifiable.Id, SecretStyleData> SecretStyleData = new Dictionary<Identifiable.Id, SecretStyleData>();
        public static Dictionary<Identifiable.Id, Action<Transform, SlimeAppearance>> ExtraApperanceApplicators = new Dictionary<Identifiable.Id, Action<Transform, SlimeAppearance>>
        {
            [Identifiable.Id.ROCK_GORDO] = (x,y) => x.Find("Vibrating/rocks_spine_LOD0").GetComponent<SkinnedMeshRenderer>().sharedMaterial = y.Structures[1].DefaultMaterials[0],
            [Identifiable.Id.TABBY_GORDO] = (x,y) => {
                GameObject earsNTailLOD0GameObject = x.Find("Vibrating/ears_n_tail_LOD0").gameObject;
                earsNTailLOD0GameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE ? GordoAssets.TabbyDlcEarsTailLOD0 : GordoAssets.EarsNTailLOD0;
                earsNTailLOD0GameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial = y.Structures[1].DefaultMaterials[0];
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    if (!TabbyResize)
                    {
                        TabbyResize = Object.Instantiate(y.Structures[0].DefaultMaterials[0]);
                        TabbyResize.SetTextureScale(Texcoord, new Vector2(0.5f, 0.5f));
                        TabbyResize.SetTextureScale(Texcoord2, new Vector2(0.5f, 0.5f));
                        TabbyResize.SetTextureOffset(Texcoord, new Vector2(0.25f, 0));
                        TabbyResize.SetTextureOffset(Texcoord2, new Vector2(0.25f, 0));
                    }
                    x.Find("Vibrating/slime_gordo").GetComponent<SkinnedMeshRenderer>().sharedMaterial = TabbyResize;
                }
            },
            [Identifiable.Id.PHOSPHOR_GORDO] = (x,y) =>
            {
                x.Find("Vibrating/bone_root/bone_slime/bone_wing_right").GetComponent<MeshFilter>().sharedMesh = y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE ? GordoAssets.PhosphorDlcWingR : GordoAssets.WingR;
                x.Find("Vibrating/bone_root/bone_slime/bone_wing_left").GetComponent<MeshFilter>().sharedMesh = y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE ? GordoAssets.PhosphorDlcWingL : GordoAssets.WingL;
                x.Find("Vibrating/bone_root/bone_slime/bone_wing_right").GetComponent<MeshRenderer>().sharedMaterial = y.Structures[2].DefaultMaterials[0];
                x.Find("Vibrating/bone_root/bone_slime/bone_wing_left").GetComponent<MeshRenderer>().sharedMaterial = y.Structures[2].DefaultMaterials[0];
                x.Find("Vibrating/ShineRotator/Shine").GetComponent<MeshRenderer>().sharedMaterial = y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE ? PlortAssets.FXPhosphorExotic : PlortAssets.FXPhosphor;
                var antenLOD0 = x.Find("Vibrating/anten_LOD0").GetComponent<SkinnedMeshRenderer>();
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    antenLOD0.enabled = false;
                    var halo = new GameObject("phosphor_exotic_halo",typeof(MeshFilter),typeof(MeshRenderer)).transform;
                    halo.SetParent(x.Find("Vibrating/bone_root/bone_slime"),false);
                    halo.localPosition = new Vector3(0, 1.1f, 0);
                    halo.GetComponent<MeshFilter>().sharedMesh = GordoAssets.PhosphorDlcHaloLOD0;
                    halo.GetComponent<MeshRenderer>().sharedMaterial = y.Structures[1].DefaultMaterials[0];
                } else
                {
                    antenLOD0.enabled = true;
                    if (x.Find("Vibrating/bone_root/bone_slime/phosphor_exotic_halo"))
                        Object.DestroyImmediate(x.Find("Vibrating/bone_root/bone_slime/phosphor_exotic_halo").gameObject);
                }
            },
            [Identifiable.Id.RAD_GORDO] = (x,y) => x.Find("Radiation").GetComponent<MeshRenderer>().sharedMaterial = y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE ? GordoAssets.SlimeRadAuraExotic : GordoAssets.SlimeRadAura,
            [Identifiable.Id.HUNTER_GORDO] = (x, y) => {
                var earsNTailLOD0 = x.Find("Vibrating/ears_n_tail_LOD0").GetComponent<SkinnedMeshRenderer>();
                earsNTailLOD0.sharedMaterial = y.Structures[1].DefaultMaterials[0];
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    if (!HunterResize)
                    {
                        HunterResize = Object.Instantiate(GordoAssets.HunterDLCEarsTail);
                        var v = HunterResize.vertices;
                        for (int i = 0; i < v.Length; i++)
                            if (v[i].y < 0.5)
                            {
                                v[i] *= 1.3f;
                                v[i] += new Vector3(0,0.05f,-0.05f);
                            }
                        HunterResize.vertices = v;
                    }
                    earsNTailLOD0.sharedMesh = HunterResize;
                }
                else
                    earsNTailLOD0.sharedMesh = GordoAssets.HunterEarsTail;
                x.GetComponent<GordoStealth>().materialStealthController = new MaterialStealthController(x.gameObject);
            },
            [Identifiable.Id.HONEY_GORDO] = (x,y) =>
            {
                var honeyplateLOD0 = x.Find("Vibrating/honneyplate_LOD0").GetComponent<SkinnedMeshRenderer>();
                honeyplateLOD0.sharedMaterial = y.Structures[1].DefaultMaterials[0];
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    if (!HoneyRepos)
                    {
                        HoneyRepos = Object.Instantiate(GordoAssets.HoneyExoticCrownLOD0);
                        var v = HoneyRepos.vertices;
                        for (int i = 0; i < v.Length; i++)
                            v[i] += new Vector3(0, -0.95f, 0.06f);
                        HoneyRepos.vertices = v;
                    }
                    honeyplateLOD0.sharedMesh = HoneyRepos;
                }
                else
                    honeyplateLOD0.sharedMesh = GordoAssets.HoneyCombLOD0;
            },
            [Identifiable.Id.CRYSTAL_GORDO] = (x, y) => x.Find("Vibrating/rocks_spine_LOD0").GetComponent<SkinnedMeshRenderer>().sharedMaterial = y.Structures[1].DefaultMaterials[0],
            [Identifiable.Id.DERVISH_GORDO] = (x,y) =>
            {
                var ring = x.Find("Vibrating/bone_root/bone_slime/bone_core/dervishRing/dervishRing_LOD0").GetComponent<Renderer>();
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    var r = x.Find("Vibrating/slime_gordo").GetComponent<Renderer>();
                    if (!DervishGordoResize)
                    {
                        DervishGordoResize = Object.Instantiate(r.sharedMaterial);
                        DervishGordoResize.name = "gordoDervishExotic";
                        DervishGordoResize.SetFloatArray("_GalaxyLargoScale", new float[] { 2, 10 });
                    }
                    r.sharedMaterial = DervishGordoResize;
                    ring.sharedMaterial = DervishGordoResize;
                } else
                    ring.sharedMaterial = y.Structures[1].DefaultMaterials[0];
            },
            [Identifiable.Id.MOSAIC_GORDO] = (x,y) =>
            {
                var mosaicgordo = x.Find("Vibrating/mosaicGordo_LOD0").GetComponent<SkinnedMeshRenderer>();
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    mosaicgordo.enabled = false;
                    var shards = new GameObject("mosaic_exotic_glass", typeof(MeshFilter), typeof(MeshRenderer)).transform;
                    shards.SetParent(x.Find("Vibrating/bone_root/bone_slime"), false);
                    shards.GetComponent<MeshFilter>().sharedMesh = GordoAssets.MosaicDlcShardsLOD0;
                    shards.GetComponent<MeshRenderer>().sharedMaterial = y.Structures[1].DefaultMaterials[0];
                }
                else
                {
                    mosaicgordo.enabled = true;
                    if (x.Find("Vibrating/bone_root/bone_slime/mosaic_exotic_glass"))
                        Object.DestroyImmediate(x.Find("Vibrating/bone_root/bone_slime/mosaic_exotic_glass").gameObject);
                }
            },
            [Identifiable.Id.TANGLE_GORDO] = (x,y) =>
            {
                var root = x.Find("Vibrating/bone_root");
                var flower = root.Find("bone_slime/bone_core/bone_jiggle_top/bone_skin_top/Flower").GetComponent<MeshRenderer>();
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    flower.enabled = false;
                    var flowerExotic = new GameObject("FlowerExotic", typeof(MeshRenderer),typeof(MeshFilter)).transform;
                    flowerExotic.SetParent(root, false);
                    flowerExotic.GetComponent<MeshRenderer>().sharedMaterial = GordoAssets.TangleFlowerMaterialExotic;
                    flowerExotic.GetComponent<MeshFilter>().sharedMesh = GordoAssets.TangleFlowerMeshExotic;
                }
                else
                {
                    flower.enabled  = true;
                    if (root.Find("FlowerExotic"))
                        Object.DestroyImmediate(root.Find("FlowerExotic").gameObject);
                }
            },
           
            [Identifiable.Id.ROCK_PLORT] = (x,y) => x.Find("rocks").GetComponent<MeshRenderer>().sharedMaterial = y.Structures[1].DefaultMaterials[0],
            [Identifiable.Id.PHOSPHOR_PLORT] = (x,y) =>
            {
                var core = x.Find("ShineRotator/Core").GetComponent<MeshRenderer>();
                var shine = x.Find("ShineRotator/Shine").GetComponent<MeshRenderer>();
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    core.sharedMaterial = PlortAssets.FXPhosphorExotic;
                    shine.sharedMaterial = PlortAssets.FXPhosphorExotic;
                }
                else
                {
                    core.sharedMaterial = PlortAssets.FXPhosphor;
                    shine.sharedMaterial = PlortAssets.FXPhosphor;
                }
            },
            [Identifiable.Id.CRYSTAL_PLORT] = (x, y) => x.Find("rocks").GetComponent<MeshRenderer>().sharedMaterial = y.Structures[2].DefaultMaterials[0],
            [Identifiable.Id.MOSAIC_PLORT] = (x,y) =>
            {
                var mosaicShell = x.Find("MosaicShell");
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    mosaicShell.GetComponent<MeshFilter>().mesh = PlortAssets.MosaicDlcShardsLOD0;
                    mosaicShell.GetComponent<MeshRenderer>().material = y.Structures[1].DefaultMaterials[0];
                    mosaicShell.localScale = new Vector3(2f, 2f, 2f);
                    Vector3 localPosition = mosaicShell.transform.localPosition;
                    localPosition.y = -0.975061f;
                    mosaicShell.localPosition = localPosition;
                } else
                {
                    mosaicShell.GetComponent<MeshFilter>().mesh = PlortAssets.NormalPlort;
                    mosaicShell.GetComponent<MeshRenderer>().material = PlortAssets.SlimeMosaicPlort;
                    mosaicShell.localScale = new Vector3(1.2051f, 1.2051f, 1.2051f);
                    Vector3 localPosition = mosaicShell.transform.localPosition;
                    localPosition.y = 0.0001f;
                    mosaicShell.localPosition = localPosition;
                }
            },
            [Identifiable.Id.DERVISH_PLORT] = (x, y) =>
            {
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    var r = x.GetComponent<Renderer>();
                    if (!DervishPlortResize)
                    {
                        DervishPlortResize = Object.Instantiate(r.sharedMaterial);
                        DervishPlortResize.name = "plortDervishExotic";
                        DervishPlortResize.SetFloatArray("_GalaxyLargoScale", new[] { 1, 0.2f });
                    }
                    r.sharedMaterial = DervishPlortResize;
                }
            },
            [Identifiable.Id.FIRE_PLORT] = (x,y) => x.Find("FireQuad").GetComponent<MeshRenderer>().sharedMaterial = y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE ? PlortAssets.FXFireExotic : PlortAssets.FXFire,
            [Identifiable.Id.TANGLE_PLORT] = (x, y) =>
            {
                var flower = x.Find("tangleFlower_LOD1").GetComponent<MeshRenderer>();
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    flower.enabled = false;
                    var flowerExotic = new GameObject("FlowerExotic", typeof(MeshRenderer), typeof(MeshFilter)).transform;
                    flowerExotic.SetParent(x, false);
                    flowerExotic.GetComponent<MeshRenderer>().sharedMaterial = GordoAssets.TangleFlowerMaterialExotic;
                    flowerExotic.GetComponent<MeshFilter>().sharedMesh = GordoAssets.TangleFlowerMeshExotic;
                    flowerExotic.localPosition = new Vector3(0,-1.6f,-0.45f);
                    flowerExotic.localScale = Vector3.one * 2f;
                    flowerExotic.localRotation = Quaternion.Euler(Vector3.right * 30);
                }
                else
                {
                    flower.enabled = true;
                    if (x.Find("FlowerExotic"))
                        Object.DestroyImmediate(x.Find("FlowerExotic").gameObject);
                }
            },
            [Identifiable.Id.PUDDLE_PLORT] = (x, y) =>
            {
                if (y.SaveSet == SlimeAppearance.AppearanceSaveSet.SECRET_STYLE)
                {
                    var flowerExotic = new GameObject("FlowerExotic", typeof(MeshRenderer), typeof(MeshFilter)).transform;
                    flowerExotic.SetParent(x, false);
                    flowerExotic.GetComponent<MeshRenderer>().sharedMaterial = y.Structures[1].DefaultMaterials[0]; ;
                    flowerExotic.GetComponent<MeshFilter>().sharedMesh = PlortAssets.PuddleDlcFlowerLOD0; ;
                    flowerExotic.localScale = new Vector3(3f, 3f, 3f);
                    Vector3 localPosition = flowerExotic.transform.localPosition;
                    localPosition.y = -2.4037f;
                    flowerExotic.transform.localPosition = localPosition;
                }
                else
                {
                    if (x.Find("FlowerExotic"))
                        Object.DestroyImmediate(x.Find("FlowerExotic").gameObject);

                }
            },
            


        };
        
        static Dictionary<Identifiable.Id, Sprite> originalGordoSprites = new Dictionary<Identifiable.Id, Sprite>();
        public static Dictionary<Identifiable.Id, Identifiable.Id> GordoToSlime = new Dictionary<Identifiable.Id, Identifiable.Id>()
        {
            [Identifiable.Id.PARTY_GORDO] = Identifiable.Id.PINK_SLIME
        };
        internal static Identifiable.Id GetSlimeForGordo(Identifiable.Id gordoId)
        {
            if (GordoToSlime.TryGetValue(gordoId, out var slimeId) && slimeId != Identifiable.Id.NONE)
                return slimeId;
            var basic = gordoId.BasicName();
            return GordoToSlime[gordoId] = Identifiable.SLIME_CLASS.FirstOrDefault(x => x.BasicName() == basic);
        }
        internal static void UpdateGordoStyles(Identifiable.Id slimeId)
        {
            if (slimeId == Identifiable.Id.NONE)
                return;
            var basicName = slimeId.BasicName();
            bool flag = Main.activesecrets.Exists((j) => j.basicName == basicName);
            var appearance = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(slimeId).GetAppearanceForSet(flag ? SlimeAppearance.AppearanceSaveSet.SECRET_STYLE : SlimeAppearance.AppearanceSaveSet.CLASSIC);
            foreach (var gordo in Resources.FindObjectsOfTypeAll<GordoIdentifiable>())
    
                if (GetSlimeForGordo(gordo.id) == slimeId && SecretStyleData.TryGetValue(gordo.id,out var data))
                {
                    var display = gordo.GetComponent<GordoDisplayOnMap>();
                    if (display)
                    {
                        bool flag2 = originalGordoSprites.TryGetValue(gordo.id, out var s);
                        if (flag && data.sprite)
                            s = data.sprite;
                        if (!flag2 && display.markerPrefab)
                        {
                            originalGordoSprites[gordo.id] = display.markerPrefab.GetComponent<Image>().sprite;
                        }
                        if (s)
                        {
                            if (display.markerPrefab)
                            {
                                display.markerPrefab.GetComponent<Animator>().enabled = false;
                                display.markerPrefab.GetComponent<Image>().sprite = s;
                            }
                            if (display.marker)
                            {
                                display.marker.GetComponent<Animator>().enabled = false;
                                display.marker.GetComponent<Image>().sprite = s;
                            }
                            foreach (var c in display.GetComponentsInChildren<MapMarker>(true))
                            {
                                c.GetComponent<Animator>().enabled = false;
                                c.GetComponent<Image>().sprite = s;
                            }
                        }
                    }
                    var _face = gordo.GetComponent<GordoFaceComponents>();
                    var _slimeGordo = gordo.transform.Find("Vibrating/slime_gordo").GetComponent<SkinnedMeshRenderer>();
                    if (_slimeGordo.sharedMaterial != appearance.Structures[0].DefaultMaterials[0])
                    {
                        _face.blinkEyes = appearance.Face.GetExpressionFace(SlimeFace.SlimeExpression.Blink).Eyes;
                        _face.strainEyes = appearance.Face.GetExpressionFace(SlimeFace.SlimeExpression.Scared).Eyes;
                        _face.chompOpenMouth = appearance.Face.GetExpressionFace(SlimeFace.SlimeExpression.ChompOpen).Mouth;
                        _face.happyMouth = appearance.Face.GetExpressionFace(SlimeFace.SlimeExpression.Happy).Mouth;
                        _face.strainMouth = appearance.Face.GetExpressionFace(SlimeFace.SlimeExpression.ChompClosed).Mouth;
                        _slimeGordo.sharedMaterial = appearance.Structures[0].DefaultMaterials[0];

                        if (ExtraApperanceApplicators.TryGetValue(gordo.id, out var f))
                            try
                            {
                                f(gordo.transform, appearance);
                            }
                            catch (Exception e)
                            {
                                Console.LogError($"An error occured while applying extra appearance applicator for {(gordo ? gordo : "null")} (Gordo Type = {gordo.id})\n{e}");
                            }
                        var animator = gordo.GetComponent<GordoFaceAnimator>();
                        if (animator.HAPPY != null)
                        {
                            if (animator.gameObject.activeInHierarchy)
                                animator.Awake();
                            if (animator.comps)
                            {
                                animator.InitStates();
                                animator.SetDefaultState();
                            }
                        }
                    }
                }
            
        }

        static Dictionary<Identifiable.Id, Material> originalPlortMaterials = new Dictionary<Identifiable.Id, Material>();

        internal static void UpdatePuzzleSlotStyles(string basicName)
        {
            var plortId = Identifiable.PLORT_CLASS.FirstOrDefault(x => x.BasicName() == basicName);
            var slimeId = Identifiable.SLIME_CLASS.FirstOrDefault(x => x.BasicName() == basicName);
            if (plortId == Identifiable.Id.NONE || slimeId == Identifiable.Id.NONE)
                return;
            bool flag2 = originalPlortMaterials.TryGetValue(plortId, out var m);
            bool flag = Main.activesecrets.Exists(j => j.basicName == basicName);
            var appearance = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(slimeId).GetAppearanceForSet(flag ? SlimeAppearance.AppearanceSaveSet.SECRET_STYLE : SlimeAppearance.AppearanceSaveSet.CLASSIC);
            if (flag)
                m = appearance.Structures[0].DefaultMaterials[0];
            foreach (var item in Resources.FindObjectsOfTypeAll<PuzzleSlot>())
            {
                if (item.catchId != plortId)
                    continue;
                var plort = item.transform.Find("Activate Toggle/plort").gameObject;
                var meshRenderer = plort.GetComponent<MeshRenderer>();
                if (!flag2)
                {
                    originalPlortMaterials.Add(plortId, meshRenderer.material);
                    flag2 = true;
                }

                if (m && meshRenderer.sharedMaterial != m)
                {
                    meshRenderer.sharedMaterial = m;
                    if (ExtraApperanceApplicators.TryGetValue(plortId, out var f))
                        try
                        {
                            f(plort.transform, appearance);

                        }
                        catch (Exception e)
                        {
                            Console.LogError($"An error occured while applying extra appearance applicator for {(item ? item : "null")} (PuzzleSlot Catch Id Type = {plortId})\n{e}");
                            throw;
                        }
                }
              

            }
        }
        internal static void UpdatePlortStyles(string basicName)
        {
            var plortId = Identifiable.PLORT_CLASS.FirstOrDefault(x => x.BasicName() == basicName);
            var slimeId = Identifiable.SLIME_CLASS.FirstOrDefault(x => x.BasicName() == basicName);
            if (plortId == Identifiable.Id.NONE || slimeId == Identifiable.Id.NONE)
                return;
            bool flag2 = originalPlortMaterials.TryGetValue(plortId, out var m);
            bool flag = Main.activesecrets.Exists(j => j.basicName == basicName);
            var appearance = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(slimeId).GetAppearanceForSet(flag ? SlimeAppearance.AppearanceSaveSet.SECRET_STYLE : SlimeAppearance.AppearanceSaveSet.CLASSIC);
            if (flag)
                m = appearance.Structures[0].DefaultMaterials[0];
            foreach (var item in Resources.FindObjectsOfTypeAll<Identifiable>())
            {
                if (item.id != plortId)
                    continue;
                var _meshRenderer = item.GetComponent<MeshRenderer>();
                if (!flag2)
                {
                    originalPlortMaterials.Add(plortId, _meshRenderer.material);
                    flag2 = true;
                }
                if (m && _meshRenderer.sharedMaterial != m)
                {
                    _meshRenderer.sharedMaterial = m;
                    if (ExtraApperanceApplicators.TryGetValue(plortId, out var f))
                        try
                        {
                            f(item.transform, appearance);
                        }
                        catch (Exception e)
                        {
                            Console.LogError($"An error occured while applying extra appearance applicator for {(item ? item : "null")} (Plort Type = {plortId})\n{e}");
                        }
                }
            }
        }
    }

    public struct SecretStyleData
    {
        public Sprite sprite;
        public SecretStyleData(Sprite Sprite)
        {
            sprite = Sprite;
        }
    }
}