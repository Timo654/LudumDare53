using TMPro;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    [SerializeField] private House[] houses;
    [SerializeField] private Transform housesParent;
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < housesParent.childCount; i++)
        {
            if (houses.Length <= i)
            {
                break;
            }
            Debug.Log(houses[i].name);
            housesParent.GetChild(i);
            housesParent.GetChild(i).GetComponent<SpriteRenderer>().sprite = houses[i].artwork;
            housesParent.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
            housesParent.GetChild(i).GetComponent<HouseObject>().SetHouse(houses[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
