using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPC_CollisionField : MonoBehaviour 
	{

        private NPC_Master npcMaster;
        private Rigidbody rigidbodyStrikingMe;

        private int damageToApply;
        public float massRequirement = 50;
        public float speedRequirement = 5;
        private float damageFactor = 0.1f;

		void OnEnable()
		{
            SetInitialReferences();
            npcMaster.EventNpcDeath += DisableThisGO;
            
		}

		void OnDisable()
		{
            npcMaster.EventNpcDeath -= DisableThisGO;
        }

        void OnTriggerEnter(Collider other)
        {

            if (other.GetComponent<Rigidbody>() != null)
            {
                rigidbodyStrikingMe = other.GetComponent<Rigidbody>();

                if (rigidbodyStrikingMe.mass >= massRequirement
                    && rigidbodyStrikingMe.velocity.sqrMagnitude >= speedRequirement * speedRequirement)
                {
                    damageToApply = (int)(rigidbodyStrikingMe.mass * rigidbodyStrikingMe.velocity.magnitude * damageFactor);
                    npcMaster.CallEventNpcDeductHealth(damageToApply);
                }
            }
        }

		void SetInitialReferences()
		{
            npcMaster = transform.root.GetComponent<NPC_Master>();
		}

        void DisableThisGO()
        {
            gameObject.SetActive(false);
        }

		
	}
}
