using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class SplashScreen : MonoBehaviour
{
    
	public bool isStart;

    #region Singleton
    public static SplashScreen Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [Header("InternetConnection-Pannel")]
    [SerializeField] public GameObject internetConnectionErrorPanel;
    [SerializeField] Button quitBtn;

    [Header("Setname-Pannel")]
    [SerializeField] public GameObject setNamePanel;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] Button setNameDoneBtn;

    [Header("Login-panel")]
    [SerializeField] public GameObject loginPannel;
    [SerializeField] Button continueWithGoogleBtn;
    [SerializeField] Button continueWithFacebookBtn;

    [Header("Loading-Pannel")]
    [SerializeField] GameObject loadingPannel;
    [SerializeField] List<Image> loadingBarImages;
    [SerializeField] Sprite yellowBarSprite;
    [SerializeField] TMP_Text percentageText;
    private float timeNow;
    private float timeMax = 0.5f;
    private int barNow;

    private void Start()
    {
        try
        {
            IronSourceAdsManager.instance.ShowBanner(IronSourceBannerPosition.TOP);
        }
        catch
        {
            Debug.Log("Unexpected Error while showing Ads");
        }

        quitBtn.onClick.AddListener(() => {
            Application.Quit();
        });
        
		continueWithGoogleBtn.onClick.AddListener(() =>
		{
			LoginManager.Instance.ContinueWithGoogle(() =>
			{
                TurnOffAllPannels();
                loadingPannel.SetActive(true);
			},
			() =>
			{
                TurnOffAllPannels();
                loginPannel.SetActive(true);
			});
		});

		continueWithFacebookBtn.onClick.AddListener(() =>
		{
			LoginManager.Instance.ContinueWithFacebook(() =>
            {
                Time.timeScale = 1f;
                TurnOffAllPannels();
                loadingPannel.SetActive(true);
			}, () =>
			{
				Time.timeScale = 1f;
                TurnOffAllPannels();
				loginPannel.SetActive(true);
			});
		});

        setNameDoneBtn.onClick.AddListener(() => {
            string _playerName = nameInputField.text;

            if (string.IsNullOrEmpty(_playerName))
                return;

            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = _playerName
            },
            (result)=> {
                setNamePanel.SetActive(false);
                isStart = true;
            },
            (result) => {
                Debug.LogError(result.ErrorMessage);
            });

            LoginManager.Instance.UpdatePlayerName(_playerName);
        });

        nameInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    public void TurnOffAllPannels()
    {
        loadingPannel.SetActive(false);
        loginPannel.SetActive(false);
    }

	private void Update()
    {
        if (!isStart)
        {
            return;
        }

        FillLoadingBar();
	}

    private void FillLoadingBar()
    {
        if (timeNow >= timeMax)
        {
            timeNow -= timeMax;
            barNow++;
            if (barNow == 1)
            {
                base.transform.parent.Find("Sound").GetComponent<GlobalSound>().AudioOnce("Sound/GetItem", 0.31f);
            }
        }
        else
        {
            timeNow += Time.deltaTime;
        }
        if (barNow >= 10)
        {
            timeNow = 0f;
            isStart = false;
            StartCoroutine(BarFull());
        }

        //Calculations for Percentage and bar filling
        float num = (float)(barNow * 30) + timeNow / timeMax * 30f;
        int BarNum = (int)( (num / 300.0f) * 14f);
        int Percentage =(int) ((num / 300.0f) * 100f);
        if ((BarNum > 13))
            BarNum = 13;
        loadingBarImages[BarNum].sprite = yellowBarSprite;
        percentageText.text = (Percentage).ToString() + "%";
    }

	private IEnumerator BarFull()
	{
		UnityEngine.Debug.Log("START~");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadSceneAsync("Main Menu");
	}

    private void OnInputFieldValueChanged(string text)
    {
        // Remove spaces from the input text
        string textWithoutSpaces = text.Replace(" ", "");
        nameInputField.text = textWithoutSpaces; // Update the text in the input field
    }
}
