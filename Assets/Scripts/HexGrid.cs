using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexGrid : MonoBehaviour {
	public enum TerrainType {water, sand, plain}

	// Colors of the tiles
	public Color grassColor = new Color(1f, 1f, 1f);
	public Color sandColor = new Color (1f, 1f, 1f);
	public Color dirtColor = new Color (1f, 1f, 1f);

	// Prefabs
	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	// Private member variables
	//HexTerrainData hexTerrain;
	MapMaker hexTerrain;
	public HexCell[] cells;

	void Awake() {
		hexTerrain = GetComponent<MapMaker>();
	}

	void Start() {
		CreateCells ();
	}

	void CreateCells() {
		cells = new HexCell[hexTerrain.height * hexTerrain.width];	
		for (int z = 0, i = 0; z < hexTerrain.height; z++) {
			for (int x = 0; x < hexTerrain.width; x++) {
				CreateCell (x, z, i++);
			}
		}
		generateMesh ();
	}

	void CreateCell(int x, int z, int i) {
		Vector3 position;

		// x coordinate has an offset on odd rows and no offset on even rows
		position.x = x * 2f * HexCell.innerRadius + (z % 2)*HexCell.innerRadius;
		position.y = Mathf.Floor(hexTerrain.perlinNoise[x,z] * hexTerrain.maxElevation);
		position.z = z * 1.5f * HexCell.outerRadius;

		HexCell cell = cells [i] = Instantiate<HexCell> (cellPrefab);
		cell.transform.SetParent (this.transform, false);
		cell.transform.localPosition = position;

		switch (hexTerrain.typeMap [x,z]) {
		case 0:
			cell.color = dirtColor;
			cell.type = TerrainType.water;
			break;
		case 1: 
			cell.color = sandColor;		
			cell.type = TerrainType.sand;
			break;
		case 2: 
			cell.color = grassColor; 	
			cell.type = TerrainType.plain;
			break;
		}
	}

	void generateMesh() {
		for (int i = 0; i < cells.Length; i++) {
			cells [i].generateMesh ((int)(hexTerrain.oceanLvl / 10f * hexTerrain.maxElevation));
		}
	}
}
