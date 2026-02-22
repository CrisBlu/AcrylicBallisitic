using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioPlayer))]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void SwitchScenes(int scene)
    {
        StartCoroutine(DelayLoadScene(scene));
    }

    IEnumerator DelayLoadScene(int scene)
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(scene);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
        audioPlayer.PlaySound("BUTTON_HOVER");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
        audioPlayer.PlaySound("BUTTON_CLICK");
    }
}
