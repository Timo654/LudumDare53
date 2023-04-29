using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private DeliveryItem _item;

    public void SetDeliveryItem(DeliveryItem item)
    {
        _item = item;
    }

    public DeliveryItem GetDeliveryItem()
    {
        return _item;
    }
}
