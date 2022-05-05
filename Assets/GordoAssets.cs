using Secret_Style_Things.Utils;
using UnityEngine;

namespace Secret_Style_Things.Assets
{
    internal class GordoAssets
    {



        internal static readonly Mesh TabbyDlcEarsTailLOD0 = SRObjects.Get<Mesh>("tabby_DLC_ears_tail_LOD0");
        internal static readonly Mesh EarsNTailLOD0 = SRObjects.Get<Mesh>("ears_n_tail_LOD0");
        
        internal static readonly Material SlimeRadAura = SRObjects.Get<Material>("slimeRadAura");
        internal static readonly Material SlimeRadAuraExotic = SRObjects.Get<Material>("slimeRadAura_exotic");
        
        internal static readonly Mesh Tail = Main.assetBundle.LoadAsset<Mesh>("tail");
        internal static readonly Mesh HunterEarsTail = SRObjects.Get<Mesh>("hunter_ears_n_tail_LOD0");
        internal static readonly Mesh HunterDLCEarsTail = SRObjects.Get<Mesh>("hunter_DLC_ears_tail_LOD0");

        internal static readonly Mesh WingL = SRObjects.Get<Mesh>("wing_l");
        internal static readonly Mesh WingR = SRObjects.Get<Mesh>("wing_r");
        internal static readonly Mesh PhosphorDlcWingR = SRObjects.Get<Mesh>("phosphor_DLC_wing_R");
        internal static readonly Mesh PhosphorDlcWingL = SRObjects.Get<Mesh>("phosphor_DLC_wing_L");
        internal static readonly Mesh PhosphorDlcHaloLOD0 = SRObjects.Get<Mesh>("phosphor_DLC_halo_LOD0");

        internal static readonly Mesh HoneyExoticCrownLOD0 = SRObjects.Get<Mesh>("honey_exotic_crown_LOD1");
        internal static readonly Mesh HoneyCombLOD0 = SRObjects.Get<Mesh>("honneyplate_LOD0");

        internal static readonly Material SlimeHunterExoticBase = SRObjects.Get<Material>("slimeHunterExoticBase");

        internal static readonly Mesh MosaicDlcShardsLOD0 = SRObjects.Get<Mesh>("mosaic_DLC_shards_LOD0");
        internal static readonly Mesh MosaicGordoLOD0 = SRObjects.Get<Mesh>("mosaicGordo_LOD0");

        internal static readonly Mesh TangleFlowerMeshExotic = SRObjects.Get<Mesh>("tangle_DLC_flower_LOD1");
        internal static readonly Material TangleFlowerMaterialExotic = SRObjects.Get<Material>("flowerTangleExotic_base");

        internal static readonly GameObject FXBoomGordoBoom = SRObjects.Get<GameObject>("FX BoomGordoBoom");

        internal static GameObject FXBoomGordoBoomExotic = null;




    }
}