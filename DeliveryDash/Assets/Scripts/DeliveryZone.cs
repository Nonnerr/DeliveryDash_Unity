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
            blackScreen.color = new Color(0, 0, 0, 0);
        if (gameOverText != null)
            gameOverText.color = new Color(1, 1, 1, 0); 
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
                  
                    if (startPoint != null)
                        other.transform.position = startPoint.position;
                }
            }
        }
    }

    private IEnumerator ShowGameOver()
    {
        
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            if (blackScreen != null)
                blackScreen.color = new Color(0, 0, 0, t);
            yield return null;
        }

       
        float s = 0f;
        while (s < 1f)
        {
            s += Time.deltaTime;
            if (gameOverText != null)
                gameOverText.color = new Color(1, 1, 1, s);
            yield return null;
        }

    
        yield return new WaitForSeconds(5f);

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
