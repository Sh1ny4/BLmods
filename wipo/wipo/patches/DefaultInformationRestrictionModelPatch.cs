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

        // Token: 0x060017CD RID: 6093 RVA: 0x000725C4 File Offset: 0x000707C4
        public override bool DoesPlayerKnowDetailsOf(Hero hero)
        {
            return true;
        }
    }
}
