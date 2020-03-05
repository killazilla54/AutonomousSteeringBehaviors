using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Actions/Seek")]
public class SeekAction : Action {

	public override void Act(StateController controller){
		Seek(controller);
	}

	private void Seek(StateController controller){
		Vector2 controllerPos = controller.transform.position;
		Vector2 targetPos = controller.target.position;

		Vector2 desired = targetPos - controllerPos;
		desired.Normalize();
		desired *= controller.vehicleProperties.maxSpeed;
		Vector2 steeringVelocity = desired - controller.velocity ; 
		Debug.DrawRay(controller.transform.position, controller.velocity.normalized * 2, Color.green);
        Debug.DrawRay(controller.transform.position, desired.normalized * 2, Color.magenta);

		steeringVelocity *= controller.vehicleProperties.seekWeight;
		controller.ApplyForce(steeringVelocity);
	}
}
