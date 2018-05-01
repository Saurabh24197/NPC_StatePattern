using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace BaseFramework
{
	public class NPC_StatePattern : MonoBehaviour 
	{

        #region StatePattern_Variables

        //Decision Making Variables
        private float checkRate = .1f;
        private float nextCheck;

        [Header("State Variables")]
        public float sightRange = 40;
        public float detectBehindRange = 5;

        [Header("Attack Behaviours")]
        public float meleeAttackRange = 7;
        public float meleeAttackDamage = 10;

        public float rangedAttackRange = 35;
        public float rangedAttackDamage = 5;
        public float rangedAttackSpread = .5f;

        [Space]
        public float attackRate = .4f;
        public float nextAttack;

        [Space]
        public float fleeRange = 25;
        public float offset = .4f;
        public int requiredDetectionCount = 15;
        public float struckWaitTime = 1.5f;

        [Header("Conditional Booleans")]
        public bool hasRangedAttack;
        public bool hasMeleeAttack;
        public bool usingMeleeAnim = true;
        public bool isMeleeAttacking;

        [Header("Key Transforms")]
        public Transform myFollowTarget;
        [HideInInspector]
        public Transform pursueTarget;
        [HideInInspector]
        public Transform myAttacker;

        [HideInInspector]
        public Vector3 locationOfInterest;
        [HideInInspector]
        public Vector3 wanderTarget;

        //Variables used for Sight
        [Header("Layers and Tags : <Sight>")]
        public LayerMask sightLayer;
        public LayerMask myEnemyLayer;
        public LayerMask myFriendlyLayer;

        public string[] myEnemyTags;
        public string[] myFriendlyTags;

        //Reference Variables
        [Header("Reference type Variables")]
        public List<Transform> waypoints;
        public Transform head;
        public MeshRenderer meshRendererFlag;
        public GameObject rangedWeapon;
        public NPC_Master npcMaster;
        [HideInInspector]
        public NavMeshAgent myNavMeshAgent;

        //State AI variables
        [Header("State AI Variables")]
        public NPCState_Interface currentState;
        public NPCState_Interface capturedState;

        [Space]
        public NPCState_Patrol patrolState;
        public NPCState_Alert alertState;
        public NPCState_Pursue pursueState;
        public NPCState_MeleeAttack meleeAttackState;
        public NPCState_RangedAttack rangedAttackState;
        public NPCState_Flee fleeState;
        public NPCState_Struck struckState;
        public NPCState_InvestigateHarm investigateHarmState;
        public NPCState_Follow followState;

        #endregion
        //

        void Awake()
        {
            SetupStateReferences();
            SetInitialReferences();
        }

        void Start()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            npcMaster.EventNpcLowHealth += ActivateFleeState;
            npcMaster.EventNpcHealthRecovered += ActivatePatrolState;
            npcMaster.EventNpcDeductHealth += ActivateStruckState;
        }


        void OnDisable()
		{
            npcMaster.EventNpcLowHealth -= ActivateFleeState;
            npcMaster.EventNpcHealthRecovered -= ActivatePatrolState;
            npcMaster.EventNpcDeductHealth -= ActivateStruckState;

            StopAllCoroutines();
        }


		void Update () 
		{
            CarryOutUpdateState();
		}

        void SetupStateReferences()
        {
            patrolState = new NPCState_Patrol(this);
            alertState = new NPCState_Alert(this);
            pursueState = new NPCState_Pursue(this);
            fleeState = new NPCState_Flee(this);
            followState = new NPCState_Follow(this);
            meleeAttackState = new NPCState_MeleeAttack(this);
            rangedAttackState = new NPCState_RangedAttack(this);
            struckState = new NPCState_Struck(this);
            investigateHarmState = new NPCState_InvestigateHarm(this);
        }

		void SetInitialReferences()
		{
            myNavMeshAgent = GetComponent<NavMeshAgent>();
            ActivatePatrolState();
		}

		void CarryOutUpdateState()
        {
            if (Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                currentState.UpdateState();
            }
        }

        void ActivatePatrolState()
        {
            currentState = patrolState;
        }

        void ActivateFleeState()
        {
            if (currentState == struckState)
            {
                capturedState = fleeState;
                return;
            }

            currentState = fleeState;
        }

        void ActivateStruckState(int dummy)
        {
            StopAllCoroutines();

            if (currentState != struckState)
            {
                capturedState = currentState;
            }

            if (rangedWeapon != null)
            {
                //Change or Remove if you have proper Gun Holding Animations.
                rangedWeapon.SetActive(false);
            }

            if (myNavMeshAgent.enabled)
            {
                //myNavMeshAgent.Stop();
                myNavMeshAgent.isStopped = true;
            }

            currentState = struckState;

            isMeleeAttacking = false;
            npcMaster.CallEventNpcStruckAnim();
            StartCoroutine(RecoverFromStruckState());
        }

        IEnumerator RecoverFromStruckState()
        {
            yield return new WaitForSeconds(struckWaitTime);

            npcMaster.CallEventNpcRecoveredAnim();

            //Evil max.Repair gun hold
            //

            if (rangedWeapon != null)
            {
                // HARD CODED STATEMENT BELOW
                yield return new WaitForSeconds(struckWaitTime / 2);
                rangedWeapon.SetActive(true);
            } 

            if (myNavMeshAgent.enabled)
            {
                //myNavMeshAgent.Resume();
                myNavMeshAgent.isStopped = false;
            } 

            currentState = capturedState;
        }

        public void OnEnemyAttack()
        {
            //Called by melee Attack Animation.

            if (pursueTarget != null)
            {
                if (Vector3.Distance(transform.position, pursueTarget.position) <= meleeAttackRange)
                {
                    Vector3 toOther = pursueTarget.position - transform.position;
                    
                    if (Vector3.Dot(toOther, transform.forward) > .5f)
                    {
                        pursueTarget.SendMessage("CallEventPlayerHealthDeduction", meleeAttackDamage, SendMessageOptions.DontRequireReceiver);
                        pursueTarget.SendMessage("ProcessDamage", meleeAttackDamage, SendMessageOptions.DontRequireReceiver);
                        pursueTarget.SendMessage("SetMyAttacker", transform.root, SendMessageOptions.DontRequireReceiver);
                    }

                }
            }

            isMeleeAttacking = false;
        }

        //public void SetMyAttacker(Transform attacker)
        //{
        //    myAttacker = attacker;
        //}

        public void Distract(Vector3 distractionPos)
        {
            locationOfInterest = distractionPos;

            if (currentState == patrolState)
            {
                currentState = alertState;
            }
        }
	}
}
