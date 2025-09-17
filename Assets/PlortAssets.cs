using Secret_Style_Things.Utils;
using UnityEngine;

namespace Secret_Style_Things.Assets
{
    internal static class PlortAssets
    {
        internal static readonly Mesh NormalPlort = SRObjects.Get<Mesh>("plort");
        internal static readonly Mesh MosaicDlcShardsLOD0 = SRObjects.Get<Mesh>("mosaic_DLC_shards_LOD0");
        internal static readonly Material SlimeMosaicPlort = SRObjects.Get<Material>("slimeMosaic_plort");
        internal static readonly Material FXPhosphor = SRObjects.Get<Material>("FX Phospher");
        internal static readonly Material FXPhosphorExotic = SRObjects.Get<Material>("FX PhosphorExotic");
        internal static readonly Material FXFire = SRObjects.Get<Material>("fx firePlortAura");
        internal static readonly Material FXFireExotic = SRObjects.Get<Material>("fx fireSlimeAura_exotic");
        internal static readonly Mesh PuddleDlcFlowerLOD0 = SRObjects.Get<Mesh>("puddle_DLC_flower_LOD0");
    }
}