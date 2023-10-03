using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayGlobal
{
	public GlobalUser userPlayer;

	public GlobalUser userEnemy;

	public GlobalCard[] cardPlayer;

	public GlobalCard[] cardEnemy;

	public List<GlobalCard> cardAnimate = new List<GlobalCard>();

	public List<Transform> cardAnimateTarget = new List<Transform>();

	public List<GameplayCharacter> playerList = new List<GameplayCharacter>();

	public List<GameplayCharacter> enemyList = new List<GameplayCharacter>();

	public GlobalSound globalSound;

	public GameplayCamera gameplayCamera;

	public GameplayGraveyard gameplayGraveyard;

	public bool[] playerCastle = new bool[3]
	{
		true,
		true,
		true
	};

	public bool[] enemyCastle = new bool[3]
	{
		true,
		true,
		true
	};

	public GameObject[] cooldownObj = new GameObject[4];

	public RectTransform[] cooldown = new RectTransform[4];

	public Transform transform;

	public Transform space;

	public Transform graveyard;

	public Transform movePath;

	public Transform game;

	public Transform panelSpawn;

	public Transform canvasEnergy;

	public Transform buttonAnimate;

	public Transform spaceNow;

	public Transform characterPlayerNow;

	public Transform characterEnemyNow;

	public float energyTimePlayer;

	public float energyTimePlayerMax = 2f;

	public float energyTimeEnemy;

	public float energyTimeEnemyMax = 2f;

	public float timeMatch;

	public float timeMatchMax = 240f;

	public int energyPlayerNow;

	public int energyPlayerMax = 10;

	public int energyEnemyNow;

	public int energyEnemyMax = 10;

	public int idCardPlayer;

	public int idCardEnemy;

	public int winLoseCondition;

	public int star;

	public bool isAnimate;

	public bool isStart;

	public bool isInjury;

	public Text textAlert;

	public Text textTime;

	public RectTransform rectBarEnergy;

	private int[] cRank = new int[6]
	{
		0,
		200,
		500,
		1000,
		2000,
		3000
	};

	private int[] cUpgrade = new int[6]
	{
		0,
		20,
		110,
		650,
		4000,
		10000
	};

	private int[] cGold = new int[6]
	{
		0,
		450,
		2300,
		8500,
		66000,
		300000
	};

	private int[] cLevel = new int[21]
	{
		1,
		1,
		1,
		1,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0
	};

	private int[] cFormation = new int[8]
	{
		1,
		2,
		3,
		4,
		0,
		0,
		0,
		0
	};

	public GameplayGlobal(Transform t)
	{
		transform = t;
		InitValue();
		InitGameobject();
		InitDebugData();
		InitGlobalUser();
		InitGlobalCard();
		InitGlobalButtonCharacter();
		InitCastle();
	}

	private void InitValue()
	{
		timeMatch = timeMatchMax;
	}

	private void InitGameobject()
	{
		Transform transform = this.transform.Find("Place");
		Transform transform2 = this.transform.Find("Canvas");
		globalSound = this.transform.parent.Find("Sound").GetComponent<GlobalSound>();
		gameplayCamera = this.transform.GetComponent<GameplayCamera>();
		gameplayGraveyard = this.transform.GetComponent<GameplayGraveyard>();
		space = transform.Find("Space");
		graveyard = transform.Find("Graveyard");
		movePath = transform.Find("MovePath");
		game = transform.Find("Game");
		textTime = transform2.Find("PanelTop").Find("PanelTime").Find("TextTime")
			.GetComponent<Text>();
		textAlert = transform2.Find("PanelCenter").Find("TextAlert").GetComponent<Text>();
		panelSpawn = transform2.Find("PanelBottom").Find("PanelSpawn");
		canvasEnergy = transform2.Find("PanelBottom").Find("PanelEnergy").Find("Energy");
		rectBarEnergy = canvasEnergy.Find("Bar").GetComponent<RectTransform>();
	}

	private void InitGlobalUser()
	{
		cardPlayer = userPlayer.card;
		cardEnemy = userEnemy.card;
	}

	private void InitGlobalCard()
	{
		List<GlobalCard> list = new List<GlobalCard>();
		List<GlobalCard> list2 = new List<GlobalCard>();
		for (int i = 0; i < cardPlayer.Length; i++)
		{
			if (cardPlayer[i] != null && cardPlayer[i].id != 0)
			{
				list.Add(cardPlayer[i]);
			}
			if (cardEnemy[i] != null && cardEnemy[i].id != 0)
			{
				list2.Add(cardEnemy[i]);
			}
		}
		List<GlobalCard> list3 = new List<GlobalCard>(list);
		List<GlobalCard> list4 = new List<GlobalCard>(list2);
		while (list.Count != 8)
		{
			list.Add(list3[Random.Range(0, list3.Count)]);
		}
		while (list2.Count != 8)
		{
			list2.Add(list4[Random.Range(0, list4.Count)]);
		}
		Shuffle(list);
		Shuffle(list2);
		cardPlayer = list.ToArray();
		cardEnemy = list2.ToArray();
	}

	private void InitGlobalButtonCharacter()
	{
		Gameplay component = this.transform.GetComponent<Gameplay>();
		for (int i = 0; i < 4; i++)
		{
			Transform transform = panelSpawn.Find("ButtonCharacterNow" + (i + 1));
			transform.Find("Active").gameObject.SetActive(value: false);
			component.ButtonCharacterNowListener(transform.Find("Button").GetComponent<Button>());
			cooldownObj[i] = transform.Find("Button").Find("Cooldown").gameObject;
			cooldown[i] = cooldownObj[i].GetComponent<RectTransform>();
		}
	}

	private void InitCastle()
	{
		GlobalCard card = new GlobalCard(0, userPlayer.level);
		GlobalCard card2 = new GlobalCard(0, userPlayer.level);
		GlobalCard card3 = new GlobalCard(-1, userPlayer.level);
		GlobalCard card4 = new GlobalCard(-1, userPlayer.level);
		game.Find("PlayerCastle1").GetComponent<GameplayCharacter>().InitCastle(card3);
		game.Find("PlayerCastle2").GetComponent<GameplayCharacter>().InitCastle(card);
		game.Find("PlayerCastle3").GetComponent<GameplayCharacter>().InitCastle(card);
		game.Find("EnemyCastle1").GetComponent<GameplayCharacter>().InitCastle(card4);
		game.Find("EnemyCastle2").GetComponent<GameplayCharacter>().InitCastle(card2);
		game.Find("EnemyCastle3").GetComponent<GameplayCharacter>().InitCastle(card2);
	}

	private void Shuffle<T>(IList<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = Random.Range(0, num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	private void InitDebugData()
	{
		bool flag = false;
		if (MainMenu.userPlayer == null || MainMenu.userEnemy == null)
		{
			flag = true;
			userPlayer = new GlobalUser(isPlayer: true);
			userEnemy = new GlobalUser();
			MainMenu.userPlayer = userPlayer;
			MainMenu.userEnemy = userEnemy;
		}
		else
		{
			userPlayer = MainMenu.userPlayer;
			userEnemy = MainMenu.userEnemy;
		}
		if (!flag)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < cRank.Length; i++)
		{
			num = i;
			if (userPlayer.rank < cRank[i])
			{
				break;
			}
		}
		int num2 = (cUpgrade[num] - cUpgrade[num - 1]) * (userPlayer.rank - cRank[num - 1]) / (cRank[num] - cRank[num - 1]) + cUpgrade[num - 1];
		int num3 = (cGold[num] - cGold[num - 1]) * (userPlayer.rank - cRank[num - 1]) / (cRank[num] - cRank[num - 1]) + cGold[num - 1];
		List<int> list = new List<int>();
		cLevel = new int[21];
		cFormation = new int[8];
		if (userPlayer.rank < 800)
		{
			List<int> list2 = new List<int>();
			list2.Add(1);
			list2.Add(2);
			list2.Add(3);
			list2.Add(4);
			list2.Add(5);
			list2.Add(6);
			list2.Add(7);
			list2.Add(8);
			list2.Add(9);
			list2.Add(10);
			list2.Add(11);
			list2.Add(12);
			list = list2;
		}
		else if (userPlayer.rank < 1500)
		{
			List<int> list2 = new List<int>();
			list2.Add(1);
			list2.Add(2);
			list2.Add(3);
			list2.Add(4);
			list2.Add(5);
			list2.Add(6);
			list2.Add(7);
			list2.Add(8);
			list2.Add(9);
			list2.Add(10);
			list2.Add(11);
			list2.Add(12);
			list2.Add(13);
			list2.Add(14);
			list2.Add(15);
			list2.Add(16);
			list2.Add(17);
			list2.Add(18);
			list2.Add(19);
			list = list2;
		}
		else
		{
			List<int> list2 = new List<int>();
			list2.Add(1);
			list2.Add(2);
			list2.Add(3);
			list2.Add(4);
			list2.Add(5);
			list2.Add(6);
			list2.Add(7);
			list2.Add(8);
			list2.Add(9);
			list2.Add(10);
			list2.Add(11);
			list2.Add(12);
			list2.Add(13);
			list2.Add(14);
			list2.Add(15);
			list2.Add(16);
			list2.Add(17);
			list2.Add(18);
			list2.Add(19);
			list2.Add(20);
			list2.Add(21);
			list = list2;
		}
		int num4 = 0;
		while (list.Count != 0)
		{
			int index = Random.Range(0, list.Count);
			int num5 = list[index];
			list.RemoveAt(index);
			bool flag2 = false;
			for (int j = 0; j < cFormation.Length; j++)
			{
				if (cFormation[j] == 0)
				{
					cFormation[j] = num5;
					cLevel[num5 - 1] = 1;
					if (j == cFormation.Length - 1)
					{
						flag2 = true;
					}
					break;
				}
			}
			if (flag2)
			{
				break;
			}
			num4++;
		}
		userEnemy.cardLevel = cLevel;
		userEnemy.cardFormation = cFormation;
		userEnemy.level = userPlayer.level;
		List<int> list3 = new List<int>(userEnemy.cardFormation);
		while (list3.Count != 0)
		{
			int index2 = Random.Range(0, list3.Count);
			int num6 = list3[index2];
			if (num6 != 0)
			{
				GlobalCard globalCard = new GlobalCard(num6, userEnemy.cardLevel[num6 - 1]);
				int num7 = userEnemy.cardUpgrade[(int)(globalCard.rarity - 1), globalCard.level];
				int num8 = userEnemy.cardGold[(int)(globalCard.rarity - 1), globalCard.level];
				if (num7 != -1 && num2 >= num7 && num3 >= num8)
				{
					num2 -= num7;
					num3 -= num8;
					userEnemy.cardLevel[globalCard.id - 1]++;
				}
				else
				{
					list3.RemoveAt(index2);
				}
			}
			else
			{
				list3.RemoveAt(index2);
			}
		}
		userEnemy.RefreshCard();
	}
}
