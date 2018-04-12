using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	[SerializeField] private Rigidbody2D myRigidibody;
	private Transform myTransform;
	public float jumpForce;
	public LayerMask whatIsGround;
	[SerializeField]
	private float groundedRadius;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private bool grounded;
	[SerializeField]
	string landingSoundName = "LandingFootSteps";

	private bool facingRight;
	private Animator animPlayer;
	Transform playerGraphics;
	private bool wasGrounded;
	AudioManager audioManager;

	private void Start()
	{
		facingRight = true;
		myRigidibody = GetComponent<Rigidbody2D>();
		myTransform = GetComponent<Transform>();
		groundCheck = GetComponent<Transform>();
		animPlayer = GetComponent<Animator>();

		audioManager = AudioManager.instance;

		playerGraphics = myTransform.transform.Find("Graphics");
		if (playerGraphics == null)
		{
			Debug.Log("There is no 'Graphics' object as a child of the player");
		}

		if (audioManager == null)
		{
			Debug.LogError("No audioManager found in the scene.");
		}

		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

	}

	void FixedUpdate()
	{
		// JUMP
		//check ground
		wasGrounded = grounded;
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		animPlayer.SetBool("Ground", grounded);

		if (wasGrounded != grounded && grounded == true)
		{
			audioManager.PlaySound(landingSoundName);
		}

		//MOVE
		float move = Input.GetAxis("Horizontal");
		animPlayer.SetFloat("Speed", Mathf.Abs(myRigidibody.velocity.x));

		myRigidibody.velocity = new Vector2(move * PlayerStatus.instance.moveSpeed, myRigidibody.velocity.y);

		if (move > 0f && !facingRight)
			Flip();

		else if (move < 0f && facingRight)
			Flip();


	}

	private void Update()
	{
		// Use Update to get the Input values, to not lose any "jump" key pressed
		if (grounded && Input.GetButtonDown("Jump"))
		{
			myRigidibody.AddForce(new Vector2(0f, jumpForce));
		}
	}

	private void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = playerGraphics.localScale;
		theScale.x *= -1;
		playerGraphics.localScale = theScale;
	}

	void OnUpgradeMenuToggle(bool active)
	{
		// Handle what happens when the upgrade menu is toggled;
		GetComponent<Player>().enabled = !active;
		Weapon _weapon = GetComponentInChildren<Weapon>();
		if (_weapon != null)
		{
			_weapon.enabled = !active;
		}
	}
	// call after destroy the Player to remove from de Delegate
	private void OnDestroy()
	{
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}

}
