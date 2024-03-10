using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        PhraseGroupData phraseGroupData;

        public void StartSesion()
        {
            Debug.Log("Start buuton works");
        }
        public void TryResumeSesion()
        {
            Debug.Log("Resuume buuton works");
        }
    }
}
