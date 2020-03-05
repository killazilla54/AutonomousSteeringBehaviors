using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Decisions/TargetLost")]
public class TargetLost : Decision {

	public override bool Decide(StateController controller){
		return CheckForTarget(controller);
	}

	private bool CheckForTarget(StateController controller){
		//do rader check, if previous target not found, true
		if(controller.target != null && Vector2.Distance(controller.transform.position, controller.target.position) <= controller.vehicleProperties.radarDistance){
			return false;
		}
		return true;
	}

}
