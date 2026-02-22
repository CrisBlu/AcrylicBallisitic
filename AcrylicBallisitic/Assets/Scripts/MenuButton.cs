using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void SwitchScenes(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public async void DelaySwitchScenes(int scene)
    {
        float timer = 0;
        float duration = 2f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            await Task.Yield();
        }

        SceneManager.LoadScene(scene);
    }
}
