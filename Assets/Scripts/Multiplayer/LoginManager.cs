using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.Services.Core;
using Unity.Services.Authentication;
using static AliScripts.AliExtras;
using System;
using Facebook.Unity;
using System.Threading;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour
{
    public static string EntityId;
    public string playerName;
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
        SplashScreen.Instance.TurnOffAllPannels();
        if (!CheckInternetConnection())
        {
            SplashScreen.Instance.internetConnectionErrorPanel.SetActive(true);
            return;
        }
        SplashScreen.Instance.loginPannel.SetActive(true);
        await UnityServices.InitializeAsync();
        googleSignInScript = AndroidGoogleSignIn.Init(this.gameObject);
        
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void ContinueWithFacebook(Action pressed, Action Failed)
    {
        pressed?.Invoke();

        List<string> perms = new List<string>();

        FB.LogInWithReadPermissions(perms,async result =>
        {
            if (FB.IsLoggedIn)
            {

                Debug.Log("Token String: " + result.AccessToken.TokenString);
                await AuthenticationService.Instance.SignInWithFacebookAsync(result.AccessToken.TokenString);

                playerName = AuthenticationService.Instance.PlayerName;
                playerId = AuthenticationService.Instance.PlayerId;

                LoginWithCustomID(false);
            }
            else
            {
                Failed?.Invoke();
                Debug.Log("User cancelled login");
            }

        });
    }
    public void ContinueWithGoogle(Action pressed, Action Failed)
    {
        pressed?.Invoke();
        googleSignInScript.SignIn("183878201568-k3sea653vehsamln30hg708oirrrcglh.apps.googleusercontent.com",
            async (Acc) => {
                Debug.Log("SignedIn with id: " + Acc.Id);

                await AuthenticationService.Instance.SignInWithGoogleAsync(Acc.Token);

                playerName = AuthenticationService.Instance.PlayerName;
                playerId = AuthenticationService.Instance.PlayerId;

                LoginWithCustomID(false);
            },
            (error) => {
                Failed?.Invoke();
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

    private void OnLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        Debug.Log("Login successful! Player ID: " + result.PlayFabId);

        EntityId = result.EntityToken.Entity.Id;

        //FbAdsManager.instance.LoadInterstitial();

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

    public async void UpdatePlayerName(string name)
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(name);
        playerName = name;
    }
}
