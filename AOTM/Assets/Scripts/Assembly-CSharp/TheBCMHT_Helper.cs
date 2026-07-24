using UnityEngine;

public static class TheBCMHT_Helper
{
	public static int getMaxValueFromTileType(TileType tileType)
	{
		int result = 1000;
		if (tileType == TileType.Corner || tileType == TileType.End)
		{
			result = 1;
		}
		return result;
	}

	public static Vector3 getVector3FromDir(int direction)
	{
		return (new Vector3[4]
		{
			Vector3.forward,
			Vector3.right,
			Vector3.back,
			Vector3.left
		})[direction];
	}

	public static Material getSampleFloor(bool v132)
	{
		return Resources.Load<Material>("Samples/" + (v132 ? "V1.3.2/Ph_Floor" : "Ph_Floor"));
	}

	public static Material getSampleWall(bool v132)
	{
		return Resources.Load<Material>("Samples/" + (v132 ? "V1.3.2/Ph_Wall" : "Ph_Wall"));
	}

	public static Material getSampleCeiling(bool v132)
	{
		return Resources.Load<Material>("Samples/" + (v132 ? "V1.3.2/Ph_Ceiling" : "Ph_Ceiling"));
	}

	public static int intClampBySize(int value, int size)
	{
		return Mathf.Clamp(value, 0, size - 1);
	}

	public static GameObject getCornMazeFlag()
	{
		return Resources.Load<GameObject>("Environment/Flag");
	}

	public static Sprite getCornMazeFlagSampleSprite()
	{
		return Resources.Load<Sprite>("Samples/Placeholder_Flag");
	}

	public static GameObject getCornMazeSign()
	{
		return Resources.Load<GameObject>("Environment/Sign");
	}

	public static Sprite getCornMazeSignSampleSprite()
	{
		return Resources.Load<Sprite>("Samples/Placeholder_Sign");
	}

	public static RoomChildDat[,] createRoomData(int sizeX, int sizeY)
	{
		if (sizeX < 2 || sizeY < 2)
		{
			Debug.LogWarning("failed to create a room data size (" + sizeX + ", " + sizeY + ")");
			return null;
		}
		RoomChildDat[,] array = new RoomChildDat[sizeX, sizeY];
		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				array[i, j] = new RoomChildDat();
				if (i == 0 && j == 0)
				{
					array[i, j].tileType = TileType.Corner;
					array[i, j].tileIndex = 0;
				}
				else if (i == 0 && j == sizeY - 1)
				{
					array[i, j].tileType = TileType.Corner;
					array[i, j].tileIndex = 1;
				}
				else if (i == sizeX - 1 && j == 0)
				{
					array[i, j].tileType = TileType.Corner;
					array[i, j].tileIndex = 3;
				}
				else if (i == sizeX - 1 && j == sizeY - 1)
				{
					array[i, j].tileType = TileType.Corner;
					array[i, j].tileIndex = 2;
				}
				else if (i == 0 && j > 0 && j <= sizeY - 2)
				{
					array[i, j].tileType = TileType.Single;
					array[i, j].tileIndex = 3;
				}
				else if (i > 0 && i <= sizeX - 2 && j == sizeY - 1)
				{
					array[i, j].tileType = TileType.Single;
					array[i, j].tileIndex = 0;
				}
				else if (i > 0 && i <= sizeX - 2 && j == 0)
				{
					array[i, j].tileType = TileType.Single;
					array[i, j].tileIndex = 2;
				}
				else if (i == sizeX - 1 && j > 0 && j <= sizeY - 2)
				{
					array[i, j].tileType = TileType.Single;
					array[i, j].tileIndex = 1;
				}
				else if (i > 0 && j > 0 && i <= sizeX - 2 && j <= sizeY - 2)
				{
					array[i, j].tileType = TileType.Open;
				}
			}
		}
		return array;
	}
}
