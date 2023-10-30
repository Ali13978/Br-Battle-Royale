using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPrepare : MonoBehaviour
{
	[SerializeField] private TextAsset jsonFile;
    [SerializeField] List<string> namesOfPlayersAway;
	[SerializeField] private Text playerHomeName;
	[SerializeField] private Text playerAwayName;

    private void Awake()
    {
        if (jsonFile != null)
        {
            string jsonString = jsonFile.text;
            PlayerNamesData data = JsonUtility.FromJson<PlayerNamesData>(jsonString);
            namesOfPlayersAway = new List<string>(data.playerNames);

        }
        else
        {
            Debug.LogError("JSON file is not assigned.");
        }
    }

    public void End()
	{
		base.transform.parent.Find("Gameplay").GetComponent<Gameplay>().StartGameplay();
		base.gameObject.SetActive(value: false);
	}

	public void Sound(string sound)
	{
		float volume = 0.81f;
		if (sound == "Slash3")
		{
			volume = 0.71f;
		}
		base.transform.root.Find("Sound").GetComponent<GlobalSound>().AudioOnce("Sound/" + sound, volume);
	}

    public void UpdateNames()
    {
		string awayTeamPlayerName = SelectRandomElement<string>(namesOfPlayersAway);
		playerAwayName.text = awayTeamPlayerName;
		playerHomeName.text = PlayerPrefs.GetString("PlayerName");
    }
    public T SelectRandomElement<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("The list is empty or null.");
        }

        System.Random random = new System.Random();
        int randomIndex = random.Next(0, list.Count);

        return list[randomIndex];
    }
}

[Serializable]
public class PlayerNamesData
{
    public List<string> playerNames;
}