using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	public Transform currentTarget;
	public float rotSpeed = 25f;
	public float radarDistance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		PingRadar();
		if(currentTarget != null){
			TurnToTarget();
		}
	}

    void TurnToTarget(){
        Vector3 dir = currentTarget.position - transform.position;
        dir.Normalize();

        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }

    void PingRadar(){
		if(currentTarget != null && Vector2.Distance(transform.position,currentTarget.position) < radarDistance){
			return;
		}
		currentTarget = null;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radarDistance, Vector2.zero);
        for (int i = 0; i < hits.Length; i++) {
			if (hits[i] == true && hits[i].transform.tag == "Player") {
                currentTarget = hits[i].transform;
            }
        }
    }
}
