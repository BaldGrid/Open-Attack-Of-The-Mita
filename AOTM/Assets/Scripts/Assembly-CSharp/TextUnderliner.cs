using TMPro;
using UnityEngine;

public class TextUnderliner : MonoBehaviour
{
	public TMP_Text text;

    private void Awake()
    {
        if(!text) text = GetComponent<TMP_Text>();
    }
    public void Underline()
	{
		text.fontStyle = FontStyles.Underline;
	}

	public void Ununderline()
	{
		text.fontStyle = FontStyles.Normal;
	}
}
