using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeliveryZone : MonoBehaviour
{
    public Transform startPoint;

    [Header("GAME OVER UI")]
    public Image blackScreen;
    public TMP_Text gameOverText;

    public DeliverySystem deliverySystem;
    public int deliveriesToWin = 3;

    private void Start()
    {
        if (blackScreen != null)
            blackScreen.color = new Color(0, 0, 0, 0); // прозрачный

        if (gameOverText != null)
            gameOverText.color = new Color(1, 1, 1, 0); // прозрачный
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DeliverySystem ds = other.GetComponent<DeliverySystem>();

            if (ds != null && ds.HasPackage)
            {
                ds.Score++;
                ds.HasPackage = false;

                if (ds.Score >= deliveriesToWin)
                {
                    StartCoroutine(ShowGameOver());
                }
                else
                {
                    // возвращаем игрока к старту
                    if (startPoint != null)
                        other.transform.position = startPoint.position;
                }
            }
        }
    }

    private IEnumerator ShowGameOver()
    {
        // 1. Чёрный экран появляется постепенно
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            if (blackScreen != null)
                blackScreen.color = new Color(0, 0, 0, t);
            yield return null;
        }

        // 2. Текст появляется
        float s = 0f;
        while (s < 1f)
        {
            s += Time.deltaTime;
            if (gameOverText != null)
                gameOverText.color = new Color(1, 1, 1, s);
            yield return null;
        }

        // 3. Ждём 5 секунд
        yield return new WaitForSeconds(5f);

        // 4. Перезапускаем уровень
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
