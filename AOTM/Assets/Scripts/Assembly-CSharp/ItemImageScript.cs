using UnityEngine;
using UnityEngine.UI;

public class Item_Image_Script : MonoBehaviour
{
	public RawImage sprite;

	[SerializeField]
	private Texture noItemSprite;

	[SerializeField]
	private Texture blankSprite;

	public GameControllerScript gs;

	private void Update()
	{
		if (gs != null)
		{
			Texture texture = gs.itemSlot[gs.itemSelected].texture;
			if (texture == blankSprite)
			{
				sprite.texture = noItemSprite;
			}
			else
			{
				sprite.texture = texture;
			}
		}
		else
		{
			sprite.texture = noItemSprite;
		}
	}
}
