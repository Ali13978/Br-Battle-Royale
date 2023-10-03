using UnityEngine;

public class GameplaySpace : MonoBehaviour
{
	private Gameplay gameplay;

	private void Start()
	{
		if (gameplay == null)
		{
			gameplay = Gameplay.gameplay;
		}
	}

	private void OnMouseDown()
	{
		if (!Gameplay.isSpace)
		{
			Gameplay.isSpace = true;
			gameplay.global.spaceNow = base.transform;
			gameplay.SpawnCharacterV1(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), isPlayer: true);
		}
	}

	private void OnMouseUp()
	{
		if (!Gameplay.isSpace)
		{
			return;
		}
		Gameplay.isSpace = false;
		Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
		if (vector.z >= -5f)
		{
			if (gameplay.SpawnCharacterV2(isPlayer: true))
			{
				if (Tutorial.tutorialNow == 28)
				{
					Tutorial.instance.Next();
					Tutorial.instance.Show();
				}
				gameplay.ButtonCharacterNowClicked(gameplay.global.idCardPlayer, isSpawn: true);
			}
		}
		else
		{
			gameplay.SpawnCharacterV3(isPlayer: true);
			gameplay.ButtonCharacterNowClicked(gameplay.global.idCardPlayer, isSpawn: false);
		}
	}

	private void OnMouseEnter()
	{
		if (Gameplay.isDrag && !Gameplay.isSpace)
		{
			Gameplay.isSpace = true;
			gameplay.global.spaceNow = base.transform;
			gameplay.SpawnCharacterV1(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), isPlayer: true);
		}
		if (Gameplay.isSpace)
		{
			gameplay.global.spaceNow = base.transform;
		}
	}
}
