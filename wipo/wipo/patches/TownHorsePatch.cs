using SandBox;
using SandBox.Missions.MissionLogics;
using SandBox.Missions.MissionLogics.Towns;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace wipo.patches
{
    internal class TownHorsePatch : TownCenterMissionController
    {
        public override void AfterStart()
        {
            bool isNight = Campaign.Current.IsNight;
            base.Mission.SetMissionMode(MissionMode.StartUp, true);
            base.Mission.IsInventoryAccessible = !Campaign.Current.IsMainHeroDisguised;
            base.Mission.IsQuestScreenAccessible = true;
            MissionAgentHandler missionBehavior = base.Mission.GetMissionBehavior<MissionAgentHandler>();
            SandBoxHelpers.MissionHelper.SpawnPlayer(base.Mission.DoesMissionRequireCivilianEquipment, true, false, false, "");
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
        }
    }
}
