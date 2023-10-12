using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AliScripts.AliExtras;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject leadeboardPannel;
    [SerializeField] private Button leaderboardOpenBtn;
    [SerializeField] private Button leaderboardCloseBtn;
    [SerializeField] private GameObject playersHolder;
    [SerializeField] private GameObject leaderboardPlayerPrefab;
    [SerializeField] private GameObject leaderboardPlayerGameObject;
    
    void Start()
    {
        leaderboardOpenBtn.onClick.AddListener(async () => {

            FbAdsManager.instance.LoadInterstitial();
            leadeboardPannel.SetActive(true);

            List<Leaderboardentity> entries = await LeaderboardManager.instance.GetEntries();
            for (int i = 0; i < entries.Count; i++)
            {
                GameObject entry = Instantiate(leaderboardPlayerPrefab, playersHolder.transform);
                string name = entries[i].playerName.Substring(0, entries[i].playerName.Length - 5);
                entry.GetComponent<LeaderboardPlayer>().UpdateUI((entries[i].rank + 1).ToString(), name, entries[i].score.ToString());
            }

            Leaderboardentity playerEntry = await LeaderboardManager.instance.GetPlayerScore();

            if (playerEntry != null)
            {
                string name = playerEntry.playerName.Substring(0, playerEntry.playerName.Length - 5);
                leaderboardPlayerGameObject.GetComponent<LeaderboardPlayer>().UpdateUI((playerEntry.rank + 1).ToString(), name, playerEntry.score.ToString());
            }
            else
            {
                leaderboardPlayerGameObject.GetComponent<LeaderboardPlayer>().UpdateUI("??", "????", "??");
            }
        });

        leaderboardCloseBtn.onClick.AddListener(() => {
            leadeboardPannel.SetActive(false);
            ResetLeaderboardPannel();
        });
    }

    private void ResetLeaderboardPannel()
    {
        DestroyChildren(playersHolder);
        leaderboardPlayerGameObject.GetComponent<LeaderboardPlayer>().UpdateUI("??", "????", "??");
    }
}
