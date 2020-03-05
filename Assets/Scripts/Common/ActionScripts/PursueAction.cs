using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Actions/Pursue")]
public class PursueAction : Action {

	public override void Act(StateController controller){
		Pursue(controller);
	}

	private void Pursue(StateController controller){
		int T = 3; //how far in future to predict

		Vector2 controllerPos = controller.transform.position;
		Vector2 targetPos = controller.target.position;

		Vector2 targetVelocity = (targetPos - controller.previousTargetPosition) / Time.deltaTime;
		Vector2 futurePosition = targetPos + targetVelocity * T;

		//Seek
		Vector2 desired = futurePosition - controllerPos;
		desired.Normalize();
		desired *= controller.vehicleProperties.maxSpeed;
		Vector2 steeringVelocity = desired - controller.velocity ; 
		Debug.DrawRay(controller.transform.position, controller.velocity.normalized * 2, Color.green);
        Debug.DrawRay(controller.transform.position, desired.normalized * 2, Color.magenta);

		//Adjust for weight and apply
		steeringVelocity *= controller.vehicleProperties.pursueWeight;
		controller.ApplyForce(steeringVelocity);
	}
}
