using TMPro;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;
    [SerializeField] public uint houseCount;
    [SerializeField] public DataContainer gameData;
    public int houseDistMin = 20;
    public int houseDistMax = 40;
    public float houseY = -1.91f;
    public float bottomTreeY = -2f;
    public float topTreeY = -1.4f;
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
            x = prev_x + Random.Range(houseDistMin, houseDistMax);
            if (gameData.treeSprites.Length > 0)
            {
                tree_loc = Random.Range(prev_x + 4f, x - 4f);

                // could be better
                if (Random.value > 0.5f)
                {
                    tree = Instantiate(treePrefab, new Vector3(tree_loc, Random.Range(-3.85f, bottomTreeY), 0), Quaternion.identity).GetComponent<SpriteRenderer>();
                    tree.sortingOrder = 11;
                }
                else
                {
                    tree = Instantiate(treePrefab, new Vector3(tree_loc, topTreeY, 0), Quaternion.identity).GetComponent<SpriteRenderer>();
                    tree.sortingOrder = 4;
                }
                tree.sprite = gameData.treeSprites[Random.Range(0, gameData.treeSprites.Length)];
            }
            prev_x = x;
            GameObject obj = Instantiate(housePrefab, new Vector3(x, houseY, 0), Quaternion.identity);
            HouseObject house = obj.GetComponent<HouseObject>();
            house.gameManager = gameManager;
            gameManager.AddToInventory(house.item);
        }

        float endLocation = Random.Range(prev_x + 20f, prev_x + 60f);
        Instantiate(treePrefab, new Vector3(Random.Range(prev_x + 10f, endLocation - 10f), Random.Range(-3.85f, -2f), 0), Quaternion.identity).GetComponent<SpriteRenderer>(); // random final tree
        endTrigger.transform.position = new Vector3(endLocation, 0.4f, 0);
        gameManager.AddToInventory(new DeliveryItem(gameData.Items[Random.Range(0, gameData.Items.Length)], 1)); // add a single extra item
        gameManager.ShuffleInventory();
    }

}
