using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatedDemoPlayer : MonoBehaviour {

	public float speed;
	
	void FixedUpdate () {
		Vector2 pos = transform.position;
		pos += Vector2.right * speed * Time.deltaTime;
		transform.position = pos;
	}
}
