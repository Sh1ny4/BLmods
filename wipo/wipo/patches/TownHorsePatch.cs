using HarmonyLib;
using SandBox;
using SandBox.Missions.MissionLogics;
using SandBox.Missions.MissionLogics.Towns;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace wipo.patches
{
    [HarmonyPatch(typeof(TownCenterMissionController), nameof(TownCenterMissionController.AfterStart))]
    internal class TownHorsePatch : TownCenterMissionController
    {
        [HarmonyPrefix]
        static bool AfterStart(ref TownCenterMissionController __instance)
        {
            bool isNight = Campaign.Current.IsNight;
            __instance.Mission.SetMissionMode(MissionMode.StartUp, true);
            __instance.Mission.IsInventoryAccessible = !Campaign.Current.IsMainHeroDisguised;
            __instance.Mission.IsQuestScreenAccessible = true;
            MissionAgentHandler missionBehavior = __instance.Mission.GetMissionBehavior<MissionAgentHandler>();
            SandBoxHelpers.MissionHelper.SpawnPlayer(__instance.Mission.DoesMissionRequireCivilianEquipment, false, false, false, "");
            missionBehavior.SpawnLocationCharacters(null);
            SandBoxHelpers.MissionHelper.SpawnHorses();
            if (!isNight)
            {
                SandBoxHelpers.MissionHelper.SpawnSheeps();
                SandBoxHelpers.MissionHelper.SpawnCows();
                SandBoxHelpers.MissionHelper.SpawnHogs();
                SandBoxHelpers.MissionHelper.SpawnGeese();
                SandBoxHelpers.MissionHelper.SpawnChicken();
            }
            return false;
        }
    }
}
