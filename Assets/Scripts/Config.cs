using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class Config : ScriptableObject
    {
        public readonly bool lockSlotsToInitialLetter;
        public readonly bool clearedSlotsGoToInitialSlot;
        public readonly bool pushLettersWhenTransferringToDeck;
    }
}
