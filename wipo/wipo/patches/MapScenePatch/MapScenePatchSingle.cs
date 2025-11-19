using HarmonyLib;
using SandBox;
using SandBox.View.Map;
using SandBox.View.Map.Visuals;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;

namespace wipo.patches.MapScenePatch
{
    [HarmonyPatch(typeof(MobilePartyVisual), "Tick")]
    public class MapScenePatchSingle
    {
        [HarmonyPostfix]
        public static void Postfix(MobilePartyVisual __instance)
        {
            GameEntity strategicEntity = __instance.StrategicEntity;
            MobileParty mobileParty = __instance.MapEntity.MobileParty;
            IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
            CampaignVec2 position = mobileParty.Position;
            if (mapSceneWrapper.GetTerrainTypeAtPosition(position) == TerrainType.Fording && strategicEntity.ChildCount == 0)
            {
                MatrixFrame identity = MatrixFrame.Identity;
                GameEntity gameEntity = GameEntity.CreateEmpty(strategicEntity.Scene, true, true, true);
                string metaMeshName = "ship_a";
                Hero leaderHero = mobileParty.LeaderHero;
                string text;
                if (leaderHero == null)
                {
                    text = null;
                }
                else
                {
                    Banner clanBanner = leaderHero.ClanBanner;
                    text = ((clanBanner != null) ? clanBanner.Serialize() : null);
                }
                string value = text;
                identity.rotation.ApplyScaleLocal(0.25f);
                gameEntity.SetFrame(ref identity, true);
                gameEntity.AddMultiMesh(MetaMesh.GetCopy(metaMeshName, true, false), true);
                bool flag2 = !string.IsNullOrEmpty(value);
                if (flag2)
                {
                    try
                    {
                    }
                    catch (Exception)
                    {
                    }
                }
                strategicEntity.AddChild(gameEntity, false);
                AgentVisuals humanAgentVisuals = __instance.HumanAgentVisuals;
                if (humanAgentVisuals != null)
                {
                    humanAgentVisuals.Reset();
                }
                AgentVisuals mountAgentVisuals = __instance.MountAgentVisuals;
                if (mountAgentVisuals != null)
                {
                    mountAgentVisuals.Reset();
                }
                AgentVisuals caravanMountAgentVisuals = __instance.CaravanMountAgentVisuals;
                if (caravanMountAgentVisuals != null)
                {
                    caravanMountAgentVisuals.Reset();
                }
                AccessTools.Property(typeof(MobilePartyVisual), "HumanAgentVisuals").SetValue(__instance, null);
                AccessTools.Property(typeof(MobilePartyVisual), "MountAgentVisuals").SetValue(__instance, null);
                AccessTools.Property(typeof(MobilePartyVisual), "CaravanMountAgentVisuals").SetValue(__instance, null);
            }
            else
            {
                IMapScene mapSceneWrapper2 = Campaign.Current.MapSceneWrapper;
                position = mobileParty.Position;
                bool flag3 = mapSceneWrapper2.GetTerrainTypeAtPosition(position) != TerrainType.Fording && strategicEntity.ChildCount > 0;
                if (flag3)
                {
                    mobileParty.Party.SetVisualAsDirty();
                }
            }
        }
    }

    [HarmonyPatch(typeof(MapScene), "GetHeightAtPoint")]
    public class GetHeightAtPointPatch
    {
        [HarmonyPostfix]
        public static void Postfix(MapScene __instance, ref float height)
        {
            height = MathF.Max(height, __instance.Scene.GetWaterLevel(), 2f);
        }
    }

    [HarmonyPatch(typeof(MapScreen), "StepSounds")]
    public class StepSoundsPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(MobileParty party)
        {
            return Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace) != TerrainType.Fording;
        }
    }

    [HarmonyPatch(typeof(MapScene), "GetFaceTerrainType")]
    public class GetFaceTerrainTypePatch
    {
        [HarmonyPostfix]
        public static void Postfix1(PathFaceRecord navMeshFace, MapScene __instance, ref TerrainType __result)
        {
            bool flag = navMeshFace.IsValid() && __instance.Scene.GetIdOfNavMeshFace(navMeshFace.FaceIndex) == 10;
            if (flag)
            {
                __result = TerrainType.Fording;
            }
        }
    }
}
