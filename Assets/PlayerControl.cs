using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public float moveForce = 35f;			// Amount of force added to move the player left and right.
	public float maxSpeed =5f;				// The fastest the player can travel in the x axis.
	// Use this for initialization
	private Camera camera;
	
	void Start () {
		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			// Cache the horizontal input.
			float h = Input.GetAxis("Horizontal");
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(h * rigidbody2D.velocity.x < maxSpeed){
				// ... add a force to the player.
				if(h < 0 && camera.WorldToScreenPoint(transform.position).x > 0){
					rigidbody2D.AddForce(Vector2.right * h * moveForce);
				} else if(h > 0 && camera.WorldToScreenPoint(transform.position).x < Screen.width){
					rigidbody2D.AddForce(Vector2.right * h * moveForce);
				}
			}
			
			// Cache the horizontal input.
			float v = Input.GetAxis("Vertical");
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(v * rigidbody2D.velocity.y < maxSpeed){
				// ... add a force to the player.
				if(v > 0 && camera.WorldToScreenPoint(transform.position).y < Screen.height){
					rigidbody2D.AddForce(Vector2.up * v * moveForce);
				} else if(v < 0 && camera.WorldToScreenPoint(transform.position).y > 0){
					rigidbody2D.AddForce(Vector2.up * v * moveForce);
				}
			}
		} else {
			this.enabled = false;
		}

	}
}