using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    //these will be used to determine where to spawn the collectables
    //I will get these values from the empty gameobjects we created by referencing their x and y values
    public GameObject lowestYSpawn;
    public GameObject highestYSpawn;

    //These are public variables to allow me to drag and drop our prefabs
    public GameObject yellowCollectable;
    public GameObject redCollectable;
    public GameObject greenCollectable;

    //random number to determine which collectable to spawn.
    private int randomPrefab;

    //which collectable to spawn
    private GameObject collectableToSpawn;

    //need a reference to time so we can determine how often to spawn a collectable
    private float time;
    //how long to wait between spawning collectables
    public float delay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //add to time.  see how much time has passed since last frame
        time += Time.deltaTime;

        if (time >= delay)
        {
            spawnObject();
            //reset time so the delay is set for the next object to spawn
            time = 0f;
        }
    }

    private void spawnObject()
    {
        //get a random number to determine which object to spawn.
        //The max number in Random.Range is exclusive (up to not including)
        //the code below will return the number: 0,1,2
        randomPrefab = Random.Range(0, 3);

        if(randomPrefab == 0 )
        {
            collectableToSpawn = Instantiate(redCollectable);
        }
        else if(randomPrefab == 1 ) 
        {
            collectableToSpawn = Instantiate(greenCollectable);
        }
        else if (randomPrefab == 2 )
        {
            collectableToSpawn = Instantiate(yellowCollectable);
        }

        collectableToSpawn.transform.position = new Vector2(lowestYSpawn.transform.position.x, Random.Range(lowestYSpawn.transform.position.y, highestYSpawn.transform.position.y));
    }
}
