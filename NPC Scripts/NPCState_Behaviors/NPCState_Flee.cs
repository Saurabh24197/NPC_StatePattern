using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BaseFramework
{
	public class NPCState_Flee : NPCState_Interface 
	{
        private Vector3 directionToEnemy;
        private NavMeshHit navHit;

        private readonly NPC_StatePattern npc;

        public NPCState_Flee(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            CheckIfIShouldFlee();
            CheckIfIShouldFight();
        }

        public void ToPatrolState()
        {
            KeepWalking();
            npc.currentState = npc.patrolState;
        }

        public void ToAlertState() { }
        public void ToPursueState() { }

        public void ToMeleeAttackState()
        {
            KeepWalking();
            npc.currentState = npc.meleeAttackState;
        }

        public void ToRangedAttackState() { }

        void CheckIfIShouldFight()
        {
            if (npc.pursueTarget == null) return;

            float distanceToTarg = Vector3.Distance(npc.transform.position, npc.pursueTarget.position);

            if (npc.hasMeleeAttack
                && distanceToTarg <= npc.meleeAttackRange)
            {
                ToMeleeAttackState();
            }
        }

        void CheckIfIShouldFlee()
        {
            npc.meshRendererFlag.material.color = Color.gray;

            Collider[] colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayer);

            if (colliders.Length == 0)
            {
                ToPatrolState();
                return;
            }

            directionToEnemy = npc.transform.position - colliders[0].transform.position;
            Vector3 checkPos = npc.transform.position + directionToEnemy;

            if (NavMesh.SamplePosition(checkPos, out navHit, 3.0f, NavMesh.AllAreas))
            {
                npc.myNavMeshAgent.destination = navHit.position;
                KeepWalking();
            }

            else StopWalking();
        }

        void KeepWalking()
        {
            //npc.myNavMeshAgent.Resume();
            npc.myNavMeshAgent.isStopped = false;
            npc.npcMaster.CallEventNpcWalkAnim();
        }

        void StopWalking()
        {
            //npc.myNavMeshAgent.Stop();
            npc.myNavMeshAgent.isStopped = true;
            npc.npcMaster.CallEventNpcIdleAnim();
        }

    }
}
