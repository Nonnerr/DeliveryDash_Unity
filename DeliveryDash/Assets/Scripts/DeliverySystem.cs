using UnityEngine;
using System;

public class DeliverySystem : MonoBehaviour
{
    public bool HasPackage = false;
    public int Score = 0;


    public Action OnPackagePickedUp;
    public Action<int> OnScoreChanged;

    public void DeliverPackage(float timeBonus)
    {
        if (HasPackage)
        {
            int points = Mathf.RoundToInt(100 * timeBonus);
            Score += points;

            HasPackage = false;

            OnScoreChanged?.Invoke(Score);
        }
    }
}
