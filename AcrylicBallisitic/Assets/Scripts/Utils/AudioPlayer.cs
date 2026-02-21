using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioAssetSO audioAssets;

    public void PlaySound(string name)
    {
        AudioClip clip = audioAssets.GetClipByName(name, out bool enabled);
        if (clip != null && enabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }
}