using UnityEngine;
using System;

public class TriggerEventZone : MonoBehaviour
{
    public static event Action OnAnyTriggerZoneEntered; // Re-work this with Unity Events
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            Debug.Log("Event triggered");
            triggered = true;
            OnAnyTriggerZoneEntered?.Invoke();
        }
    }
}