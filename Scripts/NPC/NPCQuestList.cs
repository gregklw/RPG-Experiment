using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuestList : MonoBehaviour, INPCFunctions
{
    [SerializeField]
    GameObject questButtonPrefab;

    [SerializeField]
    Transform questPanel;
    public List<Quest> questsAvailable = new List<Quest>();

    private void Start()
    {
        questPanel = UIWindowMasterScript.uiWindowMasterScript.questPanelUI.transform;
    }

    public void SpecialFunction()
    {
        if (!questPanel.gameObject.activeSelf)
        {
            questPanel.gameObject.SetActive(true);
            UIWindowMasterScript.uiWindowMasterScript.questPanelUI.source = this;
            foreach (Quest quest in questsAvailable) if (!quest.markedAsCompleted) CreateQuestButton(quest);
        }
        else
        {
            CloseUI();
        }
    }

    public void AddFollowupQuests(FollowUpQuests listClass)
    {
        if (listClass != null)
        {
            foreach (Quest followupQuest in listClass.followUpQuests)
            {
                questsAvailable.Add(followupQuest);
            }
        }
    }

    private void CreateQuestButton(Quest quest)
    {
        GameObject questButtonRef = Instantiate(questButtonPrefab);
        questButtonRef.transform.SetParent(UIWindowMasterScript.uiWindowMasterScript.questPanelUI.questcontent);
        quest.questButton = questButtonRef.GetComponent<QuestButton>();
        questButtonRef.GetComponentInChildren<Text>().text = quest.questName;
        quest.questButton.questName = quest.questName;
        quest.questButton.description = quest.description;
        quest.questButton.thisQuest = quest;
    }

    public void CloseUI()
    {
        foreach (Transform questT in UIWindowMasterScript.uiWindowMasterScript.questPanelUI.questcontent)
        {
            Destroy(questT.gameObject);
        }
        OptionsNPCBox.optionsNPCBoxScript.questButton.onClick.RemoveAllListeners();
        questPanel.gameObject.SetActive(false);
        UIWindowMasterScript.uiWindowMasterScript.questPanelUI.questDescriptionText.text = "";
        UIWindowMasterScript.uiWindowMasterScript.questPanelUI.source = null;
    }

    public Button GetNPCOptionButton()
    {
        return OptionsNPCBox.optionsNPCBoxScript.questButton;
    }

    public bool IsEmpty()
    {
        return questsAvailable.Count == 0;
    }
}
