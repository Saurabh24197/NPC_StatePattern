using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BaseFramework
{
	public class NPCState_Alert : NPCState_Interface 
	{

        private readonly NPC_StatePattern npc;

        private float informRate = 3;
        private float nextInform;
        private float offset = .3f;

        private Vector3 targetPosition;
        private RaycastHit hit;
        private Collider[] colliders;
        private Collider[] friendlyColliders;
        private Vector3 lookAtTarget;
        private Transform possibleTarget;

        private int detectionCount;
        private int lastDetectionCount;

        public NPCState_Alert(NPC_StatePattern npcStatePattern)
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
        public void ToAlertState() { }

        public void ToPursueState()
        {
            npc.currentState = npc.pursueState;
        }

        public void ToMeleeAttackState() { }
        public void ToRangedAttackState() { }

        void Look()
        {
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayer);

            lastDetectionCount = detectionCount;

            foreach(Collider col in colliders)
            {
                lookAtTarget = new Vector3(col.transform.position.x, col.transform.position.y + offset, col.transform.position.z);

                if (Physics.Linecast(npc.head.position, lookAtTarget, out hit, npc.sightLayer))
                {
                    foreach (string tags in npc.myEnemyTags)
                    {
                        if (hit.transform.CompareTag(tags))
                        {
                            detectionCount++;
                            possibleTarget = col.transform;
                            break;
                        }
                    }
                }
            }

            //Check if DetectionCount has changed else set it to 0.
            if (detectionCount == lastDetectionCount) detectionCount = 0;
            
            //Check if DetectionCount is greater than requirement, then pursue.
            if (detectionCount >= npc.requiredDetectionCount)
            {
                detectionCount = 0;
                npc.locationOfInterest = possibleTarget.position;
                npc.pursueTarget = possibleTarget.root;
                InformNearbyAllies();
                ToPursueState();

            }

            GotoLocationOfInterest();
        }

        void InformNearbyAllies()
        {
            if (Time.time > nextInform)
            {
                nextInform = Time.time + informRate;
                friendlyColliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myFriendlyLayer);

                if (friendlyColliders.Length == 0)
                {
                    return;
                }
                foreach (Collider ally in friendlyColliders)
                {
                    if (ally.transform.root.GetComponent<NPC_StatePattern>() != null)
                    {
                        NPC_StatePattern allyPattern = ally.transform.root.GetComponent<NPC_StatePattern>();

                        if (allyPattern.currentState == allyPattern.patrolState)
                        {
                            allyPattern.pursueTarget = npc.pursueTarget;
                            allyPattern.locationOfInterest = npc.pursueTarget.position;
                            allyPattern.currentState = allyPattern.alertState;
                            allyPattern.npcMaster.CallEventNpcWalkAnim();
                        }
                    }
                }
            }
        }

        void GotoLocationOfInterest()
        {
            npc.meshRendererFlag.material.color = Color.yellow;

            if (npc.myNavMeshAgent.enabled
                && npc.locationOfInterest != Vector3.zero)
            {
                npc.myNavMeshAgent.SetDestination(npc.locationOfInterest);
                //npc.myNavMeshAgent.Resume();
                npc.myNavMeshAgent.isStopped = false;
                npc.npcMaster.CallEventNpcWalkAnim();

                if (npc.myNavMeshAgent.remainingDistance <= npc.myNavMeshAgent.stoppingDistance
                    && !npc.myNavMeshAgent.pathPending)
                {
                    npc.npcMaster.CallEventNpcIdleAnim();
                    npc.locationOfInterest = Vector3.zero;
                    ToPatrolState();
                }
            }
        }
    }
}
