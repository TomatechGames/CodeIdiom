using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class LetterDeck : LetterSlotContainer
    {
        [SerializeField]
        ConfigContainer config;
        ConfigContainer Config => config ? config : (config = FindObjectOfType<ConfigContainer>());

        PhraseController phraseController;
        public void LinkToPhraseController(PhraseController phraseController) => this.phraseController = phraseController;

        //creates slots with initial letters based on the chars in jumbledLetters
        //(should be called BEFORE the initialise method of phraseController)
        public void Initialise(char[] jumbledLetters)
        {

        }

        public override void OnSlotClicked(LetterSlot clickedSlot)
        {
            if (clickedSlot.SlottedLetter)
                phraseController.TransferLetterToThisContainer(clickedSlot.SlottedLetter);
        }

        public override void TransferLetterToThisContainer(LetterInstance letter)
        {
            if (Config.clearedSlotsGoToInitialSlot || Config.lockSlotsToInitialLetter)
                letter.InitialSlot.TrySetOrSwapSlottedLetter(letter);
            else
            {
                var emptySlot = FirstSlot.GetNextEmptySlot();
                emptySlot.Value.slot.TrySetOrSwapSlottedLetter(letter);
            }
        }
    }
}
