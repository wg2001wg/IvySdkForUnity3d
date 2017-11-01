using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Demo : MonoBehaviour {

    public GUISkin guiSkin;
    public TextAsset infoJson = null;
    public Image avatarImg = null;
    private string[] imgUrl = null;

#if UNITY_ANDROID
    private string[] titles = {
		"Start AD", //0
		"Pause AD", //1
		"PassLevel AD", //2
		"Custom AD", //3
		"Banner AD", //4
		"Close Banner AD", //5
		"Exit AD", //6
		"Rate",//7
		"GetData", //8
		"Track Event", //9
		"Do Billing", //10
		"Show MoreGame", //11
		"Show FreeCoin", //12
		"Share", //13
		"Login", //14
		"Is Login", //15
		"Log out", //16
		"Like", //17
		"Invite", //18
		"Challenge", //19
		"Me", //20
		"Friends", //21
		"Submit Score", //22
		"Load LeaderBoard", //23
		"Load Global",//24
		"Show Native", //25
		"Hide Native", //26
		"Load Game Data", //27
		"Show Sales", //28
		"cache url", //29
		"load config", //30
		"Alert", //31
		"Cache Url With Tag", //32
        "Download Image"
	};
#elif UNITY_IOS
    private string[] titles = {
		"Start AD", //0
		"Pause AD", //1
		"PassLevel AD", //2
		"Custom AD", //3
		"Banner AD", //4
		"Close Banner AD", //5
		"Rate",//6
		"Do Billing", //7
		"Show FreeCoin", //8
        "Show IconAD" //9
	};
#endif

    // Use this for initialization
    void Awake () {
        RiseSdk.Instance.Init ();
        InitListeners ();
#if UNITY_EDITOR
        if (guiSkin != null) {
            guiSkin.button.fontSize = 20;
        }
#else
        if (guiSkin != null) {
            guiSkin.button.fontSize = 50;
        }
#endif
    }

    void Start () {
        initJsonData ();
    }

    private int imgIdx = 0;
    private void initJsonData () {
        if (infoJson != null) {
            List<object> data = (List<object>) RiseJson.Deserialize (infoJson.text);
            if (data != null && data.Count > 0) {
                int len = data.Count;
                imgUrl = new string[len];
                Dictionary<string, object> obj = null;
                string avatar = null;
                for (int i = 0; i < len; i++) {
                    obj = (Dictionary<string, object>) RiseJson.Deserialize (RiseJson.Serialize (data[i]));
                    avatar = obj["Avatar"].ToString ();
                    imgUrl[i] = avatar;
                    if (i == 0 && avatarImg != null) {
                        changeAvatarImg ();
                    } else {
#if UNITY_ANDROID
                        RiseSdk.Instance.DownloadFile (avatar, null);
#endif
                    }
                }
            }
        }
    }

    private void changeAvatarImg () {
        if (imgUrl == null) {
            return;
        }
        if (imgIdx < 0) {
            imgIdx = 0;
        }
#if UNITY_ANDROID
        RiseSdk.Instance.DownloadFile (imgUrl[imgIdx], (string path, WWW www) => {
            if (www != null) {
                Texture2D tex = new Texture2D (128, 128, TextureFormat.ARGB32, false);
                tex.LoadImage (www.bytes);
                Sprite sp = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0, 0));
                avatarImg.sprite = sp;
            }
        });
