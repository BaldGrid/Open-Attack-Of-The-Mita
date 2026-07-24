using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
	public List<UnknownPriProjectile> projectileScripts;

	public UnknownPriProjectile[] randomProjectile;

	public List<UnknownPriProjectile> initProjectiles;

	public List<UnknownPriProjectile> preProjectiles;

	public Transform[] projectileSpawnPoints;

	public UnknownPri_NPC unknown;

	public UnknownPriProjectile currentProjectile;

	public void Start()
	{
		RandomizePreProjectiles();
	}

	public void DestroyAllProjectiles()
	{
		foreach (UnknownPriProjectile projectileScript in projectileScripts)
		{
			projectileScript.GetComponent<UnknownPriProjectile>().gameObject.SetActive(value: false);
		}
	}

	public void SpawnPreProjectiles()
	{
		for (int i = 0; i < preProjectiles.Count; i++)
		{
			UnknownPriProjectile unknownPriProjectile = Object.Instantiate(preProjectiles[i]);
			projectileScripts.Add(unknownPriProjectile);
			unknownPriProjectile.targetTransform = Camera.main.transform;
			unknownPriProjectile.projectileSpawner = this;
			unknownPriProjectile.unknown = unknown;
			unknownPriProjectile.TeleportProjectile();
		}
	}

	public void DestroyProjectile()
	{
		int index = projectileScripts.Count - 1;
		projectileScripts[index].SelfDestruct();
	}

	public void SpawnMultipleProjectiles(int count)
	{
		for (int i = 0; i < count; i++)
		{
			UnknownPriProjectile unknownPriProjectile = Object.Instantiate(randomProjectile[Random.Range(0, randomProjectile.Length)]);
			projectileScripts.Add(unknownPriProjectile);
			unknownPriProjectile.targetTransform = Camera.main.transform;
			unknownPriProjectile.projectileSpawner = this;
			unknownPriProjectile.unknown = unknown;
			unknownPriProjectile.TeleportProjectile();
		}
	}

	public void RandomizePreProjectiles()
	{
		for (int i = 0; i < preProjectiles.Count; i++)
		{
			preProjectiles[i] = randomProjectile[Random.Range(0, randomProjectile.Length)];
		}
	}

	private void SpawnProjectile()
	{
		UnknownPriProjectile unknownPriProjectile = Object.Instantiate(randomProjectile[Random.Range(0, randomProjectile.Length)]);
		projectileScripts.Add(unknownPriProjectile);
		unknownPriProjectile.targetTransform = Camera.main.transform;
		unknownPriProjectile.projectileSpawner = this;
		unknownPriProjectile.unknown = unknown;
		unknownPriProjectile.TeleportProjectile();
		if (projectileScripts.Count >= projectileSpawnPoints.Length)
		{
			Debug.Log("Projectiles more than " + (projectileScripts.Count - 1) + " can't stop re-teleporting if they're both colliding each other!");
		}
	}
}
