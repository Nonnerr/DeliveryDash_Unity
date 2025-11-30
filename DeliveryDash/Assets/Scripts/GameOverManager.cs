using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public GameObject blackScreen;        
    public TMP_Text gameOverText;         
    public LevelTimer levelTimer;        
    public DeliverySystem deliverySystem;

    private void Start()
    {
        blackScreen.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    public void TriggerGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        
        if (levelTimer != null)
            levelTimer.HideTimer();

        blackScreen.SetActive(true);

        gameOverText.text = "GAME OVER\nYour Score: " + (deliverySystem != null ? deliverySystem.Score.ToString() : "0");

        gameOverText.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
