using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	bool isGrounded;
	// Use this for initialization
	void Start () 
	{
		isGrounded = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.LeftArrow))
		{
			this.rigidbody2D.AddForce(Vector2.right * -300 * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.RightArrow))
		{
			this.rigidbody2D.AddRelativeForce(Vector2.right * 300 * Time.deltaTime);
		}

		if (Input.GetKeyDown (KeyCode.Space) && isGrounded) 
		{
			this.rigidbody2D.AddRelativeForce(Vector2.up * 5000 * Time.deltaTime);
			isGrounded = false;
		}
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "ground") 
		{
			isGrounded = true;
		}
	}
}
