using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class NPC_ScoresUpdate : MonoBehaviour
    {
        private NPC_Master npcMaster;
        private ScoreManager scoreManager;
		private bool isRegistered = false;

        void OnEnable()
        {
            SetInitialReferences();
            AddEnemyToTotalScore();
            npcMaster.EventNpcDeath += UpdateScores;
        }

        void OnDisable()
        {
            npcMaster.EventNpcDeath -= UpdateScores;
        }

        void SetInitialReferences()
        {
            npcMaster = transform.root.GetComponent<NPC_Master>();
			scoreManager = GameManager_References._player.GetComponent<ScoreManager> ();
        }

        private void UpdateScores()
        {
            if (scoreManager != null)
            {
                scoreManager.currentScore++;
                this.enabled = false;
            }
        }

        private void AddEnemyToTotalScore()
        {
            if (isRegistered)
            {
                return;
            }

            if (scoreManager != null)
            {
                scoreManager.totalScore++;
                scoreManager.enemyCount++;

                isRegistered = true;
            }

            else
            {
                StartCoroutine(AddEnemyAfterWait());
            } 
        }

        IEnumerator AddEnemyAfterWait()
        {
            //Debug.Log("NPC on wait");
            yield return new WaitForSeconds(0.1f);
            AddEnemyToTotalScore();
        }
    }
}

