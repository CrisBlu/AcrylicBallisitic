using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] SlotsDisplay hitPointsDisplay;
    [SerializeField] SlotsDisplay ammoDisplay;
    [SerializeField] Slider netWorthSlider;

    public void UpdatePlayerHitPoints(int hitPoints)
    {
        hitPointsDisplay.SetSlots(hitPoints);
    }

    public void UpdatePlayerAmmo(int ammo)
    {
        ammoDisplay.SetSlots(ammo);
    }
}