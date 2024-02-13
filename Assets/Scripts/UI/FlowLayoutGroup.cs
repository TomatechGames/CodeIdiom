using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowLayoutGroup : LayoutGroup
{
    [SerializeField]
    public float horizontalSpacing;
    [SerializeField]
    public float verticalSpacing;
    [SerializeField]
    public float preferredWidthDelta = 0;
    [SerializeField]
    public bool controlChildWidth;
    [SerializeField]
    public bool controlChildHeight;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        float minWidth = 0;
        foreach (var child in rectChildren)
        {
            if (!child.gameObject.activeInHierarchy)
                continue;

            minWidth = Mathf.Max(child.rect.size.x, minWidth);
        }
        float preferredWidth = LayoutOperation(false, GetWidthFromDelta(preferredWidthDelta), false)+0.1f;
        SetLayoutInputForAxis(minWidth, preferredWidth, -1, 0);
    }

    public override void CalculateLayoutInputVertical()
    {
        SetLayoutInputForAxis(-1, LayoutOperation(true, false), -1, 1);
    }

    public override void SetLayoutHorizontal()
    {
        LayoutOperation(false);
    }

    public override void SetLayoutVertical()
    {
        LayoutOperation(true);
    }

    float LayoutOperation(bool vertical, bool layoutChild = true) =>
        LayoutOperation(vertical, rectTransform.rect.size.x, layoutChild);
    float LayoutOperation(bool vertical, float givenWidth, bool layoutChild = true)
    {
        float maxLineWidth = 0;
        float currentLineWidth = 0;
        float currentYPush = 0;
        float currentRowHeight = 0;

        foreach (var child in rectChildren)
        {
            if (!child.gameObject.activeInHierarchy)
                continue;

            if (currentLineWidth != 0)
                currentLineWidth += horizontalSpacing;

            Vector2 resolvedSize = new(
                controlChildWidth ? LayoutUtility.GetPreferredSize(child, 0) : child.rect.size.x,
                controlChildHeight ? LayoutUtility.GetPreferredSize(child, 1) : child.rect.size.y
                );

            if (currentLineWidth + resolvedSize.x > givenWidth - padding.horizontal)
            {
                maxLineWidth = MathF.Max(maxLineWidth, currentLineWidth-horizontalSpacing);
                currentYPush += currentRowHeight+verticalSpacing;

                currentRowHeight = 0;
                currentLineWidth = 0;
            }

            currentRowHeight = Mathf.Max(currentRowHeight, resolvedSize.y);
            if (layoutChild)
            {
                if (vertical ? controlChildHeight : controlChildWidth)
                    SetChildAlongAxis(child, vertical ? 1 : 0, vertical ? (currentYPush + padding.top) : (currentLineWidth + padding.left), vertical ? resolvedSize.y : resolvedSize.x);
                else
                    SetChildAlongAxis(child, vertical ? 1 : 0, vertical ? (currentYPush + padding.top) : (currentLineWidth + padding.left));
            }
            currentLineWidth += resolvedSize.x;
        }

        maxLineWidth = MathF.Max(maxLineWidth, currentLineWidth);
        currentYPush += currentRowHeight;

        return vertical ? (currentYPush + padding.vertical) : (maxLineWidth + padding.horizontal);
    }

    private float GetWidthFromDelta(float widthDelta)
    {
        Rect parentRect = (rectTransform.parent as RectTransform).rect;
        float widthGivenAnchors = (rectTransform.anchorMax.x-rectTransform.anchorMin.x)*parentRect.size.x;
        return widthGivenAnchors + widthDelta;
    }
}
