using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform house;
    [SerializeField] Image panel;
    //float angle = 0;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(house);
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    public async void GoIn()
    {
        speed = 0;
        Color newColor = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
        Debug.Log("Going in");
        while(Vector3.Distance(transform.position, house.position) > .1f)
        {
            newColor.a += .05f;
            panel.color = newColor;
            transform.Translate(Vector3.forward * Time.deltaTime * 200);
            await Task.Yield();
        }

        Debug.Log("In");
    }
}

