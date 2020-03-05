using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Actions/Arrive")]
public class ArriveAction : Action {

	public override void Act(StateController controller){
		Arrive(controller);
	}

	private void Arrive(StateController controller){
		Vector2 controllerPos = controller.transform.position;
		Vector2 targetPos = controller.target.position;

		Vector2 desired = targetPos - controllerPos;
		float distance = desired.magnitude;

		float rampedSpeed = controller.vehicleProperties.maxSpeed * (distance / controller.vehicleProperties.arriveDistance);
		float clippedSpeed = Mathf.Min(rampedSpeed, controller.vehicleProperties.maxSpeed);
		Vector2 desiredVelocity = (clippedSpeed / distance) * desired;
		
		Vector2 steeringVelocity = desiredVelocity - controller.velocity;

		Vector2.ClampMagnitude(steeringVelocity, controller.vehicleProperties.maxForce);		
		steeringVelocity *= controller.vehicleProperties.arriveWeight;
		controller.ApplyForce(steeringVelocity);

	}
}
