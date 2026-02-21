using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform house;
    //float angle = 0;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(house.position, Vector3.up, 20 * Time.captureDeltaTime);
        //angle += speed;
    }
}
