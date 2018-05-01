using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class NPC_ApplyRelations : MonoBehaviour
    {
        private GameManager_NPCRelationsMaster npcRelationsMaster;
        private NPC_StatePattern npcStatePattern;
        private NPC_Master npcMaster;

        private void OnEnable()
        {
            SetInitialReferences();
            npcRelationsMaster.EventUpdateNPCRelationsEverywhere += SetMyRelations;
            Invoke("SetMyRelations", 0.1f);
        }

        private void OnDisable()
        {
            npcRelationsMaster.EventUpdateNPCRelationsEverywhere -= SetMyRelations;
        }

        void SetInitialReferences()
        {
            npcStatePattern = GetComponent<NPC_StatePattern>();
            npcMaster = GetComponent<NPC_Master>();

            GameObject gameManager = GameObject.Find("GameManager");
            npcRelationsMaster = gameManager.GetComponent<GameManager_NPCRelationsMaster>();

        }

        void SetMyRelations()
        {
            if (npcRelationsMaster.npcRelationsArray == null)
            {
                return;
            }

            foreach (NPCRelationsArray npcArray in npcRelationsMaster.npcRelationsArray)
            {
                if (transform.CompareTag(npcArray.npcFaction))
                {
                    npcStatePattern.myFriendlyLayer = npcArray.myFriendlyLayers;
                    npcStatePattern.myEnemyLayer = npcArray.myEnemyLayers;
                    npcStatePattern.myFriendlyTags = npcArray.myFriendlyTags;
                    npcStatePattern.myEnemyTags = npcArray.myEnemyTags;

                    ApplySightLayers(npcStatePattern.myFriendlyTags);
                    CheckMyFollowTargetIsAlly(npcStatePattern.myEnemyTags);

                    npcMaster.CallEventNPCRelationsChange();

                    break;
                }
            }
        }

        void CheckMyFollowTargetIsAlly(string[] enemyTags)
        {
            //If the player becomes an enemy to his NPC Faction
            //then they should not follow him as their leader.

            if (npcStatePattern.myFollowTarget == null)
            {
                return;
            }

            if (enemyTags.Length > 0)
            {
                foreach (string eTag in enemyTags)
                {
                    if (npcStatePattern.myFollowTarget.CompareTag(eTag))
                    {
                        npcStatePattern.myFollowTarget = null;
                        break;
                    }
                }
            }
        }

        void ApplySightLayers(string[] friendlyTags)
        {
            //To make sure that the NPC's allies won't block their vision.

            npcStatePattern.sightLayer = LayerMask.NameToLayer("Everything");

            if (friendlyTags.Length > 0)
            {
                foreach (string fTag in friendlyTags)
                {
                    int tempINT = LayerMask.NameToLayer(fTag);
                    npcStatePattern.sightLayer = ~(1 << tempINT | 1 << LayerMask.NameToLayer("Ignore Raycast"));
                }
            }
        }


    }

}
