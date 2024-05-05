using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace TomatechGames.CodeIdiom
{
    public class PhraseController : LetterSlotContainer
    {
        [SerializeField]
        LetterDeck letterDeck;
        [SerializeField]
        Transform wordParent;
        [SerializeField]
        GameObject wordPrefab;
        [SerializeField]
        LetterSlot letterSlotPrefab;
        [SerializeField]
        UnityEvent<int> onAnimStateUpdated;

        [SerializeField]
        UnityEvent<string, string> onPreviewPanelDataChanged;

        [SerializeField]
        UnityEvent<string> onRealTimeUpdated;
        [SerializeField]
        UnityEvent<string> onGameTimeUpdated;
        [SerializeField]
        UnityEvent<string> onSubmitActivated;

        public LetterSlot SelectedSlot { get; private set; }

        
        PhraseData phraseData;
        SessionPhraseData sessionPhraseData;
        public SessionPhraseData SessionPhraseData
        {
            get
            {
                sessionPhraseData.currentPhrase = ReadCurrentPhrase();
                return sessionPhraseData;
            }
        }

        private void Start()
        {
            SessionRunner.OnTimeUpdate += (gameTime, realTime) =>
            {
                if (sessionPhraseData.submissionDuration > 0)
                    return;

                onRealTimeUpdated.Invoke(realTime);
                onGameTimeUpdated.Invoke(gameTime);
            };
        }

        bool isPreviewReady;
        public void UpdatePreviewPanel()
        {
            if (!isPreviewReady)
            {
                return;
            }
            var phrase = ReadCurrentPhrase();
            var remainder = letterDeck.GetDeckLetters();
            Debug.Log(phrase+" <> "+remainder);
            onPreviewPanelDataChanged?.Invoke(phrase, remainder);
        }

        //0 is close to left, 1 is open, 2 is close to right
        public void SetState(int state)
        {
            onAnimStateUpdated.Invoke(state);
        }

        public void SubmitPhrase()
        {
            sessionPhraseData.submissionDuration = SessionRunner.CurrentGameTime;
            sessionPhraseData.submittedAtTime = DateTime.UtcNow;
            sessionPhraseData.currentPhrase = ReadCurrentPhrase();
            ActivateSubmission();
        }

        public void ActivateSubmission()
        {
            onRealTimeUpdated.Invoke(((int)(sessionPhraseData.submittedAtTime - SessionRunner.SessionStartTime).TotalSeconds).FormatToTimeInSeconds());
            onGameTimeUpdated.Invoke(sessionPhraseData.submissionDuration.FormatToTimeInSeconds());
            onSubmitActivated.Invoke(
                (sessionPhraseData.currentPhrase==phraseData.phrase.ToUpper()) ?
                "Submission is correct!" :
                //$"Submission is incorrect. Answer is:\n\"{phraseData.phrase}\""
                "Submission is incorrect"
                );
        }

        public void ClearSlots()
        {
            var currentSlot = FirstSlot;
            while (currentSlot)
            {
                letterDeck.TransferLetterToThisContainer(currentSlot.SlottedLetter);
                currentSlot = currentSlot.NextSlot;
            }
        }

        //sets up the blank slots based on the value of initialPhrase, and
        //moves letters from the deck into the phrase based on the value of currentPhrase
        public void Initialise(PhraseData phraseData, SessionPhraseData sessionPhraseData, string deckLetters)
        {
            letterDeck.Initialise(deckLetters);
            sessionPhraseData.currentPhrase ??= "";
            this.phraseData = phraseData;
            this.sessionPhraseData = sessionPhraseData;

            if (sessionPhraseData.submissionDuration>0)
            {
                ActivateSubmission();
            }

            var wordInstance = Instantiate(wordPrefab, wordParent);
            bool nextLetterIsStartOfWord = true;

            LetterSlot twoSlotsAgo = null;
            LetterSlot prevSlot = null;
            FirstSlot = null;
            for (int i = 0; i < phraseData.phrase.Length; i++)
            {
                char thisChar = phraseData.phrase[i];
                if(thisChar==' ')
                {
                    wordInstance = Instantiate(wordPrefab, wordParent);
                    nextLetterIsStartOfWord = true;
                    continue;
                }

                var letterSlot = Instantiate(letterSlotPrefab, wordInstance.transform);

                letterSlot.Initialise(this, nextLetterIsStartOfWord);

                if (sessionPhraseData.currentPhrase.Length>i && sessionPhraseData.currentPhrase[i] != '_')
                {
                    var matchedLetter = letterDeck.GetFirstAppearanceOfLetter(sessionPhraseData.currentPhrase[i]);
                    letterSlot.TrySetOrSwapSlottedLetter(matchedLetter);
                }

                nextLetterIsStartOfWord = false;


                if (prevSlot)
                {
                    prevSlot.LinkSlots(twoSlotsAgo, letterSlot);
                }
                twoSlotsAgo = prevSlot;
                prevSlot = letterSlot;

                if(!FirstSlot)
                    FirstSlot = letterSlot;
            }
            if (prevSlot)
            {
                prevSlot.LinkSlots(twoSlotsAgo, null);
                LastSlot = prevSlot;
            }
            isPreviewReady = true;
            UpdatePreviewPanel();
        }

        public override void OnSlotClicked(LetterSlot clickedSlot)
        {
            //if slot is empty, set selected slot. if slot is selected and has letter, transfer letter to letter deck
            if (clickedSlot==SelectedSlot && SelectedSlot.SlottedLetter)
                letterDeck.TransferLetterToThisContainer(SelectedSlot.SlottedLetter);
            else
                SetSelectedSlot(clickedSlot);
        }

        void SetSelectedSlot(LetterSlot newSelection)
        {
            if (SelectedSlot)
                SelectedSlot.KeepHighlightWhenNotDragging = false;
            SelectedSlot = newSelection;
            if (SelectedSlot)
                SelectedSlot.KeepHighlightWhenNotDragging = true;
        }

        public string ReadCurrentPhrase()
        {
            StringBuilder builder = new();
            var currentSlot = FirstSlot;
            while (currentSlot)
            {
                if (currentSlot.IsStartOfWord && currentSlot!= FirstSlot)
                    builder.Append(' ');

                if (!currentSlot.SlottedLetter)
                    builder.Append('_');
                else
                    builder.Append(currentSlot.SlottedLetter.AssociatedChar);
                currentSlot = currentSlot.NextSlot;
            }
            return builder.ToString();
        }

        public override void TransferLetterToThisContainer(LetterInstance letter)
        {
            if (SelectedSlot)
            {
                SelectedSlot.TrySetOrSwapSlottedLetter(letter);
                var nextEmpty = SelectedSlot.GetNextEmptySlot();
                SetSelectedSlot(nextEmpty.HasValue ? nextEmpty.Value.slot : null);
            }
            else
            {
                var emptySlot = FirstSlot.GetNextEmptySlot();
                if (emptySlot.HasValue)
                {
                    emptySlot.Value.slot.TrySetOrSwapSlottedLetter(letter);
                    var nextEmpty = emptySlot.Value.slot.GetNextEmptySlot();
                    SetSelectedSlot(nextEmpty.HasValue ? nextEmpty.Value.slot : null);
                }
            }
        }
    }
}
