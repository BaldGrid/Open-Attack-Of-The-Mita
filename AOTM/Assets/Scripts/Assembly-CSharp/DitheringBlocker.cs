using UnityEngine;
using UnityEngine.UI;

public class DitheringBlocker : MonoBehaviour
{
	[SerializeField]
	private GameObject blocker;

	private static DitheringBlocker instance;

	private void Awake()
	{
		instance = this;
		blocker = GetComponentInChildren<Image>().gameObject;
		blocker.SetActive(value: false);
	}

	public static void Block()
	{
		instance.blocker.SetActive(value: true);
	}

	public static void UnBlock()
	{
		instance.blocker.SetActive(value: false);
	}
}
