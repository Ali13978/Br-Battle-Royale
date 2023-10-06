using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayWinLose : MonoBehaviour
{
	[SerializeField] private Button loot;
    [SerializeField] private TMP_Text ribbonText;

	//public Button share;

	//private bool isShare;

	private void Start()
	{
		base.transform.root.Find("Sound").GetComponent<GlobalSound>().GetAudioSource()
			.Stop();
	}

	public void End(int isWin)
	{
        //ADMOB.instance.ShowIntersitial();

		if (isWin == 1)
        {
            ribbonText.text = "Win";

            loot.gameObject.SetActive(true);
            loot.enabled = true;
			loot.onClick.AddListener(delegate
			{
				//ADMOB.instance.ShowBanner(AdPosition.BOTTOM_CENTER);
				SceneManager.LoadSceneAsync("Main Menu");
			});



			//share.onClick.AddListener(delegate
			//{
			//	if (!isShare)
			//	{
			//		isShare = true;
			//		share.GetComponent<Text>().text = string.Empty;
			//		//FACEBOOK.instance.FaceBookShare();
			//	}
			//});
		}
		else
		{
            //ADMOB.instance.ShowBanner(AdPosition.BOTTOM_CENTER);
            ribbonText.text = "Defeat";

			Run.After(1f, delegate
			{
				SceneManager.LoadSceneAsync("Main Menu");
			});
		}
	}

	public void Sound(string sound)
	{
		float volume = 0.71f;
		if (sound == "BattleWin")
		{
			volume = 0.51f;
		}
		else if (sound == "BattleLose")
		{
			volume = 0.61f;
		}
		else if (sound == "Sword2")
		{
			volume = 0.91f;
		}
		base.transform.root.Find("Sound").GetComponent<GlobalSound>().AudioOnce("Sound/" + sound, volume);
	}
}
