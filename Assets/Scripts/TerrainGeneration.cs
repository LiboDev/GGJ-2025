using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Pathfinding;
using System.Collections;

public class TerrainGeneration : MonoBehaviour
{
    public int width = 100;
    public int height = 100;

    public int magnitude = 10;

    public float threshold = 0.5f;
    public float falloff;
    //1 minimum - 10 super high

    public Tilemap tileMap;
    public RuleTile tile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GenerateTerrain());
    }

    private void GenerateNoise()
    {
        var randomizerX = Random.Range(0, height * 2);
        var randomizerY = Random.Range(0, height * 2);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noise = Mathf.PerlinNoise((float)(x + randomizerX) / width * magnitude, (float)(y + randomizerY) / height * magnitude);
                Vector2Int pos = new Vector2Int(x - width / 2, y - height / 2);


                if (Vector2.Distance(Vector2.zero, pos) > width/2 - 2)
                {
                    SpawnWalls(pos.x, pos.y);
                }
                if (Vector2.Distance(Vector2.zero, pos) < 5f)
                {

                }
                else if (noise > Mathf.Min(threshold, (width/falloff)/Vector2.Distance(Vector2.zero, pos))     )
                {
                    SpawnWalls(pos.x, pos.y);
                }
            }
        }

        //AstarPath.active.Scan();
    }

    private void SpawnWalls(int x, int y)
    {
        Vector3Int pos = new Vector3Int(x, y, 0);
        tileMap.SetTile(pos, tile);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    // Wait for terrain generation to be finished, then rescan the pathfinding graph
    private IEnumerator GenerateTerrain()
    {
        GenerateNoise();

        yield return new WaitForSeconds(0.1f);

        AstarPath.active.Scan();
    }
}
