using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioAssetSO audioAssets;

    public void PlaySound(string name)
    {
        AudioClip clip = audioAssets.GetClipByName(name, out bool enabled);
        if (clip != null && enabled)
        {
            print("Playing sound: " + name);
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }
}