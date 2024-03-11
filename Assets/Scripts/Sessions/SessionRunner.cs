using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{

    public class SessionRunner : MonoBehaviour
    {
        public static event Action<string, string> OnTimeUpdate;

        [SerializeField]
        CanvasGroup canvasGroup;
        [SerializeField]
        PhraseController phrasePanelPrefab;
        [SerializeField]
        Transform phrasePanelParent;

        List<PhraseController> currentControllers = new();

        bool sessionActive;
        static SessionData currentSession;

        public static int CurrentGameTime => currentSession.timeInSession;
        public static DateTime sessionStartTime;
        public static DateTime SessionStartTime => sessionStartTime;

        int selectedPhrase = 0;

        bool timerRunning;
        async void TimerLoop()
        {
            if (timerRunning)
                return;
            timerRunning = true;
            await Task.Delay(1);
            while (true)
            {
                if (!sessionActive)
                    break;
                OnTimeUpdate?.Invoke(CurrentGameTime.FormatToTimeInSeconds(), ((int)(DateTime.UtcNow - SessionStartTime).TotalSeconds).FormatToTimeInSeconds());
                await Task.Delay(1000);
                currentSession.timeInSession++;
            }
            timerRunning = false;
        }

        public void CreateSession(PhraseGroupData phraseGroupData, int difficulty)
        {
            SessionData newSession = new()
            {
                difficulty = difficulty,
                startedAtTime = DateTime.UtcNow,
                phraseGroup = phraseGroupData,
                timeInSession = 0,
                sessionPhraseData = new SessionPhraseData[phraseGroupData.phrases.Length]
            };

            for (int i = 0; i < newSession.sessionPhraseData.Length; i++)
            {
                newSession.sessionPhraseData[i] = new();
            }

            ResumeSession(newSession);
        }

        public bool IncompleteSessionExists => PlayerPrefs.HasKey("session");

        public void ResumeSession()
        {
            if (IncompleteSessionExists)
                ResumeSession(JsonUtility.FromJson<SessionData>(PlayerPrefs.GetString("session")));
        }

        public void ResumeSession(SessionData sessionData)
        {
            currentSession = sessionData;
            sessionActive = true;
            sessionStartTime = sessionData.startedAtTime;
            currentControllers.ForEach(controller => Destroy(controller));
            currentControllers.Clear();

            for (int i = 0; i < Mathf.Min(sessionData.phraseGroup.phrases.Length, sessionData.sessionPhraseData.Length); i++)
            {
                var phraseData = sessionData.phraseGroup.phrases[i];
                var sessionPhraseData = sessionData.sessionPhraseData[i];

                var spawnedController = Instantiate(phrasePanelPrefab, phrasePanelParent);
                spawnedController.Initialise(phraseData, sessionPhraseData, phraseData.phrase.JumbleLetters());
                spawnedController.SetState(2);

                currentControllers.Add(spawnedController);
            }
            currentControllers[0].SetState(1);
            TimerLoop();
            SaveSession();
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        public void SaveSession()
        {
            //update session data
            for (int i = 0; i < currentSession.sessionPhraseData.Length; i++)
            {
                currentSession.sessionPhraseData[i] = currentControllers[i].SessionPhraseData;
            }

            PlayerPrefs.SetString("session", JsonUtility.ToJson(currentSession));
        }

        public void ChangePhrase(int difference)
        {
            var prevController = currentControllers[selectedPhrase];
            selectedPhrase += difference;
            selectedPhrase = Mathf.Clamp(selectedPhrase, 0, currentControllers.Count - 1);

            prevController.SetState(difference < 0 ? 0 : 2);
            currentControllers[selectedPhrase].SetState(1);
            SaveSession();
        }

        private void OnApplicationPause(bool pause)
        {
            if(pause)
                SaveSession();
        }

        private void OnApplicationQuit()
        {
            SaveSession();
        }
    }
}
