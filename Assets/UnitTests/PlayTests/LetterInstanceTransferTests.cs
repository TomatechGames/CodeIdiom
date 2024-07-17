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

        var phrase = firstPanel.GetComponent<PhraseController>();
        var deck = firstPanel.GetComponent<LetterDeck>();

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

    [UnityTest]
    public IEnumerator DeckFullPhraseTransferValidation()
    {
        yield return SceneManager.LoadSceneAsync(0);
        //this waits 1 frame
        yield return null;

        var menu = Object.FindObjectOfType<MainMenuController>();
        menu.StartSessionTesting(0);

        yield return null;

        var sessionRunner = Object.FindObjectOfType<SessionRunner>();
        var firstPanel = sessionRunner.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);

        var phrase = firstPanel.GetComponent<PhraseController>();

        var deckSlotParent = firstPanel.GetChild(0).GetChild(3);
        Assert.IsNotNull(deckSlotParent);

        //deckFirstSlot.ClickSlot();
        //17 14 3 5 2 1 10 11 9 8 6 7 15 13 4 0 12 16
        ClickChildSlot(deckSlotParent, 17);
        ClickChildSlot(deckSlotParent, 14);
        ClickChildSlot(deckSlotParent, 3);
        ClickChildSlot(deckSlotParent, 5);
        ClickChildSlot(deckSlotParent, 2);
        ClickChildSlot(deckSlotParent, 1);
        ClickChildSlot(deckSlotParent, 10);
        ClickChildSlot(deckSlotParent, 11);
        ClickChildSlot(deckSlotParent, 9);
        ClickChildSlot(deckSlotParent, 8);
        ClickChildSlot(deckSlotParent, 6);
        ClickChildSlot(deckSlotParent, 7);
        ClickChildSlot(deckSlotParent, 15);
        ClickChildSlot(deckSlotParent, 13);
        ClickChildSlot(deckSlotParent, 4);
        ClickChildSlot(deckSlotParent, 0);
        ClickChildSlot(deckSlotParent, 12);
        ClickChildSlot(deckSlotParent, 16);

        Assert.IsTrue(phrase.ReadCurrentPhrase()=="CART BEFORE THE HORSE");
    }
    void ClickChildSlot(Transform parent, int index)=> parent.GetChild(index).GetComponent<LetterSlot>().ClickSlot();
}
