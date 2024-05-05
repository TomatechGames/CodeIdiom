using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public class BubbleController : MonoBehaviour
    {
        [SerializeField]
        BubbleElement letterBubblePrefab;
        [SerializeField]
        BubbleElement wordBubblePrefab;

        List<BubbleElement> wordBubbleInstances = new();
        List<BubbleElement> letterBubbleInstances = new();

        public void UpdateBubbles(string wordString, string letterString)
        {
            //loop through word string to put into bubble
            string[] splitWords = wordString
                .Split(" ")
                .Where(word => !string.IsNullOrWhiteSpace(word.Replace("_", "")))
                .ToArray();
            for (int i = 0; i < splitWords.Length; i++)
            {
                BubbleElement wordBubbleInstance;
                if (i >= wordBubbleInstances.Count)
                {
                    wordBubbleInstances.Add(Instantiate(wordBubblePrefab, transform));
                }

                wordBubbleInstance = wordBubbleInstances[i];
                wordBubbleInstance.SetText(splitWords[i]);
                wordBubbleInstance.gameObject.SetActive(true);
                wordBubbleInstance.transform.SetAsLastSibling();
            }
            for (int i = splitWords.Length; i < wordBubbleInstances.Count; i++)
            {
                wordBubbleInstances[i].gameObject.SetActive(false);
            }

            //loop through letter string to put into bubble
            string[] splitLetters = letterString
                .ToCharArray()
                .Select(letterChar => letterChar.ToString())
                .ToArray();
            for (int i = 0; i < splitLetters.Length; i++)
            {
                BubbleElement letterBubbleInstance;
                if (i >= letterBubbleInstances.Count)
                {
                    letterBubbleInstances.Add(Instantiate(letterBubblePrefab, transform));
                }

                letterBubbleInstance = letterBubbleInstances[i];
                letterBubbleInstance.SetText(splitLetters[i]);
                letterBubbleInstance.gameObject.SetActive(true);
                letterBubbleInstance.transform.SetAsLastSibling();
            }
            for (int i = splitLetters.Length; i < letterBubbleInstances.Count; i++)
            {
                letterBubbleInstances[i].gameObject.SetActive(false);
            }


        }

    }
}
