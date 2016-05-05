using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

	public Transform target;

	public float rotateIncr = 15f;
	public float translIncr = 45f;

	public void translateHorizontal(bool left){
		float dir = -1;
		if (!left) dir *= -1;
		transform.Translate(translIncr*dir,0,0);
	}

	public void translateVertical(bool up){
		float dir = 1;
		if (!up) dir *= -1;
		transform.Translate(0,0,translIncr*dir);

	}

	public void RotateHorizontal(bool left){
		float dir = 1;
		if (!left) dir *= -1;
		transform.Rotate(Vector3.up, rotateIncr * dir);
	}

	public void RotateVertical(bool up){
		float dir = 1;
		if (!up) dir *= -1;
		transform.Rotate(Vector3.right, rotateIncr * dir);
	}
}
