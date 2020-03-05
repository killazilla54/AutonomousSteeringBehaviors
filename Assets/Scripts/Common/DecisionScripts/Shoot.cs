using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Decision {

	public override bool Decide(StateController controller){
		return CheckShootTimer(controller);
	}

	private bool CheckShootTimer(StateController controller){
		controller.shootTimer += Time.deltaTime;
		if(controller.shootTimer > controller.vehicleProperties.shootCooldown){
			controller.shootTimer = 0;
			return true;
		}
		return false;
	}

}
