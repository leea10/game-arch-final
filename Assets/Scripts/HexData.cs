using UnityEngine;
using System.Collections;

public static class HexData {
	// The hexagon's outer radius, also the length of one of its sides
	public const float outerRadius = 5f;

	// The hexagon's inner radius is the outerRadius * cos(30 degrees)
	public const float innerRadius = outerRadius * 0.866025404f;

	// Coordinates of the hexagon's corners local to its center
	public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
	};
}

