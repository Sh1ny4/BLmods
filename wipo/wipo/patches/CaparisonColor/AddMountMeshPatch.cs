using System;
using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;

namespace wipo.patches.CaparisonColor
{ 
    [HarmonyPatch(typeof(MountVisualCreator), "AddMountMesh")]
    internal class AddMountMeshPatch
    {
        public static bool Prefix(ref List<MetaMesh> __result, MBAgentVisuals agentVisual, ItemObject mountItem, ItemObject harnessItem, string mountCreationKeyStr, Agent agent = null)
        {
            var SetVoiceDefinitionAccess = AccessTools.Method(typeof(MountVisualCreator), "SetVoiceDefinition");
            List<MetaMesh> metaMeshList = new List<MetaMesh>();
            HorseComponent horseComponent = mountItem.HorseComponent;
            uint maxValue = uint.MaxValue;
            MetaMesh multiMesh = mountItem.GetMultiMesh(false, false, true);
            if (string.IsNullOrEmpty(mountCreationKeyStr))
            {
                mountCreationKeyStr = MountCreationKey.GetRandomMountKeyString(mountItem, MBRandom.RandomInt());
            }
            MountCreationKey mountCreationKey = MountCreationKey.FromString(mountCreationKeyStr);
            if (mountItem.ItemType == ItemObject.ItemTypeEnum.Horse)
            {
                MountVisualCreator.SetHorseColors(multiMesh, mountCreationKey);
            }
            if (horseComponent.HorseMaterialNames != null && horseComponent.HorseMaterialNames.Count > 0)
            {
                MountVisualCreator.SetMaterialProperties(mountItem, multiMesh, mountCreationKey, ref maxValue);
            }
            int nondeterministicRandomInt = MBRandom.NondeterministicRandomInt;
            AddMountMeshPatch.SetVoiceDefinition(agent, nondeterministicRandomInt);
            MetaMesh metaMesh = null;
            if (harnessItem != null)
            {
                metaMesh = harnessItem.GetMultiMesh(false, false, true);
                if (harnessItem.IsUsingTeamColor)
                {
                    for (int meshIndex = 0; meshIndex < metaMesh.MeshCount; meshIndex++)
                    {
                        Mesh meshAtIndex = metaMesh.GetMeshAtIndex(meshIndex);
                        if (!meshAtIndex.HasTag("no_team_color"))
                        {
                            meshAtIndex.Color = 4281281067U;
                            meshAtIndex.Color2 = 4281281067U;
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, bool> additionalMeshesName in horseComponent.AdditionalMeshesNameList)
            {
                if (additionalMeshesName.Key.Length > 0)
                {
                    string metaMeshName = additionalMeshesName.Key;
                    if (harnessItem == null || !additionalMeshesName.Value)
                    {
                        MetaMesh copy = MetaMesh.GetCopy(metaMeshName, true, false);
                        if (maxValue != uint.MaxValue)
                        {
                            copy.SetFactor1Linear(maxValue);
                        }
                        metaMeshList.Add(copy);
                    }
                    else
                    {
                        ArmorComponent armorComponent = harnessItem.ArmorComponent;
                        if (((armorComponent != null) ? ((armorComponent.ManeCoverType != ArmorComponent.HorseHarnessCoverTypes.All) ? 1 : 0) : 1) != 0)
                        {
                            ArmorComponent armorComponent2 = harnessItem.ArmorComponent;
                            if (((armorComponent2 != null) ? ((armorComponent2.ManeCoverType > ArmorComponent.HorseHarnessCoverTypes.None) ? 1 : 0) : 0) != 0)
                            {
                                string str = metaMeshName;
                                string str2 = "_";
                                ArmorComponent.HorseHarnessCoverTypes? horseHarnessCoverTypes;
                                if (harnessItem == null)
                                {
                                    horseHarnessCoverTypes = null;
                                }
                                else
                                {
                                    ArmorComponent armorComponent3 = harnessItem.ArmorComponent;
                                    horseHarnessCoverTypes = ((armorComponent3 != null) ? new ArmorComponent.HorseHarnessCoverTypes?(armorComponent3.ManeCoverType) : null);
                                }
                                ArmorComponent.HorseHarnessCoverTypes? horseHarnessCoverTypes2 = horseHarnessCoverTypes;
                                metaMeshName = str + str2 + horseHarnessCoverTypes2.ToString();
                            }
                            MetaMesh copy2 = MetaMesh.GetCopy(metaMeshName, true, false);
                            if (maxValue != uint.MaxValue)
                            {
                                copy2.SetFactor1Linear(maxValue);
                            }
                            metaMeshList.Add(copy2);
                        }
                    }
                }
            }
            if (multiMesh != null)
            {
                metaMeshList.Add(multiMesh);
            }
            if (metaMesh != null)
            {
                if (agentVisual != null)
                {
                    MetaMesh ropeMesh = null;
                    if (NativeConfig.CharacterDetail > 2 && harnessItem.ArmorComponent != null)
                    {
                        ropeMesh = MetaMesh.GetCopy(harnessItem.ArmorComponent.ReinsRopeMesh, false, true);
                    }
                    ArmorComponent armorComponent4 = harnessItem.ArmorComponent;
                    MetaMesh copy3 = MetaMesh.GetCopy((armorComponent4 != null) ? armorComponent4.ReinsMesh : null, false, true);
                    if (ropeMesh != null && copy3 != null)
                    {
                        agentVisual.AddHorseReinsClothMesh(copy3, ropeMesh);
                        ropeMesh.ManualInvalidate();
                    }
                    if (copy3 != null)
                    {
                        metaMeshList.Add(copy3);
                    }
                }
                else
                {
                    if (harnessItem.ArmorComponent != null)
                    {
                        MetaMesh copy4 = MetaMesh.GetCopy(harnessItem.ArmorComponent.ReinsMesh, true, true);
                        if (copy4 != null)
                        {
                            metaMeshList.Add(copy4);
                        }
                    }
                }
                metaMeshList.Add(metaMesh);
            }
            __result = metaMeshList;
            return false;
        }

        public static void SetVoiceDefinition(Agent agent, int seedForRandomVoiceTypeAndPitch)
        {
            try
            {
                MBAgentVisuals agentVisuals = (agent != null) ? agent.AgentVisuals : null;
                if ((agentVisuals != null))
                {
                    string collisionInfoClassName = agent.GetSoundAndCollisionInfoClassName();
                    int length = (!string.IsNullOrEmpty(collisionInfoClassName)) ? SkinVoiceManager.GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassName(collisionInfoClassName) : 0;
                    if (length == 0)
                    {
                        agentVisuals.SetVoiceDefinitionIndex(-1, 0f);
                    }
                    else
                    {
                        int num = MathF.Abs(seedForRandomVoiceTypeAndPitch);
                        float voicePitch = (float)num * 4.656613E-10f;
                        int[] definitionIndices = new int[length];
                        SkinVoiceManager.GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassName(collisionInfoClassName, definitionIndices);
                        int voiceDefinitionIndex = definitionIndices[num % length];
                        agentVisuals.SetVoiceDefinitionIndex(voiceDefinitionIndex, voicePitch);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}