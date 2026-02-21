using UnityEngine;
using UnityEngine.UI;

public class SlotsDisplay : MonoBehaviour
{
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Sprite fullSlotSprite;
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] int maxSlots = 6;

    GameObject[] slots;
    Image[] slotImages;

    public void SetSlots(Ammo[] slots)
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (slots[i] == Ammo.Loaded)
            {
                slotImages[i].sprite = fullSlotSprite;
            }
            else
            {
                slotImages[i].sprite = emptySlotSprite;
            }
        }
    }

    public void SetHealth(int hpCount)
    {
        
        for (int i = 0; i < maxSlots; i++)
        {
            if (i < hpCount)
            {
                slotImages[i].sprite = fullSlotSprite;
            }
            else
            {
                slotImages[i].sprite = emptySlotSprite;
            }
        }

    }

    void Start()
    {
        // Delete all children (in case there are any in the editor)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        slots = new GameObject[maxSlots];
        slotImages = new Image[maxSlots];
        for (int i = 0; i < maxSlots; i++)
        {
            slots[i] = Instantiate(slotPrefab, transform);
            slotImages[i] = slots[i].GetComponent<Image>();
        }
    }
}