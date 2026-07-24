using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public RectTransform map;
    public RectTransform rectPlayer;
    public float scale = 0.1f;
    [HideInInspector] public Vector3 initialPlayerPos;
    private Vector3 worldOrigin = Vector3.zero;

    void Start()
    {
        initialPlayerPos = player.position;
    }

    void Update()
    {
        Vector3 delta = player.position - initialPlayerPos;
        map.localPosition = new Vector3(-delta.x * scale, -delta.z * scale, 0);
        Vector3 pivot = rectPlayer.localPosition;
        Vector3 mapPos = map.localPosition;
        map.localPosition = mapPos - pivot;
        rectPlayer.localRotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);
        map.localPosition += pivot;
    }
}