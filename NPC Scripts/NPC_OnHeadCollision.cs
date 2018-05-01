using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class NPC_OnHeadCollision : MonoBehaviour
    {
        private bool onHold = false;
        private NPC_StatePattern npcStatePattern;
        private NPC_Master npcMaster;

        [Header("Attach this Script to Head (or Above) GameObject")]
        public Transform head;
        public int damageToApply = 0;
        public float waitTime = 1f;

        private void Start()
        {
            npcStatePattern = gameObject.transform.root.GetComponent<NPC_StatePattern>();
            npcMaster = gameObject.transform.root.GetComponent<NPC_Master>();
            head = (head == null) ? npcStatePattern.head : head;
        }
        private void FixedUpdate()
        {
            if (!onHold)
                SphereCastAbove();
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.DrawSphere(gameObject.transform.position, .5f);
        //}

        void SphereCastAbove()
        {
            Collider[] colliders = Physics.OverlapSphere(head.transform.position, .5f);
            Collider collider = Array.Find(colliders, col => col.tag == GameManager_References._playerTag);

            if (collider != null)
            {
                npcMaster.CallEventNpcDeductHealth(damageToApply);
                onHold = true;
                StartCoroutine(ToggleOnHold());
            }
        }

        IEnumerator ToggleOnHold()
        {
            yield return new WaitForSeconds(waitTime);
            onHold = !onHold;
        }

    }
}

