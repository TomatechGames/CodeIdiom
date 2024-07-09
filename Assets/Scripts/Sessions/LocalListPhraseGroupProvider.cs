using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    [CreateAssetMenu]
    public class LocalListPhraseGroupProvider : BasePhraseGroupProvider
    {
        [SerializeField]
        List<PhraseGroupData> possiblePhraseGroups;
        public override bool IsNewPhraseGroupAvailable => PlayerPrefs.HasKey("listLastRetrievedAt") ? (DateTime.UtcNow - JsonUtility.FromJson<JsonDateTime>(PlayerPrefs.GetString("listLastRetrievedAt"))).Hours > 23 : true;
        public override PhraseGroupData CurrentPhraseGroup
        {
            get
            {
                if (IsNewPhraseGroupAvailable)
                {
                    PlayerPrefs.SetString("listLastRetrievedAt", JsonUtility.ToJson((JsonDateTime)DateTime.UtcNow.Date));
                }
                int currentDay = (DateTime.UtcNow.Date - DateTime.UnixEpoch.Date).Days % possiblePhraseGroups.Count;
                return possiblePhraseGroups[currentDay];
            }
        }
        public override PhraseGroupData GetByIndex(int index)
        {
            return possiblePhraseGroups[index % possiblePhraseGroups.Count];
        }
    }
}
