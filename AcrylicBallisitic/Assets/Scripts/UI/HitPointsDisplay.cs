using UnityEngine;
using UnityEngine.UI;

public class HitPointsDisplay : MonoBehaviour
{
    [SerializeField] GameObject hitPointPrefab;
    [SerializeField] Color fullColor = Color.green;
    [SerializeField] Color emptyColor = Color.darkGray;
    // [SerializeField] Sprite fullHitPointSprite;
    // [SerializeField] Sprite emptyHitPointSprite;
    [SerializeField] int maxHitPoints = 6;

    GameObject[] hitPoints;
    Image[] hitPointImages;

    public void SetHitPoints(int hitPoints)
    {
        for (int i = 0; i < maxHitPoints; i++)
        {
            if (i < hitPoints)
            {
                hitPointImages[i].color = fullColor;
            }
            else
            {
                hitPointImages[i].color = emptyColor;
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

        hitPoints = new GameObject[maxHitPoints];
        hitPointImages = new Image[maxHitPoints];
        for (int i = 0; i < maxHitPoints; i++)
        {
            hitPoints[i] = Instantiate(hitPointPrefab, transform);
            hitPointImages[i] = hitPoints[i].GetComponent<Image>();
        }
    }
}