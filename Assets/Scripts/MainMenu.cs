using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public delegate void OnFinishedEvent();

    public static GlobalUser userPlayer;

    public static GlobalUser userEnemy;

    public static bool isRate;

    public Sprite[] icon;

    private GlobalSound globalSound;

    private Transform canvas;

    private Transform panelTop;

    private Transform panelCenter;

    private Transform panelMenuIcon;

    private Transform panelFormationSet;

    private Transform panelContentFormationSet;

    private Transform panelAdventureFind;

    private Transform panelAdventureLoot;

    private Transform panelGiftLoot;

    private Transform panelLootUnlock;

    private Transform panelAlert;

    private Transform panelLoading;

    private Transform panelFindingMatch;

    private Text textLevel;

    private Text textRank;

    private Text textExp;

    private Text textGold;

    private Text textKey;

    private Text textTime;

    private Animator animatorFireworks;

    private DateTime timeKey;

    private DateTime endTimeKeyBasic;

    private DateTime[] timeLoot = new DateTime[3];

    private DateTime[] endTimeLootBasic = new DateTime[3];

    private DateTime[] timeGift = new DateTime[6];

    private DateTime[] endTimeGiftBasic = new DateTime[6];

    private int activeMenu = 2;

    private int activeStage = 1;

    private int lootAdsNumber;

    private int giftAdsNumber;

    private int adsVideoNumber;

    private float deltaTimeKey;

    private float deltaTimeLoot;

    private float deltaTimeGift;

    private bool isKeyStart;

    private bool isLootStart;

    private bool isGiftStart;

    private bool isButton;

    private GlobalCard selectedCard;

    private int[] cLevel = new int[21]
    {
        1,
        1,
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0
    };

    private int[] cFormation = new int[8]
    {
        1,
        2,
        3,
        4,
        0,
        0,
        0,
        0
    };

    public Text matchmakingInfoText;

    #region Singleton
    public static MainMenu Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        InitGameObject();
        RefreshMainMenu();
        RefreshKeyTime();
        if (PlayerPrefsX.GetBool("levelup"))
        {
            PlayerPrefsX.SetBool("levelup", value: false);
            LootLevelUpListener();
        }
        //FbAdsManager.instance.LoadBanner(AudienceNetwork.AdPosition.BOTTOM);
        matchmakingInfoText = panelFindingMatch.Find("Text").GetComponent<Text>();
    }

    private void OnApplicationFocus(bool b)
    {
        if (b)
        {
            RefreshKeyTime();
        }
        else
        {
            isKeyStart = false;
        }
    }

    private void Update()
    {
        if (isKeyStart)
        {
            UpdateKeyTime();
        }
        if (isLootStart)
        {
            UpdateLootTime();
        }
        if (isGiftStart)
        {
            UpdateGiftTime();
        }
    }

    private void UpdateKeyTime()
    {
        if (deltaTimeKey >= 1f)
        {
            deltaTimeKey -= 1f;
            timeKey = timeKey.AddSeconds(1.0);
            TimeSpan timeSpan = endTimeKeyBasic.Subtract(timeKey);
            string text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            if (!textTime.gameObject.activeInHierarchy)
            {
                textTime.gameObject.SetActive(value: true);
            }
            textTime.text = string.Empty + text.ToString();
            if (timeSpan.Hours <= 0 && timeSpan.Minutes <= 0 && timeSpan.Seconds <= 0)
            {
                userPlayer.AddKey();
                RefreshKeyTime();
                RefreshTop();
            }
        }
        deltaTimeKey += Time.deltaTime;
    }

    private void UpdateLootLevelUp()
    {
        if (deltaTimeLoot >= 1f)
        {
            deltaTimeLoot -= 1f;
            bool flag = false;
            for (int i = 0; i < userPlayer.loot.Length; i++)
            {
                if (userPlayer.loot[i] != 0 && userPlayer.lootMode[i] == 1)
                {
                    flag = true;
                    timeLoot[i] = timeLoot[i].AddSeconds(1.0);
                    TimeSpan timeSpan = endTimeLootBasic[i].Subtract(timeLoot[i]);
                    string text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                    Transform transform = panelAdventureLoot.Find("Loot" + (i + 1)).Find("TextTime");
                    if (!transform.gameObject.activeInHierarchy)
                    {
                        transform.gameObject.SetActive(value: true);
                    }
                    transform.GetComponent<Text>().text = string.Empty + text.ToString();
                    if (timeSpan.Hours <= 0 && timeSpan.Minutes <= 0 && timeSpan.Seconds <= 0)
                    {
                        flag = false;
                        panelLoading.gameObject.SetActive(value: true);
                        int number = i + 1;
                        Run.After(2f, delegate
                        {
                            userPlayer.OpenLoot(number);
                            RefreshLoot();
                            panelLoading.gameObject.SetActive(value: false);
                        });
                    }
                }
            }
            if (!flag)
            {
                isLootStart = false;
            }
        }
        deltaTimeLoot += Time.deltaTime;
    }

    private void UpdateLootTime()
    {
        if (deltaTimeLoot >= 1f)
        {
            deltaTimeLoot -= 1f;
            bool flag = false;
            for (int i = 0; i < userPlayer.loot.Length; i++)
            {
                if (userPlayer.loot[i] != 0 && userPlayer.lootMode[i] == 1)
                {
                    flag = true;
                    timeLoot[i] = timeLoot[i].AddSeconds(1.0);
                    TimeSpan timeSpan = endTimeLootBasic[i].Subtract(timeLoot[i]);
                    string text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                    Transform transform = panelAdventureLoot.Find("Loot" + (i + 1)).Find("TextTime");
                    if (!transform.gameObject.activeInHierarchy)
                    {
                        transform.gameObject.SetActive(value: true);
                    }
                    transform.GetComponent<Text>().text = string.Empty + text.ToString();
                    if (timeSpan.Hours <= 0 && timeSpan.Minutes <= 0 && timeSpan.Seconds <= 0)
                    {
                        flag = false;
                        panelLoading.gameObject.SetActive(value: true);
                        int number = i + 1;
                        Run.After(2f, delegate
                        {
                            userPlayer.OpenLoot(number);
                            RefreshLoot();
                            panelLoading.gameObject.SetActive(value: false);
                        });
                    }
                }
            }
            if (!flag)
            {
                isLootStart = false;
            }
        }
        deltaTimeLoot += Time.deltaTime;
    }

    private void UpdateGiftTime()
    {
        if (deltaTimeGift >= 1f)
        {
            deltaTimeGift -= 1f;
            bool flag = false;
            for (int i = 0; i < userPlayer.gift.Length; i++)
            {
                if (userPlayer.gift[i] != 0 && userPlayer.giftMode[i] == 1)
                {
                    flag = true;
                    timeGift[i] = timeGift[i].AddSeconds(1.0);
                    TimeSpan timeSpan = endTimeGiftBasic[i].Subtract(timeGift[i]);
                    string text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                    Transform transform = panelGiftLoot.Find("Loot" + (i + 1)).Find("TextTime");
                    if (!transform.gameObject.activeInHierarchy)
                    {
                        transform.gameObject.SetActive(value: true);
                    }
                    transform.GetComponent<Text>().text = string.Empty + text.ToString();
                    if (timeSpan.Hours <= 0 && timeSpan.Minutes <= 0 && timeSpan.Seconds <= 0)
                    {
                        flag = false;
                        panelLoading.gameObject.SetActive(value: true);
                        int number = i + 1;
                        Run.After(2f, delegate
                        {
                            userPlayer.OpenGift(number);
                            RefreshGift();
                            panelLoading.gameObject.SetActive(value: false);
                        });
                    }
                }
            }
            if (!flag)
            {
                isGiftStart = false;
            }
        }
        deltaTimeGift += Time.deltaTime;
    }

    private void InitGameObject()
    {
        if (userPlayer == null)
        {
            userPlayer = new GlobalUser(isPlayer: true);
        }
        globalSound = base.transform.parent.Find("Sound").GetComponent<GlobalSound>();
        canvas = base.transform.Find("Canvas");
        animatorFireworks = canvas.Find("PanelFireworks").GetComponent<Animator>();
        panelTop = canvas.Find("PanelTop");
        textLevel = panelTop.Find("Exp").Find("Level").GetComponent<Text>();
        textRank = panelTop.Find("Exp").Find("Rank").GetComponent<Text>();
        textExp = panelTop.Find("Exp").Find("Text").GetComponent<Text>();
        textGold = panelTop.Find("Gold").Find("Text").GetComponent<Text>();
        textKey = panelTop.Find("Key").Find("Text").GetComponent<Text>();
        textTime = panelTop.Find("Key").Find("Time").GetComponent<Text>();
        ButtonPlusGoldListener(panelTop.Find("Gold").Find("Plus").GetComponent<Button>());
        ButtonPlusKeyListener(panelTop.Find("Key").Find("Plus").GetComponent<Button>());
        panelMenuIcon = panelTop.Find("PanelMenuIcon");
        for (int i = 0; i < 3; i++)
        {
            ButtonMenuListener(panelMenuIcon.Find("Icon" + (i + 1)).GetComponent<Button>());
        }
        panelCenter = canvas.Find("PanelCenter");
        panelFormationSet = panelCenter.Find("PanelFormationSet");
        panelContentFormationSet = panelFormationSet.Find("Scroll View").Find("Viewport").Find("Content");
        panelAdventureFind = panelCenter.Find("PanelAdventureFind");
        panelAdventureLoot = panelCenter.Find("PanelAdventureLoot");
        panelGiftLoot = panelCenter.Find("PanelGiftLoot");
        for (int j = 0; j < 3; j++)
        {
            ButtonLootListener(panelAdventureLoot.Find("Loot" + (j + 1)).Find("Loot").GetComponent<Button>());
        }
        for (int k = 0; k < 6; k++)
        {
            ButtonGiftListener(panelGiftLoot.Find("Loot" + (k + 1)).Find("Loot").GetComponent<Button>());
        }
        Transform transform = panelContentFormationSet.Find("Formation");
        for (int l = 0; l < userPlayer.cardFormation.Length; l++)
        {
            ButtonSelectedFormationListener(transform.Find(string.Empty + (l + 1)).Find("Button").GetComponent<Button>());
        }
        for (int m = 0; m < userPlayer.cardFormation.Length; m++)
        {
            ButtonSelectedFormationListener(transform.Find("Empty" + (m + 1)).GetComponent<Button>());
        }
        Transform transform2 = panelContentFormationSet.Find("Deck");
        for (int n = 0; n < userPlayer.cardCount.Length; n++)
        {
            ButtonDeckListener(transform2.Find(string.Empty + (n + 1)).Find("Button").GetComponent<Button>());
        }
        ButtonFindListener(panelAdventureFind.Find("ButtonFind").GetComponent<Button>());
        panelLootUnlock = canvas.Find("PanelLootUnlock");
        panelAlert = canvas.Find("PanelAlert");
        ButtonOkAlertListener(panelAlert.Find("ButtonOk").GetComponent<Button>());
        panelLoading = canvas.Find("PanelLoading");
        panelFindingMatch = canvas.Find("PanelFindingMatch");
        globalSound.AudioLoop("Sound/MainMenu", 0.1f);
        if (isRate)
        {
            isRate = false;
            canvas.Find("PanelRate").gameObject.SetActive(value: true);
        }
        if (!PlayerPrefsX.GetBool("tutorial"))
        {
            GetComponent<Tutorial>().Init();
        }
    }

    public void RefreshMainMenu()
    {
        RefreshTop();
        for (int i = 0; i < 3; i++)
        {
            panelMenuIcon.Find("Icon" + (i + 1)).Find("Active").gameObject.SetActive(value: false);
        }
        panelMenuIcon.Find("Icon" + activeMenu).Find("Active").gameObject.SetActive(value: true);
        panelFormationSet.gameObject.SetActive(value: false);
        panelFormationSet.Find("Scroll View").gameObject.SetActive(value: false);
        panelAdventureFind.gameObject.SetActive(value: false);
        panelAdventureLoot.gameObject.SetActive(value: false);
        panelGiftLoot.gameObject.SetActive(value: false);
        if (activeMenu == 1)
        {
            panelFormationSet.gameObject.SetActive(value: true);
            panelFormationSet.Find("Scroll View").gameObject.SetActive(value: true);
            panelContentFormationSet.Find("Formation").gameObject.SetActive(value: false);
            panelContentFormationSet.Find("Deck").gameObject.SetActive(value: false);
            panelContentFormationSet.Find("Selected").gameObject.SetActive(value: false);
            RefreshFormation();
            RefreshDeck();
            FbAdsManager.instance.LoadInterstitial();
        }
        else if (activeMenu == 2)
        {
            panelAdventureFind.gameObject.SetActive(value: true);
            panelAdventureLoot.gameObject.SetActive(value: true);
            RefreshStage();
            RefreshLoot();
        }
        else if (activeMenu == 3)
        {
            panelGiftLoot.gameObject.SetActive(value: true);
            RefreshGift();
        }
    }

    private void RefreshKeyTime()
    {
        if (userPlayer != null)
        {
            timeKey = DateTime.Now;
            if (userPlayer.key < 5)
            {
                endTimeKeyBasic = userPlayer.timeKey;
                endTimeKeyBasic = endTimeKeyBasic.AddMinutes(30.0);
                isKeyStart = true;
            }
            else
            {
                textTime.gameObject.SetActive(value: false);
                isKeyStart = false;
            }
        }
    }

    private void RefreshTop()
    {
        textLevel.text = string.Empty + userPlayer.level;
        textRank.text = userPlayer.rank.ToString();
        textGold.text = string.Empty + userPlayer.gold;
        textExp.text = userPlayer.exp + "/" + userPlayer.GetMaxExp();
        textKey.text = userPlayer.key + "/" + userPlayer.GetMaxKey();
        float num = 100f * (float)userPlayer.exp / (float)userPlayer.GetMaxExp();
        float num2 = num / 100f * 150f;
        RectTransform component = textExp.transform.parent.Find("Backdrop").Find("Indicator").GetComponent<RectTransform>();
        RectTransform rectTransform = component;
        float x = -75f + 75f * num2 / 150f;
        Vector3 localPosition = component.localPosition;
        float y = localPosition.y;
        Vector3 localPosition2 = component.localPosition;
        rectTransform.localPosition = new Vector3(x, y, localPosition2.z);
        RectTransform rectTransform2 = component;
        float x2 = num2;
        Vector2 sizeDelta = component.sizeDelta;
        rectTransform2.sizeDelta = new Vector2(x2, sizeDelta.y);
    }

    private void RefreshStage()
    {
        panelAdventureFind.Find("Map").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Map" + activeStage);
    }

    private void RefreshAlert(string title, string content)
    {
        globalSound.AudioOnce("Sound/Alert", 0.31f);
        panelAlert.gameObject.SetActive(value: true);
        panelAlert.Find("TextTitle").GetComponent<Text>().text = string.Empty + title;
        panelAlert.Find("TextContent").GetComponent<Text>().text = string.Empty + content;
    }

    private void RefreshLoot()
    {
        for (int i = 0; i < userPlayer.loot.Length; i++)
        {
            Transform transform = panelAdventureLoot.Find("Loot" + (i + 1));
            transform.Find("Loot").gameObject.SetActive(value: false);
            for (int j = 0; j < 3; j++)
            {
                transform.Find("Loot").Find("Loot" + (j + 1)).gameObject.SetActive(value: false);
            }
            transform.Find("TextOpen").gameObject.SetActive(value: false);
            transform.Find("TextTime").gameObject.SetActive(value: false);
            transform.Find("TextFrom").gameObject.SetActive(value: false);
            if (userPlayer.loot[i] == 0)
            {
                transform.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.19f);
            }
            else if (userPlayer.loot[i] > 0)
            {
                transform.Find("Loot").gameObject.SetActive(value: true);
                transform.Find("Loot").Find("Loot" + userPlayer.loot[i]).gameObject.SetActive(value: true);
                if (userPlayer.lootMode[i] == 0)
                {
                    transform.GetComponent<Image>().color = new Color(0.5f, 0.07f, 0.07f, 0.58f);
                    transform.Find("TextTime").gameObject.SetActive(value: true);
                    transform.Find("TextFrom").gameObject.SetActive(value: true);
                    transform.Find("TextTime").GetComponent<Text>().text = "Locked";
                    transform.Find("TextFrom").GetComponent<Text>().text = "ARENA" + userPlayer.lootFrom[i];
                }
                else if (userPlayer.lootMode[i] == 1)
                {
                    transform.GetComponent<Image>().color = new Color(0.66f, 1f, 0f, 0.47f);
                    transform.Find("TextOpen").gameObject.SetActive(value: true);
                    timeLoot[i] = DateTime.Now;
                    endTimeLootBasic[i] = userPlayer.lootTime[i];
                    endTimeLootBasic[i] = endTimeLootBasic[i].AddMinutes(30.0);
                    isLootStart = true;
                }
                else
                {
                    transform.GetComponent<Image>().color = new Color(1f, 0.82f, 0f, 0.52f);
                    transform.Find("TextFrom").gameObject.SetActive(value: true);
                    transform.Find("TextFrom").GetComponent<Text>().text = "OPEN";
                }
            }
        }
    }

    private void RefreshGift()
    {
        for (int i = 0; i < userPlayer.gift.Length; i++)
        {
            Transform transform = panelGiftLoot.Find("Loot" + (i + 1));
            transform.Find("Loot").gameObject.SetActive(value: false);
            for (int j = 0; j < 3; j++)
            {
                transform.Find("Loot").Find("Loot" + (j + 1)).gameObject.SetActive(value: false);
            }
            transform.Find("TextOpen").gameObject.SetActive(value: false);
            transform.Find("TextTime").gameObject.SetActive(value: false);
            transform.Find("TextFrom").gameObject.SetActive(value: false);
            if (userPlayer.gift[i] == 0)
            {
                transform.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.19f);
            }
            else
            {
                if (userPlayer.gift[i] <= 0)
                {
                    continue;
                }
                transform.Find("Loot").gameObject.SetActive(value: true);
                transform.Find("Loot").Find("Loot" + userPlayer.gift[i]).gameObject.SetActive(value: true);
                if (userPlayer.giftMode[i] == 0)
                {
                    transform.GetComponent<Image>().color = new Color(0.5f, 0.07f, 0.07f, 0.58f);
                    transform.Find("TextTime").gameObject.SetActive(value: true);
                    transform.Find("TextFrom").gameObject.SetActive(value: true);
                    transform.Find("TextTime").GetComponent<Text>().text = "Locked";
                    transform.Find("TextFrom").GetComponent<Text>().text = "ARENA" + userPlayer.giftFrom[i];
                }
                else if (userPlayer.giftMode[i] == 1)
                {
                    transform.GetComponent<Image>().color = new Color(0.66f, 1f, 0f, 0.47f);
                    transform.Find("TextOpen").gameObject.SetActive(value: true);
                    timeGift[i] = DateTime.Now;
                    endTimeGiftBasic[i] = userPlayer.giftTime[i];
                    if (i < 1)
                    {
                        userPlayer.OpenGift(i + 1);
                    }
                    else
                    {
                        switch (i)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                endTimeGiftBasic[i] = endTimeGiftBasic[i].AddMinutes(60.0);
                                break;
                            case 5:
                                endTimeGiftBasic[i] = endTimeGiftBasic[i].AddMinutes(180.0);
                                break;
                        }
                    }
                    if (i >= 1)
                    {
                        isGiftStart = true;
                    }
                }
                else
                {
                    transform.GetComponent<Image>().color = new Color(1f, 0.82f, 0f, 0.52f);
                    transform.Find("TextFrom").gameObject.SetActive(value: true);
                    transform.Find("TextFrom").GetComponent<Text>().text = "OPEN";
                }
            }
        }
    }

    private void RefreshFormation()
    {
        Transform transform = panelContentFormationSet.Find("Formation");
        panelContentFormationSet.gameObject.SetActive(value: true);
        transform.gameObject.SetActive(value: true);
        for (int i = 0; i < userPlayer.cardFormation.Length; i++)
        {
            int num = userPlayer.cardFormation[i];
            if (num == 0)
            {
                userPlayer.card[i] = null;
                transform.Find(string.Empty + (i + 1)).gameObject.SetActive(value: false);
                continue;
            }
            Transform transform2 = transform.Find(string.Empty + (i + 1));
            GlobalCard globalCard = new GlobalCard(num, userPlayer.cardLevel[num - 1]);
            int num2 = userPlayer.cardCount[globalCard.id - 1];
            int num3 = userPlayer.cardUpgrade[(int)(globalCard.rarity - 1), globalCard.level];
            float num4 = 100f * (float)num2 / (float)num3;
            if (num4 >= 100f)
            {
                num4 = 100f;
            }
            float num5 = num4 / 100f * 75f;
            Slider component = transform2.Find("Exp").Find("Bar").GetComponent<Slider>();
            component.value = num3;
            transform2.gameObject.SetActive(value: true);
            transform2.Find("Button").Find("Image").GetComponent<Image>()
                .sprite = icon[globalCard.id - 1];
            transform2.Find("Button").Find("Text").GetComponent<Text>()
                .text = "Level " + globalCard.level;
            transform2.Find("TextEnergy").GetComponent<Text>().text = string.Empty + globalCard.energy;
            transform2.Find("Exp").Find("Text").GetComponent<Text>()
                .text = num2 + "/" + num3;
            if (num2 >= num3)
            {
                transform2.Find("Alert").gameObject.SetActive(value: true);
            }
            else
            {
                transform2.Find("Alert").gameObject.SetActive(value: false);
            }
        }
    }

    private void RefreshDeck()
    {
        List<int> list = new List<int>(userPlayer.cardFormation);
        Transform transform = panelContentFormationSet.Find("Deck");
        transform.gameObject.SetActive(value: true);
        panelFormationSet.Find("Scroll View").GetComponent<ScrollRect>().vertical = true;
        for (int i = 0; i < userPlayer.cardCount.Length; i++)
        {
            Transform transform2 = transform.Find(string.Empty + (i + 1));
            GlobalCard globalCard = new GlobalCard(i + 1, userPlayer.cardLevel[i]);
            int num = userPlayer.cardCount[globalCard.id - 1];
            int num2 = userPlayer.cardUpgrade[(int)(globalCard.rarity - 1), globalCard.level];
            float num3 = 100f * (float)num / (float)num2;
            if (num3 >= 100f)
            {
                num3 = 100f;
            }
            float num4 = num3 / 100f * 75f;
            Slider component = transform2.Find("Exp").Find("Bar").GetComponent<Slider>();
            component.value = num3;
            bool flag = true;
            if (globalCard.level == 0)
            {
                flag = false;
            }
            transform2.Find("Button").Find("Backdrop").gameObject.SetActive(flag);
            transform2.Find("Button").Find("Text").gameObject.SetActive(flag);
            transform2.Find("Exp").gameObject.SetActive(flag);
            transform2.Find("Alert").gameObject.SetActive(flag);
            if (flag)
            {
                transform2.Find("Button").Find("Image").GetComponent<Image>()
                    .color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                transform2.Find("Button").Find("Image").GetComponent<Image>()
                    .color = new Color(0.2f, 0.2f, 0.2f, 1f);
            }
            transform2.gameObject.SetActive(value: true);
            transform2.Find("Button").Find("Image").GetComponent<Image>()
                .sprite = icon[globalCard.id - 1];
            transform2.Find("Button").Find("Text").GetComponent<Text>()
                .text = "Level " + globalCard.level;
            if (flag)
            {
                transform2.Find("TextEnergy").GetComponent<Text>().text = string.Empty + globalCard.energy;
            }
            else
            {
                transform2.Find("TextEnergy").GetComponent<Text>().text = "?";
            }
            transform2.Find("Exp").Find("Text").GetComponent<Text>()
                .text = num + "/" + num2;
            if (num >= num2)
            {
                transform2.Find("Alert").gameObject.SetActive(value: true);
            }
            else
            {
                transform2.Find("Alert").gameObject.SetActive(value: false);
            }
            flag = true;
            if (!list.Contains(globalCard.id))
            {
                flag = false;
                if (globalCard.level != 0)
                {
                    transform2.Find("Button").Find("Image").GetComponent<Image>()
                        .color = new Color(1f, 1f, 1f, 1f);
                }
            }
            else
            {
                transform2.Find("Button").Find("Image").GetComponent<Image>()
                    .color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            transform2.Find("Button").Find("TextEquip").gameObject.SetActive(flag);
        }
    }

    private void RefreshCardInformation(GlobalCard card)
    {
        Transform transform = panelFormationSet.Find("Information");
        Transform transform2 = transform.Find("Card");
        Transform transform3 = transform.Find("TextInformation");
        transform.gameObject.SetActive(value: true);
        int num = userPlayer.cardCount[card.id - 1];
        int num2 = userPlayer.cardUpgrade[(int)(card.rarity - 1), card.level];
        float num3 = 100f * (float)num / (float)num2;
        if (num3 >= 100f)
        {
            num3 = 100f;
        }
        Slider component = transform2.Find("Exp").Find("Bar").GetComponent<Slider>();
        component.value = num3;
        transform2.gameObject.SetActive(value: true);
        transform2.Find("Button").Find("Image").GetComponent<Image>()
            .sprite = icon[card.id - 1];
        transform2.Find("Button").Find("Text").GetComponent<Text>()
            .text = "Level " + card.level;
        transform2.Find("TextEnergy").GetComponent<Text>().text = string.Empty + card.energy;
        transform2.Find("Exp").Find("Text").GetComponent<Text>()
            .text = num + "/" + num2;
        bool flag = true;
        if (num < num2)
        {
            flag = false;
        }
        transform2.Find("Alert").gameObject.SetActive(flag);
        transform.Find("ButtonUpgrade").gameObject.SetActive(flag);
        if (flag)
        {
            Text component2 = transform.Find("ButtonUpgrade").Find("TextCost").GetComponent<Text>();
            component2.text = "Cost: " + userPlayer.cardGold[(int)(card.rarity - 1), card.level];
            component2.color = new Color(1f, 1f, 1f, 1f);
        }
        transform3.Find("TextRarityValue").GetComponent<Text>().text = string.Empty + card.rarity.ToString();
        transform3.Find("TextTypeValue").GetComponent<Text>().text = string.Empty + card.unitType.ToString();
        transform3.Find("TextHealthValue").GetComponent<Text>().text = string.Empty + card.health;
        transform3.Find("TextAttackValue").GetComponent<Text>().text = string.Empty + card.attack;
        transform3.Find("TextAttackSpeedValue").GetComponent<Text>().text = string.Empty + card.attackSpeed;
        transform3.Find("TextTargetsValue").GetComponent<Text>().text = string.Empty + card.unitTarget.ToString().Replace("Only", string.Empty);
        transform3.Find("TextSpeedValue").GetComponent<Text>().text = string.Empty + card.speed;
        transform3.Find("TextRangeValue").GetComponent<Text>().text = string.Empty + card.range;
        transform3.Find("TextCountValue").GetComponent<Text>().text = string.Empty + card.count;
        Button component3 = transform.Find("ButtonUpgrade").GetComponent<Button>();
        Button component4 = transform.Find("ButtonEquip").GetComponent<Button>();
        Button component5 = transform.Find("ButtonBack").GetComponent<Button>();
        component3.onClick.RemoveAllListeners();
        component4.onClick.RemoveAllListeners();
        component5.onClick.RemoveAllListeners();
        ButtonUpgradeDeckInformationListener(component3);
        ButtonEquipDeckInformationListener(component4);
        ButtonBackDeckInformationListener(component5);
        List<int> list = new List<int>(userPlayer.cardFormation);
        if (list.Contains(selectedCard.id))
        {
            component4.transform.Find("Text").GetComponent<Text>().text = "REMOVE";
        }
        else
        {
            component4.transform.Find("Text").GetComponent<Text>().text = "SET";
        }
    }

    private void RefreshSelectedCard()
    {
        Transform transform = panelContentFormationSet.Find("Selected");
        RectTransform component = panelContentFormationSet.GetComponent<RectTransform>();
        RectTransform rectTransform = component;
        Vector3 localPosition = component.localPosition;
        float x = localPosition.x;
        Vector3 localPosition2 = component.localPosition;
        rectTransform.localPosition = new Vector3(x, 0f, localPosition2.z);
        panelFormationSet.Find("Scroll View").GetComponent<ScrollRect>().vertical = false;
        panelContentFormationSet.Find("Deck").gameObject.SetActive(value: false);
        transform.gameObject.SetActive(value: true);
        int num = userPlayer.cardCount[selectedCard.id - 1];
        int num2 = userPlayer.cardUpgrade[(int)(selectedCard.rarity - 1), selectedCard.level];
        float num3 = 100f * (float)num / (float)num2;
        if (num3 >= 100f)
        {
            num3 = 100f;
        }
        float num4 = num3 / 100f * 75f;
        Slider component2 = transform.Find("Exp").Find("Bar").GetComponent<Slider>();
        component2.value = num3;
        transform.gameObject.SetActive(value: true);
        transform.Find("Button").Find("Image").GetComponent<Image>()
            .sprite = icon[selectedCard.id - 1];
        transform.Find("Button").Find("Text").GetComponent<Text>()
            .text = "Level " + selectedCard.level;
        transform.Find("TextEnergy").GetComponent<Text>().text = string.Empty + selectedCard.energy;
        transform.Find("Exp").Find("Text").GetComponent<Text>()
            .text = num + "/" + num2;
        bool active = true;
        if (num < num2)
        {
            active = false;
        }
        transform.Find("Alert").gameObject.SetActive(active);
        Button component3 = transform.Find("ButtonCancel").GetComponent<Button>();
        component3.onClick.RemoveAllListeners();
        ButtonCancelSelectedCardListener(component3);
    }

    private IEnumerator OpenLoot(string[] loot)
    {
        FbAdsManager.instance.LoadInterstitial();
        List<string> loots = new List<string>(loot);
        while (loots.Count > 1)
        {
            int id = int.Parse(loots[1]);
            Transform cardTransform = panelLootUnlock.Find("Card");
            GlobalCard c = new GlobalCard(id, userPlayer.cardLevel[id - 1]);
            int count = userPlayer.cardCount[c.id - 1];
            int upgrade = userPlayer.cardUpgrade[(int)(c.rarity - 1), c.level];
            float expPercent = 100f * (float)count / (float)upgrade;
            if (expPercent >= 100f)
            {
                expPercent = 100f;
            }
            float expValue = expPercent / 100f * 150f;
            Slider rect = cardTransform.Find("Exp").Find("Bar").GetComponent<Slider>();
            rect.value = expPercent;
            cardTransform.gameObject.SetActive(value: true);
            cardTransform.Find("Button").Find("Image").GetComponent<Image>()
                .sprite = icon[c.id - 1];
            cardTransform.Find("Button").Find("Text").GetComponent<Text>()
                .text = "Level " + c.level;
            cardTransform.Find("Button").Find("TextCount").GetComponent<Text>()
                .text = "+" + loots[2];
            cardTransform.Find("TextEnergy").GetComponent<Text>().text = string.Empty + c.energy;
            cardTransform.Find("Exp").Find("Text").GetComponent<Text>()
                .text = count + "/" + upgrade;
            bool isTrue = true;
            if (count < upgrade)
            {
                isTrue = false;
            }
            cardTransform.Find("Alert").gameObject.SetActive(isTrue);
            panelLootUnlock.gameObject.SetActive(value: true);
            panelLootUnlock.GetComponent<Animator>().Play("Unlock");
            bool isLoop = true;
            Button buttonBack = panelLootUnlock.Find("ButtonBack").GetComponent<Button>();
            if (loots.Count > 3)
            {
                buttonBack.transform.Find("Text").GetComponent<Text>().text = "Next";
            }
            else
            {
                buttonBack.transform.Find("Text").GetComponent<Text>().text = "Back";
            }
            buttonBack.onClick.RemoveAllListeners();
            buttonBack.onClick.AddListener(delegate
            {
                if (Tutorial.tutorialNow == 9 && loots.Count == 3)
                {
                    Tutorial.instance.Next();
                    Tutorial.instance.Show();
                }
                isLoop = false;
                panelLootUnlock.GetComponent<Animator>().enabled = false;
            });
            while (isLoop)
            {
                yield return new WaitForEndOfFrame();
            }
            loots.RemoveAt(1);
            loots.RemoveAt(1);
            panelLootUnlock.gameObject.SetActive(value: false);
        }
    }

    public IEnumerator RefreshMatchmaking()
    {
        userPlayer.RefreshCard();
        userEnemy = new GlobalUser();
        new List<int>();
        cLevel = new int[21]
        {
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1,
                                                                                            1
        };
        cFormation = new int[8];
        List<int> formationAvailable = (userPlayer.rank <= 30) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4
                                                                                        } : ((userPlayer.rank <= 60) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            16
                                                                                        } : ((userPlayer.rank <= 90) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            6,
                                                                                            16
                                                                                        } : ((userPlayer.rank <= 120) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            6,
                                                                                            7,
                                                                                            15,
                                                                                            16
                                                                                        } : ((userPlayer.rank <= 150) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            6,
                                                                                            7,
                                                                                            8,
                                                                                            11,
                                                                                            12,
                                                                                            15,
                                                                                            16
                                                                                        } : ((userPlayer.rank <= 180) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            6,
                                                                                            7,
                                                                                            8,
                                                                                            9,
                                                                                            11,
                                                                                            12,
                                                                                            13,
                                                                                            15,
                                                                                            16
                                                                                        } : ((userPlayer.rank <= 210) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            6,
                                                                                            7,
                                                                                            8,
                                                                                            9,
                                                                                            10,
                                                                                            11,
                                                                                            12,
                                                                                            13,
                                                                                            15,
                                                                                            16
                                                                                        } : ((userPlayer.rank <= 240) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            6,
                                                                                            7,
                                                                                            8,
                                                                                            9,
                                                                                            10,
                                                                                            11,
                                                                                            12,
                                                                                            13,
                                                                                            14,
                                                                                            15,
                                                                                            16,
                                                                                            18
                                                                                        } : ((userPlayer.rank <= 270) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            6,
                                                                                            7,
                                                                                            8,
                                                                                            9,
                                                                                            10,
                                                                                            11,
                                                                                            12,
                                                                                            13,
                                                                                            14,
                                                                                            15,
                                                                                            16,
                                                                                            18,
                                                                                            19
                                                                                        } : ((userPlayer.rank > 300) ? new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            6,
                                                                                            7,
                                                                                            8,
                                                                                            9,
                                                                                            10,
                                                                                            11,
                                                                                            12,
                                                                                            13,
                                                                                            14,
                                                                                            16,
                                                                                            16,
                                                                                            18,
                                                                                            19,
                                                                                            20,
                                                                                            21
                                                                                        } : new List<int>
                                                                                        {
                                                                                            2,
                                                                                            3,
                                                                                            4,
                                                                                            6,
                                                                                            7,
                                                                                            8,
                                                                                            9,
                                                                                            10,
                                                                                            11,
                                                                                            12,
                                                                                            13,
                                                                                            14,
                                                                                            16,
                                                                                            16,
                                                                                            18,
                                                                                            19,
                                                                                            20
                                                                                        })))))))));
        float handicap = (userPlayer.rank <= 30) ? 0.5f : ((userPlayer.rank <= 60) ? 0.55f : ((userPlayer.rank > 90) ? 0.65f : 0.6f));
        yield return new WaitForEndOfFrame();
        int k = 0;
        while (formationAvailable.Count != 0)
        {
            int randomValue = UnityEngine.Random.Range(0, formationAvailable.Count);
            int cFormationNow = formationAvailable[randomValue];
            formationAvailable.RemoveAt(randomValue);
            bool isBreak = false;
            for (int j = 0; j < cFormation.Length; j++)
            {
                if (cFormation[j] == 0)
                {
                    cFormation[j] = cFormationNow;
                    cLevel[cFormationNow - 1] = 1;
                    if (j == cFormation.Length - 1)
                    {
                        isBreak = true;
                    }
                    break;
                }
            }
            if (isBreak)
            {
                break;
            }
            k++;
        }
        userEnemy.cardLevel = cLevel;
        userEnemy.cardFormation = cFormation;
        userEnemy.level = userPlayer.level;
        userEnemy.RefreshCard();
        List<int> cFormationList = new List<int>(userEnemy.cardFormation);
        while (userEnemy.RefreshPower() < userPlayer.RefreshPower() * handicap)
        {
            int indexList = UnityEngine.Random.Range(0, cFormationList.Count);
            int idCharacter = cFormationList[indexList];
            if (idCharacter != 0)
            {
                GlobalCard c = new GlobalCard(idCharacter, userEnemy.cardLevel[idCharacter - 1]);
                if (c.level < 13 && userEnemy.cardUpgrade[(int)(c.rarity - 1), c.level] != -1)
                {
                    userEnemy.cardLevel[c.id - 1]++;
                    userEnemy.RefreshCard(c);
                }
                else
                {
                    cFormationList.RemoveAt(indexList);
                }
            }
            else
            {
                cFormationList.RemoveAt(indexList);
            }
            yield return new WaitForEndOfFrame();
        }
        userEnemy.RefreshCard();
        for (int i = 0; i < userEnemy.cardFormation.Length; i++)
        {
            int id = userEnemy.cardFormation[i];
            if (id != 0)
            {
                MonoBehaviour.print("id: " + id + ", lv: " + userEnemy.cardLevel[id - 1]);
            }
        }
        SceneManager.LoadSceneAsync("Gameplay");
    }

    private void ButtonMenuListener(Button b)
    {
        GameObject gameObject = b.gameObject;
        int number = int.Parse(string.Empty + gameObject.name[gameObject.name.Length - 1]);
        b.onClick.AddListener(delegate
        {
            if ((Tutorial.tutorialNow == 0 || (Tutorial.tutorialNow == 5 && number == 3) || (Tutorial.tutorialNow == 11 && number == 1) || (Tutorial.tutorialNow == 22 && number == 2)) && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    if (Tutorial.tutorialNow == 5 || Tutorial.tutorialNow == 11 || Tutorial.tutorialNow == 22)
                    {
                        Tutorial.instance.Next();
                        Tutorial.instance.Show();
                    }
                    isButton = false;
                    activeMenu = number;
                    RefreshMainMenu();
                });
            }
        });
    }

    private void ButtonFindListener(Button b)
    {
        b.onClick.AddListener(delegate
        {
            if ((Tutorial.tutorialNow == 0 || Tutorial.tutorialNow == 23) && !isButton)
            {
                globalSound.AudioOnce("Sound/Start", 0.41f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    FindMatch();
                });
            }
        });
    }

    private void FindMatch()
    {
        if (userPlayer.ConsumeKey())
        {
            bool flag = false;
            for (int i = 0; i < userPlayer.card.Length; i++)
            {
                if (userPlayer.card[i] != null)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                if (Tutorial.tutorialNow == 23)
                {
                    Tutorial.instance.Next();
                    Tutorial.instance.Close();
                }
                RefreshKeyTime();
                RefreshTop();
                panelFindingMatch.gameObject.SetActive(value: true);



                //Matchmaker.Instance.StartMatchmaking();
                FbAdsManager.instance.LoadInterstitial();
                Run.After(UnityEngine.Random.Range(2f, 4f), delegate
                {
                    panelFindingMatch.Find("Text").GetComponent<Text>().text = "MATCH FOUND!";
                    StartCoroutine(RefreshMatchmaking());
                });
            }
            else
            {
                RefreshAlert("NO FORMATION", "Please set your formation. Right now, you have no troops at all.");
            }
        }
        else
        {
            RefreshAlert("NOT ENOUGH KEY", "Sorry, you have no enough key right now. Please wait until your key replenish, or you can use add player key feature if enabled. (^^,");
        }
    }

    private void ButtonPlusGoldListener(Button b)
    {
        b.onClick.AddListener(delegate
        {
            if (Tutorial.tutorialNow == 0 && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    RefreshAlert("ADD PLAYER GOLD", "Sorry, this feature is not implemented yet. We will implement this feature as soon as possible. Thank you for your convience (^^,");
                });
            }
        });
    }

    private void ButtonPlusKeyListener(Button b)
    {
        b.onClick.AddListener(delegate
        {
            if (Tutorial.tutorialNow == 0 && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    adsVideoNumber = 3;
                    //if (!ADMOB.instance.ShowRewardVideo(ADMOB.instance.HandleShowResult))
                    //{
                    //	RefreshAlert("CONNECTION ERROR", "Sorry, There's problem on Internet. Please wait and try again in 30s. (^^,");
                    //}
                });
            }
        });
    }

    private void ButtonOkAlertListener(Button b)
    {
        b.onClick.AddListener(delegate
        {
            if (Tutorial.tutorialNow == 0 && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    b.transform.parent.gameObject.SetActive(value: false);
                });
            }
        });
    }

    private void LootLevelUpListener()
    {
        string[] loot = userPlayer.OpenLootLevelUp();
        panelLootUnlock.GetComponent<AnimationLoot>().loot = 2;
        RefreshTop();
        StartCoroutine(OpenLoot(loot));
    }

    private void ButtonLootListener(Button b)
    {
        GameObject gameObject = b.transform.parent.gameObject;
        int number = int.Parse(string.Empty + gameObject.name[gameObject.name.Length - 1]);
        b.onClick.AddListener(delegate
        {
            if (Tutorial.tutorialNow == 0 && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    panelLootUnlock.GetComponent<AnimationLoot>().loot = userPlayer.loot[number - 1];
                    string[] array = userPlayer.OpenLoot(number);
                    if (array[0] == "0")
                    {
                        RefreshAlert("UNLOCK LOOT", "Sorry, but you can only unlock 1 loot per time. Please open the other unlocked loot first, thank you! (^^,");
                    }
                    else if (array[0] == "2")
                    {
                        lootAdsNumber = number;
                        LootADS();
                    }
                    else if (array[0] == "3")
                    {
                        RefreshTop();
                        StartCoroutine(OpenLoot(array));
                    }
                    else
                    {
                        globalSound.AudioOnce("Sound/GetItem", 0.31f);
                    }
                    if (array[0] != "2")
                    {
                        RefreshLoot();
                    }
                });
            }
        });
    }

    private void ButtonGiftListener(Button b)
    {
        GameObject gameObject = b.transform.parent.gameObject;
        int number = int.Parse(string.Empty + gameObject.name[gameObject.name.Length - 1]);
        b.onClick.AddListener(delegate
        {
            if ((Tutorial.tutorialNow == 0 || (Tutorial.tutorialNow == 9 && number == 1)) && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    panelLootUnlock.GetComponent<AnimationLoot>().loot = userPlayer.gift[number - 1];
                    string[] array = userPlayer.OpenGift(number);
                    if (array[0] == "0")
                    {
                        RefreshAlert("UNLOCK GIFT", "Sorry, but you can only unlock 1 gift per time. Please open the other unlocked gift first, thank you! (^^,");
                    }
                    else if (array[0] == "2")
                    {
                        giftAdsNumber = number;
                        GiftADS();
                    }
                    else if (array[0] == "3")
                    {
                        if (Tutorial.tutorialNow == 9)
                        {
                            Tutorial.instance.Close();
                        }
                        RefreshTop();
                        StartCoroutine(OpenLoot(array));
                    }
                    else
                    {
                        globalSound.AudioOnce("Sound/GetItem", 0.31f);
                    }
                    if (array[0] != "2")
                    {
                        RefreshGift();
                    }
                });
            }
        });
    }

    private void LootADS()
    {
        adsVideoNumber = 1;
        //if (!ADMOB.instance.ShowRewardVideo(ADMOB.instance.HandleShowResult))
        //{
        //	VideoAdsHandle(result: false);
        //}
    }

    private void GiftADS()
    {
        adsVideoNumber = 2;
        //if (!ADMOB.instance.ShowRewardVideo(ADMOB.instance.HandleShowResult))
        //{
        //	VideoAdsHandle(result: false);
        //}
    }

    public void VideoAdsHandle(bool result)
    {
        if (result)
        {
            if (adsVideoNumber == 1)
            {
                RefreshLoot();
            }
            else if (adsVideoNumber == 2)
            {
                RefreshGift();
            }
            else
            {
                userPlayer.AddKey();
            }
            RefreshAlert("WATCH ADS", "Succesfuly watch ads!\nThanks for supporting us by watching our ads. now you can open unlocked loot. (^^,");
            UnityEngine.Debug.Log("Video completed. Offer a reward to the player.");
            return;
        }
        if (adsVideoNumber == 1)
        {
            userPlayer.lootMode[lootAdsNumber - 1] = 1;
            PlayerPrefsX.SetIntArray("lootMode", userPlayer.lootMode);
            RefreshLoot();
        }
        else if (adsVideoNumber == 2)
        {
            userPlayer.giftMode[giftAdsNumber - 1] = 1;
            PlayerPrefsX.SetIntArray("giftMode", userPlayer.giftMode);
            RefreshGift();
        }
        RefreshAlert("ADS ERROR", "ADS ERROR/CANCELED. Please wait and try again in 30s. (^^,");
        UnityEngine.Debug.LogWarning("Video was skipped/error.");
    }

    private void ButtonDeckListener(Button b)
    {
        GameObject gameObject = b.transform.parent.gameObject;
        int number = int.Parse(gameObject.name);
        b.onClick.AddListener(delegate
        {
            if (Tutorial.tutorialNow == 0 || (Tutorial.tutorialNow == 14 && number == 4))
            {
                Color color = b.transform.Find("Image").GetComponent<Image>().color;
                if (color.r >= 0.5f && !isButton)
                {
                    globalSound.AudioOnce("Sound/Swing1", 0.81f);
                    isButton = true;
                    Run.After(0.1f, delegate
                    {
                        if (Tutorial.tutorialNow == 14)
                        {
                            Tutorial.instance.Next();
                            Tutorial.instance.Show();
                        }
                        isButton = false;
                        selectedCard = new GlobalCard(number, userPlayer.cardLevel[number - 1]);
                        RefreshCardInformation(selectedCard);
                    });
                }
            }
        });
    }

    private void ButtonUpgradeDeckInformationListener(Button b)
    {
        b.onClick.AddListener(delegate
        {
            if (Tutorial.tutorialNow == 0 && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    if (userPlayer.UpgradeCard(selectedCard.id))
                    {
                        animatorFireworks.Play("FireworksText");
                        animatorFireworks.transform.Find("Text").GetComponent<Text>().text = "LEVEL UP!";
                        animatorFireworks.GetComponent<RectTransform>().position = b.GetComponent<RectTransform>().position;
                        globalSound.AudioOnce("Sound/SwordSpecial", 0.41f);
                        selectedCard = new GlobalCard(selectedCard.id, userPlayer.cardLevel[selectedCard.id - 1]);
                        RefreshCardInformation(selectedCard);
                        RefreshTop();
                        RefreshFormation();
                        RefreshDeck();
                    }
                    else
                    {
                        b.transform.Find("TextCost").GetComponent<Text>().color = new Color(1f, 0f, 0f, 1f);
                    }
                });
            }
        });
    }

    private void ButtonEquipDeckInformationListener(Button b)
    {
        b.onClick.AddListener(delegate
        {
            if ((Tutorial.tutorialNow == 0 || Tutorial.tutorialNow == 18) && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    if (b.transform.Find("Text").GetComponent<Text>().text == "SET")
                    {
                        if (Tutorial.tutorialNow == 18)
                        {
                            Tutorial.instance.Next();
                            Tutorial.instance.Show();
                        }
                        RefreshSelectedCard();
                        b.transform.parent.gameObject.SetActive(value: false);
                    }
                    else
                    {
                        globalSound.AudioOnce("Sound/Failed", 0.31f);
                        userPlayer.SetFormation(selectedCard.id, 0);
                        RefreshFormation();
                        RefreshDeck();
                        panelContentFormationSet.Find("Deck").gameObject.SetActive(value: true);
                        panelFormationSet.Find("Information").gameObject.SetActive(value: false);
                    }
                });
            }
        });
    }

    private void ButtonBackDeckInformationListener(Button b)
    {
        b.onClick.AddListener(delegate
        {
            if (Tutorial.tutorialNow == 0 && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    b.transform.parent.gameObject.SetActive(value: false);
                });
            }
        });
    }

    private void ButtonCancelSelectedCardListener(Button b)
    {
        b.onClick.AddListener(delegate
        {
            if (Tutorial.tutorialNow == 0 && !isButton)
            {
                globalSound.AudioOnce("Sound/Swing1", 0.81f);
                isButton = true;
                Run.After(0.1f, delegate
                {
                    isButton = false;
                    b.transform.parent.gameObject.SetActive(value: false);
                    panelContentFormationSet.Find("Deck").gameObject.SetActive(value: true);
                    panelFormationSet.Find("Scroll View").GetComponent<ScrollRect>().vertical = true;
                });
            }
        });
    }

    private void ButtonSelectedFormationListener(Button b)
    {
        GameObject g = b.gameObject;
        if (b.gameObject.name == "Button")
        {
            g = b.transform.parent.gameObject;
        }
        int number = int.Parse(string.Empty + g.name[g.name.Length - 1]);
        b.onClick.AddListener(delegate
        {
            if (Tutorial.tutorialNow == 0 || (Tutorial.tutorialNow == 19 && number == 1))
            {
                if (g.transform.parent.parent.Find("Selected").gameObject.activeInHierarchy)
                {
                    if (!isButton)
                    {
                        globalSound.AudioOnce("Sound/Swing1", 0.81f);
                        isButton = true;
                        Run.After(0.1f, delegate
                        {
                            if (Tutorial.tutorialNow == 19)
                            {
                                Tutorial.instance.Next();
                                Tutorial.instance.Show();
                            }
                            globalSound.AudioOnce("Sound/Success", 0.31f);
                            isButton = false;
                            userPlayer.SetFormation(selectedCard.id, number);
                            RefreshFormation();
                            RefreshDeck();
                            panelContentFormationSet.Find("Deck").gameObject.SetActive(value: true);
                            panelContentFormationSet.Find("Selected").gameObject.SetActive(value: false);
                        });
                    }
                }
                else if (!isButton)
                {
                    globalSound.AudioOnce("Sound/Swing1", 0.81f);
                    isButton = true;
                    Run.After(0.1f, delegate
                    {
                        isButton = false;
                        if (userPlayer.card[number - 1] != null)
                        {
                            selectedCard = userPlayer.card[number - 1];
                            RefreshCardInformation(selectedCard);
                        }
                    });
                }
            }
        });
    }
}
