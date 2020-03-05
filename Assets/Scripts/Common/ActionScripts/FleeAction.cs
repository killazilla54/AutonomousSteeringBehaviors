using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Actions/Flee")]
public class FleeAction : Action {

	public override void Act(StateController controller){
		Flee(controller);
	}

	private void Flee(StateController controller){
		Vector2 controllerPos = controller.transform.position;
		Vector2 targetPos = controller.target.position;

		Vector2 desired = controllerPos - targetPos;
		desired.Normalize();
		desired *= controller.vehicleProperties.maxSpeed;
		
		Vector2 steeringVelocity = desired - controller.velocity ; 
		Vector2.ClampMagnitude(steeringVelocity, controller.vehicleProperties.maxForce);

		steeringVelocity *= controller.vehicleProperties.fleeWeight;
		controller.ApplyForce(steeringVelocity);
	}
}
