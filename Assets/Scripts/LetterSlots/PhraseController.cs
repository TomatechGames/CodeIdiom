using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class PhraseController : LetterSlotContainer
    {
        LetterDeck letterDeck;
        public void LinkToDeck(LetterDeck letterDeck) => this.letterDeck = letterDeck;
        LetterSlot selectedSlot;

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
        //(should be called AFTER the initialise method of letterDeck)
        public void Initialise(string initialPhrase, string currentPhrase)
        {

        }

        public override void OnSlotClicked(LetterSlot clickedSlot)
        {
            if (selectedSlot)
                selectedSlot.KeepHighlightWhenNotDragging = false;

            //if slot is empty, set selected slot. if slot is selected and has letter, transfer letter to letter deck
            if (selectedSlot && selectedSlot.SlottedLetter)
                letterDeck.TransferLetterToThisContainer(selectedSlot.SlottedLetter);
            else
                selectedSlot = clickedSlot;
            
            if(selectedSlot)
                selectedSlot.KeepHighlightWhenNotDragging = true;
        }

        public string ReadCurrentPhrase()
        {
            return "";
        }

        public override void TransferLetterToThisContainer(LetterInstance letter)
        {
            if(selectedSlot)
                selectedSlot.TrySetOrSwapSlottedLetter(letter);
            else
            {
                var emptySlot = FirstSlot.GetNextEmptySlot();
                emptySlot.Value.slot.TrySetOrSwapSlottedLetter(letter);
            }
        }
    }
}
