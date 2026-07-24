using UnityEngine;

public class SubtitleData : MonoBehaviour
{
	[Header("Subtitle Colors")]
	public Color envSubColor = Color.white;

	public Color itmSubColor = Color.gray;

	public Color baldiSubColor = Color.green;

	public Color principalSubColor = Color.blue;

	public Color sweepSubColor = Color.green;

	public Color prizeSubColor = Color.cyan;

	public Color playtimeSubColor = Color.red;

	public Color bullySubColor = new Color(1f, 0.64705884f, 0f, 1f);

	[Header("Environment Subtitles")]
	public string envStandDoorOpen = "*DOOR OPENS*";

	public string envStandDoorClose = "*DOOR CLOSES*";

	public string envSwingDoorOpen = "*SWINGING DOOR OPENS*";

	public string envFountainDrink = "*Slurp*";

	public string envStandDoorRattle = "*Rattling*";

	public string envStandDoorLock = "*Click!*";

	public string envStandDoorUnlock = "*Unclick!*";

	public string envTape = "*Annoying noise*";

	[Header("Item Subtitles")]
	public string itmBsodaShoot = "*SPRAY!*";

	public string itmNoSqueeUse = "*WHOOSH!*";

	[Header("Baldi Subtitles")]
	public string baldiWelcome = "Oh, hi! Welcome to my schoolhouse!";

	public string baldiNeed2Book = "You need to collect 2 notebook before you can use these doors!";

	public string baldiSlap = "*SLAP!*";

	[Header("Principal Captions")]
	public string priWhistle = "*Whistling*";

	public string priNoRunning = "No running in the halls!";

	public string priNoFaculty = "No entering school faculty only rooms in the halls!";

	public string priNoBullying = "No bullying in the halls!";

	public string priNoEscaping = "No escaping detention in the halls!";

	public string priNoDrinking = "No drinking drinks in the halls!";

	public string priDetention = "detention for you!";

	public string[] priSeconds = new string[11]
	{
		"15 seconds", "20 seconds", "25 seconds", "30 seconds", "35 seconds", "40 seconds", "45 seconds", "50 seconds", "55 seconds", "60 seconds",
		"99 seconds"
	};

	[Header("Sweep Captions")]
	public string sweepTime = "Looks like it's sweepin' time!";

	public string sweepSweep = "Gotta sweep sweep sweep!";

	[Header("1st Prize Captions")]
	public string prizeMotor = "*Motor running*";

	public string[] prizeRandom = new string[2] { "I HAVE BEEN PROGRAMMED TO DESIRE YOUR IMAGE", "I AM LOOKING FOR YOU" };

	public string[] prizeHug = new string[2] { "I HUG PEOPLE FOR ALL ETERNITY", "WILL YOU MARRY ME" };

	public string[] prizeLost = new string[2] { "I HAVE LOST YOU - I DON'T LIKE THAT", "OH - no" };

	public string[] prizeFound = new string[2] { "I AM COMING READY OR NOR HERE I COME", "I SEE YOU - FRIEND" };

	[Header("Playtime Captions")]
	public string playtimeMusic = "*Music*";

	public string playtimeLetsPlay = "Let's play!";

	public string playtimeReady = "Ready? Go!";

	public string playtimeOops = "Oops! You messed up!";

	public string playtimeSuccess = "Wow! That's great! Let's play again... sometime soon!";

	public string playtimeSad = "Oh! That makes me sad!";

	public string[] playtimeRandom = new string[2] { "Hehehehehee!", "I wanna play with someone!" };

	public string[] playtimeNumbers = new string[10] { "1!", "2!", "3!", "4!", "5!", "6!", "7!", "8!", "9!", "10!" };
}
