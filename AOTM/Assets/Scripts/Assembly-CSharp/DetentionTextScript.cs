using TMPro;
using UnityEngine;

public class DetentionTextScript : MonoBehaviour
{
	public DoorScript door;

	private TMP_Text text;

	private void Start()
	{
		text = GetComponent<TMP_Text>();
	}

	private void Update()
	{
		if (door.lockTime > 0f)
		{
			text.text = "Y0U h@^£ d£T£nti@n! \n" + Mathf.CeilToInt(door.lockTime) + " S£c0nds r£m@iN!";
		}
		else
		{
			text.text = string.Empty;
		}
	}
}
