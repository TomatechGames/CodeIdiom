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
        public PhraseGroupData phraseGroup;
        public int timeInSession;
        public JsonDateTime startedAtTime;
        public SessionPhraseData[] sessionPhraseData;
    }
    [Serializable]
    public class SessionPhraseData
    {
        public string currentPhrase;
        public JsonDateTime submittedAtTime;
        public int submissionDuration;
    }

    //Source: lilotop on stackexchange.com
    [Serializable]
    public struct JsonDateTime
    {
        public long value;
        public static implicit operator DateTime(JsonDateTime jdt)
        {
            Debug.Log("Converted to time");
            return DateTime.FromFileTimeUtc(jdt.value);
        }
        public static implicit operator JsonDateTime(DateTime dt)
        {
            Debug.Log("Converted to JDT");
            JsonDateTime jdt = new JsonDateTime();
            jdt.value = dt.ToFileTimeUtc();
            return jdt;
        }
    }
}

