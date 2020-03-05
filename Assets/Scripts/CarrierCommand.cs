using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierCommand : MonoBehaviour {

	public GameObject starfighterPrefab;
	public Transform spawnPoint;
	public List <Starfighter> fighters;

	//TEMP
	public Transform tempTargetVar;

	int maxFighters = 10;
	public float cooldown;
	bool launched;

	void Start () {
		StartCoroutine(SpawnFighters());
        RecallFighters();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.A)){
			DeployFightersToTarget(tempTargetVar);
		}

		if(launched){
			cooldown -= Time.deltaTime;
		}

		if(fighters.Count == maxFighters && !launched ){
			launched = true;
			DeployFightersToTarget(tempTargetVar);
		}
	}

	private void LaunchFighter(){
		GameObject fighter = Instantiate(starfighterPrefab, spawnPoint.position, spawnPoint.rotation);
		fighter.GetComponent<Starfighter>().ReceiveCommand(Starfighter.Command.Defend, transform);
		fighters.Add(fighter.GetComponent<Starfighter>());
	}

	private void DeployFightersToTarget(Transform target){
		Debug.Log("Deploying fighters to Target");
		foreach(Starfighter fighter in fighters){
			fighter.ReceiveCommand(Starfighter.Command.Attack, target);
		}
	}

	private void RecallFighters(){
		foreach(Starfighter fighter in fighters){
			fighter.ReceiveCommand(Starfighter.Command.Defend, transform);
		}
	}

	void OnDrawGizmosSelected(){
		#if UNITY_EDITOR
        float r = 20f;
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position ,new Vector3(0,0,1), r);
		#endif
             
	}

	IEnumerator SpawnFighters(){
		while(fighters.Count < maxFighters){
			LaunchFighter();
			yield return new WaitForSeconds(3);
		}
	}
}
