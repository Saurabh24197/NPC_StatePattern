﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class NPC_SetMyAttacker : MonoBehaviour
    {
        private NPC_StatePattern npcStatePattern;
        private GameManager_NPCRelationsMaster npcRelationsMaster;
        private int factionChangeAmount = 2;
        public bool applyRelationChainEffect = false;
        public Transform currentAttacker;

        // Use this for initialization
        void Start()
        {
            SetInitialReferences();
        }

        void SetInitialReferences()
        {
            npcStatePattern = GetComponent<NPC_StatePattern>();

            if (GameObject.Find("GameManager").GetComponent<GameManager_NPCRelationsMaster>() != null)
            {
                npcRelationsMaster = GameObject.Find("GameManager").GetComponent<GameManager_NPCRelationsMaster>();
            }
        }

        public void SetMyAttacker(Transform attacker)
        {
            currentAttacker = attacker;
            npcStatePattern.myAttacker = attacker;

            if (npcRelationsMaster != null)
            {
                npcRelationsMaster.CallEventNPCRelationChange(transform.tag, attacker.tag, -factionChangeAmount, applyRelationChainEffect);
            }
        }


    }
}

