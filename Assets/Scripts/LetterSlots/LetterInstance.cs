using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TomatechGames.CodeIdiom
{
    public class LetterInstance : MonoBehaviour
    {
        public char AssociatedChar {  get; private set; }
        public LetterSlot InitialSlot { get; private set; }
        public LetterSlot CurrentSlot { get; private set; }

        [SerializeField]
        UnityEvent<string> onLetterChanged;

        //when the letter is instantiated, this should be called to assign it a char
        //if CurrentSlot exists during initialisation, it is stored as InitialSlot
        public void Initialise(char associatedChar)
        {
            AssociatedChar = associatedChar;
            onLetterChanged?.Invoke(AssociatedChar.ToString());
            InitialSlot = CurrentSlot;
        }

        public void SetSlot(LetterSlot newSlot)
        {
            CurrentSlot = newSlot;
            if (!InitialSlot)
                InitialSlot = CurrentSlot;
            transform.SetParent(CurrentSlot.LetterInstanceParent);
            transform.localPosition = Vector3.zero;
            //TODO: animate to new origin
        }

        //forwards the click event of a letter to it's slot
        public void ClickSlot()
        {
            if (CurrentSlot)
                CurrentSlot.ClickSlot();
        }
    }
}
