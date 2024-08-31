using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    Vector3 playerLastPosition;
    

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist; //must be grater then the length and width of the tilemap
    float opDist;
    float optimizerColldown;
    public float optimizerColldownDur;
    void Start()
    {
        playerLastPosition = player.transform.position;
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }

        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);

        CheckAndSpawnChunk(directionName);

        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
        }
        if (directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
        }
        if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
        }
        if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
        }

    }

    void CheckAndSpawnChunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(direction).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(direction).position);
        }
    }

    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //moving horizontally more then vertically
            if (direction.y > 0.5f)
            {
                //also moving upwards
                return direction.x > 0 ? "Right Up" : "Left Up";
            }
            else if (direction.y < -0.5f)
            {
                //also moving downwards
                return direction.x > 0 ? "Right Down" : "Left Down";
            }
            else
            {
                //mpving straight horizontally
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            //moving verticallymore then horizontally
            if (direction.x > 0.5f)
            {
                //also moving right
                return direction.y > 0 ? "Right Up" : "Right Down";
            }
            else if (direction.x < -0.5f)
            {
                //also moving left
                return direction.y > 0 ? "Left Up" : "Left Down";
            }
            else
            {
                //moving straight vertically
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    
    
    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range(0,terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand],spawnPosition,Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }
    
    void ChunkOptimizer()
    {
        optimizerColldown -= Time.deltaTime;
        if(optimizerColldown <= 0f)
        {
            optimizerColldown = optimizerColldownDur;
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if(opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
