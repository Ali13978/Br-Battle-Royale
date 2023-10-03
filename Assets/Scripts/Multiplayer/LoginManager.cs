using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.Services.Core;
using Unity.Services.Authentication;
using static AliScripts.AliExtras;

public class LoginManager : MonoBehaviour
{
    public static string EntityId;
    string playerId;
    
    #region Singleton
    public static LoginManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!CheckInternetConnection())
        {
            SplashScreen.Instance.internetConnectionErrorPanel.SetActive(true);
            return;
        }
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerId = AuthenticationService.Instance.PlayerId;
        
        LoginWithCustomID(false);
    }

    public void LoginWithCustomID(bool CreateAcc)
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = playerId,
            CreateAccount = CreateAcc
        }, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful! Player ID: " + result.PlayFabId);

        EntityId = result.EntityToken.Entity.Id;

        if (result.NewlyCreated)
            SplashScreen.Instance.setNamePanel.SetActive(true);
        else
            SplashScreen.Instance.isStart = true;
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed. Error: " + error.ErrorMessage);

        if (error.ErrorMessage == "User not found")
            LoginWithCustomID(true);
        
    }
}
