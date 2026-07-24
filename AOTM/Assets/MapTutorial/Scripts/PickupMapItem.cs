using UnityEngine;

public class PickupMapItem : MonoBehaviour
{
    public PickupScript pick;
    public MiniMap miniMap;
    public RectTransform rectIcon;
    public GameObject icon;

    void Update()
    {
        Vector3 relativePos = pick.transform.position - miniMap.initialPlayerPos;

        Vector3 mapPos = new Vector3(
            relativePos.x * miniMap.scale * 1.666666666666667f,
            relativePos.z * miniMap.scale * 1.666666666666667f,
            0
        );
        
        rectIcon.localPosition = mapPos;
    }
}