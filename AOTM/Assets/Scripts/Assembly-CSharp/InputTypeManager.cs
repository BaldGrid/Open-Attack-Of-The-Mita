using UnityEngine;

public class InputTypeManager : MonoBehaviour
{
	private static InputTypeManager itm;

	public static bool usingTouch;

	private void Awake()
	{
		ControlFreak2.CF2Input.simulateMouseWithTouches = false;
		if (itm == null)
		{
			itm = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (itm != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (ControlFreak2.CF2Input.touchCount > 0 && !usingTouch)
		{
			usingTouch = true;
		}
		else if (ControlFreak2.CF2Input.anyKeyDown)
		{
			usingTouch = false;
		}
	}
}
