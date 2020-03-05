using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float moveSpeed;
    public float lifespan;
    float lifeTime;

	// Use this for initialization
	void Start () {
		lifeTime = lifespan;
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime > 0)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
