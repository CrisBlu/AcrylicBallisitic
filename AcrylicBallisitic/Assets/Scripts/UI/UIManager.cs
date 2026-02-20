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
        //Will probably be a number
        //hitPointsDisplay.SetSlots(hitPoints);
    }

    public void UpdatePlayerAmmo(Ammo[] ammo)
    {
        ammoDisplay.SetSlots(ammo);
    }
}