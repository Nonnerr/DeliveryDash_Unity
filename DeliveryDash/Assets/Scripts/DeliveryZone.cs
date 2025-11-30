using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeliveryZone : MonoBehaviour
{
    public Transform startPoint;
    public int deliveriesToWin = 3;

    [Header("GAME OVER UI")]
    public GameObject blackScreen;
    public TMP_Text gameOverText;

    private void Start()
    {
        if (blackScreen != null)
            blackScreen.SetActive(false);
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        DeliverySystem ds = other.GetComponent<DeliverySystem>();
        if (ds != null && ds.HasPackage)
        {
           
            ds.HasPackage = false;
           
            if (ds.Score >= deliveriesToWin)
            {
                StartCoroutine(ShowGameOver(ds.Score));
            }
            else
            {
                if (startPoint != null)
                    other.transform.position = startPoint.position;
            }
        }
    }

    private IEnumerator ShowGameOver(int score)
    {
        if (blackScreen != null)
            blackScreen.SetActive(true);

        float t = 0f;
        Image blackImg = blackScreen.GetComponent<Image>();
        while (t < 1f)
        {
            t += Time.deltaTime;
            if (blackImg != null)
                blackImg.color = new Color(0, 0, 0, t);
            yield return null;
        }

        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER\nYour Score: " + score;
            gameOverText.gameObject.SetActive(true);

            float s = 0f;
            while (s < 1f)
            {
                s += Time.deltaTime;
                gameOverText.color = new Color(1, 1, 1, s);
                yield return null;
            }
        }

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}