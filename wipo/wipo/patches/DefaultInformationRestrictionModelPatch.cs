using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches
{
    internal class DefaultInformationRestrictionModelPatch : DefaultInformationRestrictionModel
    {
        public override bool DoesPlayerKnowDetailsOf(Settlement settlement)
        {
            return true;
        }
        public override bool DoesPlayerKnowDetailsOf(Hero hero)
        {
            return true;
        }
    }
}
