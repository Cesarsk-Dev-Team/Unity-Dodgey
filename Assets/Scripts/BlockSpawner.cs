using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Information about blocks and game mechanic by Cesarsk */
/* ------------------------------------------------------------------
 * 
 *      BLOCKS SIZES:
        1 visible block: scale_block: 14
        2 visible block: scale_block: 9.3
        3 visible block: scale_block: 7
        4 visible block: scale_block: 5.6

---------------------------------------------------------------------

    DIFFERENT TYPES OF BLOCKS, EACH OF THEM HAS DIFFERENT EFFECTS:
    --
    White Block: Kill the Player
    Green Block: You can't hold the button, just tap it to move (for a limited time)
    --
    Red Block: Decrease Player speed of a factor X
    Yellow Block: Increase Player speed of a factor X
    --
    Purple Block: Decrease Player size of a factor Y
    Blue Block: Increase Player's size of a factor Y
    --
    Orange Block: Slow motion effect (decrease Time.scale for a limited time)
    Indigo Block: Fast motion effect (increase Time.scale for a limited time)
    --
    Rainbow Block: Invincible for a limited time

---------------------------------------------------------------------

    Blocks Speed increases by time (PARTIALLY DONE, CHECK block.cs file)
    Passing a level affects:
        - Colored blocks spawn rate (TO DO)
        - Blocks Layout (Done, spawnblocks(layout))
        - Blocks Spawn rate (Done, update method)

---------------------------------------------------------------------

*/



public class BlockSpawner : MonoBehaviour
{
    //Declaring our spawnpoints
    public Transform[] spawnPoints1B; //2 blocks (1 visible and 1 scoreBlock)
    public Transform[] spawnPoints2B; //3 blocks (2 visible and 1 scoreBlock)
    public Transform[] spawnPoints3B; //4 blocks (3 visible and 1 scoreBlock)
    public Transform[] spawnPoints4B; //5 blocks (4 visible and 1 scoreBlock)

    //Declaring our sizes
    private const float SIZE_1B = 14f;
    private const float SIZE_2B = 9.3f;
    private const float SIZE_3B = 7f;
    private const float SIZE_4B = 5.6f;

    //Declaring our vector
    private Object[] oneBlock;
    private Vector2 twoBlocks;
    private Vector2 threeBlocks;
    private Vector2 fourBlocks;

    //Other parameters
    public GameObject blockPrefab;

    private float timeToSpawn = 2f;
    private float timeBetweenWaves = 1f;
    public static int selectedLayout = 1;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeToSpawn)
        {
            // @Shiro: Change to 1, 2, 3, 4 to try different #blocks
            SpawnBlocks(selectedLayout);

            // @Shiro: Change values to change blocks spawnRate (min, max) in seconds
            timeBetweenWaves = (int)Random.Range(1f, 3f);
            timeToSpawn = timeBetweenWaves + Time.time;
        }
    }

    private void SpawnBlocks(int layout_index)
    {
        Transform[] layout;
        float size;
        switch (layout_index)
        {
            case 1:
                layout = spawnPoints1B;
                size = SIZE_1B;
                break;
            case 2:
                layout = spawnPoints2B;
                size = SIZE_2B;
                break;
            case 3:
                layout = spawnPoints3B;
                size = SIZE_3B;
                break;
            case 4:
                layout = spawnPoints4B;
                size = SIZE_4B;
                break;
            case 5:
                int random_layout = (int)Random.Range(1f, 5f);
                Debug.Log("Random Layout: "+random_layout);
                layout = PickRandomLayout(random_layout);
                size = PickRandomSize(random_layout);
                break;
            default:
                layout = spawnPoints4B;
                size = SIZE_4B;
                break;
        }

        // Selecting a random spawn point
        int randomIndex = Random.Range(0, layout.Length);

        for (int i = 0; i < layout.Length; i++)
        {
            if (randomIndex != i)
            {
                GameObject block = Instantiate(blockPrefab, layout[i].position, Quaternion.identity);

                block.GetComponent<Transform>().localScale = new Vector2(size, 4f);

                //disabling score trigger
                block.GetComponents<BoxCollider2D>()[1].enabled = false;
            }

            if (randomIndex == i)
            {
                //spawn score collider
                GameObject scoreBlock = Instantiate(blockPrefab, layout[i].position, Quaternion.identity);
                scoreBlock.GetComponent<Transform>().localScale = new Vector2(size, 4f);
                scoreBlock.GetComponents<BoxCollider2D>()[0].enabled = false;
                //Disabling score Renderer (father and child)
				foreach (Renderer r in scoreBlock.GetComponentsInChildren<Renderer>())
					r.enabled = false;
			}
        }
    }

    private Transform[] PickRandomLayout(int random_layout) 
    {
        Transform[] layout;
        switch(random_layout)
        {
            case 1:
                layout = spawnPoints1B;
                break;
            case 2:
                layout = spawnPoints2B;
                break;
            case 3:
                layout = spawnPoints3B;
                break;
            case 4:
                layout = spawnPoints4B;
                break;
            default:
                layout = spawnPoints4B;
                break;
        }
        return layout;
    }

    private float PickRandomSize(int random_size)
    {
        float size;
        switch (random_size)
        {
            case 1:
                size = SIZE_1B;
                break;
            case 2:
                size = SIZE_2B;
                break;
            case 3:
                size = SIZE_3B;
                break;
            case 4:
                size = SIZE_4B;
                break;
            default:
                size = SIZE_4B;
                break;
        }
        return size;
    }
}