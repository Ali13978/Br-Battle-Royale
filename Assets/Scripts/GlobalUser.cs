using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUser
{
	public GlobalCard[] card = new GlobalCard[8];

	public int[] cardCount = new int[21];

	public int[] cardLevel = new int[21];

	public int[] cardFormation = new int[8];

	public int[] loot = new int[3];

	public int[] lootFrom = new int[3];

	public int[] lootMode = new int[3];

	public int[] gift = new int[6];

	public int[] giftFrom = new int[6];

	public int[] giftMode = new int[6];

	public int[,] cardUpgrade = new int[4, 13]
	{
		{
			1,
			2,
			4,
			10,
			20,
			50,
			100,
			200,
			400,
			800,
			1000,
			2000,
			5000
		},
		{
			1,
			2,
			4,
			10,
			20,
			50,
			100,
			200,
			400,
			800,
			1000,
			-1,
			-1
		},
		{
			1,
			2,
			4,
			10,
			20,
			50,
			100,
			200,
			-1,
			-1,
			-1,
			-1,
			-1
		},
		{
			1,
			2,
			4,
			10,
			20,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1
		}
	};

	public int[,] cardGold = new int[4, 13]
	{
		{
			0,
			5,
			20,
			50,
			150,
			400,
			1000,
			2000,
			4000,
			8000,
			20000,
			50000,
			100000
		},
		{
			0,
			50,
			150,
			400,
			1000,
			2000,
			4000,
			8000,
			20000,
			50000,
			100000,
			-1,
			-1
		},
		{
			0,
			400,
			2000,
			4000,
			8000,
			20000,
			50000,
			100000,
			-1,
			-1,
			-1,
			-1,
			-1
		},
		{
			0,
			5000,
			20000,
			50000,
			100000,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1
		}
	};

	public int[,] cardExp = new int[4, 13]
	{
		{
			0,
			4,
			5,
			6,
			10,
			25,
			50,
			100,
			200,
			400,
			600,
			800,
			1600
		},
		{
			0,
			6,
			10,
			25,
			50,
			100,
			200,
			400,
			600,
			800,
			1600,
			-1,
			-1
		},
		{
			0,
			25,
			100,
			200,
			400,
			600,
			800,
			1600,
			-1,
			-1,
			-1,
			-1,
			-1
		},
		{
			0,
			250,
			600,
			800,
			1600,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1
		}
	};

	public DateTime[] lootTime = new DateTime[3]
	{
		DateTime.Now,
		DateTime.Now,
		DateTime.Now
	};

	public DateTime[] giftTime = new DateTime[6]
	{
		DateTime.Now,
		DateTime.Now,
		DateTime.Now,
		DateTime.Now,
		DateTime.Now,
		DateTime.Now
	};

	public bool rate;

	public bool firstRate;

	public int level;

	public int rank;

	public int exp;

	public int gold;

	public int key;

	public DateTime timeKey;

	public int[] maxExp = new int[12]
	{
		20,
		40,
		80,
		160,
		320,
		540,
		780,
		860,
		910,
		1020,
		1150,
		1300
	};

	private int[] gachaCommon1 = new int[4]
	{
		1,
		2,
		3,
		4
	};

	private int[] gachaCommon2 = new int[7]
	{
		1,
		2,
		3,
		4,
		13,
		14,
		16
	};

	private int[] gachaCommon3 = new int[7]
	{
		1,
		2,
		3,
		4,
		13,
		14,
		16
	};

	private int[] gachaRare1 = new int[4]
	{
		5,
		6,
		7,
		8
	};

	private int[] gachaRare2 = new int[6]
	{
		5,
		6,
		7,
		8,
		15,
		17
	};

	private int[] gachaRare3 = new int[6]
	{
		5,
		6,
		7,
		8,
		15,
		17
	};

	private int[] gachaEpic1 = new int[4]
	{
		9,
		10,
		11,
		12
	};

	private int[] gachaEpic2 = new int[6]
	{
		9,
		10,
		11,
		12,
		18,
		19
	};

	private int[] gachaEpic3 = new int[6]
	{
		9,
		10,
		11,
		12,
		18,
		19
	};

	private int[] gachaLegendary1 = new int[0];

	private int[] gachaLegendary2 = new int[0];

	private int[] gachaLegendary3 = new int[2]
	{
		20,
		21
	};

	public GlobalUser(bool isPlayer)
	{
		if (isPlayer)
		{
			IsDatabaseExist();
			RefreshCard();
		}
	}

	public GlobalUser()
	{
	}

	public float RefreshCard()
	{
		float num = 0f;
		for (int i = 0; i < cardFormation.Length; i++)
		{
			int num2 = cardFormation[i];
			if (num2 != 0)
			{
				card[i] = new GlobalCard(num2, cardLevel[num2 - 1]);
				num += card[i].GetPower();
			}
		}
		return num;
	}

	public float RefreshCard(GlobalCard c)
	{
		float num = 0f;
		for (int i = 0; i < cardFormation.Length; i++)
		{
			if (c.id == cardFormation[i])
			{
				card[i] = new GlobalCard(c.id, cardLevel[c.id - 1]);
			}
			if (card[i] != null)
			{
				num += card[i].GetPower();
			}
		}
		return num;
	}

	public float RefreshPower()
	{
		float num = 0f;
		for (int i = 0; i < cardFormation.Length; i++)
		{
			if (card[i] != null)
			{
				num += card[i].GetPower();
			}
		}
		return num;
	}

	public int GetMaxExp()
	{
		if (level < maxExp.Length)
		{
			return maxExp[level - 1];
		}
		return 0;
	}

	public int GetMaxKey()
	{
		return 5;
	}

	public int WinLoseBattle(int star, bool isWin)
	{
		return RewardCalculate(star, isWin);
	}

	private int RewardCalculate(int star, bool isWin)
	{
		int num = 1;
		if (rank >= 1500)
		{
			num = 3;
		}
		else if (rank >= 800)
		{
			num = 2;
		}
		if (isWin)
		{
			rank += 30;
		}
		else
		{
			rank = -20;
		}
		if (rank <= 0)
		{
			rank = 0;
		}
		exp += 5 * star;
		if (isWin)
		{
			exp += num * 5;
		}
		while (level < maxExp.Length && exp >= maxExp[level - 1])
		{
			exp -= maxExp[level - 1];
			level++;
			PlayerPrefsX.SetBool("levelup", value: true);
		}
		gold += 3 * star;
		if (isWin)
		{
			gold += num * 6;
		}
		PlayerPrefs.SetInt("level", level);
		PlayerPrefs.SetInt("rank", rank);
		PlayerPrefs.SetInt("exp", exp);
		PlayerPrefs.SetInt("gold", gold);
		if (isWin)
		{
			int num2 = 0;
			int num3 = UnityEngine.Random.Range(1, 101);
			num2 = ((num3 <= 70) ? 1 : ((num3 > 90) ? 3 : 2));
			for (int i = 0; i < loot.Length; i++)
			{
				if (loot[i] == 0)
				{
					loot[i] = num2;
					lootFrom[i] = num;
					lootMode[i] = 0;
					lootTime[i] = DateTime.Now;
					PlayerPrefsX.SetIntArray("loot", loot);
					PlayerPrefsX.SetIntArray("lootFrom", lootFrom);
					PlayerPrefsX.SetIntArray("lootMode", lootMode);
					string[] stringArray = new string[3]
					{
						lootTime[0].ToString(),
						lootTime[1].ToString(),
						lootTime[2].ToString()
					};
					PlayerPrefsX.SetStringArray("lootTime", stringArray);
					break;
				}
			}
			return num2;
		}
		return 0;
	}

	public void DrawBattle()
	{
		int num = 1;
		if (rank >= 1500)
		{
			num = 3;
		}
		else if (rank >= 800)
		{
			num = 2;
		}
		rank -= 30;
		if (rank <= 0)
		{
			rank = 0;
		}
		exp += 10 + num * 10;
		exp += 100 + num * 100;
		while (level < maxExp.Length && exp >= maxExp[level - 1])
		{
			exp -= maxExp[level - 1];
			level++;
		}
		gold += 5 + (num - 1) * 5;
		PlayerPrefs.SetInt("level", level);
		PlayerPrefs.SetInt("rank", rank);
		PlayerPrefs.SetInt("exp", exp);
		PlayerPrefs.SetInt("gold", gold);
	}

	public bool ConsumeKey()
	{
		int @int = PlayerPrefs.GetInt("key");
		if (@int != 0)
		{
			if (@int == 5)
			{
				timeKey = DateTime.Now;
				PlayerPrefs.SetString("timeKey", timeKey.ToString());
			}
			key = @int - 1;
			PlayerPrefs.SetInt("key", key);
			return true;
		}
		return false;
	}

	public bool AddKey()
	{
		int @int = PlayerPrefs.GetInt("key");
		if (@int < 5)
		{
			key = @int + 1;
			PlayerPrefs.SetInt("key", key);
			PlayerPrefs.SetString("timeKey", DateTime.Now.ToString());
			return true;
		}
		return false;
	}

	public string[] OpenLootLevelUp()
	{
		List<string> list = new List<string>();
		list.Add("3");
		string[] array = Gacha(2, 1);
		for (int i = 0; i < array.Length; i += 2)
		{
			int num = int.Parse(array[i]);
			int num2 = int.Parse(array[i + 1]);
			cardCount[num - 1] += num2;
			if (cardLevel[num - 1] == 0)
			{
				cardCount[num - 1] -= cardUpgrade[(int)(new GlobalCard(num, cardLevel[num - 1]).rarity - 1), cardLevel[num - 1]];
				cardLevel[num - 1]++;
			}
			list.Add(string.Empty + num);
			list.Add(string.Empty + num2);
		}
		PlayerPrefsX.SetIntArray("cardLevel", cardLevel);
		PlayerPrefsX.SetIntArray("cardCount", cardCount);
		return list.ToArray();
	}

	public string[] OpenLoot(int number)
	{
		bool flag = false;
		for (int i = 0; i < loot.Length; i++)
		{
			bool flag2 = loot[i] != 0 && lootMode[i] == 1;
			bool flag3 = loot[i] != 0 && lootMode[i] == 2;
			if (i + 1 == number && flag2)
			{
				continue;
			}
			if (i + 1 == number && flag3)
			{
				flag = false;
				break;
			}
			if (flag2)
			{
				DateTime t = lootTime[i].AddMinutes(30.0);
				if (DateTime.Now < t)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			return new string[1]
			{
				"0"
			};
		}
		if (lootMode[number - 1] == 0)
		{
			lootMode[number - 1] = 1;
			PlayerPrefsX.SetIntArray("lootMode", lootMode);
			lootTime[number - 1] = DateTime.Now;
			string[] stringArray = new string[3]
			{
				lootTime[0].ToString(),
				lootTime[1].ToString(),
				lootTime[2].ToString()
			};
			PlayerPrefsX.SetStringArray("lootTime", stringArray);
			return new string[1]
			{
				"1"
			};
		}
		if (lootMode[number - 1] == 1)
		{
			lootMode[number - 1] = 2;
			PlayerPrefsX.SetIntArray("lootMode", lootMode);
			return new string[1]
			{
				"2"
			};
		}
		List<string> list = new List<string>();
		list.Add("3");
		string[] array = Gacha(loot[number - 1], lootFrom[number - 1]);
		for (int j = 0; j < array.Length; j += 2)
		{
			int num = int.Parse(array[j]);
			int num2 = int.Parse(array[j + 1]);
			cardCount[num - 1] += num2;
			if (cardLevel[num - 1] == 0)
			{
				cardCount[num - 1] -= cardUpgrade[(int)(new GlobalCard(num, cardLevel[num - 1]).rarity - 1), cardLevel[num - 1]];
				cardLevel[num - 1]++;
			}
			list.Add(string.Empty + num);
			list.Add(string.Empty + num2);
		}
		gold += 25;
		loot[number - 1] = 0;
		PlayerPrefs.SetInt("gold", gold);
		PlayerPrefsX.SetIntArray("loot", loot);
		PlayerPrefsX.SetIntArray("cardLevel", cardLevel);
		PlayerPrefsX.SetIntArray("cardCount", cardCount);
		return list.ToArray();
	}

	public string[] OpenGift(int number)
	{
		bool flag = false;
		for (int i = 0; i < gift.Length; i++)
		{
			bool flag2 = gift[i] != 0 && giftMode[i] == 1;
			bool flag3 = gift[i] != 0 && giftMode[i] == 2;
			if (i + 1 == number && flag2)
			{
				continue;
			}
			if (i + 1 == number && flag3)
			{
				flag = false;
				break;
			}
			if (flag2)
			{
				DateTime t = giftTime[i].AddMinutes(180.0);
				if (i == 5)
				{
					t = giftTime[i].AddMinutes(60.0);
				}
				if (DateTime.Now < t)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			return new string[1]
			{
				"0"
			};
		}
		if ((number <= 1 && giftMode[number - 1] == 0) || giftMode[number - 1] == 1)
		{
			giftMode[number - 1] = 2;
			PlayerPrefsX.SetIntArray("giftMode", giftMode);
			if (number > 1)
			{
				return new string[1]
				{
					"2"
				};
			}
			return new string[1]
			{
				"1"
			};
		}
		if (giftMode[number - 1] == 0)
		{
			giftMode[number - 1] = 1;
			PlayerPrefsX.SetIntArray("giftMode", giftMode);
			giftTime[number - 1] = DateTime.Now;
			string[] stringArray = new string[6]
			{
				giftTime[0].ToString(),
				giftTime[1].ToString(),
				giftTime[2].ToString(),
				giftTime[3].ToString(),
				giftTime[4].ToString(),
				giftTime[5].ToString()
			};
			PlayerPrefsX.SetStringArray("giftTime", stringArray);
			return new string[1]
			{
				"1"
			};
		}
		List<string> list = new List<string>();
		list.Add("3");
		string[] array = Gacha(gift[number - 1], giftFrom[number - 1]);
		for (int j = 0; j < array.Length; j += 2)
		{
			int num = int.Parse(array[j]);
			int num2 = int.Parse(array[j + 1]);
			cardCount[num - 1] += num2;
			if (cardLevel[num - 1] == 0)
			{
				cardCount[num - 1] -= cardUpgrade[(int)(new GlobalCard(num, cardLevel[num - 1]).rarity - 1), cardLevel[num - 1]];
				cardLevel[num - 1]++;
			}
			list.Add(string.Empty + num);
			list.Add(string.Empty + num2);
		}
		gold += 25;
		gift[number - 1] = 0;
		PlayerPrefs.SetInt("gold", gold);
		PlayerPrefsX.SetIntArray("gift", gift);
		PlayerPrefsX.SetIntArray("cardLevel", cardLevel);
		PlayerPrefsX.SetIntArray("cardCount", cardCount);
		return list.ToArray();
	}

	public string[] Gacha(int loot, int lootFrom)
	{
		List<string> list = new List<string>();
		string[] array = GachaCalculation(lootFrom, 100, 4, 0, 0, 0, 0, 0, 0);
		list.Add(array[0]);
		list.Add(array[1]);
		array = GachaCalculation(lootFrom, 60, 4, 95, 2, 100, 1, 0, 0);
		list.Add(array[0]);
		list.Add(array[1]);
		if (loot == 1)
		{
			return list.ToArray();
		}
		array = GachaCalculation(lootFrom, 60, 4, 95, 2, 100, 1, 0, 0);
		list.Add(array[0]);
		list.Add(array[1]);
		if (loot == 2)
		{
			return list.ToArray();
		}
		array = GachaCalculation(lootFrom, 30, 10, 70, 6, 100, 2, 0, 0);
		list.Add(array[0]);
		list.Add(array[1]);
		return list.ToArray();
	}

	public string[] GachaCalculation(int lootFrom, int pCommon, int vCommon, int pRare, int vRare, int pEpic, int vEpic, int pLegendary, int vLegendary)
	{
		int num = UnityEngine.Random.Range(1, 11);
		if (num <= 4)
		{
			num = 1;
		}
		else if (num <= 8)
		{
			num = 2;
		}
		else
		{
			num = 3;
		}
		int[] array = new int[0];
		int[] array2 = new int[0];
		int[] array3 = new int[0];
		int[] array4 = new int[0];
		switch (lootFrom)
		{
		case 1:
			array = gachaCommon1;
			array2 = gachaRare1;
			array3 = gachaEpic1;
			array4 = gachaLegendary1;
			break;
		case 2:
			array = gachaCommon2;
			array2 = gachaRare2;
			array3 = gachaEpic2;
			array4 = gachaLegendary2;
			break;
		case 3:
			array = gachaCommon3;
			array2 = gachaRare3;
			array3 = gachaEpic3;
			array4 = gachaLegendary3;
			break;
		}
		List<string> list = new List<string>();
		int num2 = UnityEngine.Random.Range(1, 101);
		if (num2 <= pCommon)
		{
			list.Add(string.Empty + array[UnityEngine.Random.Range(0, array.Length)]);
			list.Add(string.Empty + vCommon);
		}
		else if (num2 <= pRare)
		{
			list.Add(string.Empty + array2[UnityEngine.Random.Range(0, array2.Length)]);
			list.Add(string.Empty + vRare);
		}
		else if (num2 <= pEpic)
		{
			list.Add(string.Empty + array3[UnityEngine.Random.Range(0, array3.Length)]);
			list.Add(string.Empty + vEpic);
		}
		else
		{
			list.Add(string.Empty + array4[UnityEngine.Random.Range(0, array4.Length)]);
			list.Add(string.Empty + vLegendary);
		}
		return list.ToArray();
	}

	public void SetFormation(int idCharacter, int idFormation)
	{
		if (idFormation == 0)
		{
			for (int i = 0; i < cardFormation.Length; i++)
			{
				if (cardFormation[i] == idCharacter)
				{
					cardFormation[i] = 0;
					break;
				}
			}
		}
		else
		{
			cardFormation[idFormation - 1] = idCharacter;
		}
		PlayerPrefsX.SetIntArray("cardFormation", cardFormation);
		RefreshCard();
	}

	public bool UpgradeCard(int idCharacter)
	{
		GlobalCard globalCard = new GlobalCard(idCharacter, cardLevel[idCharacter - 1]);
		int num = cardCount[globalCard.id - 1];
		int num2 = cardUpgrade[(int)(globalCard.rarity - 1), globalCard.level];
		int num3 = cardGold[(int)(globalCard.rarity - 1), globalCard.level];
		if (num2 != -1 && num >= num2 && gold >= num3)
		{
			cardCount[globalCard.id - 1] -= num2;
			PlayerPrefsX.SetIntArray("cardCount", cardCount);
			cardLevel[globalCard.id - 1]++;
			PlayerPrefsX.SetIntArray("cardLevel", cardLevel);
			gold -= num3;
			PlayerPrefs.SetInt("gold", gold);
			RefreshCard();
			return true;
		}
		return false;
	}

	private void IsDatabaseExist()
	{
		if (!PlayerPrefsX.GetBool("database") || !PlayerPrefsX.GetBool("tutorial"))
		{
			CreateDatabase();
		}
		ReadDatabase();
	}

	private void ReadDatabase()
	{
		rate = PlayerPrefsX.GetBool("rate");
		firstRate = PlayerPrefsX.GetBool("firstRate");
		level = PlayerPrefs.GetInt("level");
		rank = PlayerPrefs.GetInt("rank");
		exp = PlayerPrefs.GetInt("exp");
		gold = PlayerPrefs.GetInt("gold");
		key = PlayerPrefs.GetInt("key");
		timeKey = DateTime.Parse(PlayerPrefs.GetString("timeKey"));
		while (level < maxExp.Length && exp >= maxExp[level - 1])
		{
			exp -= maxExp[level - 1];
			level++;
			PlayerPrefs.SetInt("level", level);
			PlayerPrefs.SetInt("exp", exp);
		}
		cardCount = PlayerPrefsX.GetIntArray("cardCount");
		cardLevel = PlayerPrefsX.GetIntArray("cardLevel");
		cardFormation = PlayerPrefsX.GetIntArray("cardFormation");
		loot = PlayerPrefsX.GetIntArray("loot");
		lootFrom = PlayerPrefsX.GetIntArray("lootFrom");
		lootMode = PlayerPrefsX.GetIntArray("lootMode");
		string[] stringArray = PlayerPrefsX.GetStringArray("lootTime");
		lootTime = new DateTime[3]
		{
			DateTime.Parse(stringArray[0]),
			DateTime.Parse(stringArray[1]),
			DateTime.Parse(stringArray[2])
		};
		string[] stringArray2 = PlayerPrefsX.GetStringArray("giftTime");
		if (DateTime.Parse(stringArray2[0]).DayOfYear != DateTime.Now.DayOfYear)
		{
			NewGift();
		}
		gift = PlayerPrefsX.GetIntArray("gift");
		giftFrom = PlayerPrefsX.GetIntArray("giftFrom");
		giftMode = PlayerPrefsX.GetIntArray("giftMode");
		stringArray2 = PlayerPrefsX.GetStringArray("giftTime");
		giftTime = new DateTime[6]
		{
			DateTime.Parse(stringArray2[0]),
			DateTime.Parse(stringArray2[1]),
			DateTime.Parse(stringArray2[2]),
			DateTime.Parse(stringArray2[3]),
			DateTime.Parse(stringArray2[4]),
			DateTime.Parse(stringArray2[5])
		};
	}

	private void CreateDatabase()
	{
		PlayerPrefsX.SetBool("database", value: true);
		PlayerPrefsX.SetBool("tutorial", value: false);
		PlayerPrefsX.SetBool("rate", value: false);
		PlayerPrefsX.SetBool("firstRate", value: false);
		PlayerPrefsX.SetBool("levelup", value: false);
		PlayerPrefs.SetInt("level", 1);
		PlayerPrefs.SetInt("rank", 0);
		PlayerPrefs.SetInt("exp", 0);
		PlayerPrefs.SetInt("gold", 0);
		PlayerPrefs.SetInt("key", 5);
		PlayerPrefs.SetString("timeKey", DateTime.Now.ToString());
		cardCount = new int[21];
		PlayerPrefsX.SetIntArray("cardCount", cardCount);
		int[] array = new int[21];
		array[3] = 1;
		cardLevel = array;
		PlayerPrefsX.SetIntArray("cardLevel", cardLevel);
		cardFormation = new int[8];
		PlayerPrefsX.SetIntArray("cardFormation", cardFormation);
		loot = new int[3];
		PlayerPrefsX.SetIntArray("loot", loot);
		lootFrom = new int[3];
		PlayerPrefsX.SetIntArray("lootFrom", lootFrom);
		lootMode = new int[3];
		PlayerPrefsX.SetIntArray("lootMode", lootMode);
		lootTime = new DateTime[3]
		{
			DateTime.Now,
			DateTime.Now,
			DateTime.Now
		};
		string[] stringArray = new string[3]
		{
			lootTime[0].ToString(),
			lootTime[1].ToString(),
			lootTime[2].ToString()
		};
		PlayerPrefsX.SetStringArray("lootTime", stringArray);
		NewGift();
	}

	private void NewGift()
	{
		gift = new int[6]
		{
			1,
			1,
			1,
			1,
			1,
			2
		};
		PlayerPrefsX.SetIntArray("gift", gift);
		giftFrom = new int[6]
		{
			1,
			1,
			1,
			1,
			1,
			1
		};
		PlayerPrefsX.SetIntArray("giftFrom", giftFrom);
		giftMode = new int[6];
		PlayerPrefsX.SetIntArray("giftMode", giftMode);
		giftTime = new DateTime[6]
		{
			DateTime.Now,
			DateTime.Now,
			DateTime.Now,
			DateTime.Now,
			DateTime.Now,
			DateTime.Now
		};
		string[] stringArray = new string[6]
		{
			giftTime[0].ToString(),
			giftTime[1].ToString(),
			giftTime[2].ToString(),
			giftTime[3].ToString(),
			giftTime[4].ToString(),
			giftTime[5].ToString()
		};
		PlayerPrefsX.SetStringArray("giftTime", stringArray);
	}
}