#endif
        if (++imgIdx >= imgUrl.Length) {
            imgIdx = 0;
        }
    }

    void InitListeners () {
        // Set get free coin event
        RiseSdkListener.OnAdEvent -= GetFreeCoin;
        RiseSdkListener.OnAdEvent += GetFreeCoin;
        // On payment result
        RiseSdkListener.OnPaymentEvent -= OnPaymentResult;
        RiseSdkListener.OnPaymentEvent += OnPaymentResult;

#if UNITY_ANDROID
        RiseSdkListener.OnSNSEvent -= OnSNSEvent;
        RiseSdkListener.OnSNSEvent += OnSNSEvent;

        RiseSdkListener.OnCacheUrlResult -= OnCacheUrl;
        RiseSdkListener.OnCacheUrlResult += OnCacheUrl;

        RiseSdkListener.OnReceiveServerExtra -= OnReceiveServerExtra;
        RiseSdkListener.OnReceiveServerExtra += OnReceiveServerExtra;

        RiseSdkListener.OnReceiveNotificationData -= OnNotificationData;
        RiseSdkListener.OnReceiveNotificationData += OnNotificationData;
#endif

        //RiseSdkListener.OnLeaderBoardEvent -= OnLeaderBoardResult;
        //RiseSdkListener.OnLeaderBoardEvent += OnLeaderBoardResult;

        //RiseSdkListener.OnReceiveServerResult -= OnServerResult;
        //RiseSdkListener.OnReceiveServerResult += OnServerResult;
    }

    void OnNotificationData (string data) {
        Debug.LogError ("receive notification data: " + data);
    }

    void OnReceiveServerExtra (string data) {
        Debug.LogError ("receive server result" + data);
    }

    void OnCacheUrl (bool result, int tag, string path) {
        Debug.LogError ("cache url result " + result + " tag " + tag + " path: " + path);
    }

    // Update is called once per frame
    void Update () {
#if UNITY_ANDROID
        if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Home)) {
            RiseSdk.Instance.OnExit ();
        }
