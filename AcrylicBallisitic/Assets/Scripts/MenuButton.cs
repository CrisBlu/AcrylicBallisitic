using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void SwitchScenes(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
