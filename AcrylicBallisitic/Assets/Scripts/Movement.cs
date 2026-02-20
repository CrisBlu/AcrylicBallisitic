using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] int Speed = 1;

    //TODO: Make Camera a static object (probably)
    [SerializeField] Camera SceneCamera;
    [SerializeField] Transform Gun;


    private InputAction movement;
    private Rigidbody rigidbody;
    private InputAction attack;
    Vector2 direction = Vector2.zero;
    Vector3 cursorPos;
    
    

    void Start()
    {
        movement = InputSystem.actions.FindAction("Move");
        attack = InputSystem.actions.FindAction("Attack");

        attack.performed += Shoot;

        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Movement
        direction = movement.ReadValue<Vector2>();
        Vector3 directionv3 = new Vector3(direction.x, 0, direction.y);
        rigidbody.MovePosition(rigidbody.position + directionv3 * Speed * Time.deltaTime);

        //Look At Cursor for aiming
        cursorPos = WorldPositionFromMouse();
        cursorPos.y = transform.position.y;
        transform.LookAt(cursorPos);


        
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        Ray ray = new Ray(Gun.position, Gun.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {
            Debug.Log("hit" + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("miss");
        }
    }

    public Vector3 WorldPositionFromMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = SceneCamera.nearClipPlane;
        Ray ray = SceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    


}
