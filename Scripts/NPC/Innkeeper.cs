using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Innkeeper : MonoBehaviour, INPCFunctions
{
    public int price;

    public void CloseUI()
    {
        throw new System.NotImplementedException();
    }

    public Button GetNPCOptionButton()
    {
        return OptionsNPCBox.optionsNPCBoxScript.innkeeperButton;
    }

    public void SpecialFunction()
    {
        PopUpPromptScript.popUpPromptScript.confirmationPromptScript.ActivateConfirmationPrompt(StayOvernight, "It costs " + price + " to stay overnight. Spend the night here?");
    }

    private void StayOvernight()
    {
        Debug.Log(PlayerGoldAmount.playerGoldAmount.GoldAmount);
        if (PlayerGoldAmount.playerGoldAmount.GoldAmount >= price)
        {
            PlayerGoldAmount.playerGoldAmount.ChangeGoldAmount(-price);
            FadeInFadeOutScreen.fadeScript.FadeInOut();
            PlayerHealthEvent.playerHealthEvent.HealMaxHealth();
        }
        else
        {
            PopUpPromptScript.popUpPromptScript.regularPromptScript.ActivateRegularPrompt("Not enough gold.");
        }
    }
}
