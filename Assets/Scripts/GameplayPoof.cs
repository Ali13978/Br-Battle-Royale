using UnityEngine;

public class GameplayPoof : MonoBehaviour
{
	public void End()
	{
		Gameplay.gameplay.global.gameplayGraveyard.AddPoof(base.transform);
	}
}
