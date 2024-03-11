using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        SessionRunner sessionRunner;
        [SerializeField]
        PhraseGroupData phraseGroupData;


        public void StartSesion()
        {
            sessionRunner.CreateSession(phraseGroupData, 0);
            gameObject.SetActive(false);
        }

        public void TryResumeSesion()
        {
            sessionRunner.ResumeSession();
            gameObject.SetActive(false);
        }
    }
}
