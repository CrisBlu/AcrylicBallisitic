using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotsDisplay : MonoBehaviour
{
    public enum SlotType
    {
        Full,
        Empty,
        Special
    }

    [SerializeField] GameObject slotPrefab;
    [SerializeField] Dictionary<SlotType, Sprite> slotSprites;
    [SerializeField] Sprite fullSlotSprite;
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] Sprite specialSlotSprite;
    [SerializeField] int maxSlots = 6;

    GameObject[] slots;
    Image[] slotImages;

    public void SetSlots(int slotsCount)
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (i < slotsCount)
            {
                slotImages[i].sprite = fullSlotSprite;
            }
            else
            {
                slotImages[i].sprite = emptySlotSprite;
            }
        }
    }

    public void SetSlot(int index, SlotType type)
    {
        if (index < 0 || index >= maxSlots) return;
        slotImages[index].sprite = slotSprites[type];
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