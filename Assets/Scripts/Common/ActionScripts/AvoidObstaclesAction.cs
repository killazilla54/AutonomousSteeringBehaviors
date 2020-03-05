using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Actions/AvoidObstacle")]
public class AvoidObstaclesAction : Action {

	public override void Act(StateController controller){
		AvoidObstacles(controller);
	}

	private void AvoidObstacles(StateController controller){
		Vector2 controllerPos = controller.transform.position;


		Debug.DrawRay(controllerPos, controller.transform.up*5f);
		RaycastHit2D hit = Physics2D.Raycast(controllerPos, controller.transform.up, 5f,LayerMask.GetMask("Obstacles"));
		if(hit){
			Vector2 targetPos = hit.transform.position;
			Vector2 steeringVelocity = hit.normal * controller.vehicleProperties.maxForce;
			Debug.DrawLine(controllerPos, steeringVelocity, Color.yellow);
			// acceleration = Vector2.zero;  Might need to add something to controller

			//Adjust for weight and apply
			steeringVelocity *= controller.vehicleProperties.avoidObstaclesWeight;
			controller.ApplyForce(steeringVelocity);
		}
	}
}