#endif
    }

    void OnGUI () {
        GUI.skin = guiSkin;
        float w = Screen.width * .46f;
        //float h = Screen.height * .06f;
        float h = 1f * Screen.height / (titles.Length / 2 + 2);
        float x = 0, y = 0;
        for (int i = 0; i < titles.Length; i++) {
            int l = ((int) i / 2) + 1;
            y = 1 + (l - 1) * (h + 2);
            if (i % 2 == 0)
                x = 5;
            else
                x = Screen.width - w - 5;
            if (GUI.Button (new Rect (x, y, w, h), titles[i])) {
                doAction (i);
            }
        }
    }

    void doAction (int id) {
#if UNITY_ANDROID
        switch (id) {
            case 0:
                RiseSdk.Instance.ShowAd (RiseSdk.M_START);
                break;
            case 1:
                RiseSdk.Instance.ShowAd (RiseSdk.M_PAUSE);
                break;
            case 2:
                RiseSdk.Instance.ShowAd (RiseSdk.M_PASSLEVEL);
                break;
            case 3:
                RiseSdk.Instance.ShowAd (RiseSdk.M_CUSTOM);
                break;
            case 4:
                RiseSdk.Instance.ShowBanner ("default", RiseSdk.POS_BANNER_MIDDLE_BOTTOM);
                break;
            case 5:
                RiseSdk.Instance.CloseBanner ();
                break;
            case 6:
                RiseSdk.Instance.OnExit ();
                break;
            case 7:
                RiseSdk.Instance.Rate ();
                break;
            case 8:
                Debug.LogError ("get extra data: " + RiseSdk.Instance.GetExtraData ());
                break;
            case 9:
                RiseSdk.Instance.TrackEvent ("category", "action", "label", 323);
                break;
            case 10:
                RiseSdk.Instance.Pay (1);
                break;
            case 11:
                RiseSdk.Instance.ShowMore ();
                break;
            case 12:
                RiseSdk.Instance.ShowRewardAd ("default", 1);
                break;
            case 13:
                RiseSdk.Instance.Share ();
                break;

            case 14:
                RiseSdk.Instance.Login ();
                break;

            case 15:
                Debug.LogError ("is login: " + RiseSdk.Instance.IsLogin ());
                break;

            case 16:
                RiseSdk.Instance.Logout ();
                break;

            case 17:
                RiseSdk.Instance.Like ();
                break;

            case 19:
                RiseSdk.Instance.Challenge ("your see", "speed coming...");
                break;

            case 18:
                RiseSdk.Instance.Invite ();
                break;

            case 20:
                string mestring = RiseSdk.Instance.Me ();
                object me = MiniJSON.jsonDecode (mestring);
                if (me == null) {
                    Debug.LogError ("me is null");
                } else {
                    Debug.LogError ("me is: " + me);
                }
                break;

            case 21:
                string friendstring = RiseSdk.Instance.GetFriends ();
                object friends = MiniJSON.jsonDecode (friendstring);
                Debug.LogError ("friends are: " + friends);
                break;
            /*
        case 22:
            RiseSdk.Instance.SubmitScore ("endless", 1234, "userName: haha");
            break;

        case 23:
            RiseSdk.Instance.LoadFriendLeaderBoard ("endless", 1, 32, "");
            break;

        case 24:
            RiseSdk.Instance.LoadGlobalLeaderBoard ("endless", 1, 32);
            break;
            */

            case 25:
                RiseSdk.Instance.ShowNativeAd ("lock_pre", 20);
                break;

            case 26:
                RiseSdk.Instance.HideNativeAd ("lock_pre");
                break;
            /*
        case 27:
            RiseSdk.Instance.LoadGameData (1);
            break;

        case 28:
            RiseSdk.Instance.ShowSales (1);
            break;
            */

            case 29:
                RiseSdk.Instance.CacheUrl ("http://img4.imgtn.bdimg.com/it/u=3087502007,2322343371&fm=21&gp=0.jpg");
                break;

            case 30:
                Debug.LogError ("app id is " + RiseSdk.Instance.GetConfig (RiseSdk.CONFIG_KEY_APP_ID));
                break;

            case 31:
                RiseSdk.Instance.Alert ("haha", "Very good");
                //RiseSdk.Instance.Pay (0);
                break;

            case 32:
                RiseSdk.Instance.CacheUrl (1, "http://img4.imgtn.bdimg.com/it/u=3087502007,2322343371&fm=21&gp=0.jpg");
                break;
            case 33:
                changeAvatarImg ();
                break;
        }
#elif UNITY_IOS
        switch (id) {
            case 0:
                RiseSdk.Instance.ShowAd (RiseSdk.M_START);
                break;
            case 1:
                RiseSdk.Instance.ShowAd (RiseSdk.M_PAUSE);
                break;
            case 2:
                RiseSdk.Instance.ShowAd (RiseSdk.M_PASSLEVEL);
                break;
            case 3:
                RiseSdk.Instance.ShowAd (RiseSdk.M_CUSTOM);
                break;
            case 4:
                RiseSdk.Instance.ShowBanner ("default", RiseSdk.POS_BANNER_MIDDLE_BOTTOM);
                break;
            case 5:
                RiseSdk.Instance.CloseBanner ();
                break;
            case 6:
                RiseSdk.Instance.Rate ();
                break;
            case 7:
                RiseSdk.Instance.Pay (1);
                break;
            case 8:
                RiseSdk.Instance.ShowRewardAd ("default", 1);
                break;
            case 9:
                RiseSdk.Instance.ShowIconAd (56, .2f, .2f);
                break;
        }
#endif
    }

    void OnPaymentResult (RiseSdk.PaymentResult resultCode, int billId) {
        switch (resultCode) {
            case RiseSdk.PaymentResult.Success:
                switch (billId) {
                    case 1:// the first billing Id success 
                        break;
                    case 2:// the second billing Id success
                        break;
                    case 3:
                        break;
                }
                Debug.LogError ("On billing success : " + billId);
                break;

            case RiseSdk.PaymentResult.Failed:
                switch (billId) {
                    case 1:
                        break;
                }
                Debug.LogError ("On billing failure : " + billId);
                break;

            case RiseSdk.PaymentResult.Cancel:
                break;
        }
    }

