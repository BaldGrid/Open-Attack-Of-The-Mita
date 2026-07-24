using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public GameControllerScript gc;
    public Transform[] spawnPoints;

    public GameObject[] MISC;
    public GameObject[] FOOD;
    public GameObject[] FACULTY;
    public GameObject[] TAPE;
    public GameObject[] JANITOR;

    private List<Transform> availablePoints = new List<Transform>();

    void Start()
    {
        availablePoints.AddRange(spawnPoints);

        for (int i = 0; i < spawnPoints.Length; i++)
            Spawn();
    }

    void Spawn()
    {
        if (availablePoints.Count == 0) return;

        int index = Random.Range(0, availablePoints.Count);
        Transform point = availablePoints[index];

        GameObject prefab = null;
        if (point.name.EndsWith("MISC"))
            prefab = MISC[Random.Range(0, MISC.Length)];
        else if (point.name.EndsWith("FOOD"))
            prefab = FOOD[Random.Range(0, FOOD.Length)];
        else if (point.name.EndsWith("FACULTY"))
            prefab = FACULTY[Random.Range(0, FACULTY.Length)];
        else if (point.name.EndsWith("TAPE"))
            prefab = TAPE[Random.Range(0, TAPE.Length)];
        else if (point.name.EndsWith("JANITOR"))
            prefab = JANITOR[Random.Range(0, JANITOR.Length)];
        else
            return;

        // 传递 point.position 而不是 point
        gc.spawnItem(prefab, point.position);
        availablePoints.RemoveAt(index);
    }
}