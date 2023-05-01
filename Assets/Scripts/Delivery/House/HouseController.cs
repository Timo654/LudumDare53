using TMPro;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;
    [SerializeField] public uint houseCount;
    [SerializeField] public DataContainer gameData;
    public GameObject housePrefab;
    public GameObject treePrefab;
    public GameObject endTrigger;
    private SpriteRenderer tree;
    // Start is called before the first frame update
    void Awake()
    {
        float prev_x = 0;
        float x;
        float tree_loc;
        // generate houses
        for (var i= 0; i < houseCount; i++)
        {
            x = prev_x + Random.Range(20, 40);
            Debug.Log(x);
            Debug.Log(x - prev_x);
            tree_loc = Random.Range(prev_x + 4f, x - 4f);

            // could be better
            if (Random.value > 0.5f)
            {
                tree = Instantiate(treePrefab, new Vector3(tree_loc, 1.85f, 0), Quaternion.identity).GetComponent<SpriteRenderer>();
                tree.sortingOrder = 11;
            }
            else
            {
                tree = Instantiate(treePrefab, new Vector3(tree_loc, 2.65f, 0), Quaternion.identity).GetComponent<SpriteRenderer>();
                tree.sortingOrder = 4;
            }
            tree.sprite = gameData.treeSprites[Random.Range(0, gameData.treeSprites.Length)];

            prev_x = x;
            GameObject obj = Instantiate(housePrefab, new Vector3(x, 1.85f, 0), Quaternion.identity);
            HouseObject house = obj.GetComponent<HouseObject>();
            house.gameManager = gameManager;
            Debug.Log(house.item);
            gameManager.AddToInventory(house.item);
        }
        endTrigger.transform.position = new Vector3(Random.Range(prev_x + 20f, prev_x + 60f), 0.4f, 0);
        gameManager.AddToInventory(new DeliveryItem(gameData.Items[Random.Range(0, gameData.Items.Length)], 1)); // add a single extra item
        gameManager.ShuffleInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
