using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TomatechGames.CodeIdiom
{
    public class LetterInstance : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        ConfigContainer config;
        ConfigContainer Config => config ? config : (config = FindObjectOfType<ConfigContainer>());

        public static event Action<LetterInstance> OnDraggedLetterChanged;
        public char AssociatedChar {  get; private set; }
        public LetterSlot InitialSlot { get; private set; }
        public LetterSlot CurrentSlot { get; private set; }
        [SerializeField]
        CanvasGroup canvasGroup;
        [SerializeField]
        Transform animatedChild;
        [SerializeField]
        UnityEvent animateToOrigin;

        [SerializeField]
        UnityEvent<string> onLetterChanged;
        [SerializeField]
        UnityEvent onLetterLocked;

        public event Action onReslotted;

        public bool isLocked { get; private set; } = false;

        //when the letter is instantiated, this should be called to assign it a char
        //if CurrentSlot exists during initialisation, it is stored as InitialSlot
        public void Initialise(char associatedChar, bool isLocked = false)
        {
            AssociatedChar = associatedChar;
            onLetterChanged?.Invoke(AssociatedChar.ToString());
            InitialSlot = CurrentSlot;
        }

        public void SetLocked()
        {
            isLocked = true;
            //make visual changes to signify locked state
            onLetterLocked?.Invoke();
        }

        public void SetSlot(LetterSlot newSlot)
        {
            bool isNew = newSlot != CurrentSlot;
            CurrentSlot = newSlot;
            if (!InitialSlot)
                InitialSlot = CurrentSlot;
            Vector3 childPorldPos = animatedChild.position;

            transform.SetParent(CurrentSlot.LetterInstanceParent);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            animatedChild.position = childPorldPos;
            animateToOrigin.Invoke();
            if (isNew)
                onReslotted?.Invoke();
        }

        //forwards the click event of a letter to it's slot
        public void ClickSlot()
        {
            SetLocked();
            //if (CurrentSlot)
                //CurrentSlot.ClickSlot();
        }

        Vector3 dragOffset = Vector3.zero;
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isLocked)
                return;
            OnDraggedLetterChanged?.Invoke(this);
            var newParent = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            transform.SetParent(newParent, true);
            canvasGroup.blocksRaycasts = false;
            dragOffset = eventData.pointerCurrentRaycast.worldPosition - transform.position;
            if (Config.SnapDraggedLetterToCursor)
                dragOffset = Vector3.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isLocked)
                return;
            transform.position = eventData.pointerCurrentRaycast.worldPosition - dragOffset;
            SessionRunner.Instance.SetInteractable(false);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isLocked)
                return;
            SessionRunner.Instance.SetInteractable(true);
            LetterSlot hoveredSlot = null;
            if (eventData.pointerCurrentRaycast.gameObject)
            {
                var searchObject = eventData.pointerCurrentRaycast.gameObject;
                hoveredSlot = searchObject.GetComponentInParent<LetterSlot>();
                if (!hoveredSlot)
                {
                    var hoveredInstance = searchObject.GetComponentInParent<LetterInstance>();
                    hoveredSlot = hoveredInstance? hoveredInstance.CurrentSlot : null;
                }
            }

            bool success = hoveredSlot && hoveredSlot.TrySetOrSwapSlottedLetter(this);
            if (!success)
                SetSlot(CurrentSlot);
            canvasGroup.blocksRaycasts = true;
            OnDraggedLetterChanged?.Invoke(null);
        }
    }
}
