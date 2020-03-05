using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Actions/Wander")]
public class WanderAction : Action {

	public override void Act(StateController controller){
		Wander(controller);
	}

	private void Wander(StateController controller){

		Vector2 controllerPos = controller.transform.position;

		Vector2 circlePos = controllerPos + controller.velocity.normalized * controller.vehicleProperties.wanderCircleDistance; 
		Vector2 circleTargetSpot = Random.insideUnitCircle * controller.vehicleProperties.wanderCircleRadius;
		Vector2 targetVector = circlePos + circleTargetSpot;
		Debug.DrawLine(circlePos, targetVector, Color.cyan);

		//Seek
		Vector2 desired = targetVector - controllerPos;
		desired.Normalize();
		desired *= controller.vehicleProperties.maxSpeed;
		Vector2 steeringVelocity = desired - controller.velocity ; 
		Debug.DrawRay(controller.transform.position, controller.velocity.normalized * 2, Color.green);
        Debug.DrawRay(controller.transform.position, desired.normalized * 2, Color.magenta);

		steeringVelocity *= controller.vehicleProperties.wanderWeight;
		controller.ApplyForce(steeringVelocity);	
	}
}
