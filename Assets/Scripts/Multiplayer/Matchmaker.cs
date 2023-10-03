using System.Collections;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerModels;
using UnityEngine.UI;

public class Matchmaker : MonoBehaviour
{
    private string ticketId;
    private Coroutine pollTicketCoroutine;

    private static string QueueName = "DefaultQueue";

    #region Singleton
    public static Matchmaker Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void StartMatchmaking()
    {
        MainMenu.Instance.matchmakingInfoText.text = "Submitting Ticket";
        MainMenu.Instance.matchmakingInfoText.gameObject.SetActive(true);

        PlayFabMultiplayerAPI.CreateMatchmakingTicket(
            new CreateMatchmakingTicketRequest
            {
                Creator = new MatchmakingPlayer
                {
                    Entity = new EntityKey
                    {
                        Id = LoginManager.EntityId,
                        Type = "title_player_account",
                    },
                    Attributes = new MatchmakingPlayerAttributes
                    {
                        DataObject = new { }
                    }
                },

                GiveUpAfterSeconds = 20,

                QueueName = QueueName
            },
            OnMatchmakingTicketCreated,
            OnMatchmakingError
        );
    }

    public void LeaveQueue()
    {
        //queueStatusText.gameObject.SetActive(false);

        PlayFabMultiplayerAPI.CancelMatchmakingTicket(
            new CancelMatchmakingTicketRequest
            {
                QueueName = QueueName,
                TicketId = ticketId
            },
            OnTicketCanceled,
            OnMatchmakingError
        );
    }

    private void OnTicketCanceled(CancelMatchmakingTicketResult result)
    {

    }

    private void OnMatchmakingTicketCreated(CreateMatchmakingTicketResult result)
    {
        ticketId = result.TicketId;
        pollTicketCoroutine = StartCoroutine(PollTicket(result.TicketId));

        MainMenu.Instance.matchmakingInfoText.text = "Ticket Created";
    }

    private void OnMatchmakingError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private IEnumerator PollTicket(string ticketId)
    {
        while (true)
        {
            PlayFabMultiplayerAPI.GetMatchmakingTicket(
                new GetMatchmakingTicketRequest
                {
                    TicketId = ticketId,
                    QueueName = QueueName
                },
                OnGetMatchMakingTicket,
                OnMatchmakingError
            );

            yield return new WaitForSeconds(6);
        }
    }

    private void OnGetMatchMakingTicket(GetMatchmakingTicketResult result)
    {
        MainMenu.Instance.matchmakingInfoText.text = $"Status: {result.Status}";

        switch (result.Status)
        {
            case "Matched":
                StopCoroutine(pollTicketCoroutine);
                StartMatch(result.MatchId);
                break;
            case "Canceled":
                StopCoroutine(pollTicketCoroutine);
                MainMenu.Instance.matchmakingInfoText.text = $"Match Found";
                StartCoroutine(MainMenu.Instance.RefreshMatchmaking());
                break;
        }
    }

    private void StartMatch(string matchId)
    {
        MainMenu.Instance.matchmakingInfoText.text = $"Match Found";

        PlayFabMultiplayerAPI.GetMatch(
            new GetMatchRequest
            {
                MatchId = matchId,
                QueueName = QueueName
            },
            OnGetMatch,
            OnMatchmakingError
        );
    }

    private void OnGetMatch(GetMatchResult result)
    {
        MainMenu.Instance.matchmakingInfoText.text = $"{result.Members[0].Entity.Id} vs {result.Members[1].Entity.Id}";

    }
}