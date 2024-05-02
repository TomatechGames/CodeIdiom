using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TomatechGames.CodeIdiom
{
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class RectTransformBlend : LayoutGroup
    {
        [SerializeField]
        RectTransform fromTransform;

        [SerializeField]
        RectTransform toTransform;

        [SerializeField]
        bool useX;

        [SerializeField]
        bool useY;

        [SerializeField, Range(0,1)]
        float factor;
        float lastFactor;
        public float Factor
        {
            get => factor;
            set
            {
                if (lastFactor == value || !isActiveAndEnabled)
                    return;

                factor = value;
                lastFactor = value;

                UpdateTargetSize();
            }
        }

        Vector2 targetSize = -Vector2.one;

        void UpdateTargetSize()
        {
            if (!fromTransform || !toTransform)
                return;

            targetSize = Vector2.Lerp(fromTransform.rect.size, toTransform.rect.size, factor);

            if (!useX)
                targetSize.x = -1;

            if (!useY)
                targetSize.y = -1;

            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }

        public override float minWidth => targetSize.x;
        public override float minHeight => targetSize.y;

        public override void CalculateLayoutInputVertical()
        {
            UpdateTargetSize();
        }

        public override void SetLayoutHorizontal()
        {

        }

        public override void SetLayoutVertical()
        {

        }

#if UNITY_EDITOR

        private void Update()
        {
            if (factorMightBeDirty)
                Factor = factor;
            factorMightBeDirty = false;
        }

        bool factorMightBeDirty;
        protected override void OnValidate()
        {
            factorMightBeDirty = true;
        }
#endif

    }
}
