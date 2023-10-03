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
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms,async (result) =>
        {
            if (FB.IsLoggedIn)
            {
                // AccessToken class will have session details
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                Debug.Log(aToken.UserId);

                await AuthenticationService.Instance.SignInWithFacebookAsync(result.AccessToken.TokenString);

                playerId = AuthenticationService.Instance.PlayerId;

                LoginWithCustomID(false);
                // Print current access token's granted permissions
                foreach (string perm in aToken.Permissions)
                {
                    Debug.Log(perm);
                }
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
        googleSignInScript.SignIn("1019811479668-bj69025autfkqlpji5patu7knt1kijsk.apps.googleusercontent.com",
            async (Acc) => {
                Debug.Log("SignedIn with id: " + Acc.Id);

                await AuthenticationService.Instance.SignInWithGoogleAsync(Acc.Token);

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
