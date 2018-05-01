using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// // A Set of scripts which Turns off entities.
/// </summary>

namespace BaseFramework
{
	public class NPCTO_NavMeshAgent : MonoBehaviour 
	{
        
        private NPC_Master npcMaster;
        private NavMeshAgent myNavMeshAgent;

		void OnEnable()
		{
            SetInitialReferences();
            npcMaster.EventNpcDeath += TurnOffNavmeshAgent;
		}

		void OnDisable()
		{
            npcMaster.EventNpcDeath -= TurnOffNavmeshAgent;
        }

		void SetInitialReferences()
		{
            npcMaster = GetComponent<NPC_Master>();

            if (GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
		}

        void TurnOffNavmeshAgent()
        {
            if (myNavMeshAgent != null) myNavMeshAgent.enabled = false;
        }

		
	}
}
