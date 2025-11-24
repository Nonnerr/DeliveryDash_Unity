using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public GameObject blackScreen;        // чёрный экран
    public TMP_Text gameOverText;         // текст "Game Over"
    public LevelTimer levelTimer;         // ссылка на твой таймер
    public DeliverySystem deliverySystem; // чтобы показать счёт

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
        // 1. Прячем таймер
        if (levelTimer != null)
            levelTimer.HideTimer();

        // 2. Показываем чёрный экран
        blackScreen.SetActive(true);

        // 3. Показываем текст Game Over
        gameOverText.text = "GAME OVER\nYour Score: " + deliverySystem.Score;
        gameOverText.gameObject.SetActive(true);

        // 4. Ждём 5 секунд
        yield return new WaitForSeconds(5f);

        // 5. Рестарт сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
