using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
		target = GameObject.Find("Player").transform;
    }
	// Update is called once per frame
	void Update () {
            transform.position = new Vector3(target.position.x, target.position.y, -100f);		

	}
}
