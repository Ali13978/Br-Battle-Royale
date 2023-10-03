using admob;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
	public static Gameplay gameplay;

	public static bool isSpace;

	public static bool isDrag;

	public Material bwMaterial;

	public Sprite[] icon;

	public Sprite[] loot;

	public GameplayGlobal global;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		gameplay = this;
		isSpace = false;
		isDrag = false;
		global = new GameplayGlobal(base.transform);
		RefreshAllCard();
		InitAnimationPrepare();
		//ADMOB.instance.ShowBanner(AdPosition.TOP_CENTER);
	}

	private void Update()
	{
		if (global.isStart && !IsWinLose())
		{
			UpdateEnergyPlayer();
			UpdateEnergyEnemy();
			UpdateTimeMatch();

            if (Input.GetKeyDown(KeyCode.Space))
                LeaderboardManager.instance.AddScore(30);
		}
	}

	public void StartGameplay()
	{
		base.transform.Find("Canvas").gameObject.SetActive(value: true);
		global.isStart = true;
		global.energyPlayerNow = 5;
		global.energyEnemyNow = 5;
		if (!PlayerPrefsX.GetBool("tutorial"))
		{
			GetComponent<Tutorial>().Init();
		}
	}

	public void SetSpaceActive(bool b)
	{
		global.space.gameObject.SetActive(b);
		if (!b)
		{
			return;
		}
		int id = global.cardPlayer[global.idCardPlayer - 1].id;
		for (int i = 0; i < 3; i++)
		{
			Transform transform = global.space.Find("SpaceEnemy" + (i + 1));
			if (id != 1 && id != 5)
			{
				transform.GetComponent<BoxCollider>().enabled = !global.enemyCastle[i];
				transform.GetComponent<SpriteRenderer>().enabled = global.enemyCastle[i];
			}
			else
			{
				transform.GetComponent<BoxCollider>().enabled = true;
				transform.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}

	public bool SpawnCharacterV1(Vector3 v3, bool isPlayer)
	{
		GlobalCard globalCard = null;
		globalCard = ((!isPlayer) ? global.cardEnemy[global.idCardEnemy - 1] : global.cardPlayer[global.idCardPlayer - 1]);
		Transform character = global.gameplayGraveyard.GetCharacter(globalCard.id);
		string str = "Player";
		GameplayCharacter component = character.GetComponent<GameplayCharacter>();
		if (!isPlayer)
		{
			str = "Enemy";
			global.enemyList.Add(component);
			global.characterEnemyNow = character;
		}
		else
		{
			global.playerList.Add(component);
			global.characterPlayerNow = character;
		}
		character.gameObject.SetActive(value: true);
		character.gameObject.name = string.Empty + str;
		character.tag = string.Empty + str;
		character.SetParent(global.game);
		character.localPosition = new Vector3(v3.x, v3.y, 0f);
		component.InitMoveAbleSpace();
		return true;
	}

	public bool SpawnCharacterV2(bool isPlayer)
	{
		GlobalCard globalCard = null;
		GlobalCard[] array;
		int num;
		Transform transform;
		if (isPlayer)
		{
			array = global.cardPlayer;
			globalCard = array[global.idCardPlayer - 1];
			transform = global.characterPlayerNow;
			num = global.energyPlayerNow;
		}
		else
		{
			array = global.cardEnemy;
			globalCard = array[global.idCardEnemy - 1];
			transform = global.characterEnemyNow;
			num = global.energyEnemyNow;
		}
		if (num >= globalCard.energy)
		{
			Vector3 localPosition = transform.localPosition;
			for (int i = 0; i < globalCard.count; i++)
			{
				if (i != 0)
				{
					SpawnCharacterV1(localPosition, isPlayer);
				}
				transform = ((!isPlayer) ? global.characterEnemyNow : global.characterPlayerNow);
				transform.GetComponent<GameplayCharacter>().InitCharacter(globalCard);
				GameObject gameObject = transform.gameObject;
				string name = gameObject.name;
				gameObject.name = name + "-" + globalCard.id + "-" + (i + 1);
			}
			if (isPlayer)
			{
				global.energyPlayerNow -= globalCard.energy;
				global.canvasEnergy.Find("Text").GetComponent<Text>().text = global.energyPlayerNow + "/" + global.energyPlayerMax;
				global.spaceNow = null;
				global.characterPlayerNow = null;
				RefreshAnimateCard(array[4], global.panelSpawn.Find("ButtonCharacterNow" + global.idCardPlayer));
				UsingCard(globalCard, array);
				RefreshAllCard();
			}
			else
			{
				global.energyEnemyNow -= globalCard.energy;
				UsingCard(globalCard, array);
				global.characterEnemyNow = null;
			}
			return true;
		}
		global.gameplayGraveyard.AddCharacter(globalCard.id, transform);
		if (isPlayer)
		{
			global.globalSound.AudioOnce(global.gameplayGraveyard.resSound[8], 0.71f);
			global.gameplayCamera.InitShake(0.2f);
			global.spaceNow = null;
			global.characterPlayerNow = null;
		}
		else
		{
			global.characterEnemyNow = null;
		}
		return false;
	}

	public bool SpawnCharacterV3(bool isPlayer)
	{
		GlobalCard globalCard = null;
		Transform character;
		if (isPlayer)
		{
			GlobalCard[] cardPlayer = global.cardPlayer;
			globalCard = cardPlayer[global.idCardPlayer - 1];
			character = global.characterPlayerNow;
			global.spaceNow = null;
			global.characterPlayerNow = null;
		}
		else
		{
			GlobalCard[] cardPlayer = global.cardEnemy;
			globalCard = cardPlayer[global.idCardEnemy - 1];
			character = global.characterEnemyNow;
			global.characterEnemyNow = null;
		}
		global.gameplayGraveyard.AddCharacter(globalCard.id, character);
		return false;
	}

	public bool IsCastleAlive(bool isPlayer, int number)
	{
		if (isPlayer)
		{
			return global.playerCastle[number - 1];
		}
		return global.enemyCastle[number - 1];
	}

	public void DestroyCastle(bool isPlayer, int number)
	{
		if (isPlayer)
		{
			global.playerCastle[number - 1] = false;
		}
		else
		{
			global.enemyCastle[number - 1] = false;
		}
	}

	public void ButtonCharacterNowListener(Button b)
	{
		GameObject gameObject = b.transform.parent.gameObject;
		int number = int.Parse(string.Empty + gameObject.name[gameObject.name.Length - 1]);
		b.onClick.AddListener(delegate
		{
			if (Tutorial.tutorialNow == 0 || Tutorial.tutorialNow == 27)
			{
				if (Tutorial.tutorialNow == 27)
				{
					Tutorial.instance.Next();
					Tutorial.instance.Show();
				}
				global.globalSound.AudioOnce(global.gameplayGraveyard.resSound[9], 0.81f);
				ButtonCharacterNowClicked(number, isSpawn: false);
			}
		});
	}

	public void ButtonCharacterNowClicked(int number, bool isSpawn)
	{
		if (!isDrag)
		{
			if (global.idCardPlayer != 0)
			{
				global.panelSpawn.Find("ButtonCharacterNow" + global.idCardPlayer).Find("Active").gameObject.SetActive(value: false);
				global.panelSpawn.Find("ButtonCharacterNow" + global.idCardPlayer).localScale = new Vector3(1f, 1f, 1f);
			}
			if (global.idCardPlayer != number)
			{
				global.panelSpawn.Find("ButtonCharacterNow" + number).Find("Active").gameObject.SetActive(value: true);
				global.panelSpawn.Find("ButtonCharacterNow" + number).localScale = new Vector3(1.1f, 1.1f, 1f);
				global.idCardPlayer = number;
				SetSpaceActive(b: true);
			}
			else
			{
				global.panelSpawn.Find("ButtonCharacterNow" + number).gameObject.SetActive(!isSpawn);
				global.idCardPlayer = 0;
				SetSpaceActive(b: false);
			}
		}
	}

	public void ButtonRestart(Button b)
	{
		b.onClick.AddListener(delegate
		{
			Blank.targetLoadLevel = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene("Blank");
		});
	}

	public void ButtonBeginDragListener(Button b)
	{
		GameObject gameObject = b.transform.parent.gameObject;
		int number = int.Parse(string.Empty + gameObject.name[gameObject.name.Length - 1]);
		ButtonCharacterNowClicked(number, isSpawn: false);
		isDrag = true;
	}

	public void ButtonEndDragListener(Button b)
	{
		isDrag = false;
		if (!isSpace)
		{
			return;
		}
		isSpace = false;
		Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
		if (vector.z >= -5f)
		{
			if (SpawnCharacterV2(isPlayer: true))
			{
				if (Tutorial.tutorialNow == 28)
				{
					Tutorial.instance.Next();
					Tutorial.instance.Show();
				}
				ButtonCharacterNowClicked(global.idCardPlayer, isSpawn: true);
			}
		}
		else
		{
			SpawnCharacterV3(isPlayer: true);
			ButtonCharacterNowClicked(global.idCardPlayer, isSpawn: false);
		}
	}

	private void InitAnimationPrepare()
	{
		base.transform.parent.Find("Loading").gameObject.SetActive(value: true);
		base.transform.Find("Canvas").gameObject.SetActive(value: false);
		StartCoroutine(global.gameplayGraveyard.Init(delegate
		{
			base.transform.parent.Find("Loading").gameObject.SetActive(value: false);
			base.transform.parent.Find("Prepare").gameObject.SetActive(value: true);
			base.transform.parent.Find("Prepare").GetComponent<Animator>().enabled = true;
			base.transform.parent.Find("Prepare").GetComponent<Animator>().Play("Fight");
			global.globalSound.AudioLoop("Sound/Gameplay", 0.32f);
		}));
	}

	private void UpdateTimeMatch()
	{
		if (global.timeMatch > 0f)
		{
			global.timeMatch -= Time.deltaTime;
			if (global.timeMatch <= 0f)
			{
				global.timeMatch = 0f;
			}
			if (!global.isInjury && global.timeMatch <= 60f)
			{
				global.isInjury = true;
				global.globalSound.AudioOnce("Sound/Injury", 0.71f);
				Transform transform = base.transform.Find("Canvas").Find("PanelTop").Find("PanelTime");
				transform.Find("Text").gameObject.SetActive(value: false);
				transform.Find("TextInjury").gameObject.SetActive(value: true);
			}
			int num = (int)global.timeMatch;
			int num2 = num / 60;
			int num3 = num % 60;
			string str = "0" + num2;
			string empty = string.Empty;
			empty = ((num3 < 10) ? ("0" + num3) : (string.Empty + num3));
			global.textTime.text = str + ":" + empty;
		}
		else
		{
			global.isStart = false;
			WinLoseCondition();
		}
	}

	private bool IsWinLose()
	{
		if (!global.enemyCastle[0])
		{
			global.isStart = false;
			WinLoseCondition();
			return true;
		}
		if (!global.playerCastle[0])
		{
			global.isStart = false;
			WinLoseCondition();
			return true;
		}
		return false;
	}

	private void WinLoseCondition()
	{
		int num = 0;
		global.star = 0;
		for (int i = 0; i < global.enemyCastle.Length; i++)
		{
			if (!global.playerCastle[i])
			{
				num++;
			}
			if (!global.enemyCastle[i])
			{
				global.star++;
			}
		}
		if (num < global.star)
		{
			global.winLoseCondition = 1;
		}
		else if (num > global.star)
		{
			global.winLoseCondition = 2;
		}
		else
		{
			global.winLoseCondition = 3;
		}
		WinLoseDraw();
	}

	private void WinLoseDraw()
	{
		if (global.winLoseCondition > 0)
		{
			Transform winLoseAnimation = base.transform.parent.Find("WinLose").Find("Canvas").Find("PanelCenter")
				.Find("WinLoseAnimation");

			string condition = string.Empty;
            if (global.winLoseCondition == 1)
            {
                condition = "Win";
                int num = MainMenu.userPlayer.WinLoseBattle(global.star, isWin: true);
                winLoseAnimation.Find("Loot").Find("Loot").GetComponent<Image>()
                    .sprite = loot[num - 1];

                LeaderboardManager.instance.AddScore(30);
            }
            else if (global.winLoseCondition == 2)
            {
                condition = "Lose";
                MainMenu.userPlayer.WinLoseBattle(global.star, isWin: false);
            }
            else
            {
                condition = "Lose";
                MainMenu.userPlayer.WinLoseBattle(global.star, isWin: false);
                winLoseAnimation.Find("ImageLose").Find("Text").GetComponent<Text>()
                    .text = "DRAW";
            }
					for (int i = 0; i < global.star; i++)
					{
						winLoseAnimation.Find("Star").Find(string.Empty + (i + 1)).Find("star")
							.gameObject.SetActive(value: true);
						}
						Run.After(0.5f, delegate
						{
							base.transform.Find("Canvas").gameObject.SetActive(value: false);
						});
						Run.After(1.5f, delegate
						{
							MonoBehaviour.print("You" + condition);
							if (global.winLoseCondition == 1 && global.userPlayer.rank >= 30 && !MainMenu.userPlayer.rate)
							{
								if (!MainMenu.userPlayer.firstRate)
								{
									MainMenu.userPlayer.firstRate = true;
									PlayerPrefsX.SetBool("firstRate", value: true);
									MainMenu.isRate = true;
								}
								else if (UnityEngine.Random.Range(1, 11) <= 3)
								{
									MainMenu.isRate = true;
								}
							}
							if (global.winLoseCondition > 1)
							{
								winLoseAnimation.Find("Star").gameObject.SetActive(value: false);
							}
							base.transform.parent.Find("WinLose").gameObject.SetActive(value: true);
							base.transform.parent.Find("WinLose").GetComponent<Animator>().enabled = true;
							base.transform.parent.Find("WinLose").GetComponent<Animator>().Play(string.Empty + condition);
						});
					}
				}

				private void UpdateEnergyPlayer()
				{
					if (global.energyPlayerNow >= global.energyPlayerMax)
					{
						return;
					}
					if (global.energyTimePlayer >= global.energyTimePlayerMax)
					{
						global.energyTimePlayer -= global.energyTimePlayerMax;
						global.energyPlayerNow++;
						global.canvasEnergy.Find("Text").GetComponent<Text>().text = global.energyPlayerNow + "/" + global.energyPlayerMax;
					}
					else
					{
						float num = Time.deltaTime;
						if (global.isInjury)
						{
							num *= 2f;
						}
						global.energyTimePlayer += num;
					}
					if (global.energyPlayerNow >= global.energyPlayerMax)
					{
						global.energyTimePlayer = 0f;
					}
					float num2 = (float)(global.energyPlayerNow * 30) + global.energyTimePlayer / global.energyTimePlayerMax * 30f;
					global.rectBarEnergy.localPosition = new Vector3(-150f + num2 / 2f, 0f, 0f);
					global.rectBarEnergy.sizeDelta = new Vector2(num2, 15f);
					for (int i = 0; i < 4; i++)
					{
						if (global.energyPlayerNow < global.cardPlayer[i].energy)
						{
							if (!global.cooldownObj[i].activeInHierarchy)
							{
								global.cooldownObj[i].SetActive(value: true);
								global.cooldownObj[i].transform.parent.Find("Image").GetComponent<Image>().material = bwMaterial;
							}
							num2 = (float)(global.energyPlayerNow * 70 / global.cardPlayer[i].energy) + global.energyTimePlayer / global.energyTimePlayerMax * 70f / (float)global.cardPlayer[i].energy;
							global.cooldown[i].localPosition = new Vector3(0f, -1f * num2 / 2f, 0f);
							global.cooldown[i].sizeDelta = new Vector2(70f, 70f - num2);
						}
						else if (global.cooldownObj[i].activeInHierarchy)
						{
							global.cooldownObj[i].SetActive(value: false);
							global.cooldownObj[i].transform.parent.Find("Image").GetComponent<Image>().material = null;
						}
					}
				}

				private void UpdateEnergyEnemy()
				{
					if (global.energyEnemyNow < global.energyEnemyMax)
					{
						if (global.energyTimeEnemy >= global.energyTimeEnemyMax)
						{
							global.energyTimeEnemy -= global.energyTimeEnemyMax;
							global.energyEnemyNow++;
						}
						else
						{
							float num = Time.deltaTime;
							if (global.isInjury)
							{
								num *= 2f;
							}
							global.energyTimeEnemy += num;
						}
					}
					if (global.isAnimate)
					{
						if (Vector2.Distance(global.buttonAnimate.localPosition, global.cardAnimateTarget[0].localPosition) >= 0.1f)
						{
							global.buttonAnimate.localPosition = Vector2.MoveTowards(global.buttonAnimate.localPosition, global.cardAnimateTarget[0].localPosition, 1000f * Time.deltaTime);
							return;
						}
						global.buttonAnimate.localPosition = global.cardAnimateTarget[0].localPosition;
						global.buttonAnimate.gameObject.SetActive(value: false);
						global.cardAnimateTarget[0].gameObject.SetActive(value: true);
						global.cardAnimate.RemoveAt(0);
						global.cardAnimateTarget.RemoveAt(0);
						global.isAnimate = false;
					}
					else if (global.cardAnimate.Count > 0)
					{
						if (global.buttonAnimate == null)
						{
							global.buttonAnimate = global.panelSpawn.Find("ButtonCharacterAnimate");
						}
						global.buttonAnimate.Find("Button").Find("Image").GetComponent<Image>()
							.sprite = icon[global.cardAnimate[0].id - 1];
							global.buttonAnimate.localPosition = global.panelSpawn.Find("ButtonCharacterNext").localPosition;
							global.buttonAnimate.gameObject.SetActive(value: true);
							global.isAnimate = true;
						}
					}

					private void UsingCard(GlobalCard card, GlobalCard[] cards)
					{
						bool flag = false;
						GlobalCard globalCard = null;
						for (int i = 0; i < cards.Length; i++)
						{
							if (!flag)
							{
								if (cards[i] == card)
								{
									flag = true;
									globalCard = card;
									cards[i] = cards[4];
									i = 3;
								}
							}
							else if (i == cards.Length - 1)
							{
								cards[i] = globalCard;
							}
							else
							{
								cards[i] = cards[i + 1];
							}
						}
					}

					private void RefreshAllCard()
					{
						RefreshNowCard();
						RefreshNextCard();
					}

					private void RefreshNowCard()
					{
						for (int i = 0; i < 4; i++)
						{
							Transform transform = global.panelSpawn.Find("ButtonCharacterNow" + (i + 1));
							transform.Find("TextEnergy").GetComponent<Text>().text = string.Empty + global.cardPlayer[i].energy;
							transform.Find("Button").Find("Image").GetComponent<Image>()
								.sprite = icon[global.cardPlayer[i].id - 1];
							}
						}

						private void RefreshNextCard()
						{
							global.panelSpawn.Find("TextEnergy").GetComponent<Text>().text = string.Empty + global.cardPlayer[4].energy;
							global.panelSpawn.Find("ButtonCharacterNext").Find("Image").GetComponent<Image>()
								.sprite = icon[global.cardPlayer[4].id - 1];
							}

							private void RefreshAnimateCard(GlobalCard card, Transform target)
							{
								global.cardAnimate.Add(card);
								global.cardAnimateTarget.Add(target);
							}

							public static void Shuffle<T>(IList<T> list)
							{
								int num = list.Count;
								while (num > 1)
								{
									num--;
									int index = UnityEngine.Random.Range(0, num + 1);
									T value = list[index];
									list[index] = list[num];
									list[num] = value;
								}
							}
						}
