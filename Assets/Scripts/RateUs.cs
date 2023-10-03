using UnityEngine;
using UnityEngine.UI;

public class RateUs : MonoBehaviour
{
	private Transform star;

	private string[] rateString = new string[5]
	{
		"BAD",
		"NOT BAD",
		"GOOD",
		"VERY GOOD",
		"EXCELENT!"
	};

	private int starNumber;

	public void StarClick(Transform t)
	{
		if (star == null)
		{
			star = base.transform.Find("Star");
			base.transform.Find("ButtonOk").gameObject.SetActive(value: true);
		}
		starNumber = int.Parse(string.Empty + t.parent.gameObject.name);
		base.transform.Find("TextTitle").GetComponent<Text>().text = string.Empty + rateString[starNumber - 1];
		for (int i = 0; i < 5; i++)
		{
			if (i + 1 <= starNumber)
			{
				star.Find(string.Empty + (i + 1)).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				star.Find(string.Empty + (i + 1)).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}
	}

	public void OkClick()
	{
		if (starNumber >= 4)
		{
			Application.OpenURL("market://details?id=com.gamebolt.battleroyale");
		}
		MainMenu.userPlayer.rate = true;
		MainMenu.userPlayer.gold += 100;
		PlayerPrefsX.SetBool("rate", MainMenu.userPlayer.rate);
		PlayerPrefs.SetInt("gold", MainMenu.userPlayer.gold);
		base.gameObject.SetActive(value: false);
		base.transform.parent.parent.GetComponent<MainMenu>().RefreshMainMenu();
	}
}
