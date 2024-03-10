using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    //Intended for changing overall game rules based on results from player testing
    public class ConfigContainer : ScriptableObject
    {
        public readonly bool lockSlotsToInitialLetter;
        public readonly bool clearedSlotsGoToInitialSlot;
        public readonly bool pushLettersWhenTransferringToDeck;
    }
}
