using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] int Speed = 1;
    [SerializeField] float BulletLineDuration = .05f;
    [SerializeField] public float MultiShotPenalty = .2f;
    [SerializeField] float PenaltyDuration = .1f;

    //TODO: Make Camera a static object (probably)
    [SerializeField] Transform Gun;
    [SerializeField] GameObject BulletFX;



    private InputAction movement;
    private Rigidbody rb;
    private InputAction attack;
    [HideInInspector] public int penaltyLevel = 0;
    Vector2 direction = Vector2.zero;
    Vector3 ShootVec;
    


    

    void Start()
    {
        movement = InputSystem.actions.FindAction("Move");
        attack = InputSystem.actions.FindAction("Attack");

        attack.performed += Shoot;

        rb = GetComponent<Rigidbody>();
        
        
        
    }

    void Update()
    {
        //Movement
        direction = movement.ReadValue<Vector2>();
        Vector3 directionv3 = new Vector3(direction.x, 0, direction.y);
        rb.MovePosition(rb.position + directionv3 * Speed * Time.deltaTime);


        ShootVec = SceneCamera.cursorPos;
        ShootVec.y = transform.position.y;
        transform.LookAt(ShootVec);


    }

    public void Shoot(InputAction.CallbackContext context)
    {
        //Will shoot past the cursor location and hit anything behind, can limit range to when was click if needed
        Debug.Log(penaltyLevel);

        Vector3 GunShootDir = Vector3.Normalize(ShootVec - Gun.position);

        if (MultiShotPenalty > 0)
        {
            GunShootDir.x += (Random.Range(-MultiShotPenalty, MultiShotPenalty) * penaltyLevel);
            GunShootDir.z += (Random.Range(-MultiShotPenalty, MultiShotPenalty) * penaltyLevel);
        }

        Ray ray = new Ray(Gun.position, Vector3.Normalize(GunShootDir));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20))
        {

            //Bullet Line
            GameObject BFXObj = Instantiate(BulletFX, Gun);
            LineRenderer BFXLine = BFXObj.GetComponent<LineRenderer>();
            BFXLine.SetPosition(0, Gun.position);
            BFXLine.SetPosition(1, hit.point);

            BFXLineFade(BFXObj);

            penaltyLevel++;
            Invoke("DecreasePenalty", PenaltyDuration * penaltyLevel);
            


        }
        else
        {
            //This should never happen
            Debug.Log("miss");
        }

        
    }

    public async void BFXLineFade(GameObject BFXObj)
    {
        float timer = 0;
        while (timer < BulletLineDuration)
        {
            timer += Time.deltaTime;
            await Task.Yield();
        }

        Destroy(BFXObj);

    }

    public void DecreasePenalty()
    {
        if (penaltyLevel > 0)
            penaltyLevel--;

    }
    

  
    


}
