using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace wipo.patches.Leveling
{
    [HarmonyPatch(typeof(DefaultCharacterAttributes), "InitializeAll")]
    internal class DefaultAttributeInitializePatch 
    {
        [HarmonyPrefix]
        static bool Prefix(ref DefaultSkills __instance, ref CharacterAttribute ____vigor, ref CharacterAttribute ____control, ref CharacterAttribute ____endurance, ref CharacterAttribute ____cunning, ref CharacterAttribute ____social, ref CharacterAttribute ____intelligence)
        {

            ____vigor.Initialize(new TextObject("{=YWkdD7Ki}Vigor", null), new TextObject("{=jJ9sLOLb}Vigor represents the ability to move with speed and force. It's important for melee combat.", null), new TextObject("{=Ve8xoa3i}VIG", null));
            ____control.Initialize(new TextObject("{=controlskill}Control", null), new TextObject("{=vx0OCvaj}Control represents the ability to use strength without sacrificing precision. It's necessary for using ranged weapons.", null), new TextObject("{=HuXafdmR}CTR", null));
            ____endurance.Initialize(new TextObject("{=kvOavzcs}Endurance", null), new TextObject("{=K8rCOQUZ}Endurance is the ability to perform taxing physical activity for a long time.", null), new TextObject("{=d2ApwXJr}END", null));
            ____cunning.Initialize(new TextObject("{=JZM1mQvb}Cunning", null), new TextObject("{=YO5LUfiO}Cunning is the ability to predict what other people will do, and to outwit their plans.", null), new TextObject("{=tH6Ooj0P}CNG", null));
            ____social.Initialize(new TextObject("{=socialskill}Social", null), new TextObject("{=XMDTt96y}Social is the ability to understand people's motivations and to sway them.", null), new TextObject("{=PHoxdReD}SOC", null));
            ____intelligence.Initialize(new TextObject("{=sOrJoxiC}Intelligence", null), new TextObject("{=TeUtEGV0}Intelligence represents aptitude for reading and theoretical learning.", null), new TextObject("{=Bn7IsMpu}INT", null)); 
            return false;
        }

    }
}
