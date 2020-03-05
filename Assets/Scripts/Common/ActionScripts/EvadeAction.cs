using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Actions/Evade")]

public class EvadeAction : Action {

	public override void Act(StateController controller){
		Evade(controller);
	}

	private void Evade(StateController controller){
		int T = 3;
		//Calc positional vectors
		Vector2 controllerPos = controller.transform.position;
		Vector2 targetPos = controller.target.position;
		Vector2 targetVelocity = (targetPos - controller.previousTargetPosition) /Time.deltaTime;
		Vector2 futurePosition = targetPos + targetVelocity * T;

		//Flee
		Vector2 desired = controllerPos - targetPos;
		desired.Normalize();
		desired *= controller.vehicleProperties.maxSpeed;
		Vector2 steeringVelocity = desired - controller.velocity ; 
		Vector2.ClampMagnitude(steeringVelocity, controller.vehicleProperties.maxForce);

		steeringVelocity *= controller.vehicleProperties.evadeWeight;
		controller.ApplyForce(steeringVelocity);
	}
}
