using UnityEngine;

public class AlertSwitch : MonoBehaviour
{
    public Animator chudevilAnimator;
    public string alertTrigger = "Alert";

    public void TriggerAlertAnimation()
    {
        if (chudevilAnimator != null)
        {
            chudevilAnimator.SetTrigger(alertTrigger);
        }
        else
        {
            Debug.LogError("Animator reference not set in AlertSwitch!");
        }
    }
}