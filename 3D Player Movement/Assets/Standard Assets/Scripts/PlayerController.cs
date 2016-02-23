using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	[SerializeField]
	int PlayerNumber = 1;

	Transform enemy;

	Rigidbody2D rigidbody2D;
	Animator animator;

	float horizontal;
	float vertical;

	[SerializeField]
	float maxSpeed = 25;
	Vector3 movement;
	bool crouch;

	[SerializeField]
	float jumpForce = 20;

    [SerializeField]
	float jumpDuration = 0.1f;

	[SerializeField]
	float noDamageTimer;

	public bool damage;
	float noDamage =  1;

	float hopForce;
	float hopDuration;
	bool jumpKey;
	bool falling;
	bool onGround;

	public bool specialAttack;
	public GameObject projectile;

	[SerializeField]
	float attackRate = 0.3f;

	bool[] attack = new bool[2];
	float[] attackTimer = new float[2];
	int[] timesPressed = new int[2];

	//Start, what happens at the begining of the scene.
	void Start()
	{
		rigidbody2D = GetComponent<Rigidbody2D> ();
		animator = GetComponentInChildren<Animator> ();

		hopForce = jumpForce;

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		foreach(GameObject player in players)
		{
			enemy = player.transform;
		}
	}
	//Update, everything that happens every frame.
	void Update()
	{
		//Debug.Log (this.onGround);
		AttackInput ();
		ScaleCheck ();
		OnGroundCheck ();
		Damage ();
		//SpecialAttack();
		UpdateAnimator ();
	}
	//FixedUpdate, all of the physics in action.
	void FixedUpdate()
	{
		horizontal = Input.GetAxis ("Horizontal" + PlayerNumber.ToString ());
		vertical = Input.GetAxis ("Vertical" + PlayerNumber.ToString ());

		Vector3 movement = new Vector3 (horizontal, 0, 0);

		crouch = (vertical < -0.1f);

		if (vertical > 0.1f)
		{
			if(!jumpKey)
			{
				hopDuration += Time.deltaTime;
				hopForce += Time.deltaTime;

				if(hopDuration < jumpDuration)
				{
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, hopForce);
				}
				else
				{
					jumpKey = true;
				}
			}
		}

		if (!onGround && vertical < 0.1f)
		{
			falling = true;
			crouch = false;
		}

		if(attack[0] && !jumpKey || attack[1] && !jumpKey)
		{
			movement = Vector3.zero;
		}

		if (!crouch)
			rigidbody2D.AddForce (movement * maxSpeed);
		else
			rigidbody2D.velocity = Vector3.zero;
	}
	//Makes sure that the players are constantly facing eachother.
	void ScaleCheck()
	{
		if (transform.position.x < enemy.position.x)
			transform.localScale = new Vector3 (-1, 1, 1);
		else
			transform.localScale = Vector3.one;
	}
	//Timers between attacks and limits to how many times you can attack within an instance.
	void AttackInput()
	{
		if (Input.GetButtonDown ("Attack1" + PlayerNumber.ToString ()))
		{
			attack[0] = true;
			attackTimer[0] = 0;
			timesPressed[0]++;
		}

		if(attack[0])
		{
			attackTimer[0] += Time.deltaTime;

			if(attackTimer[0] > attackRate || timesPressed[0] >= 4)
			{
				attackTimer[0] = 0;
				attack[0] = false;
				timesPressed[0] = 0;
			}
		}

		if (Input.GetButtonDown ("Attack2" + PlayerNumber.ToString ()))
		{
			attack[1] = true;
			attackTimer[1] = 0;
			timesPressed[1]++;
		}
		
		if(attack[1])
		{
			attackTimer[1] += Time.deltaTime;
			
			if(attackTimer[1] > attackRate || timesPressed[1] >= 4)
			{
				attackTimer[1] = 0;
				attack[1] = false;
				timesPressed[1] = 0;
			}
		}

	}
	// What happens when the player takes damage.
	void Damage()
	{
		if(damage)
		{
			noDamageTimer += Time.deltaTime;

			if(noDamageTimer > noDamage)
			{
				damage = false;
				noDamageTimer = 0;
			}
		}

		// Player falls backwards if hit in mid-air.
		/*
		if(!onGround)
		{
			rigidbody2D.gravityScale = 10;
			Vector3 direction = enemy.position - transform.position;
			rigidbody2D.AddForce (-direction * 25);
		}
		*/
	}
	/*
	void SpecialAttack()
	{
		if (specialAttack)
		{
			GameObject pr = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
			Vector3 nrDir = new Vector3(enemy.position.x, transform.position.y, 0);
			Vector3 dir = nrDir - transform.position;
			pr.GetComponent<Rigidbody2D>().AddForce(dir * 100, ForceMode2D.Impulse);

			specialAttack = false;
		}
	}
	*/

	//Changes the gravity, (players movement ability) depending on if you are on the ground or in the air.
	void OnGroundCheck()
	{
		if (!onGround)
		{
			rigidbody2D.gravityScale = 5;
		}
		else
		{
			rigidbody2D.gravityScale = 1;
		}
	}
	//Keeps the animator informed on what's happening.
	void UpdateAnimator()
	{
		animator.SetBool ("Crouch", crouch);
		animator.SetBool ("OnGround", this.onGround);
		animator.SetBool ("Falling", this.falling);
		animator.SetFloat ("Movement", Mathf.Abs (horizontal));
		animator.SetBool ("Attack1", attack[0]);
		animator.SetBool ("Attack2", attack[1]);

	}
	//Detector for being on the ground.
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.tag == "Ground")
		{
			onGround = true;
			jumpKey = false;
			hopDuration = 0;
			hopForce = jumpForce;
			falling = false;
		}
	}
	//Detector for being in the air.
	void OnCollisionExit2D(Collision2D col)
	{
		if (col.collider.tag == "Ground")
		{
			onGround = false;
		}
	}

}
