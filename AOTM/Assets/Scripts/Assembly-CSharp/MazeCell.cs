using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
	private List<int> availableDirs = new List<int>();

	private List<MeshRenderer> walls = new List<MeshRenderer>();

	private MeshRenderer floor;

	private MeshRenderer ceiling;

	public bool initialized;

	public Vector3 gridPosition;

	public Transform parent;

	public bool visited;

	public void InitializeCell()
	{
		if (initialized)
		{
			return;
		}
		if (Resources.Load("Tiles/Full") == null)
		{
			Debug.LogWarning("Initialization failed, Tiles/Full doesn't exist");
			return;
		}
		availableDirs.Add(0);
		availableDirs.Add(1);
		availableDirs.Add(2);
		availableDirs.Add(3);
		MeshRenderer[] array = new MeshRenderer[4];
		MeshRenderer[] componentsInChildren = Object.Instantiate(Resources.Load<GameObject>("Tiles/Full"), gridPosition, Quaternion.identity, parent).GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			if (meshRenderer.gameObject.name == "Wall")
			{
				array[0] = meshRenderer;
			}
			else if (meshRenderer.gameObject.name == "Wall (1)")
			{
				array[1] = meshRenderer;
			}
			else if (meshRenderer.gameObject.name == "Wall (2)")
			{
				array[2] = meshRenderer;
			}
			else if (meshRenderer.gameObject.name == "Wall (3)")
			{
				array[3] = meshRenderer;
			}
			else if (meshRenderer.gameObject.name.Contains("Floor"))
			{
				floor = meshRenderer;
			}
			else if (meshRenderer.gameObject.name.Contains("Ceiling"))
			{
				ceiling = meshRenderer;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			walls.Add(array[j]);
		}
	}

	public void RemoveCeiling()
	{
		if (ceiling != null)
		{
			Object.DestroyImmediate(ceiling.gameObject);
		}
	}

	public void ApplyMaterials(Material ceiling, Material wall, Material floor)
	{
		foreach (MeshRenderer wall2 in walls)
		{
			wall2.sharedMaterial = wall;
		}
		this.floor.sharedMaterial = floor;
		if (this.ceiling != null)
		{
			this.ceiling.sharedMaterial = ceiling;
		}
	}

	public void DestroyWall(int direction)
	{
		if (availableDirs.Contains(direction))
		{
			MeshRenderer meshRenderer = walls[GetIntegerFromDir(direction)];
			walls.Remove(meshRenderer);
			Object.DestroyImmediate(meshRenderer.gameObject);
			availableDirs.Remove(direction);
		}
	}

	private int GetIntegerFromDir(int direction)
	{
		for (int i = 0; i < availableDirs.Count; i++)
		{
			if (availableDirs[i] == direction)
			{
				return i;
			}
		}
		return 0;
	}
}
