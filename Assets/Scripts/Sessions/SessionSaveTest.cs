using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class SessionSaveTest : MonoBehaviour
    {
        public SessionData sessionData;
        public string sessionJson;
        [ContextMenu("stringSave")]
        public void SaveJson()
        {
            sessionJson = JsonUtility.ToJson(sessionData);
        }
        [ContextMenu("stringLoad")]
        public void LoadJson()
        {
            sessionData = JsonUtility.FromJson<SessionData>(sessionJson); 
        }
    }
}