#if UNITY_ANDROID
    void OnSNSEvent (RiseSdk.SnsEventType eventType, int extra) {
        switch (eventType) {
            case RiseSdk.SnsEventType.LoginSuccess:
                Debug.LogError ("login success");
                break;
            case RiseSdk.SnsEventType.LoginFailed:
                Debug.LogError ("login failed");
                break;
            case RiseSdk.SnsEventType.InviteSuccess:
                Debug.LogError ("invite success");
                break;
            case RiseSdk.SnsEventType.InviteFailed:
                Debug.LogError ("invite failed");
                break;
            case RiseSdk.SnsEventType.LikeSuccess:
                Debug.LogError ("like success");
                break;
            case RiseSdk.SnsEventType.LikeFailed:
                Debug.LogError ("like failed");
                break;
            case RiseSdk.SnsEventType.ChallengeSuccess:
                Debug.LogError ("challenge success");
                break;
            case RiseSdk.SnsEventType.ChallengeFailed:
                Debug.LogError ("challenge failed");
                break;
        }
    }
#endif

    // Get Free coin handler
#if UNITY_ANDROID
    void GetFreeCoin (RiseSdk.AdEventType result, int rewardId, string tag) {
        if (result == RiseSdk.AdEventType.RewardAdShowFinished) {
            switch (rewardId) {
                case 1:
                    // you can add random golds, eg. 10
                    //player.gold += 10;
                    break;
            }
            Debug.LogError ("success: free coin: " + rewardId + ", " + tag);
        } else if (result == RiseSdk.AdEventType.RewardAdShowFailed) {
            Debug.LogError ("fails: free coin: " + rewardId + ", " + tag);
        }
    }
#elif UNITY_IOS
    void GetFreeCoin (RiseSdk.AdEventType result, string tag, int rewardId) {
        if (result == RiseSdk.AdEventType.RewardAdShowFinished) {
            switch (rewardId) {
                case 1:
                    // you can add random golds, eg. 10
                    //player.gold += 10;
                    break;
            }
            Debug.LogError ("RewardAdShowFinished, " + tag + ", " + rewardId);
        } else if (result == RiseSdk.AdEventType.RewardAdLoadCompleted) {
            Debug.LogError ("RewardAdLoadCompleted, " + tag + ", " + rewardId);
        } else if (result == RiseSdk.AdEventType.RewardAdLoadFailed) {
            Debug.LogError ("RewardAdLoadFailed, " + tag + ", " + rewardId);
        } else if (result == RiseSdk.AdEventType.RewardAdShowStart) {
            Debug.LogError ("RewardAdShowStart, " + tag + ", " + rewardId);
        }
    }
#endif

    /*
    void OnLeaderBoardResult(bool submit, bool success, string leaderBoardId, string extraData) {
        if (submit) {
            if (success) {
                Debug.LogError ("submit to leader board success: " + leaderBoardId);
            } else {
                Debug.LogError ("submit to leader board failure: " + leaderBoardId);
            }
        } else {
            if (success) {
                Debug.LogError ("load leader board " + leaderBoardId + " success: " + extraData);
            } else {
                Debug.LogError ("load leader board failure " + leaderBoardId);
            }
        }
    }

    void OnServerResult(int resultCode, bool success, string data) {
        switch (resultCode) {
        case RiseSdk.SERVER_RESULT_RECEIVE_GAME_DATA:
            if (success) {
                Debug.LogError ("load extra: " + data);
            } else {
                Debug.LogError ("load extra fails");
            }
            break;

        case RiseSdk.SERVER_RESULT_SALES_CLICK:
            if (success) {
                Debug.LogError ("sales click");
            } else {
                // do nothing...
            }
            break;

        case RiseSdk.SERVER_RESULT_VERIFY_CODE:
            if (success) {
                Debug.LogError ("verify code success: " + data);
            } else {
                // fails
            }
            break;
        }
    }
    */
}
