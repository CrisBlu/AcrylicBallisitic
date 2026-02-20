using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

//TODO: Call this anything else please
public class Movement : MonoBehaviour
{
    [SerializeField] int Speed = 1;
    [SerializeField] float BulletLineDuration = .05f;
    
    [SerializeField] float PenaltyDuration = .1f;
    [SerializeField] float ReloadTime = 2f;

    [SerializeField] Transform Gun;
    [SerializeField] GameObject BulletFX;


    //Called or will need to be called in different script, maybe player stats SO or Static is needed
    [SerializeField] public float MultiShotPenalty = .2f;
    [HideInInspector] public int penaltyLevel = 0;

    private InputAction movement;
    private Rigidbody rb;
    private InputAction attack;
    private float penaltyTimer = 0;
    Vector2 direction = Vector2.zero;
    Vector3 LookVec;
    private bool canShoot = true;
    


    

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

        //Rotation to look at cursor
        LookVec = SceneCamera.cursorPos;
        LookVec.y = transform.position.y;
        transform.LookAt(LookVec);

        //Multishot Penalty Timer
        if (penaltyTimer > 0)
        {
            penaltyTimer -= Time.deltaTime;
        }
        else
        {
            penaltyLevel = 0;
        }


    }

    //TODO: Add Ammo and Reload
    public void Shoot(InputAction.CallbackContext context)
    {
        if(!canShoot)
        {
            Debug.Log("Out of Ammo");
            return;
        }

        //Will shoot past the cursor location and hit anything behind, can limit range to where was click if needed
        Debug.Log(penaltyLevel);

        Vector3 ShootAtPoint = LookVec;
  
        if (MultiShotPenalty > 0)
        {
            ShootAtPoint.x += (Random.Range(-MultiShotPenalty, MultiShotPenalty) * penaltyLevel);
            ShootAtPoint.z += (Random.Range(-MultiShotPenalty, MultiShotPenalty) * penaltyLevel);
        }

        Vector3 GunShootDir = Vector3.Normalize(ShootAtPoint - Gun.position);

        

        Ray ray = new Ray(Gun.position, GunShootDir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            //Shooting
            GameManager.GetManager().UseBullet(true);
            if(GameManager.GetManager().GetPlayerAmmoCount() == 0)
            {
                canShoot = false;
                Reload();
            }
            

            //Bullet Line
            GameObject BFXObj = Instantiate(BulletFX, Gun);
            LineRenderer BFXLine = BFXObj.GetComponent<LineRenderer>();
            BFXLine.SetPosition(0, Gun.position);
            BFXLine.SetPosition(1, hit.point);

            BFXLineFade(BFXObj);

            penaltyLevel++;
            penaltyTimer = PenaltyDuration;

            


        }
        else
        {
            //This should never happen currently
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

    public async void Reload()
    {
        float timer = 0;
        float reloadTimePerBullet = ReloadTime / 6;
        
        //Maybe this is a bad idea, but for animation purposes I had the bullets reload one by one
        for (int i = 0; i < 6; i++)
        {
            while (timer < reloadTimePerBullet)
            {
                timer += Time.deltaTime;
                await Task.Yield();
            }

            timer = 0;
            GameManager.GetManager().ReloadBullet();
        }

        canShoot = true;
        

        
    }

    public void DecreasePenalty()
    {
        if (penaltyLevel > 0)
            penaltyLevel--;

    }
    

  
    


}
