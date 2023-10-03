using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class SplashScreen : MonoBehaviour
{

	private RectTransform bar;

	private float timeNow;

	private float timeMax = 0.5f;

	private int barNow;

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
    [SerializeField] GameObject loginPannel;
    [SerializeField] Button continueWithGoogleBtn;
    [SerializeField] Button continueWithFacebookBtn;

    private void Start()
	{
		bar = base.transform.Find("Canvas").Find("PanelBottom").Find("PanelEnergy")
			.Find("Energy")
			.Find("Bar")
			.GetComponent<RectTransform>();
		bar.transform.parent.Find("Text").GetComponent<Text>().text = "- Now Loading -";

        quitBtn.onClick.AddListener(() => {
            Application.Quit();
        });

		loginPannel.SetActive(true);

		continueWithGoogleBtn.onClick.AddListener(() =>
		{
			LoginManager.Instance.ContinueWithGoogle(() =>
			{
				loginPannel.SetActive(false);
			},
			() =>
			{
				loginPannel.SetActive(true);
			});
		});

		continueWithFacebookBtn.onClick.AddListener(() =>
		{
			LoginManager.Instance.ContinueWithFacebook(() =>
            {
                Time.timeScale = 1f;
                loginPannel.SetActive(false);
			}, () =>
			{
				Time.timeScale = 1f;
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
        });
	}

	private void Update()
	{
		if (!isStart)
		{
			return;
		}
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
		float num = (float)(barNow * 30) + timeNow / timeMax * 30f;
		bar.localPosition = new Vector3(-150f + num / 2f, 0f, 0f);
		bar.sizeDelta = new Vector2(num, 15f);
	}

	private IEnumerator BarFull()
	{
		UnityEngine.Debug.Log("START~");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadSceneAsync("Main Menu");
	}
}
