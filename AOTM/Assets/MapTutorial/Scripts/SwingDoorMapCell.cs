using UnityEngine;
using UnityEngine.UI;

public class SwingDoorMapCell : MonoBehaviour
{
    public SwingingDoorScript swing;
    private Image img;
    public Sprite locked;
    public Sprite unlocked;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (swing.bDoorLocked)
            img.sprite = locked;
        else
            img.sprite = unlocked;
    }
}
