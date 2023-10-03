using UnityEngine;
using UnityEngine.UI;

public class AnimationLoot : MonoBehaviour
{
	public int loot = 1;

	public Sprite[] lootClose;

	public Sprite[] lootOpen;

	public void Close()
	{
		base.transform.Find("Loot").GetComponent<Image>().sprite = lootClose[loot - 1];
	}

	public void Open()
	{
		base.transform.Find("Loot").GetComponent<Image>().sprite = lootOpen[loot - 1];
	}

	public void Sound(string sound)
	{
		float volume = 1f;
		if (sound == "ChestOpen")
		{
			volume = 0.31f;
		}
		base.transform.root.Find("Sound").GetComponent<GlobalSound>().AudioOnce("Sound/" + sound, volume);
	}
}
