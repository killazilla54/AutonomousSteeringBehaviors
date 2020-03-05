using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/VehicleProperties")]

public class VehicleProperties : ScriptableObject {

	//Physics
	public float maxSpeed;
	public float maxForce;
	public float mass;

	//Behavior influence weight
	public float seekWeight;
	public float pursueWeight;
	public float arriveWeight;
	public float arriveDistance;
	public float fleeWeight;
	public float evadeWeight;
	public float wanderWeight;
	public float wanderCircleDistance;
	public float wanderCircleRadius;
	public float avoidObstaclesWeight;
	
	//Flocking weights
	public float separationWeight;
	public float cohesionWeight;

	//For finding close targets
	public float radarDistance;

	public float shootCooldown;
}
