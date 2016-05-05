using UnityEngine;
using System.Collections;

public class CamInputManager : MonoBehaviour {

	CameraOrbit cam;
	enum Mode{Translate, Rotate, Zoom};

	// Use this for initialization
	void Start () {
		cam = GetComponent<CameraOrbit>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			cam.translateHorizontal(true);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)){
			cam.translateHorizontal(false);
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow)){
			cam.translateVertical(true);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)){
			cam.translateVertical(false);
		}
	}
}
