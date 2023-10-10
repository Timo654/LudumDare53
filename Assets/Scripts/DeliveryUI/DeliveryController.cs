using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryController : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform deliveryItemPanel;

    public void UpdateItems()
    {
        gameManager.inventory.RemoveAll(item => item.getIsEmpty());
        for (var i = 0; i < deliveryItemPanel.childCount; i++)
        {
            if (gameManager.inventory.Count <= i)
            {
                deliveryItemPanel.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            deliveryItemPanel.GetChild(i);
            deliveryItemPanel.GetChild(i).GetChild(0).GetComponent<Image>().sprite = gameManager.inventory[i].GetArtwork();
            deliveryItemPanel.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
            deliveryItemPanel.GetChild(i).GetComponent<ItemSlot>().SetDeliveryItem(gameManager.inventory[i]);
            deliveryItemPanel.GetChild(i).Find("Name").GetComponent<TMP_Text>().text = gameManager.inventory[i].GetName();

        }
    }
    public void SelectDelivery(ItemSlot itemSlot)
    {

        DeliveryItem deliveryItem = itemSlot.GetDeliveryItem();
        gameManager.HandOverItem(deliveryItem);
    }
}
