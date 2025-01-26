using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private float time;
    private int kills;
    private int bubbles;

    private bool gameover = false;

    public static ScoreManager Instance;
    //ScoreManager.Instance.Death();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(gameover == false)
        {
            time += Time.deltaTime;
        }
    }

    public void Kill()
    {
        kills++;
    }

    public void Bubble()
    {
        bubbles++;
    }

    public void GameOver()
    {
        gameover= true;
    }
}
