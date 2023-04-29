using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DeliveryController : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform deliveryItemPanel;
    [SerializeField] private DeliveryItem[] deliveries;
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < deliveryItemPanel.childCount; i++)
        {
            if (deliveries.Length <= i)
            {
                break;
            }
            Debug.Log(deliveries[i].name);
            deliveryItemPanel.GetChild(i);
            deliveryItemPanel.GetChild(i).GetComponent<Image>().sprite = deliveries[i].artwork;
            deliveryItemPanel.GetChild(i).GetComponent<Image>().color = Color.white;
            deliveryItemPanel.GetChild(i).GetComponent<ItemSlot>().SetDeliveryItem(deliveries[i]);
            deliveryItemPanel.GetChild(i).Find("Name").GetComponent<TMP_Text>().text = deliveries[i].name;
        }
    }

    public void SelectDelivery(ItemSlot itemSlot)
    {

        DeliveryItem deliveryItem = itemSlot.GetDeliveryItem();
        gameManager.HandOverItem(deliveryItem);
    }
}
