using TMPro;
using UnityEngine;

public class HouseController : MonoBehaviour
{

    [SerializeField] private Transform housesParent;
    [SerializeField] public GameManager gameManager;
    [SerializeField] public uint houseCount;
    public GameObject housePrefab;
    // Start is called before the first frame update
    void Start()
    {
        float prev_x = 0;
        float x;
        // generate houses
        for (var i= 0; i < houseCount; i++)
        {
            x = prev_x + Random.Range(10, 20);
            Debug.Log(x);
            Debug.Log(x - prev_x);
            prev_x = x;
            GameObject obj = Instantiate(housePrefab, new Vector3(x, 0.5f, 0), Quaternion.identity);
            HouseObject house = obj.GetComponent<HouseObject>();
            house.gameManager = gameManager;
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
