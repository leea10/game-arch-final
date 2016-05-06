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

		if (Input.GetKey(KeyCode.A)){
			cam.translateHorizontal(true);
		}
		else if (Input.GetKey(KeyCode.D)){
			cam.translateHorizontal(false);
		}
		else if (Input.GetKey(KeyCode.W)){
			cam.translateVertical(true);
		}
		else if (Input.GetKey(KeyCode.S)){
			cam.translateVertical(false);
		}
		else if (Input.GetKey(KeyCode.LeftArrow)){
			cam.RotateHorizontal(true);
		}
		else if (Input.GetKey(KeyCode.RightArrow)){
			cam.RotateHorizontal(false);
		}
		else if (Input.GetKey(KeyCode.UpArrow)){
			cam.RotateVertical(true);
		}	
		else if (Input.GetKey(KeyCode.DownArrow)){
			cam.RotateVertical(false);
		}
		else if(Input.GetKey(KeyCode.Equals)){
			Debug.Log("plus is clicked");
			cam.ZoomIn(true);
		}
		else if(Input.GetKey(KeyCode.Minus)){
			cam.ZoomIn(false);
		}
	}
}
