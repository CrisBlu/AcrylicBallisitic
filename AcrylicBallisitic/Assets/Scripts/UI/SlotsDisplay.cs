using UnityEngine;
using UnityEngine.UI;

public class SlotsDisplay : MonoBehaviour
{
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Color fullColor = Color.green;
    [SerializeField] Color emptyColor = Color.darkGray;
    // [SerializeField] Sprite fullSlotSprite;
    // [SerializeField] Sprite emptySlotSprite;
    [SerializeField] int maxSlots = 6;

    GameObject[] slots;
    Image[] slotImages;

    public void SetSlots(int slotsCount)
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (i < slotsCount)
            {
                slotImages[i].color = fullColor;
            }
            else
            {
                slotImages[i].color = emptyColor;
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