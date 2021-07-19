using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour, IPointerEnterHandler
{
    public bool inProgress;
    public string questName;
    public string description;
    public Quest thisQuest;
    public Button thisButton;

    private void Start()
    {
        if (thisQuest.isCompleted)
        {
            GetComponentInChildren<Text>().color = Color.red;
            GetComponentInChildren<Text>().text += "\n [Completed]";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIWindowMasterScript.uiWindowMasterScript.questPanelUI.questDescriptionText.text = description;
    }

    public void ButtonEffect()
    {
        if (!thisQuest.isCompleted)
        {
            if (!thisQuest.inProgress)
            {
                UnityAction[] calls = { delegate { PlayerQuestLog.playerQuestLog.AddQuest(thisQuest); }, delegate { thisQuest.RevealQuestGameObject(); } };
                PopUpPromptScript.popUpPromptScript.confirmationPromptScript.ActivateConfirmationPrompt(calls, "Add this quest to questlog?");
            }
            else
            {
                ActivePlayerUI.activePlayerUI.Set_UI_Opened_Ref(PopUpPromptScript.popUpPromptScript.regularPromptScript.gameObject);
                PopUpPromptScript.popUpPromptScript.regularPromptScript.regularPromptMessage.text = "Quest in progress/not finished.";
                PopUpPromptScript.popUpPromptScript.regularPromptScript.gameObject.SetActive(true);
            }
        }
        else
        {
            PlayerQuestLog.playerQuestLog.GiveAllGivingObjectives();
            if (thisQuest.noReward)
            {
                PlayerQuestLog.playerQuestLog.AddCompletedQuest(thisQuest);
                UnityAction[] calls = { delegate { UIWindowMasterScript.uiWindowMasterScript.questPanelUI.source.AddFollowupQuests(thisQuest.followUpQuestsClass); },
                delegate { ActivePlayerUI.activePlayerUI.CloseTargetUI(UIWindowMasterScript.uiWindowMasterScript.questPanelUI.gameObject); }};
                PopUpPromptScript.popUpPromptScript.regularPromptScript.ActivateRegularPrompt(calls, "Quest completed!");
                Destroy(gameObject);
            }
            else
            {
                UnityAction[] calls = { delegate { UIWindowMasterScript.uiWindowMasterScript.questRewardUIScript.RevealQuestRewards(thisQuest.questReward, true); },
                delegate {PlayerQuestLog.playerQuestLog.AddCompletedQuest(thisQuest); },
                delegate { UIWindowMasterScript.uiWindowMasterScript.questPanelUI.source.AddFollowupQuests(thisQuest.followUpQuestsClass); },
                delegate { ActivePlayerUI.activePlayerUI.CloseTargetUI(UIWindowMasterScript.uiWindowMasterScript.questPanelUI.gameObject); }};
                PopUpPromptScript.popUpPromptScript.regularPromptScript.ActivateRegularPrompt(calls, "Quest completed! Show rewards.");
            }
        }
    }
}
