using UnityEngine;

public class StanAnimCtrl : MonoBehaviour
{

    private Animator AnimationController;
    private Rigidbody Rigid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AnimationController = GetComponent<Animator>();

       Rigid =  GetComponentInParent<Rigidbody>();

        

        if (AnimationController == null)
        {
            Debug.LogError("Animator component not found on this GameObject!");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        AnimationController.SetFloat("PlayerVelocity",Rigid.linearVelocity.x + Rigid.linearVelocity.z);

        print("Stans velocity is" + Rigid.linearVelocity.x + Rigid.linearVelocity.z);
    }
}
