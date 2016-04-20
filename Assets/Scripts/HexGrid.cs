using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexGrid : MonoBehaviour {
	// Colors of the tiles
	public Color[] colorChoices;

	// Prefabs
	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	// Private member variables
	HexTerrainData hexTerrain;
	HexCell[] cells;

	/*
	Mesh hexMesh;
	List<Vector3> vertices;
	List<int> triangles;
	List<Color> colors;
	*/

	void Awake() {
		hexTerrain = GetComponent<HexTerrainData>();
		cells = new HexCell[hexTerrain.height * hexTerrain.width];	

		/*
		GetComponent<MeshFilter> ().mesh = hexMesh = new Mesh ();
		hexMesh.name = "Hex Mesh";
		vertices = new List<Vector3> ();
		triangles = new List<int> ();
		colors = new List<Color> ();
		*/

		for (int z = 0, i = 0; z < hexTerrain.height; z++) {
			for (int x = 0; x < hexTerrain.width; x++) {
				CreateCell (x, z, i++);
			}
		}	
	}

	void Start() {
		Triangulate ();
	}

	void CreateCell(int x, int z, int i) {
		Vector3 position;

		// x coordinate has an offset on odd rows and no offset on even rows
		position.x = x * 2f * HexCell.innerRadius + (z % 2)*HexCell.innerRadius;
		position.y = Random.Range(0, hexTerrain.maxElevation+1);
		position.z = z * 1.5f * HexCell.outerRadius;

		HexCell cell = cells [i] = Instantiate<HexCell> (cellPrefab);
		cell.transform.SetParent (this.transform, false);
		cell.transform.localPosition = position;
		cell.color = colorChoices[Random.Range(0, colorChoices.Length)];
	}

	void Triangulate() {
		for (int i = 0; i < cells.Length; i++) {
			cells [i].Triangulate ();
		}
	}
}
