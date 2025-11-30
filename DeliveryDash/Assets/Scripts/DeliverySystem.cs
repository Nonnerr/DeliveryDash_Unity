using UnityEngine;
using System;

public class DeliverySystem : MonoBehaviour
{
    public bool HasPackage = false;
    public int Score = 0;

    public Action OnPackagePickedUp;
    public Action<int> OnScoreChanged;

    public void PickupPackage()
    {
        HasPackage = true;
        Score += 1; 
        OnScoreChanged?.Invoke(Score);
        OnPackagePickedUp?.Invoke();
    }

    public void DeliverPackage()
    {
        if (HasPackage)
        {
            HasPackage = false;
        }
    }
}