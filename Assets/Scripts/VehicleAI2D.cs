using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAI2D : MonoBehaviour {

	public float mass = 5;
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

	//Weights set priority for actions.
	//Increase a weight to increase the influence of that Behavior Force
	public float seekWeight = 1f;
	public float fleeWeight = 1f;
	public float arriveWeight = 1f;
	public float pursuitWeight = 1f;
	public float evadeWeight = 1f;
	public float wanderWeight = 1f;
	public float obstacleAvoidanceWeight = 1f;
	public float separationWeight = 1f;

	void Start () {
		acceleration = Vector2.zero;
		previousTargetPosition = target.position; //Might remove this.  Can probably get the same way I get players
	}
	
	void FixedUpdate () {
		position = transform.position;
		switch(currentState){
			case AIState.Idle:
				break;
			case AIState.Seek:
				Vector2 seekForce = Seek(target.position);
				seekForce *= seekWeight;
				ApplyForce(seekForce);
				break;
			case AIState.Arrive:
				Vector2 arriveForce = Arrive(target.position);
				arriveForce *= arriveWeight;
				Debug.Log("Arrive Force: " +arriveForce);
				ApplyForce(arriveForce);
				break;
			case AIState.Pursuit: 
				Vector2 pursuitForce = Pursuit(target.position);
				pursuitForce *= pursuitWeight;
				ApplyForce(pursuitForce);
				break;
			case AIState.Wander:
				Vector2 wanderForce = Wander();
				wanderForce *= wanderWeight;
				ApplyForce(wanderForce);
				break;
			case AIState.Flee:
				Vector2 fleeForce = Flee(target.position);
				fleeForce *= fleeWeight;
				ApplyForce(fleeForce);
				break;
			case AIState.Evade:
				Vector2 evadeForce = Evade(target.position);
				evadeForce *= evadeWeight;
				ApplyForce(evadeForce);
				break;
		}
		Vector2 obstacleAvoidForce = CheckForObstacles();
		obstacleAvoidForce *= obstacleAvoidanceWeight;
		ApplyForce(obstacleAvoidForce);

		Vector2 separationAvoidForce = Separation();
		ApplyForce(separationAvoidForce);
		DoSteering();
	}

	void ApplyForce(Vector2 force){
		acceleration += force / mass;
	}

	void DoSteering(){
		Vector2.ClampMagnitude(acceleration, maxForce); //clamp on max force

		velocity += acceleration; //add accelleration to current velocity
		velocity = Vector2.ClampMagnitude(velocity, maxSpeed); //clamp on max speed

		//rotation and movement
		float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90; // -90 is an angle adjustment to point to the up vector
		transform.rotation = Quaternion.Lerp (transform.rotation,Quaternion.Euler (new Vector3 (0, 0, angle)), 2 * Time.deltaTime);
		transform.Translate(Vector3.up * velocity.magnitude * Time.deltaTime);

		//Reset values we recalculate each frame		
		acceleration = Vector2.zero;
		if(target != null){
			previousTargetPosition = target.position;
		}
	}

	public Vector2 Seek(Vector2 targetPos){
		Vector2 desired = targetPos - position;
		desired.Normalize();
		desired *= maxSpeed;
		Vector2 steeringVelocity = desired - velocity ; 
		Debug.DrawRay(transform.position, velocity.normalized * 2, Color.green);
        Debug.DrawRay(transform.position, desired.normalized * 2, Color.magenta);
		return steeringVelocity;
	}

	public Vector2 Flee(Vector2 target){
		Vector2 desired = position - target;
		desired.Normalize();
		desired *= maxSpeed;
		Vector2 steeringVelocity = desired - velocity ; 
		Vector2.ClampMagnitude(steeringVelocity, maxForce);
		return steeringVelocity;
	}

	public Vector2 Arrive(Vector2 target){
		Vector2 desired = target - position;
		float d = desired.magnitude;
		if(d<100){
			float m = Map(0,100, 0,maxSpeed, d);
			desired = desired.normalized * m * Time.deltaTime;
		} else {
			desired = desired.normalized * maxSpeed * Time.deltaTime;
		}

		Vector2 steeringVelocity = desired - velocity ; 
		Vector2.ClampMagnitude(steeringVelocity, maxForce);
		return steeringVelocity;
	}

	public Vector2 Pursuit(Vector2 target){
		int T = 3;
		Vector2 targetVelocity = (target - previousTargetPosition) / Time.deltaTime;
		Vector2 futurePosition = target + targetVelocity * T;
		Debug.Log("target: " +target + " : " + "future: " + futurePosition + " : prev target: " + previousTargetPosition);

		//Can call seek because we are just seeking a future position!
		return Seek(futurePosition);
	}

	public Vector2 Evade(Vector2 target){
		int T = 3;
		Vector2 targetVelocity = (target - previousTargetPosition) /Time.deltaTime;
		Vector2 futurePosition = target + targetVelocity * T;
		return Flee(futurePosition);
	}

	public Vector2 Wander(){
		Vector2 circlePos = position + velocity.normalized * WANDER_CIRCLE_DISTANCE; 
		Vector2 circleTargetSpot = Random.insideUnitCircle * WANDER_CIRCLE_RADIUS;
		Vector2 targetVector = circlePos + circleTargetSpot;
		Debug.DrawLine(circlePos, targetVector, Color.cyan);
		return Seek(targetVector); 
	}

	public Vector2 CheckForObstacles(){
		Debug.DrawRay(position, transform.up*5f);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 5f,LayerMask.GetMask("Obstacles"));
		if(hit){
			Vector2 targetPos = hit.transform.position;
			Vector2 dodgeVelocity = hit.normal * maxForce;
			Debug.DrawLine(position, dodgeVelocity, Color.yellow);
			acceleration = Vector2.zero;
			return dodgeVelocity;
		}
		return Vector2.zero;
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

	public Vector2 Separation(){
		Vector2 force = Vector2.zero;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, 2f, Vector2.zero);
        for (int i = 0; i < hits.Length; i++){
            if (hits[i] == true && hits[i].transform.tag == transform.tag && hits[i].transform != this.transform){
				Debug.Log("Found a friend");
                Vector2 localTarget = hits[i].transform.position;
				Vector2 thisForce = Flee(localTarget);
                Debug.DrawLine(position, thisForce, Color.blue);
                force += thisForce;
            }
        }
		force *= separationWeight;
        
        return force;
	}

	public Vector2 Cohesion(){
        Vector2 force = Vector2.zero;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, 2f, Vector2.zero);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == true && hits[i].transform.tag == transform.tag && hits[i].transform != this.transform)
            {
                Vector2 localTarget = hits[i].transform.position;
                force += localTarget;
            }
        }
		force = force / hits.Length;
        return Seek(force);
	}
    void OnDrawGizmos()
    {
		 #if UNITY_EDITOR
        float r = 2f;
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, new Vector3(0, 0, 1), r);
		#endif
    }
}
