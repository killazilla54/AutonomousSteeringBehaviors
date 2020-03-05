using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed;

	public float maxSpeed = 5f;
	public float rotationSpeed = 90f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Quaternion rot = transform.rotation;
		// float z = rot.eulerAngles.z;

		// z -= Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

		// rot = Quaternion.Euler(0,0,z);

		// Vector3 pos = transform.position;

		

		// Vector3 velocity = new Vector3(0, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0);


		////OLD
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		Vector3 mouse_pos = Input.mousePosition;

		Vector3 object_pos = Camera.main.WorldToScreenPoint (transform.position);
		mouse_pos.x = mouse_pos.x - object_pos.x;
		mouse_pos.y = mouse_pos.y - object_pos.y;
		float angle = (Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg) - 90;

		Quaternion rot = Quaternion.Lerp (transform.rotation,Quaternion.Euler (new Vector3 (0, 0, angle)), 2 * Time.deltaTime);

		transform.rotation = rot;

		Vector3 velocity = new Vector3(0,Input.GetAxis("Vertical") * Time.deltaTime * speed,0);
		// Debug.Log(rot * velocity);
		transform.position += rot * velocity;

		// transform.position += new Vector3 (h * speed *Time.deltaTime, v * speed*Time.deltaTime, 0);
	}
}
