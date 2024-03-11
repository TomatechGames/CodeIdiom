using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    //Intended for changing overall game rules based on results from player testing
    [CreateAssetMenu(menuName ="Config Container")]
    public class ConfigContainer : ScriptableObject
    {
        [field:SerializeField]
        public bool LockSlotsToInitialLetter { get; private set; }
        [field: SerializeField]
        public bool ClearedSlotsGoToInitialSlot { get; private set; }
        [field: SerializeField]
        public bool PushLettersWhenTransferringToDeck { get; private set; }
        [field: SerializeField]
        public bool SnapDraggedLetterToCursor { get; private set; }
    }
}
