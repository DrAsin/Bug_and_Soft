﻿using System.Collections;
using Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class Dialogue : MonoBehaviour
{
    
    public LocalizedString[] locales;
    public DIALOGUE_ACTION[] dialogueActions;
    
    public UnityEvent OnDialogueFinish;

    private protected DialogueGroup dialogueGroup;
    private protected uint currentDialogueIndex;
    private protected uint selectedChoiceIndex;
    private protected DialogueData currentDialogueData;
    
    private protected string currentText = "";

    private protected float startDelay = 2f;
    private protected float delay = 0.035f;
    
    private protected bool choiceSelected;

    //private int from, to;
    private int current = -1;
    
    
    private void Awake()
    {
        if (OnDialogueFinish == null)
            OnDialogueFinish = new UnityEvent();
    }

    public void ShowDialogues()
    {
        StopAllCoroutines();
        currentDialogueIndex = 0;
        DialogueManager.textRenderer.text = "";
        DialogueManager.Instance.HideInteractionButton();
        StartCoroutine(ShowText());
    }
    
    private protected DialogueData GetDialogueLocale(int index)
    {
        if (index < locales.Length && index < dialogueActions.Length)
        {
            return new DialogueData(locales[index], dialogueActions[index], null);
        }

        return new DialogueData(null, DIALOGUE_ACTION.NEXT, null);
    }
    
    private protected  int GetDialogueCount()
    {
        return locales.Length;
    }
    
    public DialogueData NextDialogue()
    {
        current++;
        return GetDialogueLocale(current);
    }
    
    public void SetDialogue(int index)
    {
        current = index;
    }

    public virtual IEnumerator ShowText(uint defaultDialogueIndex = 0)
    {
        yield return null;
    }
    
    private protected virtual void OnChoiceEvent(uint dialogueIndex)
    {
        DialogueManager.Instance.ActivateDialogueFinishButton();
        
        //... custom impl here
    }
    
    private protected virtual void OnDialogueFinished()
    {
        DialogueManager.Instance.SetButtonListener(() =>
        {
            OnDialogueFinish.Invoke();
            DialogueManager.Instance.HideDialogues();
            GameManager.Instance.PlayerExitInteractionMode();
            GameManager.Instance.ExitDialogueMode();
            
        });
        DialogueManager.Instance.ActivateDialogueFinishButton();
    }
    
    private protected void ChooseOption(uint i)
    {
        choiceSelected = true;
        selectedChoiceIndex = i;
    }

}