using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.CampaignBehaviors;

namespace wipo
{
    public class main : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("wipo.patches").PatchAll();
        }

        public override void OnGameInitializationFinished(Game game)
        {
            if (!(game.GameType is Campaign)) return;
            Campaign.Current.CampaignBehaviorManager.RemoveBehavior<BackstoryCampaignBehavior>();
        }
    }
}