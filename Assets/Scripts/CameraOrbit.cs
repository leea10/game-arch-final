using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

	public Transform target;

	public float rotateMove = 15f;
	public float translMove = 45f;

	void Start(){
		Debug.Log("position of terrain = " + target.position);
	}

	public void translateHorizontal(bool left){
		float dir = -1;
		if (!left) dir *= -1;
		transform.Translate(translMove*dir,0,0);
	}

	public void translateVertical(bool up){
		float dir = 1;
		if (!up) dir *= -1;
		transform.Translate(0,0,translMove*dir);
	}

	public void MoveHorizontal(bool left){
		float dir = 1;
		if (!left) dir *= -1;
		transform.RotateAround(transform.position, Vector3.up, rotateMove * dir);
	}

	public void MoveVertical(bool up){
		float dir = 1;
		if (!up) dir *= -1;
		transform.RotateAround(transform.position, transform.TransformDirection(Vector3.right), rotateMove * dir);
	}
}
