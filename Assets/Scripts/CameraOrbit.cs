using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

	Transform target;

	float rotateIncr = 1f;
	float translIncr = 10f;
	float zoomIncr = 5f;

	public void grabTargetCell(){
		//target = GameObject.Find("Terrain").GetComponent<HexGrid>().getCenterCell();
	}

	public void translateHorizontal(bool left){
		float dir = -1;
		if (!left) dir *= -1;
		transform.parent.Translate(translIncr*dir,0,0);
	}

	public void translateVertical(bool up){
		float dir = 1;
		if (!up) dir *= -1;
		transform.parent.Translate(0,0,translIncr*dir);
	}

    public void ZoomIn(bool zoomin){
    	float dir = 1;
		if (!zoomin) dir *= -1;
		transform.Translate(0,0,zoomIncr*dir);
    }

	public void RotateHorizontal(bool left){
		float dir = -1;
		if (!left) dir *= -1;
		if (target!=null) transform.RotateAround(target.position, Vector3.up, rotateIncr*dir);
	}

	public void RotateVertical(bool up){
		float dir = -1;
		if (!up) dir *= -1;
		transform.Rotate(Vector3.right, rotateIncr * dir);
	}




}
