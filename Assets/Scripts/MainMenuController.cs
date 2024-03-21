using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TomatechGames.CodeIdiom
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        SessionRunner sessionRunner;
        [SerializeField]
        BasePhraseGroupProvider phraseGroupProvider;
        [SerializeField]
        GameObject startButton;
        [SerializeField]
        GameObject resumeButton;
        [SerializeField]
        GameObject completeMessage;
        [SerializeField]
        UnityEvent sessionExistsWarning;

        private void Start()
        {
            startButton.SetActive(phraseGroupProvider.IsNewPhraseGroupAvailable);
            resumeButton.SetActive(sessionRunner.IncompleteSessionExists);
            completeMessage.SetActive(!resumeButton.activeSelf && !startButton.activeSelf);
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        }

        static readonly KeyCode[] resetKeys = new KeyCode[]
        {
            KeyCode.R,
            KeyCode.E,
            KeyCode.S,
            KeyCode.E,
            KeyCode.T,
        };
        int resetStage;
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(resetKeys[resetStage]))
                {
                    resetStage++;
                    if (resetStage == resetKeys.Length)
                        ResetPrefs();
                }
                else
                    resetStage = 0;
            }
        }

        public void ResetPrefs()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(0);
        }

        public void TryStartSession()
        {
            if (sessionRunner.IncompleteSessionExists)
                sessionExistsWarning?.Invoke();
            else
                StartSesion();
        }

        public void StartSesion()
        {
            sessionRunner.CreateSession(phraseGroupProvider.CurrentPhraseGroup, 0);
            gameObject.SetActive(false);
        }

        public void TryResumeSesion()
        {
            sessionRunner.ResumeSession();
            gameObject.SetActive(false);
        }
    }

    public abstract class BasePhraseGroupProvider : ScriptableObject 
    { 
        public abstract bool IsNewPhraseGroupAvailable { get; }
        public abstract PhraseGroupData CurrentPhraseGroup { get; }
        public virtual PhraseGroupData[] RecentPhraseGroups { get; }
    }
}
