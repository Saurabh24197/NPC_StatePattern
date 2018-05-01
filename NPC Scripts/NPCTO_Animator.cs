using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// // A Set of scripts which Turns off entities.
/// </summary>

namespace BaseFramework
{
	public class NPCTO_Animator : MonoBehaviour 
	{
        

        private NPC_Master npcMaster;
        private Animator myAnimator;

		void OnEnable()
		{
            SetInitialReferences();
            npcMaster.EventNpcDeath += TurnOffAnimator;
		}

		void OnDisable()
		{
            npcMaster.EventNpcDeath -= TurnOffAnimator;
        }

        void SetInitialReferences()
        {
            npcMaster = GetComponent<NPC_Master>();

            if (GetComponent<Animator>() != null)
            {
                myAnimator = GetComponent<Animator>();
            }
		}

        void TurnOffAnimator()
        {
            if (myAnimator != null) myAnimator.enabled = false;
        }
        
        	
	}
}
