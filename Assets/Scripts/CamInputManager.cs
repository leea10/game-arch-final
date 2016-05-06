using UnityEngine;
using System.Collections;

public class CamInputManager : MonoBehaviour {

	CameraOrbit cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<CameraOrbit>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.A)){
			cam.translateHorizontal(true);
		}
		else if (Input.GetKeyDown(KeyCode.D)){
			cam.translateHorizontal(false);
		}
		else if (Input.GetKeyDown(KeyCode.W)){
			cam.translateVertical(true);
		}
		else if (Input.GetKeyDown(KeyCode.S)){
			cam.translateVertical(false);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)){
			cam.RotateHorizontal(true);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)){
			cam.RotateHorizontal(false);
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow)){
			cam.RotateVertical(true);
		}	
		else if (Input.GetKeyDown(KeyCode.DownArrow)){
			cam.RotateVertical(false);
		}
	}
}
