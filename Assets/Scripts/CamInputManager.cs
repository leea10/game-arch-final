using UnityEngine;
using System.Collections;

public class CamInputManager : MonoBehaviour {

	CameraOrbit cam;
	enum Mode{Translate, Rotate, Zoom};
	Mode currentMode;

	// Use this for initialization
	void Start () {
		cam = GetComponent<CameraOrbit>();
		currentMode = Mode.Translate;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R))
			currentMode = Mode.Rotate;
		else if (Input.GetKeyDown(KeyCode.T))
			currentMode = Mode.Translate;


		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			if (currentMode == Mode.Translate) 
				cam.translateHorizontal(true);
			else if (currentMode == Mode.Rotate) 
				cam.RotateHorizontal(true);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)){
			if (currentMode == Mode.Translate) 
				cam.translateHorizontal(false);
			else if (currentMode == Mode.Rotate) 
				cam.RotateHorizontal(false);
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow)){
			if (currentMode == Mode.Translate) 
				cam.translateVertical(true);
			else if (currentMode == Mode.Rotate) 
				cam.RotateVertical(true);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)){
			if (currentMode == Mode.Translate) 
				cam.translateVertical(false);
			else if (currentMode == Mode.Rotate) 
				cam.RotateVertical(false);
		}
	}
}
