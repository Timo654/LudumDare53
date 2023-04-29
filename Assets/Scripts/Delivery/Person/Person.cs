using UnityEngine;

public class Person
{
    public Sprite _artwork;
    public DeliveryItem _desiredItem;

    public Person(Sprite artwork, DeliveryItem desiredItem)        
    {
        _artwork = artwork;
        _desiredItem = desiredItem;
        Debug.Log(desiredItem.GetRandomLine());
    }

    public Sprite GetArtwork()
    {
        return _artwork;
    }

    public void SetArtwork(Sprite artwork)
    {
        _artwork = artwork;
    }

    public DeliveryItem GetDesiredItem()
    {
        return _desiredItem;
    }

    public void SetDesiredItem(DeliveryItem item)
    {
        _desiredItem = item;
    }
}
