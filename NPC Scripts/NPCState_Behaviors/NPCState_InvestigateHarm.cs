using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPCState_InvestigateHarm : NPCState_Interface 
	{
        private readonly NPC_StatePattern npc;
        private float offset = 0.3f;
        private RaycastHit hit;
        private Vector3 lookAtTarget;

        public NPCState_InvestigateHarm(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            Look();
        }

        public void ToPatrolState()
        {
            npc.currentState = npc.patrolState;
        }

        public void ToAlertState()
        {
            npc.currentState = npc.alertState;
        }

        public void ToPursueState()
        {
            npc.currentState = npc.pursueState;
        }

        public void ToMeleeAttackState() { }
        public void ToRangedAttackState() { }

        void Look()
        {
            if (npc.pursueTarget == null)
            {
                ToPatrolState();
                return;
            }

            CheckIfTargetIsInDirectSight();
        }

        void CheckIfTargetIsInDirectSight()
        {
            lookAtTarget = new Vector3(npc.pursueTarget.position.x, npc.pursueTarget.position.y + offset, npc.pursueTarget.position.z);

            if (Physics.Linecast(npc.head.position, lookAtTarget, out hit, npc.sightLayer))
            {
                if (hit.transform.root == npc.pursueTarget)
                {
                    npc.locationOfInterest = npc.pursueTarget.position;
                    GoToLocationOfInterest();

                    if (Vector3.Distance(npc.transform.position, lookAtTarget) <= npc.sightRange)
                    {
                        ToPursueState();
                    }
                }

                else ToAlertState();
            }

            else ToAlertState();
        }

        void GoToLocationOfInterest()
        {
            npc.meshRendererFlag.material.color = Color.black;

            if (npc.myNavMeshAgent.enabled && npc.locationOfInterest != Vector3.zero)
            {
                npc.myNavMeshAgent.SetDestination(npc.locationOfInterest);
                //npc.myNavMeshAgent.Resume();
                npc.myNavMeshAgent.isStopped = false;
                npc.npcMaster.CallEventNpcWalkAnim();

                if (npc.myNavMeshAgent.remainingDistance <= npc.myNavMeshAgent.stoppingDistance)
                {
                    npc.locationOfInterest = Vector3.zero;
                    ToPatrolState();
                }
            }

            else ToPatrolState();
        }
    }
}
