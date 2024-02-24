using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class PhraseController : LetterSlotContainer
    {
        LetterDeck letterDeck;
        public void LinkToDeck(LetterDeck letterDeck) => this.letterDeck = letterDeck;

        public void ClearSlots()
        {
            var currentSlot = FirstSlot;
            while (currentSlot)
            {
                letterDeck.TransferLetterToContainer(currentSlot.SlottedLetter);
                currentSlot = currentSlot.NextSlot;
            }
        }

        public void Initialise(string initialPhrase, string currentPhrase)
        {

        }

        public override void OnSlotClicked(LetterSlot clickedSlot)
        {
            throw new System.NotImplementedException();
        }

        public string ReadCurrentPhrase()
        {
            return "";
        }

        public override void TransferLetterToContainer(LetterInstance letter)
        {
            throw new System.NotImplementedException();
        }
    }
}
