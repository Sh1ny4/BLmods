using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.GameMenus;

namespace wipo.patches.EliteInCastle
{
    internal class CastleRecruitMenu : PlayerTownVisitCampaignBehavior
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddCastleRecruitMenus));
        }

        private void AddCastleRecruitMenus(CampaignGameStarter campaignGameSystemStarter)
        {
            campaignGameSystemStarter.AddGameMenuOption("castle", "recruit_volunteers", "{=E31IJyqs}Recruit troops", new GameMenuOption.OnConditionDelegate(game_menu_recruit_castle_volunteers_on_condition), new GameMenuOption.OnConsequenceDelegate(game_menu_recruit_castle_volunteers_on_consequence), false, 4, false,null);
        }

        private static void game_menu_recruit_castle_volunteers_on_consequence(MenuCallbackArgs args)
        {
            args.MenuContext.OpenRecruitVolunteers();
        }

        private static bool game_menu_recruit_castle_volunteers_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Recruit;
            return true;
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
    }
}