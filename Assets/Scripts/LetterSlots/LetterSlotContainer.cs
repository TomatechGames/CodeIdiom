using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public abstract class LetterSlotContainer : MonoBehaviour
    {
        public LetterSlot FirstSlot { get; protected set; }
        public LetterSlot LastSlot { get; protected set; }


        public abstract void OnSlotClicked(LetterSlot clickedSlot);
        public abstract void TransferLetterToThisContainer(LetterInstance letter);
    }
}
