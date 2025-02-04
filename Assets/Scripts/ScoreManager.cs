using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private float time = 0;
    private int kills = 0;
    private int bubbles = 0;

    private bool gameover = false;

    public TextMeshProUGUI timeSurvived;
    public TextMeshProUGUI kill;
    public TextMeshProUGUI bubble;
    public TextMeshProUGUI score;

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

        time = Mathf.Round(time);

        timeSurvived.text = "Seconds Survived: " + time;
        kill.text = "Kills: " + kills;
        bubble.text = "Bubbles: " + bubbles;

        var num = time + kills * 10;

        score.text = "Final Score: " + num;
    }
}
