using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class LetterInstance : MonoBehaviour
    {
        public char AssociatedChar {  get; private set; }
        public LetterSlot InitialSlot { get; private set; }
        public LetterSlot CurrentSlot { get; private set; }

        public void Initialise(char associatedChar)
        {
            AssociatedChar = associatedChar;
        }

        public void SetSlot(LetterSlot newSlot)
        {
            CurrentSlot = newSlot;
            if (!InitialSlot)
                InitialSlot = CurrentSlot;
        }

        public void ClickSlot()
        {
            if (CurrentSlot)
                CurrentSlot.ClickSlot();
        }
    }
}
