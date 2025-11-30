using UnityEngine;

public class PackagePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DeliverySystem ds = other.GetComponent<DeliverySystem>();
            if (ds != null)
            {
                ds.PickupPackage();
            }
            Destroy(gameObject);
        }
    }
}