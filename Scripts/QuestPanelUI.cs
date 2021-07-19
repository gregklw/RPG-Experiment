using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestPanelUI : MonoBehaviour, UIWindow
{
    public NPCQuestList source;
    public Transform questcontent;
    public Transform descriptioncontent;
    public TextMeshProUGUI questDescriptionText;

    public void CloseUI()
    {
        source.CloseUI();
    }
}
