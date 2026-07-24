using UnityEngine;

public class CursorControllerScript : MonoBehaviour
{
	private void Update()
	{
	}

	public void LockCursor()
	{
		ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
		ControlFreak2.CFCursor.visible = false;
	}

	public void UnlockCursor()
	{
		ControlFreak2.CFCursor.lockState = CursorLockMode.None;
		ControlFreak2.CFCursor.visible = true;
	}
}
