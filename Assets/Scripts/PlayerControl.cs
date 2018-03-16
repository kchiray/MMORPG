using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private Vector3 moveDirection;
    private float speed = 3f;
    private Rigidbody rb;
    private bool canJump = false;
    private float jumpForce = 10f;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //Renders mouse on screen = invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        IsGrounded();

    }
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        PlayerMovement();
        Sprint();

        if (Input.GetMouseButtonDown(0))
        {
            CmdFire();
        }
    }

    /// <summary>
    /// Player AND camera movement
    /// </summary>
    void PlayerMovement()
    {
        var mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 150.0f;
        var mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * 100.0f;
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, mouseX, 0);
        transform.Rotate(mouseY, 0, 0);
        transform.Translate(0, 0, z);
        transform.Translate(x, 0, 0);
    }


    /// <summary>
    /// If player is not grounded = ability to jump
    /// </summary>
    void IsGrounded()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rb.transform.localPosition.y < 7)
            {
                canJump = true;
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                Debug.Log("Jump");

            }
            else
            {
                Debug.Log("NO JUMP");
                canJump = false;
            }
        }


    }

    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 20f;
        }
    }
  


    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 18.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}

