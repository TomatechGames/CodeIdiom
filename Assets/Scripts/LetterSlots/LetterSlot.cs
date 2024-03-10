using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class LetterSlot : MonoBehaviour
    {
        [SerializeField]
        ConfigContainer config;
        ConfigContainer Config => config ? config : (config = FindObjectOfType<ConfigContainer>());

        public LetterSlot NextSlot { get; private set; }
        public LetterSlot PrevSlot { get; private set; }
        public LetterInstance SlottedLetter { get; private set; }
        public LetterSlotContainer Container { get; private set; }
        public LetterInstance InitialLetter { get; private set; }

        [field: SerializeField]
        public Transform LetterInstanceParent { get; private set; }

        [SerializeField]
        GameObject highlightObject;
        bool keepHighlightWhenNotDragging;
        public bool KeepHighlightWhenNotDragging
        {
            get => keepHighlightWhenNotDragging;
            set
            {
                keepHighlightWhenNotDragging = value;
                UpdateHighlight();
            }
        }

        public bool IsEmpty => !SlottedLetter;

        //when the slot is instantiated, this should be called to link it to a container
        //if SlottedLetter exists during initialisation, it is stored as InitialLetter
        public void Initialise(LetterSlotContainer container)
        {
            Container = container;
            InitialLetter = SlottedLetter;
        }

        bool currentDragMatchesSlot = false;
        public void UpdateCurrentlyDraggedLetter(LetterInstance draggedLetter)
        {
            currentDragMatchesSlot = draggedLetter && IsEmpty && CanSlotLetter(draggedLetter);
        }

        //called when a letter starts/stops being dragged
        void UpdateHighlight()
        {
            highlightObject.SetActive(currentDragMatchesSlot || keepHighlightWhenNotDragging);
        }

        //if enabled in the config, this locks slots to only accept the letter it spawned with (if the slot didnt start with a letter, it still accepts all letters)
        public bool CanSlotLetter(LetterInstance newLetter)
        {
            if (!Config.lockSlotsToInitialLetter)
                return true;
            return !InitialLetter || !newLetter || InitialLetter == newLetter;
        }

        //if this slot can hold the incoming letter, slots the incoming letter.
        //if this slot had a letter, slots the existing letter in the incoming letter's previous slot
        public bool TrySetOrSwapSlottedLetter(LetterInstance newLetter)
        {
            if (!CanSlotLetter(newLetter) || !newLetter)
                return false;

            if (newLetter)
            {
                if(!newLetter.CurrentSlot.CanSlotLetter(SlottedLetter))
                    return false;
                newLetter.CurrentSlot.SetSlottedLetter(SlottedLetter);
            }

            SetSlottedLetter(newLetter);
            return true;
        }

        //assumes that the letter has been validated
        void SetSlottedLetter(LetterInstance newLetter)
        {
            SlottedLetter = newLetter;
            if (!SlottedLetter)
                return;
            SlottedLetter.SetSlot(this);
        }

        //null indicates that none of the next/previous slots are empty
        public DistantEmptySlot? GetNextEmptySlot(bool requireVisible = false)=> GetDistantEmptySlot(0, true, requireVisible);
        public DistantEmptySlot? GetPrevEmptySlot(bool requireVisible = false) => GetDistantEmptySlot(0, false, requireVisible);

        DistantEmptySlot? GetDistantEmptySlot(int currentIndex, bool forward, bool requireVisible = false)
        {
            if (IsEmpty && (gameObject.activeInHierarchy || !requireVisible))
                return new(this, currentIndex);
            var slot = forward ? NextSlot : PrevSlot;
            return slot ? slot.GetDistantEmptySlot(currentIndex + 1, forward, requireVisible) : null;
        }

        public struct DistantEmptySlot
        {
            public LetterSlot slot;
            public int distance;
            public DistantEmptySlot(LetterSlot slot, int distance)
            {
                this.slot = slot;
                this.distance = distance;
            }
        }

        //tries to push in the closest available direction
        public bool TryPushClosest()
        {
            //cant push if slot is empty
            if (IsEmpty)
                return false;

            int nextSlotDist = GetNextEmptySlot()?.distance ?? 9999;
            int prevSlotDist = GetPrevEmptySlot()?.distance ?? 9999;

            // if both directions are full (which should never happen), the push is abandoned and a warning is logged
            if (nextSlotDist==9999 && prevSlotDist==9999)
            {
                Debug.LogWarning("Push abandoned, as somehow all slots are full (how did we get here?)");
                return false;
            }

            //if both are equal, next is prioritised
            bool prioritiseNext = prevSlotDist >= nextSlotDist;

            if (prevSlotDist == 9999)
                prioritiseNext = true;
            if (nextSlotDist == 9999)
                prioritiseNext = false;

            if (!(prioritiseNext ? TryPushNext() : TryPushPrev()))
                return !prioritiseNext ? TryPushNext() : TryPushPrev();
            return true;
        }

        public bool TryPushNext() => TryPush(NextSlot, true);
        public bool TryPushPrev() => TryPush(PrevSlot, false);

        //validates a push attempt, and pushes if the validation succeeds
        bool TryPush(LetterSlot toSlot, bool direction)
        {
            //cant push if slot is empty
            if (IsEmpty)
                return false;
            if (!toSlot || !toSlot.ValidatePush(direction, SlottedLetter))
                return false;
            toSlot.PushLetter(direction, SlottedLetter);
            return true;
        }

        //ensures theres space to push a letter in the specified direction
        bool ValidatePush(bool forward, LetterInstance letter)
        {
            if(IsEmpty)
                return true;
            //if the previous letter cant be slotted in this slot for whatever reason, deny the push
            if(!CanSlotLetter(letter))
                return false;
            var slot = forward ? NextSlot : PrevSlot;
            return slot.ValidatePush(forward, SlottedLetter);
        }

        //pushes a letter in the specified direction (assumes a push validation was performed successfully)
        void PushLetter(bool forward, LetterInstance letter)
        {
            if (IsEmpty)
                return;
            var slot = forward ? NextSlot : PrevSlot;
            slot.PushLetter(forward, SlottedLetter);
            SetSlottedLetter(letter);
        }

        //forwards the click event of a letter/slot to it's container
        public void ClickSlot()
        {
            if(Container)
                Container.OnSlotClicked(this);
        }
    }
}
