using UnityEngine;
using System.Collections;

public class RunnerMotor : MonoBehaviour 
{
	//Anim
	private CharacterController controller;
	private Animator anim;

	//Movement
	private const float turnSpeed = 0.05f;
	private float jumpForce = 5.0f;
	private float gravity = 12.0f;
	private float verticalVelocity;
	private int desiredLane = 1; //0 = left, 1 = Middle, 2 = Right
	private const float LaneDistance = 2.5f;

	//Speed Modifier
	private float originalSpeed = 7.0f;
	private float speed;
	private float speedIncreaseLastTick;
	private float speedIncreaseTime = 2.5f;
	private float speedIncreaseAmount = 0.1f;


	private Vector3 offset = new Vector3(0, 5, -12);
	private bool isRunning = false;

	private void Start()
	{
		speed = originalSpeed;
		controller = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
	}
	private void Update()
	{
		if (!isRunning)
			return;
		
		//speed modifier
		if (Time.time - speedIncreaseLastTick > speedIncreaseTime) 
		{
			speedIncreaseLastTick = Time.time;
			speed += speedIncreaseAmount;
			gameManager.Instance.UpdateModifier (speed - originalSpeed);
		}

		//Gather input on which lane we should be
		if (MobileInput.Instance.SwipeLeft)
			MoveLane (false);	
		if (MobileInput.Instance.SwipeRight)
			MoveLane (true);
		
		//Calculate where we should be
		Vector3 targetPosition = transform.position.z * Vector3.forward;
		if (desiredLane == 0)
			targetPosition += Vector3.left * LaneDistance;
		else if (desiredLane == 2)
			targetPosition += Vector3.right * LaneDistance;

		//Calculate our move delta
		Vector3 moveVector = new Vector3(0, 0, 0);
		moveVector.x = (targetPosition - transform.position).normalized.x * speed;

		bool Ground = isGrounded ();
		anim.SetBool ("IsGrounded", Ground);

		// Calculate Y
		if (Ground) {
			
			verticalVelocity = -0.1f;

			if (MobileInput.Instance.SwipeUp) 
			{
				//Jump
				anim.SetTrigger ("Jump");
				verticalVelocity = jumpForce;
			} 
			else if (MobileInput.Instance.SwipeDown) 
			{
				//Slide
				StartSliding();
				Invoke ("StopSliding", 1.1f);
			}
		} 
		else 
		{
			verticalVelocity -= (gravity * Time.deltaTime);

			//fast falling mechanic
			if (MobileInput.Instance.SwipeDown) 
			{
				verticalVelocity = -jumpForce;
			}
		}
		
		moveVector.y = verticalVelocity;
		moveVector.z = speed;

		// Move Pengu
		controller.Move (moveVector * Time.deltaTime);

		//Rotate Pengu
		Vector3 dir = controller.velocity;

		if (dir != Vector3.zero) 
		{
			dir.y = 0;
			transform.forward = Vector3.Lerp(transform.forward, dir,turnSpeed);
		}
	}

	private void MoveLane(bool goingRight)
	{
		desiredLane += (goingRight) ? 1 : -1;
		desiredLane = Mathf.Clamp (desiredLane, 0, 2);
	}

	private bool isGrounded()
	{
		Ray groundRay = new Ray(new Vector3(controller.bounds.center.x,
			(controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
			controller.bounds.center.z),
			Vector3.down);
		Debug.DrawRay (groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

		return (Physics.Raycast(groundRay,0.2f + 0.1f));
	}

	private void StartSliding()
	{
		anim.SetTrigger ("Slide");
		controller.height /= 1.0f;
		controller.center = new Vector3 (controller.center.x, controller.center.y / 1.0f, controller.center.z);
	}

	private void StopSliding()
	{
		anim.SetTrigger ("Running");
		controller.height *= 1.0f;
		controller.center = new Vector3 (controller.center.x, controller.center.y * 1.0f, controller.center.z);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		switch (hit.gameObject.tag) 
		{
			case "Obstacle":
			Crash();
			break;
		}
	}

	private void Crash()
	{
		anim.SetTrigger ("Death");
		isRunning = false;
		gameManager.Instance.OnDeath();
	}

	public void StartRunning()
	{
		isRunning = true;
		anim.SetTrigger ("StartRunning");
	}

	public void Revive()
	{
		StartRunning();
		anim.SetTrigger("Respawn");
		transform.position = transform.TransformPoint(offset);
	}
}
