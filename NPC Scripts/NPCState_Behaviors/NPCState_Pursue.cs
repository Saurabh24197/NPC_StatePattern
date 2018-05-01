using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPCState_Pursue : NPCState_Interface 
	{
        private readonly NPC_StatePattern npc;
        private float capturedDistance;

        public NPCState_Pursue(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            Look();
            Pursue();
        }

        public void ToPatrolState()
        {
            KeepWalking();
            npc.currentState = npc.patrolState;
        }

        public void ToAlertState()
        {
            KeepWalking();
            npc.currentState = npc.alertState;
        }

        public void ToPursueState() { }

        public void ToMeleeAttackState()
        {
            npc.currentState = npc.meleeAttackState;
        }

        public void ToRangedAttackState()
        {
            npc.currentState = npc.rangedAttackState;
        }

        void Look()
        {
            if (npc.pursueTarget == null)
            {
                ToPatrolState();
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayer);

            if (colliders.Length == 0)
            {
                npc.pursueTarget = null;
                ToPatrolState();
                return;
            }

            capturedDistance = npc.sightRange * 2;

            foreach (Collider col in colliders)
            {
                float distanceToTarg = Vector3.Distance(npc.transform.position, col.transform.position);

                if (distanceToTarg < capturedDistance)
                {
                    capturedDistance = distanceToTarg;
                    npc.pursueTarget = col.transform.root;
                }
            }
        }

        void KeepWalking()
        {
            //npc.myNavMeshAgent.Resume();
            npc.myNavMeshAgent.isStopped = false;
            npc.npcMaster.CallEventNpcWalkAnim();
        }

        void Pursue()
        {
            npc.meshRendererFlag.material.color = Color.red;

            if (npc.myNavMeshAgent.enabled && npc.pursueTarget != null)
            {
                npc.myNavMeshAgent.SetDestination(npc.pursueTarget.position);
                npc.locationOfInterest = npc.pursueTarget.position;
                KeepWalking();

                float distanceToTarg = Vector3.Distance(npc.transform.position, npc.pursueTarget.position);

                if (distanceToTarg <= npc.rangedAttackRange
                    && distanceToTarg > npc.meleeAttackRange)
                {
                    if (npc.hasRangedAttack)
                    {
                        ToRangedAttackState();
                    }
                }

                else if (distanceToTarg <= npc.meleeAttackRange) // + npc.meleeAttackRange * 0.25f)
                {
                    if (npc.hasMeleeAttack)
                    {
                        ToMeleeAttackState();
                    }

                    else if (npc.hasRangedAttack)
                    {
                        ToRangedAttackState();
                    }
                }
            }

            else ToAlertState();
        }
    }
}
