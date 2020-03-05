using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Decisions/ArriveAtTarget")]
public class ArriveAtTarget : Decision {

	public override bool Decide(StateController controller){
		return HasArrived(controller);
	}

	private bool HasArrived(StateController controller){
		float distanceThreshold = 1f;
		if(distanceThreshold >= Vector2.Distance(controller.transform.position, controller.target.position)){
			return true;
		}
		return false;
	}

}
