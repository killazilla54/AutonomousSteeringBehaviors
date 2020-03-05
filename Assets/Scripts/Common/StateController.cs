using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

	
	public State currentState;
	public State remainState;
	public VehicleProperties vehicleProperties;
	
	[HideInInspector]
	public Transform target;
	[HideInInspector]
	public Vector2 previousTargetPosition;
	[HideInInspector]
	public Vector2 velocity;
	private Vector2 acceleration;
	
	[HideInInspector]
	public float shootTimer;

	void FixedUpdate(){
		currentState.UpdateState(this);
		DoSteering();
	}

	public void TransitionToState(State nextState){
		if(nextState != remainState){
			currentState = nextState;
			OnExitState();
		}
	}

	public void ApplyForce(Vector2 steeringForce){
		acceleration += steeringForce / vehicleProperties.mass;
	}

	private void DoSteering(){
		Vector2.ClampMagnitude(acceleration, vehicleProperties.maxForce); //clamp on max force

		velocity += acceleration; //add accelleration to current velocity
		velocity = Vector2.ClampMagnitude(velocity, vehicleProperties.maxSpeed); //clamp on max speed

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

	public void FireShot(){

	}

	private void OnExitState(){

	}
}
