using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Player_Move : MonoBehaviour
{
    Rigidbody rb; 
    [Tooltip("Current players speed")]
    public float currentSpeed;
    [Tooltip("Assign players camera here")]
    [HideInInspector] public Transform cameraMain;
    [Tooltip("Force that moves player into jump")]
    public float jumpForce = 500;
    [Tooltip("Position of the camera inside the player")]
    [HideInInspector] public Vector3 cameraPosition;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraMain = transform.Find("Player Camera").transform;
    }
    private Vector3 slowdownV;
    private Vector2 horizontalMovement; // 수평운동

    [Tooltip("The maximum speed you want to achieve")]
    public int maxSpeed = 5;
    [Tooltip("The higher the number the faster it will stop")]
    public float deaccelerationSpeed = 15.0f;


    [Tooltip("Force that is applied when moving forward or backward")]
    public float accelerationSpeed = 50000.0f;

    void Start()
    {
        PlayerMovementLogic();
    }

    void PlayerMovementLogic()
    {
        currentSpeed = rb.velocity.magnitude;   // 현재 속도의 크기(x,y,z의 루트제곱)
        horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z); // 수평운동, x와 z 축 속도
        if (horizontalMovement.magnitude > maxSpeed)    // 수평속도크기가 한계치보다 크면
        {
            horizontalMovement = horizontalMovement.normalized; // 벡터의 방향은 그대로 둔채로 크기를 1로 정규화시킴
            horizontalMovement *= maxSpeed; // max스피드를 곱해줘서 벡터의 방향은 그대로 두고 크기를 Max 스피드로 유지시킴, -> 결론적으로 속도의 크기가 Max를 넘지못하게함
        }
        rb.velocity = new Vector3(
            horizontalMovement.x,
            rb.velocity.y,
            horizontalMovement.y    // z축임
        );

        if (grounded)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity,
                new Vector3(0, rb.velocity.y, 0),
                ref slowdownV,
                deaccelerationSpeed);
            // 속도  = 현재위치, 목표, 현재속도, 최대감속속도
        }

        if (grounded)
        {
            rb.AddRelativeForce(Input.GetAxis("Horizontal") * accelerationSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * accelerationSpeed * Time.deltaTime);
        }
        else
        {
            rb.AddRelativeForce(Input.GetAxis("Horizontal") * accelerationSpeed / 2 * Time.deltaTime, 0, Input.GetAxis("Vertical") * accelerationSpeed / 2 * Time.deltaTime);
        }
        
        
        /*
		 * Slippery issues fixed here
		 */
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) // 계속 움직이지 않는다면 항상 감속해야함 수평 또는 수직
        {
            deaccelerationSpeed = 0.5f;
        }
        else
        {
            deaccelerationSpeed = 0.1f;
        }
    }

    void Jumping()  // Space로 점프
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddRelativeForce(Vector3.up * jumpForce);
        }
    }

    void Crouching()    // C눌러서 앉기
    {
        if (Input.GetKey(KeyCode.C))
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 0.6f, 1), Time.deltaTime * 15);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 15);

        }
    }

    [Tooltip("Tells us weather the player is grounded or not.")]
    private bool grounded;      
    /*
	* checks if our player is contacting the ground in the angle less than 60 degrees
	*	if it is, set groudede to true
	*/
    void OnCollisionStay(Collision other)
    {
        foreach (ContactPoint contact in other.contacts)
        {
            if (Vector2.Angle(contact.normal, Vector3.up) < 60)
            {
                grounded = true;
            }
        }
    }
    /*
	* On collision exit set grounded to false
	*/
    void OnCollisionExit()
    {
        grounded = false;
    }
    // Update is called once per frame
    void Update()
    {
        Jumping();

        Crouching();
    }
}
