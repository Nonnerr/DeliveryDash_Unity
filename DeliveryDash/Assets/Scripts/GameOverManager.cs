using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public GameObject blackScreen;
    public TMP_Text gameOverText;

    private void Start()
    {
        if (blackScreen != null)
            blackScreen.SetActive(false);
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }

    public void TriggerGameOver(int score)
    {
        StartCoroutine(GameOverSequence(score));
    }

    IEnumerator GameOverSequence(int score)
    {
        if (blackScreen != null)
            blackScreen.SetActive(true);

        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER\nYour Score: " + score;
            gameOverText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
