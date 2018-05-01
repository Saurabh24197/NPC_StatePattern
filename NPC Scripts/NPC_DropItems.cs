using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPC_DropItems : MonoBehaviour 
	{

        private NPC_Master npcMaster;
        private NPC_StatePattern npcStatePattern;

        public bool instItem = true;

        public GameObject[] ItemDrops;
        public GameObject[] ammoDrops;

        public bool randomizeAmmo = false;

        public GameObject objPause;
        public Vector3 offsetPos;

		void OnEnable()
		{
            SetInitialReferences();
            npcMaster.EventNpcDeath += DropItems;
		}

		void OnDisable()
		{
            npcMaster.EventNpcDeath -= DropItems;
        }

		void SetInitialReferences()
		{
            npcMaster = GetComponent<NPC_Master>();

            if (GetComponent<NPC_StatePattern>() != null)
            {
                npcStatePattern = GetComponent<NPC_StatePattern>();
            }
		}

        void DropItems()
        {
            if (ItemDrops.Length > 0)
            {
                foreach (GameObject item in ItemDrops)
                {
                    StartCoroutine(PauseBeforeDrop(item));
                    //If not coroutine then event gets fired off before the start method on Item master can run.
                }
            }

            if (ammoDrops.Length > 0)
            {
                foreach (GameObject ammo in ammoDrops)
                {
                    if (randomizeAmmo)
                    {
                        if (Random.value > 0.5f) { StartCoroutine(PauseBeforeDrop(ammo)); }
                    }

                    else StartCoroutine(PauseBeforeDrop(ammo));
                }
            }
        }

        void InstanciateItem(GameObject itemToDrop)
        {
            //itemToDrop.GetComponent<Gun_Master>() is probably null So just Instanciate
            GameObject go = Instantiate(itemToDrop, transform.root.position, transform.root.rotation);

            if (go.GetComponent<Rigidbody>() != null)
            {
                float vectorVal = (Random.value > 0.5f) ? Random.Range(0, 10) : Random.Range(-10, 0);
                Vector3 itemDropForce = new Vector3(vectorVal, vectorVal, vectorVal);
                go.GetComponent<Rigidbody>().AddForce(itemDropForce, ForceMode.Impulse);
            }
        }

		IEnumerator PauseBeforeDrop(GameObject itemToDrop)
        {
            if (itemToDrop == null) yield break;
            yield return new WaitForSeconds(.05f);

            Vector3 instancePosition = gameObject.transform.position + offsetPos;
            itemToDrop.transform.parent = null;
            itemToDrop.transform.position = instancePosition;
            itemToDrop.SetActive(true);

            yield return new WaitForSeconds(.05f);

            //Item Instanciate.
            if (instItem)
            {
                InstanciateItem(itemToDrop);
                yield break;
            }

            //Item Throw.
            if (itemToDrop.GetComponent<Gun_Master>() != null)
            {
                itemToDrop.GetComponent<Animator>().enabled = false;

                if (itemToDrop == npcStatePattern.rangedWeapon)
                {
                    GameObject objInstance = null;

                    //Limits to Only 1 weapon for enemy no?
                    if (objPause != null)
                    {
                        float maxScale = Mathf.Max(itemToDrop.transform.localScale.x, Mathf.Max(itemToDrop.transform.localScale.y, itemToDrop.transform.localScale.z));
                        maxScale = Mathf.CeilToInt(maxScale);
                        objPause.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
                        objInstance = Instantiate(objPause, itemToDrop.transform.position, objPause.transform.rotation);
                    }

                    yield return new WaitForSeconds(3f);
                    Destroy(objInstance);
                }

                //The Object that should show is missing so just ObjThrow.
                itemToDrop.GetComponent<Item_Master>().CallEventObjectThrow();
                yield break;
            }

            //Default Case : Item Instanciate
            InstanciateItem(itemToDrop);
        }


	}
}
