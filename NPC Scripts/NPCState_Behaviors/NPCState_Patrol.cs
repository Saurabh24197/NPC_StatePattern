using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace BaseFramework
{
    public class NPCState_Patrol : NPCState_Interface
    {
        private readonly NPC_StatePattern npc;
        private int nextWaypoint;
        private Collider[] colliders;
        private Vector3 lookAtPoint;
        private Vector3 heading;
        private float dotProd;

        public NPCState_Patrol(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            Look();
            Patrol();
        }
        public void ToPatrolState() { }

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

        void Look()
        {
            //Check Medium Range.
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange / 3, npc.myEnemyLayer);

            if (colliders.Length > 0)
            {
                VisibilityCalculations(colliders[0].transform);

                if (dotProd > 0)
                {
                    AlertStateActions(colliders[0].transform);
                    return;
                }
            }

            //Checking for Max Range
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

        void Patrol()
        {
            npc.meshRendererFlag.material.color = Color.green;

            if (npc.myFollowTarget != null)
            {
                npc.currentState = npc.followState;
            }

            if (!npc.myNavMeshAgent.enabled)
            {
                return;
            }
        
            if (npc.waypoints.Count > 0)
            {
                MoveTo(npc.waypoints[nextWaypoint].position);

                if (HavIReachedDestination())
                {
                    nextWaypoint = (nextWaypoint + 1) % npc.waypoints.Count;
                }
            }

            else
            {
                //Wander about if there are no waypoints.
                if (HavIReachedDestination())
                {
                    StopWalking();

                    if (RandomWanderTarget(npc.transform.position, npc.sightRange, out npc.wanderTarget))
                    {
                        MoveTo(npc.wanderTarget);
                    }
                }
            }
        }

        bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            NavMeshHit navHit;

            Vector3 randomPoint = center + Random.insideUnitSphere * npc.sightRange;
            if (NavMesh.SamplePosition(randomPoint, out navHit, 3.0f, NavMesh.AllAreas))
            {
                result = navHit.position;
                return true;
            }

            else
            {
                result = center;
                return false;
            }
        }

        bool HavIReachedDestination()
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

        void MoveTo(Vector3 targetPos)
        {
            if (Vector3.Distance(npc.transform.position, targetPos) > npc.myNavMeshAgent.stoppingDistance + 1)
            {
                npc.myNavMeshAgent.SetDestination(targetPos);
                KeepWalking();
            }
        }
    }
}
