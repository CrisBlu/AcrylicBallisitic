using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] HitPointsDisplay hitPointsDisplay;
    [SerializeField] Slider netWorthSlider;

    public void UpdatePlayerHitPoints(int hitPoints)
    {
        hitPointsDisplay.SetHitPoints(hitPoints);
    }
}