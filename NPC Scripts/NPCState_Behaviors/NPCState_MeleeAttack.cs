using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPCState_MeleeAttack : NPCState_Interface 
	{
        private readonly NPC_StatePattern npc;
        private float distanceToTarget;

        public NPCState_MeleeAttack(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            Look();
            TryToAttack();
        }

        public void ToPatrolState()
        {
            KeepWalking();
            npc.isMeleeAttacking = false;
            npc.currentState = npc.patrolState;
        }

        public void ToAlertState() { }

        public void ToPursueState()
        {
            KeepWalking();
            npc.isMeleeAttacking = false;
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

            Collider[] colliders = Physics.OverlapSphere(npc.transform.position, npc.meleeAttackRange, npc.myEnemyLayer);
             
            if (colliders.Length == 0)
            {
                //npc.pursueTarget = null;
                //ToPatrolState();
                ToPursueState();
                return;
            }

            foreach (Collider col in colliders)
            {
                if (col.transform.root == npc.pursueTarget)
                {
                    return;
                }
            }

            //npc.pursueTarget = null;
            //ToPatrolState();
            ToPursueState();
        }

        void TryToAttack()
        {
            if (npc.pursueTarget != null)
            {
                npc.meshRendererFlag.material.color = Color.magenta;

                if (Time.time > npc.nextAttack && !npc.isMeleeAttacking)
                {
                    npc.nextAttack = Time.time + npc.attackRate;

                    //float distanceXZ = Vector3.Distance(
                    //    new Vector3(npc.transform.position.x, 0, npc.transform.position.z),
                    //    new Vector3(npc.pursueTarget.position.x, 0, npc.pursueTarget.position.z));
                    //if (distanceXZ <= npc.meleeAttackRange)

                    if (Vector3.Distance(npc.transform.position, npc.pursueTarget.position) <= npc.meleeAttackRange)
                    {
                        Vector3 newPos = new Vector3(npc.pursueTarget.position.x, npc.transform.position.y, npc.pursueTarget.position.z);
                        npc.transform.LookAt(newPos);
                        npc.npcMaster.CallEventNpcAttackAnim();
                        npc.isMeleeAttacking = true;

                        if (!npc.usingMeleeAnim)
                            npc.OnEnemyAttack();
                    }

                    else ToPursueState();
                }
            }

            else ToPursueState(); //ToPatrolState();
        }

        void KeepWalking()
        {
            //npc.myNavMeshAgent.Resume();
            npc.myNavMeshAgent.isStopped = false;
            npc.npcMaster.CallEventNpcWalkAnim();
        }
    }
}
