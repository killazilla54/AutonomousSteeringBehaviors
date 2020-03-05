using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Decisions/Retreat")]

public class Retreat : Decision {

	public override bool Decide(StateController controller){
		return CheckRetreat(controller);
	}

	private bool CheckRetreat(StateController controller){
		//check health threshold
		//if below, true
		return false;
	}
}
