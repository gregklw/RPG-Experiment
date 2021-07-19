using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shopkeeper : MonoBehaviour, INPCFunctions
{
    [SerializeField]
    GameObject shopItemButtonPrefab;

    Transform shopT;
    Transform shopItemsContent;

    private List<GameObject> shopItemObjects = new List<GameObject>();

    public ShopItem[] itemsForSale;

    ShopPanelUI shopPanelUIScript;

    private void Start()
    {
        shopPanelUIScript = UIWindowMasterScript.uiWindowMasterScript.shopPanelUIScript;
        shopT = shopPanelUIScript.transform;
        shopItemsContent = shopPanelUIScript.shopcontent.transform;
    }

    public void SpecialFunction()
    {
        if (!shopT.gameObject.activeSelf)
        {
            shopT.gameObject.SetActive(true);
            shopPanelUIScript.source = this;
            foreach (ShopItem shopitem in itemsForSale)
            {
                CreateShopItemButton(shopitem);
            }
        }
        else
        {
            CloseUI();
        }
    }

    private void CreateShopItemButton(ShopItem shopitem)
    {
        GameObject buttonObj = Instantiate(shopItemButtonPrefab);
        shopItemObjects.Add(buttonObj);
        buttonObj.name = shopitem.itemObj.GetComponent<Item>().itemName;
        buttonObj.transform.SetParent(shopItemsContent);
        buttonObj.GetComponent<ShopItemButton>().item = shopitem.itemObj;
        buttonObj.GetComponent<ShopItemButton>().item.SetActive(true);
        buttonObj.GetComponent<ShopItemButton>().price = shopitem.price;

        GameObject itemDescription = buttonObj.transform.Find("ItemDescriptions").gameObject;
        itemDescription.transform.Find("ItemNameText").GetComponent<Text>().text = shopitem.itemObj.GetComponent<Item>().itemName;
        itemDescription.transform.Find("ItemPriceText").GetComponent<Text>().text = "Price: " + shopitem.price.ToString();
    }

    public void CloseUI()
    {
        foreach (GameObject itemObj in shopItemObjects)
        {
            Destroy(itemObj);
        }
        OptionsNPCBox.optionsNPCBoxScript.shopButton.onClick.RemoveAllListeners();
        shopT.gameObject.SetActive(false);
        shopPanelUIScript.source = null;
    }

    public Button GetNPCOptionButton()
    {
        return OptionsNPCBox.optionsNPCBoxScript.shopButton;
    }

    public bool IsEmpty()
    {
        return itemsForSale.Length == 0;
    }
}

[System.Serializable]
public class ShopItem
{
    public int price;
    public GameObject itemObj;
}

