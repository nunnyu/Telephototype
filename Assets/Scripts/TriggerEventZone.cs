using UnityEngine;
using System;
using UnityEngine.Events;

public class TriggerEventZone : MonoBehaviour
{
    public UnityEvent OnAnyTriggerZoneEntered; 
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