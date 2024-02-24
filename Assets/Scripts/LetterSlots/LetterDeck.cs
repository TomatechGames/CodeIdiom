using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class LetterDeck : LetterSlotContainer
    {
        PhraseController phraseController;
        public void LinkToPhraseController(PhraseController phraseController) => this.phraseController = phraseController;

        public void Initialise(char[] jumbledLetters)
        {

        }

        public override void OnSlotClicked(LetterSlot clickedSlot)
        {
            throw new System.NotImplementedException();
        }

        public override void TransferLetterToContainer(LetterInstance letter)
        {
            throw new System.NotImplementedException();
        }
    }
}
