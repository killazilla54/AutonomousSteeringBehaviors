using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DemoSceneManager : MonoBehaviour {

	public string sceneName;
	public float lifeTime = 6f;
	float timer;
	// Use this for initialization
	void Start () {
		timer = 0f;
		 Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer >= lifeTime){
			SceneManager.LoadScene(sceneName);
		}

        //for video only
        if (Input.GetKeyDown(KeyCode.Alpha5)){
            SceneManager.LoadScene("ArriveDemo");
        } else if (Input.GetKeyDown(KeyCode.Alpha6)){
            SceneManager.LoadScene("ArriveObstacleDemo");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4)){
            SceneManager.LoadScene("EvadeDemo");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)){
            SceneManager.LoadScene("FleeDemo");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha8)){
            SceneManager.LoadScene("ObstacleDemo");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            SceneManager.LoadScene("PursuitDemo");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1)){
            SceneManager.LoadScene("SeekDemo");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha7)){
            SceneManager.LoadScene("WanderDemo");
        }

    }




}
