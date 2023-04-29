using TMPro;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;
    [SerializeField] public uint houseCount;
    public GameObject housePrefab;
    // Start is called before the first frame update
    void Awake()
    {
        float prev_x = 0;
        float x;
        // generate houses
        for (var i= 0; i < houseCount; i++)
        {
            x = prev_x + Random.Range(15, 25);
            Debug.Log(x);
            Debug.Log(x - prev_x);
            prev_x = x;
            GameObject obj = Instantiate(housePrefab, new Vector3(x, 1.85f, 0), Quaternion.identity);
            HouseObject house = obj.GetComponent<HouseObject>();
            house.gameManager = gameManager;
            Debug.Log(house.item);
            gameManager.AddToInventory(house.item);
        }
        gameManager.ShuffleInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
