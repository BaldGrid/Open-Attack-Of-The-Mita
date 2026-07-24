using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
	public GameControllerScript gc;

	public Transform player;

	private SpriteRenderer spriteRenderer;

	public List<string> itemNames;

	private void Start()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	public void ReplaceItem()
	{
		if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
		{
			spriteRenderer.sprite = gc.itemSpr[gc.item[gc.itemSelected]];
			base.gameObject.name = itemNames[gc.item[gc.itemSelected]];
		}
	}

	private void Update()
	{
		if ((!ControlFreak2.CF2Input.GetMouseButtonDown(0) && (!ControlFreak2.CF2Input.GetKeyDown(KeyCode.E) || Time.timeScale == 0f || !(Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) & (Vector3.Distance(player.position, hitInfo.point) < 10f)))) || !Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out hitInfo))
		{
			return;
		}
		if ((hitInfo.transform.name == "Pickup_EnergyFlavoredZestyBar") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(1);
		}
		else if ((hitInfo.transform.name == "Pickup_YellowDoorLock") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(2);
		}
		else if ((hitInfo.transform.name == "Pickup_Key") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(3);
		}
		else if ((hitInfo.transform.name == "Pickup_BSODA") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(4);
		}
		else if ((hitInfo.transform.name == "Pickup_Quarter") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(5);
		}
		else if ((hitInfo.transform.name == "Pickup_Tape") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(6);
		}
		else if ((hitInfo.transform.name == "Pickup_AlarmClock") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(7);
		}
		else if ((hitInfo.transform.name == "Pickup_WD-3D") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(8);
		}
		else if ((hitInfo.transform.name == "Pickup_SafetyScissors") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(9);
		}
		else if ((hitInfo.transform.name == "Pickup_BigBoots") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(10);
		}
		else if ((hitInfo.transform.name == "Pickup_Teleporter") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(11);
		}
		else if ((hitInfo.transform.name == "Pickup_PortalPoster") & (Vector3.Distance(player.position, base.transform.position) < 10f))
		{
			ReplaceItem();
			hitInfo.transform.gameObject.SetActive(value: false);
			if ((gc.item[0] != 0) & (gc.item[1] != 0) & (gc.item[2] != 0) & (gc.item[3] != 0) & (gc.item[4] != 0))
			{
				hitInfo.transform.gameObject.SetActive(value: true);
			}
			gc.CollectItem(12);
		}
	}
}
