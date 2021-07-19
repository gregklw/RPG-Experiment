using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DoubleClickScript : MonoBehaviour, IPointerClickHandler
{
    int clickAmount;
    float clickdelay = 0.5f;
    float clicktime;
    bool doubleClickWindow;

    public void OnPointerClick(PointerEventData eventData)
    {

        clickAmount++;
        if (clickAmount == 1)
            StartCoroutine("ResetClicks");
    }

    IEnumerator ResetClicks()
    {
        StartCoroutine("ClickDelay");
        while (doubleClickWindow)
        {
            if (clickAmount == 2)
            {
                DoubleClickEffect();
                break;
            }
            yield return null;
        }
        clickAmount = 0;
    }
    IEnumerator ClickDelay()
    {
        doubleClickWindow = true;
        yield return new WaitForSecondsRealtime(clickdelay);
        doubleClickWindow = false;
    }
    protected abstract void DoubleClickEffect();
}
