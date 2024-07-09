using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TomatechGames.CodeIdiom;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class LetterInstanceTransferTests
{
    // A UnityTest behaves like a coroutine in Play Mode.
    [UnityTest]
    public IEnumerator DeckToPhraseTransfer()
    {
        yield return SceneManager.LoadSceneAsync(0);
        //this waits 1 frame
        yield return null;

        var menu = Object.FindObjectOfType<MainMenuController>();
        menu.StartSessionTesting(0);

        yield return null;

        var sessionRunner = Object.FindObjectOfType<SessionRunner>();
        var firstPanel = sessionRunner.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);

        //var phrase = firstPanel.GetComponent<PhraseController>();
        //var deck = firstPanel.GetComponent<LetterDeck>();

        var deckFirstSlot = firstPanel.GetChild(0).GetChild(3).GetChild(0).GetComponent<LetterSlot>();
        var phraseFirstSlot = firstPanel.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<LetterSlot>();
        Assert.IsNotNull(deckFirstSlot);
        Assert.IsNotNull(phraseFirstSlot);

        Assert.IsNotNull(deckFirstSlot.SlottedLetter);
        Assert.IsNull(phraseFirstSlot.SlottedLetter);

        deckFirstSlot.ClickSlot();

        Assert.IsNull(deckFirstSlot.SlottedLetter);
        Assert.IsNotNull(phraseFirstSlot.SlottedLetter);
    }
}
