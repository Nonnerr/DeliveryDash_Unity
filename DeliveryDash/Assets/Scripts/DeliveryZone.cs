using UnityEngine;
using TMPro;
using System.Collections;

public class DeliveryZone : MonoBehaviour
{
    public Transform startPoint;
    public TMP_Text messageText;

    private void Start()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false);
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

                ShowMessage("Package Delivered! Score: " + ds.Score);

                if (startPoint != null)
                    other.transform.position = startPoint.position;
            }
        }
    }

    private void ShowMessage(string text)
    {
        messageText.text = text;
        messageText.gameObject.SetActive(true);

    
        StartCoroutine(HideAfterDelay(2f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.gameObject.SetActive(false);
    }
}
