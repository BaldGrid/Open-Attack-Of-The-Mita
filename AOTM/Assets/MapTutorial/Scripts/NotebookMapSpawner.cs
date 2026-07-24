using UnityEngine;

public class NotebookMapSpawner : MonoBehaviour
{
    public MiniMap miniMap;
    public GameObject notebookPrefab;

    void Start()
    {
        foreach (var book in FindObjectsOfType<NotebookScript>())
        {
            GameObject bookie = Instantiate(notebookPrefab, gameObject.transform, false);
            bookie.name = book.transform.name;
            bookie.GetComponent<NotebookMapItem>().notebook = book;
            bookie.GetComponent<NotebookMapItem>().miniMap = miniMap;
        }
    }
}
