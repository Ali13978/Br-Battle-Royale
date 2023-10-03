using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.Services.Core;
using Unity.Services.Authentication;
using static AliScripts.AliExtras;
using System;

public class LoginManager : MonoBehaviour
{
    public static string EntityId;
    string playerId;

    AndroidGoogleSignIn googleSignInScript;

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
        googleSignInScript = AndroidGoogleSignIn.Init(this.gameObject);
    }
    public void ContinueWithGoogle(Action succ)
    {
        googleSignInScript.SignIn("1019811479668-bj69025autfkqlpji5patu7knt1kijsk.apps.googleusercontent.com",
            async (Acc) => {
                succ?.Invoke();

                Debug.Log("SignedIn with id: " + Acc.Id);

                await AuthenticationService.Instance.SignInWithGoogleAsync(Acc.Token);

                playerId = AuthenticationService.Instance.PlayerId;

                LoginWithCustomID(false);
            },
            (error) => {
                Debug.Log("Failed to login error: " + error);
            });
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
