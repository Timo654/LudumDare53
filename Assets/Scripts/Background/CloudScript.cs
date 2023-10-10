using UnityEngine;

public class CloudScript : MonoBehaviour
{
    //Set these variables to whatever you want the slowest and fastest speed for the clouds to be, through the inspector.
    //If you don't want clouds to have randomized speed, just set both of these to the same number.
    //For Example, I have these set to 2 and 5
    public float minSpeed;
    public float maxSpeed;

    //Set these variables to the lowest and highest y values you want clouds to spawn at.
    //For Example, I have these set to 1 and 4
    public float minY;
    public float maxY;

    //Set this variable to how far off screen you want the cloud to spawn, and how far off the screen you want the cloud to be for it to despawn. You probably want this value to be greater than or equal to half the width of your cloud.
    //For Example, I have this set to 4, which should be more than enough for any cloud.
    public float buffer;

    float speed;
    float width;
    private Vector3 spawnPosition;
    private void OnEnable()
    {
        //Set Cloud Movement Speed, and Position to random values within range defined above
        speed = Random.Range(minSpeed, maxSpeed);
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        bool direction = Random.value < 0.5f;
        if (direction)
        {
            spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
            spawnPosition.x -= width / 2;
        }

        else
        {
            spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
            spawnPosition.x += width / 2;
        }
        spawnPosition.z = 0; // Set the z position to zero
        transform.position = spawnPosition + Vector3.up * Random.Range(minY, maxY);
    }

    // Update is called once per frame
    void Update()
    {
        //Translates the cloud to the right at the speed that is selected
        transform.Translate(-speed * Time.deltaTime, 0, 0);
        //If cloud is off Screen, Destroy it.
        if (Camera.main.WorldToViewportPoint(transform.position - Vector3.right * width / 2).x < -1)
        {
            gameObject.SetActive(false);
        }
        else if (Camera.main.WorldToViewportPoint(transform.position + Vector3.right * width).x > 2)
        {
            gameObject.SetActive(false);
        }
    }

}

