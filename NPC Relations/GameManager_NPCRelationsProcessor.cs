using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class GameManager_NPCRelationsProcessor : MonoBehaviour
    {
        private GameManager_NPCRelationsMaster npcRelationsMaster;

        private void OnEnable()
        {
            SetInitialReferences();
            npcRelationsMaster.EventNPCRelationChange += ProcessFactionRelation;

            FillTags();
            SetLayers();
            UpdateNPCRelationsEverywhere();
        }

        private void OnDisable()
        {
            npcRelationsMaster.EventNPCRelationChange -= ProcessFactionRelation;
        }

        void SetInitialReferences()
        {
            npcRelationsMaster = GetComponent<GameManager_NPCRelationsMaster>();
        }

        void UpdateNPCRelationsEverywhere()
        {
            npcRelationsMaster.CallEventUpdateNPCRelationsEverywhere();
        }

        void ProcessFactionRelation(string factionAffected, string factionInstigating, int relationChange, bool applyChainEffect)
        {
            if (npcRelationsMaster.npcRelationsArray == null)
            {
                return;
            }

            foreach (NPCRelationsArray npcArray in npcRelationsMaster.npcRelationsArray)
            {
                //Change the Instigator Faction's relation with Affector's relation.
                if (npcArray.npcFaction == factionInstigating)
                {
                    foreach (NPCRelations npcRelation in npcArray.npcRelations)
                    {
                        if (npcRelation.npcTag == factionAffected)
                        {
                            npcRelation.npcFactionRating += relationChange;
                            break;
                        }
                    }
                }

                //Change the Affected Faction's relation with Instigator's relation.
                if (npcArray.npcFaction == factionAffected)
                {
                    foreach (NPCRelations npcRelation in npcArray.npcRelations)
                    {
                        if (npcRelation.npcTag == factionInstigating)
                        {
                            npcRelation.npcFactionRating += relationChange;
                            break;
                        }
                    }
                }

                //Change Other Observer Factions' relation with Instigator's relation.
                if (npcArray.npcFaction != factionAffected 
                    && npcArray.npcFaction != factionInstigating 
                    && applyChainEffect)
                {
                    foreach (NPCRelations npcRelation in npcArray.npcRelations)
                    {
                        if (npcRelation.npcTag == factionAffected)
                        {
                            //See if the Affected's faction is Hostile to Current, if not
                            //Adjust Current faction's relation with the Instigator
                            //Similarly change affected faction's relation.

                            if (npcRelation.npcFactionRating > npcRelationsMaster.hostileThreshold)
                            {
                                foreach (NPCRelations npcRel in npcArray.npcRelations)
                                {
                                    if (npcRel.npcTag == factionInstigating)
                                    {
                                        npcRel.npcFactionRating += relationChange;
                                        EditInstigatorRelationWithObserver(npcArray.npcFaction, factionInstigating, relationChange);
                                        break;
                                    }
                                }
                            }

                            else
                            {
                                foreach (NPCRelations npcRel in npcArray.npcRelations)
                                {
                                    if (npcRel.npcTag == factionInstigating)
                                    {
                                        npcRel.npcFactionRating += -relationChange;
                                        EditInstigatorRelationWithObserver(npcArray.npcFaction, factionInstigating, -relationChange);
                                        break;
                                    }
                                }
                            }


                        }
                    }
                }

                FillTags();
                SetLayers();
                UpdateNPCRelationsEverywhere();
            }


        }

        void EditInstigatorRelationWithObserver (string observingNPCFaction, string instigatorFaction, int relationChange)
        {
            //Bystander factions will adjust relations with the Instigator
            foreach (NPCRelationsArray npcArray in npcRelationsMaster.npcRelationsArray)
            {
                if (npcArray.npcFaction == instigatorFaction)
                {
                    foreach (NPCRelations npcRelation in npcArray.npcRelations)
                    {
                        if (npcRelation.npcTag == observingNPCFaction)
                        {
                            npcRelation.npcFactionRating += relationChange;
                            break;
                        }
                    }
                }


            }

        }

        void SetLayers()
        {
            //Called as SetFriendlyAndEnemyLayers in GTGD
            //Sets Friendly and Enemy layers.

            if (npcRelationsMaster.npcRelationsArray == null)
            {
                return;
            }

            foreach (NPCRelationsArray npcArray in npcRelationsMaster.npcRelationsArray)
            {
                LayerMask tmpFriendly = new LayerMask();
                LayerMask tmpEnemy = new LayerMask();

                foreach (NPCRelations npcRelation in npcArray.npcRelations)
                {
                    if (npcRelation.npcFactionRating > npcRelationsMaster.hostileThreshold)
                    {
                        tmpFriendly |= 1 << LayerMask.NameToLayer(npcRelation.npcTag);
                    }

                    else tmpEnemy |= 1 << LayerMask.NameToLayer(npcRelation.npcTag);
                }

                npcArray.myFriendlyLayers = tmpFriendly;
                npcArray.myEnemyLayers = tmpEnemy;
            }
        }

        void FillTags()
        {
            //Called as FillFriendlyAndEnemyTags in GTGD

            if (npcRelationsMaster.npcRelationsArray == null)
            {
                return;
            }

            foreach (NPCRelationsArray npcArray in npcRelationsMaster.npcRelationsArray)
            {
                List<string> tmpFriendlyTags = new List<string>();
                List<string> tmpEnemyTags = new List<string>();

                foreach (NPCRelations npcRelation in npcArray.npcRelations)
                {
                    if (npcRelation.npcFactionRating > npcRelationsMaster.hostileThreshold)
                    {
                        tmpFriendlyTags.Add(npcRelation.npcTag);
                    }

                    else tmpEnemyTags.Add(npcRelation.npcTag);
                }

                npcArray.myFriendlyTags = tmpFriendlyTags.ToArray();
                npcArray.myEnemyTags = tmpEnemyTags.ToArray();
            }
        }
    }
}


