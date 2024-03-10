using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//made by wisdom 
namespace TomatechGames.CodeIdiom
{
    [Serializable]
    public class PhraseGroupData
    {
        public string linkTerm;
        public string hint;
        public PhraseData[] phrases;

    }
    [Serializable]
    public class PhraseData
    {
        public string phrase;
        public string description;
        public string ctxLink;
    }
    [Serializable]
    public class SessionData
    {
        public int difficulty;
        public PhraseGroupData[] phraseGroup;
        public int timeInSession;
        public DateTime stratedAtTime;
        public SessionPhraseData[] sessionPhraseData;
    }
    [Serializable]
    public class SessionPhraseData
    {
        public string currentPhrase;
        public DateTime submittedAtTime;
        public int submissionDuration;
    }
}
