using UnityEngine;
using System.Collections;
// based on https://stackoverflow.com/questions/38921035/how-to-animate-2d-clouds-in-unity3d

public class CloudManagerScript : MonoBehaviour
{
    //Set this variable to your Cloud Prefab through the Inspector
    [SerializeField] public DataContainer gameData;
    //Set this variable to how often you want the Cloud Manager to make clouds in seconds.
    //For Example, I have this set to 2
    public float delay;

    //If you ever need the clouds to stop spawning, set this variable to false, by doing: CloudManagerScript.spawnClouds = false;
    public static bool spawnClouds = true;

    // Use this for initialization
    void Start()
    {
        //Begin SpawnClouds Coroutine
        Prewarm();
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        //This will always run
        while (true)
        {
            //Only spawn clouds if the boolean spawnClouds is true
            while (spawnClouds)
            {
                //Instantiate Cloud Prefab and then wait for specified delay, and then repeat
                GameObject cloud = ObjectPool.SharedInstance.GetPooledObject();
                if (cloud != null)
                {
                    cloud.GetComponent<SpriteRenderer>().sprite = gameData.cloudSprites[Random.Range(0, gameData.cloudSprites.Length)];
                    cloud.GetComponent<SpriteRenderer>().color = Color.white;
                    cloud.SetActive(true);
                }
                //GameObject cloud = Instantiate(cloudPrefab);           
                yield return new WaitForSeconds(delay);
            }
            break;
        }
    }

    void Prewarm()
    {
        if (gameData.cloudSprites.Length <= 0)
        {
            spawnClouds = false;
            return;
        }
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        for (int i= 0; i < 10; i++)
        {
            float spawnPos = Camera.main.transform.position.x - camWidth + (i * 2);
            GameObject cloud = ObjectPool.SharedInstance.GetPooledObject();
            if (cloud != null)
            {
                cloud.GetComponent<SpriteRenderer>().sprite = gameData.cloudSprites[Random.Range(0, gameData.cloudSprites.Length)];
                cloud.GetComponent<SpriteRenderer>().color = Color.white;
                cloud.SetActive(true);
                Vector3 temp = transform.position;
                temp.x = spawnPos;
                cloud.transform.position = temp;
            }
        }
    }
}