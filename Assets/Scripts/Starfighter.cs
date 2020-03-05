using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VehicleAI2D))]
public class Starfighter : MonoBehaviour {

	public float wanderSpeed;
	public float chaseSpeed;
	public float fleeSpeed;

	public Transform commandTarget; //Some outside force determines this like carrier ship
	public Transform localTarget; //Target set by the starfighter itself from radar pings

	public float defendLeashDist = 20f;
    public float radarDistance = 30f;

	private VehicleAI2D vehicleAI;
	public AIState state;

	public enum AIState {Defend, Attack, Hunt, Heal, Strafe};
	public enum Command {Attack,Defend,Hunt};

	public GameObject laserPrefab;
	public Transform laserSpawn;
	float shootCooldown = 1.5f;
	public float shootTimer;

	// Use this for initialization
	void Awake () {
		vehicleAI = GetComponent<VehicleAI2D>();
	}
	
	// Update is called once per frame
	void Update () {
		RadarPing();
        CheckShoot();
		switch(state){
			case AIState.Attack:
				Attack(commandTarget);		
				break;
			case AIState.Defend:
				Defend(commandTarget);		
				break;
			case AIState.Hunt:
				Hunt();		
				break;
		}
	}

	public void Attack(Transform target){
		vehicleAI.currentState = VehicleAI2D.AIState.Seek;
		if(localTarget == null){
			vehicleAI.target = commandTarget;
		} else {
			vehicleAI.target = localTarget;
		}
	}

	public void Chase(Transform target){
		//Might just be pursuit, with arrival to stay behind
		//Attack shooting would be automatic
		//For moving targets
	}

	// If wander too far from target, Seek back to thing defending until within a curtain range / time frame
	// Seek targets that enter sensor range, with same distance constraints as wander
	public void Defend(Transform target){
		
		if(Vector2.Distance(transform.position,commandTarget.position) > defendLeashDist){ // turn around and return
			vehicleAI.currentState = VehicleAI2D.AIState.Seek;
			vehicleAI.target = commandTarget;
            vehicleAI.maxSpeed = wanderSpeed;
        } else if(localTarget != null){ //Attack
			vehicleAI.currentState = VehicleAI2D.AIState.Seek;
            vehicleAI.maxSpeed = chaseSpeed;
            vehicleAI.target = localTarget;
		} else { //Wander
            vehicleAI.maxSpeed = wanderSpeed;
            vehicleAI.currentState = VehicleAI2D.AIState.Wander;
		}
	}

	// Maybe Strafe is a mini behavior where target velocity below a threshold (basically stationary)
	// Where chasing would mean sitting still which is not what we want here.
	public void Strafe(Transform target){
		//Seek/pursuit until within a certain range of target, then pass by
		// #1 Could trigger flee until far enough away
		// #2 Could calculate a target vector x distance beyond the target
		//Once fighter reaches this distance or extended target vector, seek target again.
		//Getting within range of the target will trigger firing behavior automatically
	}

	public void Hunt(){
		//Wander()?
		
	}

	public void Heal(Transform target){
		// if(shields > 0)
		// go back to old behavior
		// else
		// Evade()
	}

	public void ReceiveCommand(Command command, Transform target){
		commandTarget = target;
		switch(command){
			case Command.Defend:
				state = AIState.Defend;
				vehicleAI.maxSpeed = chaseSpeed;
				break;
			case Command.Hunt:
				state = AIState.Hunt;
                vehicleAI.currentState = VehicleAI2D.AIState.Wander; //prob want to make this a setter method in vehiceAI
                vehicleAI.maxSpeed = wanderSpeed;
				break;
            case Command.Attack:
                state = AIState.Attack;
                vehicleAI.maxSpeed = chaseSpeed;
                break;
		}

	}

	//Some sort of line of sight or radar ping
	//If
	void RadarPing(){
        //If target != null, do a simple distance check
        //If it exceeds distance, local target = null, then do circleCast for new target
		if(localTarget != null && Vector2.Distance(transform.position, localTarget.position) <= radarDistance){
			return;
		}
		//Need to take into account how I manage Stationary Target logic so that it is not lost when fighter strafes.
		


		localTarget = null;
		Vector2 origin = new Vector2(transform.position.x,transform.position.y);
		RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, 20f,Vector2.zero);
		for(int i = 0; i < hits.Length; i++){
			if(hits[i] == true && hits[i].transform.tag == "Player"){
				localTarget = hits[i].transform;
			}
		}
	}
	//When starfighter engages a target like a cruiser, it should pick a hardpoint to attack and make that its target.
	//Straffing run would be Seek/Pursuit, fire when in range, have seek overruled by obstacle avoidance, flee, turn around and strafe again

	void CheckShoot(){
		shootTimer -= Time.deltaTime;
		if(localTarget != null && Vector2.Distance(transform.position, localTarget.position) < 10f){
			if(shootTimer <= 0){
				Instantiate(laserPrefab, laserSpawn.position, laserSpawn.rotation);
				shootTimer = shootCooldown;
			}
		}
	}
}
