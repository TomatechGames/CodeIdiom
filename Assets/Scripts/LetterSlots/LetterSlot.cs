using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class LetterSlot : MonoBehaviour
    {
        [SerializeField]
        Config _config;
        Config config=> _config ? _config : (_config = FindObjectOfType<Config>());

        public LetterSlot NextSlot { get; private set; }
        public LetterSlot PrevSlot { get; private set; }
        public LetterInstance SlottedLetter { get; private set; }
        public LetterSlotContainer Container { get; private set; }

        public bool IsEmpty => !SlottedLetter;

        public void Initialise(LetterSlotContainer container)
        {
            Container = container;
        }

        public bool CanSlotLetter(LetterInstance newLetter)
        {
            // return !newLetter.InitialSlot || newLetter.InitialSlot == this
            return true;
        }

        public void SetOrSwapSlottedLetter(LetterInstance newLetter)
        {
            if (!CanSlotLetter(newLetter))
                return;
            if (newLetter == null)
                SlottedLetter = null;

            if (SlottedLetter)
                newLetter.CurrentSlot.SetSlottedLetter(SlottedLetter);
            SetSlottedLetter(newLetter);
        }

        void SetSlottedLetter(LetterInstance newLetter)
        {
            SlottedLetter = newLetter;
            SlottedLetter.SetSlot(this);
            //animate to new position
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

        public bool TryPushClosest()
        {
            //cant push if slot is empty
            if (IsEmpty)
                return false;

            int nextSlotDist = GetNextEmptySlot()?.distance ?? 9999;
            int prevSlotDist = GetPrevEmptySlot()?.distance ?? 9999;

            // if both are invalid (which should never happen), the push is abandoned and a warning is logged
            if (nextSlotDist==9999 && prevSlotDist==9999)
            {
                Debug.LogWarning("Push abandoned, as somehow all slots are full (how did we get here?)");
                return false;
            }

            //if both are equal, next is prioritised
            bool prioritiseNext = prevSlotDist >= nextSlotDist;

            if (prevSlotDist == 9999)
                prioritiseNext = true;
            if (prevSlotDist == 9999)
                prioritiseNext = true;

            bool result = prioritiseNext ? TryPushNext() : TryPushPrev();
            if (!result)
                result = !prioritiseNext ? TryPushNext() : TryPushPrev();
            return result;
        }
        public bool TryPushNext()
        {
            //cant push if slot is empty
            if (IsEmpty)
                return false;
            if (!NextSlot || !NextSlot.ValidatePush(true, SlottedLetter))
                return false;
            NextSlot.PushLetter(true, SlottedLetter);
            return true;
        }
        public bool TryPushPrev()
        {
            //cant push if slot is empty
            if (IsEmpty)
                return false;
            if (!PrevSlot || !PrevSlot.ValidatePush(false, SlottedLetter))
                return false;
            PrevSlot.PushLetter(false, SlottedLetter);
            return true;
        }

        bool ValidatePush(bool forward, LetterInstance letter)
        {
            if(IsEmpty)
                return true;
            if(!CanSlotLetter(letter))
                return false;
            var slot = forward ? NextSlot : PrevSlot;
            return slot.ValidatePush(forward, SlottedLetter);
        }
        void PushLetter(bool forward, LetterInstance letter)
        {
            if (IsEmpty)
                return;
            var slot = forward ? NextSlot : PrevSlot;
            slot.PushLetter(forward, SlottedLetter);
            SetSlottedLetter(letter);
        }

        public void ClickSlot()
        {
            if(Container)
                Container.OnSlotClicked(this);
        }
    }
}
