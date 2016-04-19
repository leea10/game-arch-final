using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HexGrid : MonoBehaviour {

	// Grid dimensions
	public int width = 6;
	public int height = 6;

	// Prefabs
	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	// Private member variables
	Canvas gridCanvas;
	HexCell[] cells;
	HexMesh hexMesh;

	void Awake() {
		gridCanvas = GetComponentInChildren<Canvas>();
		cells = new HexCell[height * width];		
		hexMesh = GetComponentInChildren<HexMesh> ();

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell (x, z, i++);
			}
		}	
	}

	void Start() {
		hexMesh.Triangulate (cells);
	}

	void CreateCell(int x, int z, int i) {
		Vector3 position;

		// x coordinate has an offset on odd rows and no offset on even rows
		position.x = x * 2f * HexData.innerRadius + (z % 2)*HexData.innerRadius;
		position.y = 0f;
		position.z = z * 1.5f * HexData.outerRadius;

		HexCell cell = cells [i] = Instantiate<HexCell> (cellPrefab);
		cell.transform.SetParent (this.transform, false);
		cell.transform.localPosition = position;

		Text label = Instantiate<Text> (cellLabelPrefab);
		label.rectTransform.SetParent (gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2 (position.x, position.z);
		label.text = x.ToString() + "\n" + z.ToString();
	}
}
