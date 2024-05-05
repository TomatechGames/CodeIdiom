using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TomatechGames.CodeIdiom
{
    public class BubbleElement : MonoBehaviour
    {
        [SerializeField]
        UnityEvent<string> onBubbleTextChanged;
        public void SetText(string text)
        {
            onBubbleTextChanged?.Invoke(text);
        }
    }
}
