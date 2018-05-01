using UnityEngine;

namespace BaseFramework
{
    [System.Serializable]
    public class NPCRelations
    {
        public string npcTag;
        public int npcFactionRating;

        NPCRelations(string npcTag, int npcFactionRating)
        {
            this.npcTag = npcTag;
            this.npcFactionRating = npcFactionRating;
        }
    }

    [System.Serializable]
    public class NPCRelationsArray
    {
        public string npcFaction;
        public NPCRelations[] npcRelations;
        public string[] myFriendlyTags;
        public string[] myEnemyTags;
        public LayerMask myFriendlyLayers;
        public LayerMask myEnemyLayers;

        NPCRelationsArray(string npcTagsOfInterest, NPCRelations[] npcRelationsArray, string[] fTags, string[] eTags, LayerMask fLayers, LayerMask eLayers)
        {
            npcFaction = npcTagsOfInterest;
            npcRelations = npcRelationsArray;
            myFriendlyTags = fTags;
            myEnemyTags = eTags;
            myFriendlyLayers = fLayers;
            myEnemyLayers = eLayers;
        }

    }
}
