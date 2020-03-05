using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Actions/Attack")]
public class AttackAction : Action {
	float cooldownTimer = 0f;

	public override void Act(StateController controller){
		Attack(controller);
	}

	private void Attack(StateController controller){
		cooldownTimer += Time.deltaTime;
		if(cooldownTimer >= controller.vehicleProperties.shootCooldown){
			controller.FireShot();
			cooldownTimer = 0f;
		}
	}

}
