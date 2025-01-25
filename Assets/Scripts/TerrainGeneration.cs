using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TerrainGeneration : MonoBehaviour
{
    public int width = 100;
    public int height = 100;

    public int magnitude = 10;

    public Tilemap tileMap;
    public Tile tile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateNoise();
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


/*                if(Vector2.Distance(Vector2.zero,pos) > 48f)
                {
                    SpawnWalls(pos.x, pos.y);
                }*/
                if(Vector2.Distance(Vector2.zero, pos) < 10f)
                {

                }
                else if (noise > Mathf.Min(0.5f, Vector2.Distance(Vector2.zero, pos)/(30)))
                {
                    SpawnWalls(pos.x, pos.y);
                }
            }
        }
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
}
