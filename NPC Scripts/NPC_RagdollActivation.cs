using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace BaseFramework
{
	public class NPC_RagdollActivation : MonoBehaviour 
	{

        private NPC_Master npcMaster;
        private Rigidbody myRigidbody;
        private Collider myCollider;

		void OnEnable()
		{
            SetInitialReferences();
            npcMaster.EventNpcDeath += ActivateRagdoll;
		}

		void OnDisable()
		{
            npcMaster.EventNpcDeath -= ActivateRagdoll;
        }

		void SetInitialReferences()
		{
            npcMaster = transform.root.GetComponent<NPC_Master>();

            if (GetComponent<Collider>() != null) myCollider = GetComponent<Collider>();
            if (GetComponent<Rigidbody>() != null) myRigidbody = GetComponent<Rigidbody>();
		}

		void ActivateRagdoll()
        {
            if (myCollider != null)
            {
                myCollider.enabled = true;
                myCollider.isTrigger = false;

				//TurnRagdollToObstacle();
            }

            if (myRigidbody != null)
            {
                myRigidbody.isKinematic = false;
                myRigidbody.useGravity = true;
            }

            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        void TurnRagdollToObstacle()
        {
            if (gameObject.GetComponent<NavMeshObstacle>() == null)
            {
                gameObject.AddComponent<NavMeshObstacle>();
            }
            
            NavMeshObstacle navMeshObstacle = gameObject.GetComponent<NavMeshObstacle>();

            navMeshObstacle.shape = NavMeshObstacleShape.Box;
            navMeshObstacle.center = (myCollider is BoxCollider) ? (myCollider as BoxCollider).center : (myCollider as CapsuleCollider).center;

            Vector3 size = (myCollider is BoxCollider) ? (myCollider as BoxCollider).size : new Vector3((myCollider as CapsuleCollider).height/2f, (myCollider as CapsuleCollider).height/2f, (myCollider as CapsuleCollider).height/2f);
            navMeshObstacle.size = size;

            //navMeshObstacle.carveOnlyStationary = true;
            //navMeshObstacle.carvingTimeToStationary = .01f;
            //navMeshObstacle.carving = true;
        }
	}
}
