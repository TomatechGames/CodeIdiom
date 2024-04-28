using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class LetterDeck : LetterSlotContainer 
    {
        [SerializeField]
        ConfigContainer config;
        ConfigContainer Config => config ? config : (config = FindObjectOfType<ConfigContainer>());

        [SerializeField]
        PhraseController phraseController;
        [SerializeField]
        LetterSlot letterSlotPrefab;
        [SerializeField]
        LetterInstance letterInstancePrefab;
        [SerializeField]
        Transform slotParent;

        //creates slots with initial letters based on the chars in jumbledLetters
        public void Initialise(string jumbledLetters)
        {
            LetterSlot twoSlotsAgo = null;
            LetterSlot prevSlot = null;
            FirstSlot = null;
            for (int i = 0; i < jumbledLetters.Length; i++)
            {
                var letterSlot = Instantiate(letterSlotPrefab, slotParent);
                var letterInstance = Instantiate(letterInstancePrefab, slotParent);
                letterInstance.Initialise(jumbledLetters[i]);
                letterInstance.onReslotted += phraseController.UpdatePreviewPanel;
                letterSlot.TrySetOrSwapSlottedLetter(letterInstance);
                letterSlot.Initialise(this);
                

                if (prevSlot)
                {
                    prevSlot.LinkSlots(twoSlotsAgo, letterSlot);
                }
                twoSlotsAgo = prevSlot;
                prevSlot = letterSlot;
                if (!FirstSlot)
                    FirstSlot = letterSlot;
            }
            if (prevSlot)
            {
                prevSlot.LinkSlots(twoSlotsAgo, null);
                LastSlot = prevSlot;
            }
        }

        public string GetDeckLetters()
        {
            StringBuilder builder = new();
            var currentSlot = FirstSlot;
            while (currentSlot)
            {
                if (currentSlot.SlottedLetter)
                    builder.Append(currentSlot.SlottedLetter.AssociatedChar);
                currentSlot = currentSlot.NextSlot;
            }
            return builder.ToString();
        }

        public LetterInstance GetFirstAppearanceOfLetter(char withChar)
        {
            var slotToCheck = FirstSlot;
            while (slotToCheck && (!slotToCheck.SlottedLetter || slotToCheck.SlottedLetter.AssociatedChar != withChar))
                slotToCheck = slotToCheck.NextSlot;
            return slotToCheck ? slotToCheck.SlottedLetter : null;
        }

        public override void OnSlotClicked(LetterSlot clickedSlot)
        {
            if (clickedSlot.SlottedLetter)
                phraseController.TransferLetterToThisContainer(clickedSlot.SlottedLetter);
            else if (phraseController.SelectedSlot && phraseController.SelectedSlot.SlottedLetter)
                TransferLetterToThisContainer(phraseController.SelectedSlot.SlottedLetter);
        }

        public override void TransferLetterToThisContainer(LetterInstance letter)
        {
            if (Config.ClearedSlotsGoToInitialSlot || Config.LockSlotsToInitialLetter)
                letter.InitialSlot.TrySetOrSwapSlottedLetter(letter);
            else
            {
                var emptySlot = FirstSlot.GetNextEmptySlot();
                if (emptySlot.HasValue)
                    emptySlot.Value.slot.TrySetOrSwapSlottedLetter(letter);
            }
        }
    }
}
