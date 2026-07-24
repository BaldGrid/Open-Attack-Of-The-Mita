using TMPro;
using UnityEngine;

public class JumpRopeScript : MonoBehaviour
{
	public TMP_Text jumpCount;

	public PlayerScript ps;

	public Billboard bl;

	public TMP_Text jumptext;

	public Animator rope;

	public CameraScript cs;

	public PlaytimeScript playtime;

	public GameObject mobileIns;

	public int jumps;

	public float jumpDelay;

	public float ropePosition;

	public bool ropeHit;

	public bool jumpStarted;

	public int jumpstodo;

	private void OnEnable()
	{
		jumpstodo = Mathf.RoundToInt(Random.Range(1f, 7f));
		jumpDelay = 0.1f;
		ropeHit = true;
		jumpStarted = false;
		jumps = 0;
		jumpCount.text = 0 + "/" + jumpstodo;
		cs.jumpHeight = 0f;
		playtime.audioDevice.PlayOneShot(playtime.aud_ReadyGo);
	}

	private void Update()
	{
		if (jumpDelay > 0f)
		{
			jumpDelay -= Time.deltaTime;
		}
		else if (!jumpStarted)
		{
			jumpStarted = true;
			ropePosition = 1f;
			rope.SetTrigger("ActivateJumpRope");
			ropeHit = false;
		}
		if (ropePosition > 0f)
		{
			ropePosition -= Time.deltaTime;
		}
		else if (!ropeHit)
		{
			RopeHit();
		}
	}

	private void RopeHit()
	{
		ropeHit = true;
		if (cs.jumpHeight <= 0.2f)
		{
			if (!ps.gc.debugMode)
			{
				Fail();
			}
			else
			{
				playtime.Disappoint();
				ps.DeactivateJumpRope();
			}
		}
		else
		{
			Success();
		}
		jumpStarted = false;
	}

	private void Success()
	{
		playtime.audioDevice.Stop();
		playtime.audioDevice.PlayOneShot(playtime.aud_Numbers[jumps]);
		jumps++;
		jumpCount.text = jumps + "/" + jumpstodo;
		jumpDelay = 0.1f;
		if (jumps >= jumpstodo)
		{
			playtime.audioDevice.Stop();
			playtime.audioDevice.PlayOneShot(playtime.aud_Congrats);
			ps.DeactivateJumpRope();
		}
	}

	private void Fail()
	{
		jumpCount.text = jumps + "HAHAHAHAHAHAHA";
		jumptext.text = "Y@u n@0B";
		ps.PTgameOver = true;
		bl.enabled = true;
		RenderSettings.skybox = ps.blackSky;
		ps.hud.enabled = false;
	}
}
