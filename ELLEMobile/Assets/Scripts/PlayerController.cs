using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	// Variables for player jumping
	public float jumpForce;
	private bool isGrounded = true;
	public Transform groundCheck;
	private float groundRadius = .2f;
	public LayerMask whatIsGround;
	Rigidbody2D rb;

	public Text testText;

	// Variables for player forward movement
	public float moveSpeed;
	private Vector3 playerDirection = Vector3.right;

	public void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	public void FixedUpdate ()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		ForwardMovement();

		float accel = Input.acceleration.z;
		//testText.text = accel.ToString();

		if (PlayerPrefs.GetInt("Motion Toggle") == 1)
		{
			if (accel < -1f)
			{
				Jump();
			}
		}
	}

	// Adds force to the player's y direction causing him to jump when this
	// function is called
	public void Jump()
	{
		if (isGrounded)
		{
			rb.AddForce(Vector2.up * jumpForce);
		}
	}

	public void ForwardMovement()
	{
        int difficulty = PlayerPrefs.GetInt("Difficulty Setting");

        if (difficulty == 0)
    		transform.Translate(playerDirection * moveSpeed * Time.deltaTime);
        else if (difficulty == 1)
            transform.Translate(playerDirection * (moveSpeed + 4) * Time.deltaTime);
        else
            transform.Translate(playerDirection * (moveSpeed + 8) * Time.deltaTime);


	}
}
