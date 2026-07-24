using UnityEngine;
using UnityEngine.UI;

public class DetentionDoorMapCell : MonoBehaviour
{
    public DoorScript door;
    private Image img;
    public Sprite locked;
    public Sprite unlocked;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (door.DoorLocked)
            img.sprite = locked;
        else
            img.sprite = unlocked;
    }
}