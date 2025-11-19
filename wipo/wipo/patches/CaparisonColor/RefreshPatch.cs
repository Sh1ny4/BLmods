using HarmonyLib;
using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;

namespace wipo.patches.CaparisonColor
{ 
    [HarmonyPatch(typeof(AgentVisuals), "Refresh", new Type[] { typeof(bool),typeof(bool),typeof(Equipment), typeof(bool), typeof(bool) })]
    internal class RefreshPatch
    {
        public static bool Prefix(bool __state, ref AgentVisualsData ____data , bool needBatchedVersionForWeaponMeshes, bool removeSkeleton = false, Equipment oldEquipment = null, bool isRandomProgress = false, bool forceUseFaceCache = false)
        {
            if (SavedColorData.colordata == null && ____data.BannerData != null) 
            { 
                SavedColorData.colordata = new SavedColor(____data.BannerData.GetPrimaryColor(), ____data.BannerData.GetSecondaryColor()); 
            }
            return true;
        }
        public static void Postfix(bool __state, bool needBatchedVersionForWeaponMeshes, bool removeSkeleton = false, Equipment oldEquipment = null, bool isRandomProgress = false, bool forceUseFaceCache = false)
        {
            if (__state) 
            { 
                SavedColorData.colordata = null; 
            }    
        }
    }
}