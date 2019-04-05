using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBall : MonoBehaviour 
{
	private Rigidbody _rb;
	private SphereCollider _col;
	public LayerMask _mask;
	public bool consoleOutput;
	public float startSpeed = 1f;
	public int collisions = 0;
	
	public bool increaseSpeed;
	public float time = 0f;
	public float speedMagnitude;

	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	private void OnCollisionEnter(Collision other)
	{
		if(consoleOutput)
			Debug.Log( Time.fixedTime + " Ball collided with " + other.gameObject );

		// Give permission to increase speed later
		if(!increaseSpeed)
			increaseSpeed = true;
		collisions++;
	}

	// Use this for initialization
	void Start () 
	{
		_rb = GetComponent<Rigidbody>();
		_col = GetComponent<SphereCollider>();
		_mask = LayerMask.GetMask("Walls");

		// Randomize start direction
		Vector3 startDirection = new Vector3(
			Random.Range(0f,360f),
			Random.Range(0f,360f),
			Random.Range(0f,360f));

		// Face start direction
		transform.rotation = Quaternion.Euler(startDirection);

		_rb.AddForce(transform.forward * startSpeed, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Cap movement speed
		if(_rb.velocity.magnitude > 100f)
			_rb.velocity = _rb.velocity.normalized * 100f;

		// Update speed shown in Inspector
		speedMagnitude = _rb.velocity.magnitude;

		// If we have a permission to increase speed...
		if(increaseSpeed)
		{
			time += Time.deltaTime;
			// If enough time has passed since collision, increase speed
			if(time >= 0.05f)
				IncreaseVelocity();
		}
	}

	private void IncreaseVelocity() 
	{
		// Reset time
		time = 0f;
		// Increase speed
		float newSpeed = speedMagnitude * 1.05f; 
		// Speed has been increased this update cycle
		increaseSpeed = false;
		
		_rb.AddForce(_rb.velocity.normalized * newSpeed, ForceMode.VelocityChange);
	}
}
