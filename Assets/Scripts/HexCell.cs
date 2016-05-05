using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour {

	public enum Edge {topRight, right, bottomRight, bottomLeft, left, topLeft}

	// The hexagon's outer radius, also the length of one of its sides
	public static float outerRadius = 15f;

	// The hexagon's inner radius is the outerRadius * cos(30 degrees)
	public static float innerRadius = outerRadius * 0.866025404f;

	// Coordinates of the hexagon's corners local to its center
	public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
	};
		
	Mesh hexMesh;
	List<Vector3> vertices;
	List<int> triangles;
	List<Color> colors;
	public HashSet<Edge> exposedEdges;

	public HexGrid.TerrainType type;
	public Color color;
	public Color sideColor;
	public bool hasTree;
	Color waterColor;

	void Awake() {
		sideColor = new Color(0.62f,0.46f,0.11f);
		GetComponent<MeshFilter> ().mesh = hexMesh = new Mesh ();
		vertices = new List<Vector3> ();
		triangles = new List<int> ();
		colors = new List<Color> ();
		exposedEdges = new HashSet<Edge> ();
		waterColor = new Color (0.23f, 0.64f, 0.78f, 0.4f);
	}

	public void generateMesh(int waterLevel) {
		hexMesh.Clear ();
		vertices.Clear ();
		triangles.Clear ();
		colors.Clear ();

		// Add the top hexagon plane
		for (int i = 0; i < 6; i++) {
			vertices.Add (corners [i]); // Position
			colors.Add(color); // Color
		}

		for (int i = 1; i <= 4; i++) {
			triangles.Add (0);
			triangles.Add (i);
			triangles.Add (i + 1);
		}

		float elevation = getElevation ();

		int vertexIndex = vertices.Count;
		// Add the side planes for the column
		for (int i = 0; i < 6; i++) {
			vertexIndex = vertices.Count;
			vertices.Add (vertices [i]);
			vertices.Add (new Vector3 (
				vertices [i].x,
				-elevation,
				vertices [i].z));
			vertices.Add (vertices [i + 1]);
			vertices.Add (new Vector3 (
				vertices [i+1].x,
				-elevation,
				vertices [i+1].z));

			for (int j = 0; j < 4; j++) {
				colors.Add (sideColor);
			}

			triangles.Add (vertexIndex);
			triangles.Add (vertexIndex + 1);
			triangles.Add (vertexIndex + 2);

			triangles.Add (vertexIndex + 2);
			triangles.Add (vertexIndex + 1);
			triangles.Add (vertexIndex + 3);
		}

		// Add the top plane of water, if applicable
		if (elevation <= waterLevel) {
			generateWaterMesh (waterLevel);
		}

		hexMesh.vertices = vertices.ToArray ();
		hexMesh.triangles = triangles.ToArray ();
		hexMesh.colors = colors.ToArray ();
		hexMesh.RecalculateNormals();
	}

	public void generateWaterMesh(int waterLevel) {
		float elevation = getElevation ();
		int vertexIndex = vertices.Count;

		// Add the top hexagon plane of the water
		for (int i = 0; i < 6; i++) {
			vertices.Add (new Vector3( 
				corners[i].x,
				waterLevel + 0.01f - elevation,
				corners[i].z
			)); // Position
			colors.Add(waterColor); // Color
		}

		for (int i = 1; i <= 4; i++) {
			triangles.Add (vertexIndex);
			triangles.Add (vertexIndex + i);
			triangles.Add (vertexIndex + i + 1);
		}

		List<Edge> e = exposedEdges.ToList ();
		int newVertexIndex = vertices.Count;
		// Edge and corner cases, where we have to add the side planes of the water
		for (int i = 0; i < e.Count; i++) {
			int j = (int)e [i];
			newVertexIndex = vertices.Count;

			vertices.Add(vertices[vertexIndex + j]);
			vertices.Add (vertices [j]);
			vertices.Add (vertices [(j + 1)%6]);
			vertices.Add (vertices [vertexIndex + (j + 1)%6]);

			triangles.Add (newVertexIndex);
			triangles.Add (newVertexIndex + 1);
			triangles.Add (newVertexIndex + 2);

			triangles.Add (newVertexIndex);
			triangles.Add (newVertexIndex + 2);
			triangles.Add (newVertexIndex + 3);

			for (int k = 0; k < 4; k++) {
				colors.Add (waterColor);
			}
		}
	}

	public float getElevation() {
		return transform.localPosition.y;
	}
}
