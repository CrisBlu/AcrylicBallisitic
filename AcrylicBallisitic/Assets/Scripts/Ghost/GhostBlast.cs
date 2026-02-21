using PrimeTween;
using UnityEngine;

public class GhostBlast : MonoBehaviour
{
    [SerializeField] float blastRadiusMultiplier = 1.25f;
    [SerializeField] float damage = 20f;
    [SerializeField] float flashInterval = 0.1f;

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        Tween tween = Tween.Custom(0f, 1f, flashInterval, (_) => {
            if (Random.value > 0.85f)
            {
                meshRenderer.enabled = !meshRenderer.enabled;
            }
        }, Ease.Linear, -1, CycleMode.Yoyo);
        
        Tween.Scale(transform, transform.localScale * blastRadiusMultiplier, 1.0f, Ease.OutCubic).OnComplete(() =>
        {
            Destroy(gameObject);
            tween.Stop();
        });
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetManager().DamagePlayer();
        }
    }
}