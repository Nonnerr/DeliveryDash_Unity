using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public float levelTime = 60f; 
    public TMP_Text timerText;
    public void HideTimer()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        levelTime -= Time.deltaTime;

        if (levelTime < 0)
            levelTime = 0;
        if (levelTime == 0)
        {
            FindObjectOfType<GameOverManager>().TriggerGameOver();
        }

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(levelTime / 60f);
            int seconds = Mathf.FloorToInt(levelTime % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            
        }
    }
    

}
