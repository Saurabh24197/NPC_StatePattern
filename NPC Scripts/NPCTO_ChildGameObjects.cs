using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class NPCTO_ChildGameObjects : MonoBehaviour
    {

        private NPC_Master npcMaster;

		[Header("Turns these GOs off onNPCDeath")]
        public GameObject[] turnOffGO;
        public GameObject[] destroyGO;

        public float destroyWaitTime = 0f;

        private void OnEnable()
        {
            npcMaster = GetComponent<NPC_Master>();

            npcMaster.EventNpcDeath += TurnOffChild;
            npcMaster.EventNpcDeath += DestroyGO;
        }

        private void OnDisable()
        {
            npcMaster.EventNpcDeath -= TurnOffChild;
            npcMaster.EventNpcDeath -= DestroyGO;
        }

        void TurnOffChild()
        {
            foreach (GameObject child in turnOffGO)
            {
                if (child != null)
                {
                    child.SetActive(false);
                }
            }
        }

        void DestroyGO()
        {
            foreach (GameObject child in destroyGO)
            {
                if (child != null)
                {
                    Destroy(child, destroyWaitTime);
                }
            }
        }
    }
}


