using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAI : MonoBehaviour
{
	private Gameplay gameplay;

	private bool isThink = true;

	private bool isAI;

	private float time;

	private float timeThink = 2f;

	private void Start()
	{
		if (PlayerPrefsX.GetBool("tutorial"))
		{
			isAI = true;
		}
	}

	private void Update()
	{
		if (isThink && isAI)
		{
			if (time >= timeThink)
			{
				isThink = false;
				time = 0f;
				StartCoroutine(ThinkCoroutine());
			}
			else
			{
				time += Time.deltaTime;
			}
		}
	}

	private IEnumerator ThinkCoroutine()
	{
		yield return new WaitForEndOfFrame();
		if (gameplay == null)
		{
			gameplay = Gameplay.gameplay;
		}
		List<GameplayCharacter> gc = gameplay.global.playerList;
		int left = 0;
		int right = 0;
		int mid = 0;
		for (int k = 0; k < gc.Count; k++)
		{
			if (gc[k] == null)
			{
				continue;
			}
			Vector3 pos = gc[k].GetLocalPosition();
			if (!(pos.y < 0.7f))
			{
				if (pos.y >= 2.5f)
				{
					mid++;
				}
				else if (pos.x < 0f)
				{
					left++;
				}
				else
				{
					right++;
				}
			}
		}
		List<int> idCardList = new List<int>
		{
			1,
			2,
			3,
			4
		};
		int spawnCount = UnityEngine.Random.Range(0, 4);
		for (int j = 0; j < spawnCount; j++)
		{
			int idCard = 0;
			int i = 0;
			while (idCardList.Count != 0)
			{
				int randomIndex = UnityEngine.Random.Range(0, idCardList.Count);
				idCard = idCardList[randomIndex];
				idCardList.RemoveAt(randomIndex);
				GlobalCard card = gameplay.global.cardEnemy[idCard - 1];
				if (gameplay.global.energyEnemyNow >= card.energy)
				{
					break;
				}
				i++;
			}
			bool isLoop = true;
			if (idCard != 0)
			{
				if (left > 0 || right > 0 || mid > 0)
				{
					Vector3 spawnPos = new Vector3(0f, 3.35f, 0f);
					if (left > right && left > mid)
					{
						spawnPos = new Vector3(-1.5f + UnityEngine.Random.Range(-0.65f, 0.65f), 1.65f + UnityEngine.Random.Range(-0.65f, 0.65f), 0f);
					}
					else if (right > left && right > mid)
					{
						spawnPos = new Vector3(1.5f + UnityEngine.Random.Range(-0.65f, 0.65f), 1.65f + UnityEngine.Random.Range(-0.65f, 0.65f), 0f);
					}
					gameplay.global.idCardEnemy = idCard;
					gameplay.SpawnCharacterV1(spawnPos, isPlayer: false);
					CharacterEnemyZPositioning();
					gameplay.SpawnCharacterV2(isPlayer: false);
				}
				else if (UnityEngine.Random.Range(1, 11) <= 2)
				{
					Vector3 spawnPos2 = new Vector3(-1.5f + UnityEngine.Random.Range(-0.65f, 0.65f), 1.65f + UnityEngine.Random.Range(-0.65f, 0.65f), 0f);
					if (UnityEngine.Random.Range(1, 3) == 1)
					{
						spawnPos2 = new Vector3(1.5f + UnityEngine.Random.Range(-0.65f, 0.65f), 1.65f + UnityEngine.Random.Range(-0.65f, 0.65f), 0f);
					}
					gameplay.global.idCardEnemy = idCard;
					gameplay.SpawnCharacterV1(spawnPos2, isPlayer: false);
					CharacterEnemyZPositioning();
					gameplay.SpawnCharacterV2(isPlayer: false);
				}
				Run.After(0.1f, delegate
				{
					isLoop = false;
				});
			}
			else
			{
				isLoop = false;
			}
			while (isLoop)
			{
				yield return new WaitForEndOfFrame();
			}
			if (idCardList.Count == 0)
			{
				break;
			}
		}
		isThink = true;
		timeThink = UnityEngine.Random.Range(1f, 3.1f);
	}

	private void CharacterEnemyZPositioning()
	{
		Transform transform = gameplay.global.characterEnemyNow.Find("CharacterSprite");
		Transform transform2 = transform;
		Vector3 localPosition = transform.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = gameplay.global.characterEnemyNow.localPosition;
		transform2.localPosition = new Vector2(x, 1f - localPosition2.y / 100f);
	}
}
