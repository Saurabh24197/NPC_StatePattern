using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPC_TakeDamage : MonoBehaviour 
	{

        private NPC_Master npcMaster;

        public int damageMultiplier = 1;
        public bool shouldRemoveCollider;

		void OnEnable()
		{
            SetInitialReferences();
            npcMaster.EventNpcDeath += RemoveThis;
		}

		void OnDisable()
		{
            npcMaster.EventNpcDeath -= RemoveThis;
        }


		void SetInitialReferences()
		{
            npcMaster = transform.root.GetComponent<NPC_Master>();
		}

        public void ProcessDamage(int damage)
        {
            int damageToApply = damage * damageMultiplier;

			if (npcMaster == null) npcMaster = transform.root.GetComponent<NPC_Master>();
            npcMaster.CallEventNpcDeductHealth(damageToApply);
        }

        void RemoveThis()
        {
            if (shouldRemoveCollider)
            {
                if (GetComponent<Collider>() != null) Destroy(GetComponent<Collider>());
                if (GetComponent<Rigidbody>() != null) Destroy(GetComponent<Rigidbody>());
            }

            //To Stop the AI's detection.
            gameObject.layer = LayerMask.NameToLayer("Default");
            Destroy(this);
        }

		
	}
}
