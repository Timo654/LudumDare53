using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryItem
{
    private string _itemName;
    private Sprite _artwork;
    private string[] _text;
    private uint _count;

    // Start is called before the first frame update
    public DeliveryItem(DeliveryItemData itemData, uint count)
    {
        _itemName = itemData.itemName;
        _artwork = itemData.artwork;
        _text = itemData.text;
        _count = count;
    }

    public Sprite GetArtwork()
    {
        return _artwork;
    }

    public string GetRandomLine()
    {
        return _text[Random.Range(0, _text.Length)];
    }

    public string GetName()
    {
        return _itemName;
    }
    public bool getIsEmpty()
    {
        return _count < 1;
    }

    public void decrementCount()
    {
        _count -= 1;
    }
}
