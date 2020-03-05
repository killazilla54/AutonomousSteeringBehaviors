using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Decisions/Strafe")]
public class Strafe : Decision {

	public override bool Decide(StateController controller){
		return CheckForStrafe(controller);
	}

	public bool CheckForStrafe(StateController controller){
		return false;
	}
}
