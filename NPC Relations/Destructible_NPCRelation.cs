using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class Destructible_NPCRelation : MonoBehaviour
    {
        private GameManager_NPCRelationsMaster npcRelationsMaster;
        public int relationsChangeOnDestroy = 50;
        public bool applyRelationChainEffect = true;
        public string factionAffected;
        private string factionInstigating;

        private void Start()
        {
            SetInitialReferences();
        }

        private void OnDestroy()
        {
            ApplyRelationChangeOnDestruction();
        }

        void SetInitialReferences()
        {
            if (GameObject.Find("GameManager").GetComponent<GameManager_NPCRelationsMaster>() != null)
            {
                npcRelationsMaster = GameObject.Find("GameManager").GetComponent<GameManager_NPCRelationsMaster>();
            }
        }

        public void SetMyAttacker(Transform attacker)
        {
            factionInstigating = attacker.tag;
        }

        void ApplyRelationChangeOnDestruction()
        {
            if (factionInstigating == null)
            {
                return;
            }

            npcRelationsMaster.CallEventNPCRelationChange(factionAffected, factionInstigating, relationsChangeOnDestroy, applyRelationChainEffect);
        }
    }
}

