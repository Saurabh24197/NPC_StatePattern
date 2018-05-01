using System.Collections;
using UnityEngine;

namespace BaseFramework
{
    public class GameManager_NPCRelationsMaster : MonoBehaviour
    {
        public delegate void NPCRelationChangeEventHandler(string factionAffected, string factionCausing, int adjustment, bool chain);
        public event NPCRelationChangeEventHandler EventNPCRelationChange;

        public delegate void UpdateNPCRelationsEventHandler();
        public event UpdateNPCRelationsEventHandler EventUpdateNPCRelationsEverywhere;

        public int hostileThreshold = 40;
        public NPCRelationsArray[] npcRelationsArray;

        public void CallEventNPCRelationChange(string factionAffected, string factionCausingChange, int relationChangeAmt, bool applyChainEffect)
        {
            if (EventNPCRelationChange != null)
            {
                EventNPCRelationChange(factionAffected, factionCausingChange, relationChangeAmt, applyChainEffect);
            }
        }

        public void CallEventUpdateNPCRelationsEverywhere()
        {
            if (EventUpdateNPCRelationsEverywhere != null)
            {
                EventUpdateNPCRelationsEverywhere();
            }
        }
    }
}

