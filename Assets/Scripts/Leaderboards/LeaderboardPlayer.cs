using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardPlayer : MonoBehaviour
{
    [SerializeField] TMP_Text RankText;
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text ScoreText;

    public void UpdateUI(string _rank, string _name, string _score)
    {
        RankText.text = _rank;
        NameText.text = _name;
        ScoreText.text = _score;
    }
}
