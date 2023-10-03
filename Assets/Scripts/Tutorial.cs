using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
	public static Tutorial instance;

	public static int tutorialNow;

	public Transform panelTutorial;

	public Transform[] tutorialPosition;

	private GlobalSound globalSound;

	private Transform tap;

	private Transform talk;

	private Text talkText;

	private int tutorialPositionNow;

	private string[] tutorialText = new string[31]
	{
		"Pst pst.. You..\nYou are that new general right?",
		"Who am I?\nI'm appointed by the king advisor to help you..",
		"You will lead your troops to invade another territory.",
		"And right now, you don't have any troops at all.",
		"Before we prepare the troops, let's go to gift store.",
		string.Empty,
		"Once a day, there's 6 gift chests ready to be unlocked.",
		"What is the chest content? You will get a random troops.",
		"Let's just open one chest, you will know what I mean.",
		string.Empty,
		"Nice! Let's go to barrack to prepare your troops.",
		string.Empty,
		"Just as you see, right now you have no unit set in formation..",
		"Let's assign knight into your formation troops.",
		string.Empty,
		"For your information, if you have same number unit..",
		"..You can upgrade it to make that unit stronger.",
		"Now, let's set knight in your troops.",
		string.Empty,
		string.Empty,
		"Nice! Later You can set another units to your formation.",
		"Now you are ready to invade another country.",
		string.Empty,
		string.Empty,
		"Your objective is simple: destroy the enemy's last castle.",
		"To achieve that, use your wisdom to set the troop's position.",
		"Let's try set our knight. Knight will cost you 3 energy.",
		string.Empty,
		string.Empty,
		"Great! Now you can explore the mechanism yourself.",
		"Go claim yourself a honor for your king!"
	};

	public void Init()
	{
		instance = this;
		tap = panelTutorial.Find("Tap");
		talk = panelTutorial.Find("Talk");
		talkText = talk.Find("Panel").Find("Text").GetComponent<Text>();
		globalSound = base.transform.parent.Find("Sound").GetComponent<GlobalSound>();
		ButtonTalkListener(talk.GetComponent<Button>());
		Show();
	}

	public void Show()
	{
		if (tutorialNow < tutorialText.Length)
		{
			panelTutorial.gameObject.SetActive(value: true);
			tap.gameObject.SetActive(value: false);
			talk.gameObject.SetActive(value: false);
			if (tutorialText[tutorialNow] != string.Empty)
			{
				globalSound.AudioOnce("Sound/Sword2", 0.41f);
				talkText.text = string.Empty + tutorialText[tutorialNow];
				talk.gameObject.SetActive(value: true);
				Time.timeScale = 0f;
			}
			else
			{
				globalSound.AudioOnce("Sound/Tutorial", 0.31f);
				tap.GetComponent<RectTransform>().position = tutorialPosition[tutorialPositionNow].GetComponent<RectTransform>().position;
				tap.gameObject.SetActive(value: true);
				Time.timeScale = 1f;
			}
		}
		else
		{
			if (!PlayerPrefsX.GetBool("tutorial"))
			{
				PlayerPrefsX.SetBool("tutorial", value: true);
			}
			tutorialNow = 0;
			panelTutorial.gameObject.SetActive(value: false);
			Time.timeScale = 1f;
		}
	}

	public void Next()
	{
		if (tutorialText[tutorialNow] == string.Empty)
		{
			tutorialPositionNow++;
		}
		tutorialNow++;
	}

	public void Close()
	{
		panelTutorial.gameObject.SetActive(value: false);
	}

	private void ButtonTalkListener(Button b)
	{
		b.onClick.AddListener(delegate
		{
			Next();
			Show();
		});
	}
}
