using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPCState_RangedAttack : NPCState_Interface 
	{
        private readonly NPC_StatePattern npc;
        private RaycastHit hit;

        public NPCState_RangedAttack (NPC_StatePattern npcStatePattern)
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
            npc.pursueTarget = null;
            npc.currentState = npc.patrolState;
        }

        public void ToAlertState()
        {
            KeepWalking();
            npc.currentState = npc.alertState;
        }

        public void ToPursueState()
        {
            KeepWalking();
            npc.currentState = npc.pursueState;
        }

        public void ToMeleeAttackState()
        {
            npc.currentState = npc.meleeAttackState;
        }

        public void ToRangedAttackState() { }


        void TurnTowardsTarget()
        {
            Vector3 newPos = new Vector3(npc.pursueTarget.position.x, npc.transform.position.y, npc.pursueTarget.position.z);
            npc.transform.LookAt(newPos);
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
                ToPatrolState();
                return;
            }

            foreach (Collider col in colliders)
            {
                if (col.transform.root == npc.pursueTarget)
                {
                    TurnTowardsTarget();
                    return;
                } 
            }

            ToPatrolState();
        }

        void TryToAttack()
        {
            if (npc.pursueTarget != null)
            {
                npc.meshRendererFlag.material.color = Color.cyan;

                if (!isTargetInSight())
                {
                    ToPursueState();
                    return;
                }

                if (Time.time > npc.nextAttack)
                {
                    npc.nextAttack = Time.time + npc.attackRate;

                    float distanceToTarget = Vector3.Distance(npc.transform.position, npc.pursueTarget.position);

                    TurnTowardsTarget();

                    if (distanceToTarget <= npc.rangedAttackRange)
                    {
                        StopWalking();

                        if (npc.rangedWeapon.GetComponent<Gun_Master>() != null)
                        {
                            npc.rangedWeapon.GetComponent<Gun_Master>().CallEventNpcInput(npc.rangedAttackSpread);
                            return;
                        }
                    }

                    if (distanceToTarget <= npc.meleeAttackRange && npc.hasMeleeAttack)
                    {
                        ToMeleeAttackState();
                    }
                }
            }

            else ToPatrolState();
        }

        void KeepWalking()
        {
            if (npc.myNavMeshAgent.enabled)
            {
                //npc.myNavMeshAgent.Resume();
                npc.myNavMeshAgent.isStopped = false;
                npc.npcMaster.CallEventNpcWalkAnim();
            }
        }

        void StopWalking()
        {
            if (npc.myNavMeshAgent.enabled)
            {
                //npc.myNavMeshAgent.Stop();
                npc.myNavMeshAgent.isStopped = true;
                npc.npcMaster.CallEventNpcIdleAnim();
            }
        }

        bool isTargetInSight()
        {
            RaycastHit hit;

            Vector3 weaponLookAtVector = new Vector3(npc.pursueTarget.position.x, npc.pursueTarget.position.y + npc.offset, npc.pursueTarget.position.z);
            npc.rangedWeapon.transform.LookAt(weaponLookAtVector);

            if (Physics.Raycast(npc.rangedWeapon.transform.position, npc.rangedWeapon.transform.forward, out hit))
            {
                foreach (string tags in npc.myEnemyTags)
                {
                    if (hit.transform.CompareTag(tags)) return true;
                }

                return false;
            }

            else return false;
        }
    }
}
