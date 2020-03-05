using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SteeringBehaviors/Decisions/DetectEnemy")]
public class DetectEnemy : Decision {

	public override bool Decide(StateController controller){
		return CheckForTarget(controller);
	}

	private bool CheckForTarget(StateController controller){
		
		//controller.target = null;
		Vector2 origin = new Vector2(controller.transform.position.x,controller.transform.position.y);
		RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, controller.vehicleProperties.radarDistance,Vector2.zero);
		for(int i = 0; i < hits.Length; i++){
			if(hits[i] == true && hits[i].transform.tag == "Player"){
				controller.target = hits[i].transform;
				return true;
			}
		}

		return false;
	}

}
