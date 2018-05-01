using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPCTO_StatePattern : MonoBehaviour 
	{

        private NPC_Master npcMaster;
        private NPC_StatePattern npcStatePattern;

		void OnEnable()
		{
            SetInitialReferences();
            npcMaster.EventNpcDeath += TurnOffStatePattern;
		}

		void OnDisable()
		{
            npcMaster.EventNpcDeath -= TurnOffStatePattern;
        }

		void SetInitialReferences()
		{
            npcMaster = GetComponent<NPC_Master>();

            if (GetComponent<NPC_StatePattern>() != null)
            {
                npcStatePattern = GetComponent<NPC_StatePattern>();
            }
		}

        void TurnOffStatePattern()
        {
            if (npcStatePattern != null)
            {
                npcStatePattern.enabled = false;
            }
        }
	}
}
