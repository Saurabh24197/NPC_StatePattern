using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPCState_Follow : NPCState_Interface 
	{

        private readonly NPC_StatePattern npc;
        private Collider[] colliders;
        private Vector3 lookAtPoint;
        private Vector3 heading;
        private float dotProd;

        public NPCState_Follow(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            Look();
            FollowTarget();
        }

        public void ToPatrolState()
        {
            npc.currentState = npc.patrolState;
        }

        public void ToAlertState()
        {
            npc.currentState = npc.alertState;
        }

        public void ToPursueState() { }
        public void ToMeleeAttackState() { }
        public void ToRangedAttackState() { }

        void AlertStateActions(Transform target)
        {
            npc.locationOfInterest = target.position;
            ToAlertState();
        }

        void VisibilityCalculations(Transform target)
        {
            lookAtPoint = new Vector3(target.position.x, target.position.y + npc.offset, target.position.z);
            heading = lookAtPoint - npc.transform.position;
            dotProd = Vector3.Dot(heading, npc.transform.forward);
        }

        bool HaveIReachedDestination()
        {
            if (npc.myNavMeshAgent.remainingDistance <= npc.myNavMeshAgent.stoppingDistance
                && !npc.myNavMeshAgent.pathPending)
            {
                StopWalking();
                return true;
            }

            else
            {
                KeepWalking();
                return false;
            }
        }

        void Look()
        {
            //Start Checking in near Range
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange / 3, npc.myEnemyLayer);

            if (colliders.Length > 0)
            {
                AlertStateActions(colliders[0].transform);
                return;
            }

            //Checking Medium Range.
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange / 2, npc.myEnemyLayer);

            if (colliders.Length > 0)
            {
                VisibilityCalculations(colliders[0].transform);

                if (dotProd > 0)
                {
                    AlertStateActions(colliders[0].transform);
                    return;
                }
            }

            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayer);

            foreach (Collider col in colliders)
            {
                RaycastHit hit;

                VisibilityCalculations(col.transform);

                if (Physics.Linecast(npc.head.position, lookAtPoint, out hit, npc.sightLayer))
                {
                    foreach (string tags in npc.myEnemyTags)
                    {
                        if (hit.transform.CompareTag(tags))
                        {
                            if (dotProd > 0)
                            {
                                AlertStateActions(col.transform);
                                return;
                            }
                        }
                    }
                }
            }
        }

        void FollowTarget()
        {
            npc.meshRendererFlag.material.color = Color.blue;

            if (!npc.myNavMeshAgent.enabled) return;

            if (npc.myFollowTarget != null)
            {
                npc.myNavMeshAgent.SetDestination(npc.myFollowTarget.position);
                KeepWalking();

                //Added for Vehicles
                if (!npc.myFollowTarget.gameObject.activeSelf)
                {
                    if (Vector3.Distance( npc.transform.position, npc.myFollowTarget.position) < 10)
                    {
                        npc.transform.SetParent(npc.myFollowTarget.parent);
                        return;
                    }
                }
            }

            else
            {
                ToPatrolState();
            }

            if (HaveIReachedDestination()) StopWalking();
        }

        void StopWalking()
        {
            //npc.myNavMeshAgent.Stop();
            npc.myNavMeshAgent.isStopped = true;
            npc.npcMaster.CallEventNpcIdleAnim();
        }

        void KeepWalking()
        {
            //npc.myNavMeshAgent.Resume();
            npc.myNavMeshAgent.isStopped = false;
            npc.npcMaster.CallEventNpcWalkAnim();
        }


    }
}
