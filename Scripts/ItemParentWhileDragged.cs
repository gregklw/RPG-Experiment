using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemParentWhileDragged : MonoBehaviour
{
    public static ItemParentWhileDragged itemDragParent;
    private void Awake()
    {
        itemDragParent = this;
    }
}
