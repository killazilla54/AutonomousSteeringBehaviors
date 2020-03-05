using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {

	float mass =5;
	Vector2 position;
	Vector2 velocity;
	Vector2 acceleration;
	public float maxForce;
	public float maxSpeed;

	public Transform target;
	public Vector2 previousTargetPosition;

	public enum AIState {Idle, Arrive, Seek, Pursuit, Evade, Flee, Wander};
	public AIState currentState;

	public float WANDER_CIRCLE_DISTANCE = 50;
	public float WANDER_CIRCLE_RADIUS = 15;

	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		acceleration = Vector2.zero;
		previousTargetPosition = target.position;
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		position = transform.position;
		//Phase 2 of this will need priority for behaviors.
		//Ex. Check for obstacles before commiting to flee vector.
		switch(currentState){
			case AIState.Idle:
				break;
			case AIState.Seek:
				Seek(target.position);
				break;
			case AIState.Arrive:
				Arrive(target.position);
				break;
			case AIState.Pursuit: 
				Pursuit(target.position);
				break;
			case AIState.Wander:
				Wander();
				break;
		}
		CheckForObstacles();
		DoSteering();
	}

	void ApplyForce(Vector2 force){
		acceleration += force / mass;
	}

	void DoSteering(){
		Vector2.ClampMagnitude(acceleration, maxForce);
		velocity += acceleration; //add accelleration to current velocity
		velocity = Vector2.ClampMagnitude(velocity, maxSpeed);//clamp on max speed

		//rotation and movement
		float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90; // -90 is an angle adjustment to point to the up vector
		transform.rotation = Quaternion.Lerp (transform.rotation,Quaternion.Euler (new Vector3 (0, 0, angle)), 2 * Time.deltaTime);
		transform.Translate(Vector3.up * velocity.magnitude * Time.deltaTime);

		//Reset temp values		
		acceleration = Vector2.zero;
		previousTargetPosition = target.position;
	}

	//Seems to work best for stationary targets.  Use pursuit for Moving.
	protected void Seek(Vector2 targetPos){
		Vector2 desired = targetPos - position;
		desired.Normalize();
		desired *= maxSpeed;
		Vector2 steeringVelocity = desired - velocity ; 
		// Vector2.ClampMagnitude(steeringVelocity, maxForce);
		ApplyForce(steeringVelocity);

		//NOTE: Rotation works, will properly implement later once basics are done
		 transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desired, -Vector3.forward), 10f * Time.deltaTime);
		//in this case, unit always moves on forward vector, rotation is applied to implement steering force


		Debug.DrawRay(transform.position, velocity.normalized * 2, Color.green);
        Debug.DrawRay(transform.position, desired.normalized * 2, Color.magenta);
	}
	//Also seems to be for stationary target. Use evade for moving.
	void Flee(Vector2 target){
		Vector2 desired = position - target;
		desired.Normalize();
		// Debug.Log("desired Normalized:" + desired);
		desired *= maxSpeed;
		// Debug.Log("desired:" + desired);
		// Debug.Log("velocity: " +velocity);
		Vector2 steeringVelocity = desired - velocity ; 
		// Debug.Log("steeringVelocity in Seek: " +steeringVelocity );
		Vector2.ClampMagnitude(steeringVelocity, maxForce);
		// Debug.Log("steeringVelocity in Seek2: " +steeringVelocity );
		ApplyForce(steeringVelocity);
	}

	void Arrive(Vector2 target){
		Vector2 desired = target - position;
		float d = desired.magnitude;
		if(d<100){
			float m = Map(0,100, 0,maxSpeed, d);
			desired = desired.normalized * m * Time.deltaTime;
		} else {
			desired = desired.normalized * maxSpeed * Time.deltaTime;
		}

		Vector2 steeringVelocity = desired - velocity ; 
		// Debug.Log("steeringVelocity in Seek: " +steeringVelocity );
		Vector2.ClampMagnitude(steeringVelocity, maxForce);
		// Debug.Log("steeringVelocity in Seek2: " +steeringVelocity );
		ApplyForce(steeringVelocity);

	}

	//
	void Pursuit(Vector2 target){
		int T = 3;
		Vector2 targetVelocity = (target - previousTargetPosition) / Time.deltaTime;
		Vector2 futurePosition = target + targetVelocity * T;
		//Can call seek because we are just seeking a future position!
		Seek(futurePosition);
	}

	void Evade(Vector2 target){
		int T = 3;
		Vector2 targetVelocity = (target - previousTargetPosition) /Time.deltaTime;
		Vector2 futurePosition = target + targetVelocity * T;
		Flee(futurePosition);
	}

	//wander
	//vars
	//wander ring disance from unit
	//wander ring radius
	//
	//code
	//cirlcle pos = position + velocity.normalized * wander ring distance
	//target = circle pos + new vector2 (wander radius,0).rotate(random between 0,360 aka unit circle) <--vector from circle center to edge
	//return seek(target)

	void Wander(){
		Vector2 circlePos = position + velocity.normalized * WANDER_CIRCLE_DISTANCE; 
		//transform.rotation * Quaternion.Euler(random.Range(-10,10),0,0));
		Vector2 circleTargetSpot = Random.insideUnitCircle * WANDER_CIRCLE_RADIUS;
		Vector2 targetVector = circlePos + circleTargetSpot;
		Debug.DrawLine(circlePos, targetVector, Color.cyan);
		// Debug.DrawLine(targetVector, position , Color.magenta);
		Seek(targetVector); 
	}

	void CheckForObstacles(){
		Debug.DrawRay(position, transform.up*5f);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 5f);
		if(hit){
			Debug.Log("HIT");
			Vector2 targetPos = hit.transform.position;
			Vector2 dodgeVelocity = hit.normal * maxForce;
			Debug.Log("Dodge Velocity: " + dodgeVelocity);
			Debug.DrawLine(position, dodgeVelocity, Color.yellow);
			acceleration = Vector2.zero;
			ApplyForce(dodgeVelocity);
		}
	}

	float Map(float from, float to, float from2, float to2, float value){
		if(value <= from2){
			return from;
		}
		else if(value >= to2){
			return to;
		}
		return (to - from) * ((value - from2) / (to2 - from2)) + from;
	}

	Vector2 SetAngle(Vector2 vector, float value){
		float mag = vector.magnitude;
		vector.x = Mathf.Cos(value * Mathf.Deg2Rad) * mag;
		vector.y = Mathf.Sin(value * Mathf.Deg2Rad) * mag;
		// Debug.Log("New Angle: " + vector);
		return vector;
	}
}
