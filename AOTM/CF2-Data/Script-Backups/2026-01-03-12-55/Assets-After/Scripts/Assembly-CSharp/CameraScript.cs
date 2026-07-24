using UnityEngine;

public class CameraScript : MonoBehaviour
{
	public GameObject player;

	public PlayerScript ps;

	public Transform baldi;

	public Transform pt;

	public Transform bullys;

	public Transform nulll;

	public float initVelocity;

	public float velocity;

	public float gravity;

	private int lookBehind;

	public Vector3 offset;

	public float jumpHeight;

	public Vector3 jumpHeightV3;

	private void Start()
	{
		offset = base.transform.position - player.transform.position;
	}

	private void Update()
	{
		if (ps.jumpRope)
		{
			velocity -= gravity * Time.deltaTime;
			jumpHeight += velocity * Time.deltaTime;
			if (jumpHeight <= 0f)
			{
				jumpHeight = 0f;
				if (ControlFreak2.CF2Input.GetMouseButtonDown(0) || ControlFreak2.CF2Input.GetKeyDown(KeyCode.E))
				{
					velocity = initVelocity;
				}
			}
			jumpHeightV3 = new Vector3(0f, jumpHeight, 0f);
		}
		if (ControlFreak2.CF2Input.GetButton("Look Behind"))
		{
			lookBehind = 180;
		}
		else
		{
			lookBehind = 0;
		}
	}

	private void LateUpdate()
	{
		base.transform.position = player.transform.position + offset;
		if (!ps.gameOver & !ps.jumpRope & !ps.PTgameOver & !ps.BullygameOver & !ps.PRIgameOver)
		{
			base.transform.position = player.transform.position + offset;
			base.transform.rotation = player.transform.rotation * Quaternion.Euler(0f, lookBehind, 0f);
		}
		else if (ps.gameOver)
		{
			float num = 5.2f;
			base.transform.position = baldi.transform.position + baldi.transform.forward * 2f + new Vector3(0f, num, 0f);
			base.transform.LookAt(new Vector3(baldi.position.x, baldi.position.y + num, baldi.position.z));
		}
		else if (ps.PTgameOver)
		{
			float num2 = 3f;
			base.transform.position = pt.transform.position + pt.transform.forward * 2f + new Vector3(0f, num2, 0f);
			base.transform.LookAt(new Vector3(pt.position.x, pt.position.y + num2, pt.position.z));
		}
		else if (ps.BullygameOver)
		{
			float num3 = 3.6f;
			base.transform.position = bullys.transform.position + bullys.transform.forward * 2f + new Vector3(0f, num3, 0f);
			base.transform.LookAt(new Vector3(bullys.position.x, bullys.position.y + num3, bullys.position.z));
		}
		else if (ps.PRIgameOver)
		{
			float num4 = 5.2f;
			base.transform.position = nulll.transform.position + nulll.transform.forward * 2f + new Vector3(0f, num4, 0f);
			base.transform.LookAt(new Vector3(nulll.position.x, nulll.position.y + num4, nulll.position.z));
		}
		else if (ps.jumpRope)
		{
			base.transform.position = player.transform.position + offset + jumpHeightV3 * 2.3f;
			base.transform.rotation = player.transform.rotation;
		}
	}
}
