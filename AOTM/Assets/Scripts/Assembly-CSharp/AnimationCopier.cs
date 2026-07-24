using UnityEngine;
using UnityEngine.UI;

public class AnimationCopier : MonoBehaviour
{
	[SerializeField]
	private Image source;

	[SerializeField]
	private Image[] target;

	private void LateUpdate()
	{
		Image[] array = target;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sprite = source.sprite;
		}
	}
}
