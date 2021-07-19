using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerQuestLog : MonoBehaviour
{
    public static PlayerQuestLog playerQuestLog;

    public List<Quest> questsInProgress = new List<Quest>();
    public List<Quest> questsCompleted = new List<Quest>();
    public static QuestInProgressButton Selected_QIP_Button;
    public static Quest MostRecentQuestAdded { get; private set; }
    public static Quest MostRecentQuestCompleted { get; private set; }

    public PlayerQuestLogPanel playerQuestLogPanel;

    [SerializeField]
    Transform questInProgressContent;

    [SerializeField]
    Transform questInProgressSidePanelContent;

    [SerializeField]
    GameObject questInProgressButton;

    [SerializeField]
    Transform questCompletedContent;

    [SerializeField]
    GameObject questCompletedButton;

    [SerializeField]
    GameObject questProgressSummaryText;

    private void Awake()
    {
        playerQuestLog = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!playerQuestLogPanel.gameObject.activeSelf)
            {
                playerQuestLogPanel.gameObject.SetActive(true);
                ActivePlayerUI.activePlayerUI.Set_UI_Opened_Ref_No_Freeze(playerQuestLogPanel.gameObject);
            }
            else
            {
                ActivePlayerUI.activePlayerUI.CloseTargetUINoFreeze(playerQuestLogPanel.gameObject);
            }
        }
    }

    public void AddQuest(Quest quest)
    {
        questsInProgress.Add(quest);
        quest.inProgress = true;
        GameObject questInProgressButtonRef = Instantiate(questInProgressButton);
        quest.questProgressButton = questInProgressButtonRef.GetComponent<QuestInProgressButton>();
        questInProgressButtonRef.transform.SetParent(questInProgressContent);
        quest.questProgressButton.quest = quest;
        questInProgressButtonRef.GetComponentInChildren<Text>().text = quest.questName;
        quest.questProgressButton.resignButton = playerQuestLogPanel.resignButton;
        quest.questProgressButton.viewDescriptionButton = playerQuestLogPanel.viewDescriptionButton;
        AddQuestProgressSummary(quest);
        MostRecentQuestAdded = quest;
    }

    private void AddQuestProgressSummary(Quest quest)
    {
        GameObject questProgressSummaryTextRef = Instantiate(questProgressSummaryText);
        questProgressSummaryTextRef.transform.SetParent(questInProgressSidePanelContent);
        quest.questProgressText = questProgressSummaryTextRef.GetComponentInChildren<TextMeshProUGUI>();
        quest.questProgressText.text = quest.ToString();
        quest.questProgressSummaryT = questProgressSummaryTextRef.transform;
    }

    public void AddCompletedQuest(Quest quest)
    {
        GameObject questCompletedInfo = Instantiate(questCompletedButton, questCompletedContent);
        questCompletedInfo.GetComponent<QuestCompletedInfobar>().thisQuest = quest;
        Debug.Log(quest.questName);
        questCompletedInfo.GetComponent<QuestCompletedInfobar>().thisText.text = quest.questName;
        quest.markedAsCompleted = true;
        Destroy(quest.questProgressButton.gameObject);
        Destroy(quest.questProgressSummaryT.gameObject);
        Destroy(quest.questButton.gameObject);
        MostRecentQuestCompleted = quest;
    }

    public void UpdateAllKillObjectives(GameObject objToCheck)
    {
        foreach (Quest quest in questsInProgress)
        {
            quest.UpdateKillingObjectives(objToCheck);
        }
    }

    public void UpdateAllEquipObjectives(GameObject objToCheck)
    {
        foreach (Quest quest in questsInProgress)
        {
            quest.UpdateEquipObjectives(objToCheck);
        }
    }

    public void UpdateAllGivingObjectives(GameObject objToCheck)
    {
        foreach (Quest quest in questsInProgress)
        {
            quest.UpdateGivingObjectives(objToCheck);
        }
    }

    public void GiveAllGivingObjectives()
    {
        foreach (Quest quest in questsInProgress)
        {
            foreach (ObjectiveGiving og in quest.givingObjectives)
            {
                if (og.ReturnListOfObjectivesInInventory().Count > 0)
                {
                    foreach (GameObject g in og.ReturnListOfObjectivesInInventory())
                    {
                        Debug.Log(g.name);
                        g.transform.parent.GetComponent<SlotState>().ChangeToVacantState();
                        g.transform.SetParent(null);
                        g.SetActive(false);
                    }
                }
            }
        }
    }
}
