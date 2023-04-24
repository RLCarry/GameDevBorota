using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;
    private float moveSpeed=15, gravityModifier=2, jumpPower=10,runSpeed = 20;
    public CharacterController charCon;

    private Vector3 moveInput;
    public Transform camTransform;
    private float mouseSensitivity=2;
    public bool invertX;
    public bool invertY;

    private bool canJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGrounded;

    public Animator anim;
    public GameObject bullet;
    public Transform attackPoint;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float yStore = moveInput.y;
        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizMove = transform.right * Input.GetAxis("Horizontal");
       
        moveInput = horizMove+vertMove;
        moveInput.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput *= runSpeed;
        }
        else
        {
            moveInput *= moveSpeed;
        }

        moveInput.y = yStore;
        moveInput.y += Physics.gravity.y * gravityModifier*Time.deltaTime;

        if (charCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y*gravityModifier*Time.deltaTime;
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGrounded).Length > 0;

        if (Input.GetKeyDown(KeyCode.Space)&& canJump==true)
        {
            moveInput.y = jumpPower;
        }
        
        charCon.Move(moveInput * Time.deltaTime);

        //control camera rotatation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"))*mouseSensitivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }

        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z );
        
        camTransform.rotation = Quaternion.Euler(camTransform.rotation.eulerAngles + new Vector3(-mouseInput.y,0f, 0f));
        
        //Handle Shooting
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 50f))
            {
                if (Vector3.Distance(camTransform.position, hit.point) > 2f)
                {
                    attackPoint.LookAt(hit.point);
                }
            }
            else
            {
                attackPoint.LookAt(camTransform.position + (camTransform.forward*30f));
            }

            Instantiate(bullet, attackPoint.position, attackPoint.rotation);
        }
        
        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("Ground", canJump);
    }
    
    
    
}
