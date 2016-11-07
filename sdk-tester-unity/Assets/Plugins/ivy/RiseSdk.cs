#region Using
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion

public sealed class RiseSdk
{
    private static RiseSdk _instance = null;
    private AndroidJavaClass _class = null;
    private bool paymentSystemValid = false;

    public const int PAYMENT_RESULT_SUCCESS = 1;
    public const int PAYMENT_RESULT_FAILS = 2;
    public const int PAYMENT_RESULT_CANCEL = 3;

    public const int POS_BANNER_LEFT_TOP = 1;
    public const int POS_BANNER_MIDDLE_TOP = 3;
    public const int POS_BANNER_RIGHT_TOP = 6;
    public const int POS_BANNER_MIDDLE_MIDDLE = 5;
    public const int POS_BANNER_LEFT_BOTTOM = 2;
    public const int POS_BANNER_MIDDLE_BOTTOM = 4;
    public const int POS_BANNER_RIGHT_BOTTOM = 7;

    public const string M_START = "start";
    public const string M_PAUSE = "pause";
    public const string M_PASSLEVEL = "passlevel";
    public const string M_CUSTOM = "custom";

    public const int SNS_EVENT_LOGIN = 1;
    public const int SNS_EVENT_INVITE = 2;
    public const int SNS_EVENT_CHALLENGE = 3;
    public const int SNS_EVENT_LIKE = 4;

    public const int CONFIG_KEY_APP_ID = 1;
    public const int CONFIG_KEY_LEADER_BOARD_URL = 2;
    public const int CONFIG_KEY_API_VERSION = 3;
    public const int CONFIG_KEY_SCREEN_WIDTH = 4;
    public const int CONFIG_KEY_SCREEN_HEIGHT = 5;
    public const int CONFIG_KEY_LANGUAGE = 6;
    public const int CONFIG_KEY_COUNTRY = 7;
    public const int CONFIG_KEY_VERSION_CODE = 8;
    public const int CONFIG_KEY_VERSION_NAME = 9;
    public const int CONFIG_KEY_PACKAGE_NAME = 10;
    /*
	public const int SERVER_RESULT_RECEIVE_GAME_DATA = 1;
	public const int SERVER_RESULT_SAVE_USER_DATA = 2;
	public const int SERVER_RESULT_RECEIVE_USER_DATA = 3;
	public const int SERVER_RESULT_VERIFY_CODE = 4;
	public const int SERVER_RESULT_SALES_CLICK = 5;
	*/

    public void SetPaymentSystemValid(bool valid)
    {
        paymentSystemValid = valid;
    }

    public static RiseSdk Instance
    {
        get
        {
            if (null == _instance)
                _instance = new RiseSdk();
            return _instance;
        }
    }

    private RiseSdk()
    {
    }

    public void Init()
    {
        if (_class != null)
            return;
        #if UNITY_ANDROID
        try
        {
            RiseSdkListener.Instance.enabled = true;
            _class = new AndroidJavaClass("com.android.client.Unity");
            if (_class != null)
            {
                AndroidJNIHelper.debug = true;
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        _class.CallStatic("onCreate", context);
                    }
                    ;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.StackTrace);
            _class = null;
        }
        #endif
    }

    public void ShowBanner(string tag, int pos)
    {
        if (_class != null)
            _class.CallStatic("showBanner", tag, pos);
    }

    public void ShowBanner(int pos)
    {
        if (_class != null)
            _class.CallStatic("showBanner", pos);
    }

    public void CloseBanner()
    {
        if (_class != null)
            _class.CallStatic("closeBanner");
    }

    public void ShowAd(String tag)
    {
        if (_class != null)
            _class.CallStatic("showFullAd", tag);
    }

    public void ShowMore()
    {
        if (_class != null)
            _class.CallStatic("moreGame");
    }

    public bool HasRewardAd()
    {
        if (_class != null)
            return _class.CallStatic<bool>("hasRewardAd");
        return false;
    }

    public bool HasRewardAd(string tag)
    {
        if (_class != null)
            return _class.CallStatic<bool>("hasRewardAd", tag);
        return false;
    }

    public void ShowRewardAd(int rewardId)
    {
        if (_class != null)
            _class.CallStatic("showRewardAd", rewardId);
    }

    public void ShowRewardAd(string tag, int rewardId)
    {
        if (_class != null)
            _class.CallStatic("showRewardAd", tag, rewardId);
    }

    public void OnResume()
    {
        if (_class != null)
            _class.CallStatic("onResume");
    }

    public void OnPause()
    {
        if (_class != null)
            _class.CallStatic("onPause");
    }

    public void OnStart()
    {
        if (_class != null)
            _class.CallStatic("onStart");
    }

    public void OnStop()
    {
        if (_class != null)
            _class.CallStatic("onStop");
    }

    public void OnDestroy()
    {
        if (_class != null)
            _class.CallStatic("onDestroy");
    }

    public void OnExit()
    {
        if (_class != null)
            _class.CallStatic("onQuit");
    }

    public void HasPaid(int billingId)
    {
        if (_class != null)
        {
            _class.CallStatic("query", billingId);
        }
    }

    public bool IsPayEnabled()
    {
        return paymentSystemValid;
    }

    public void Pay(int billingId)
    {
        if (_class != null)
        {
            _class.CallStatic("pay", billingId);
        }
    }

    public void Share()
    {
        if (_class == null)
            return;
        _class.CallStatic("share");
    }

    public string GetExtraData()
    {
        if (_class == null)
            return null;
        return _class.CallStatic<string>("getExtraData");
    }

    public void TrackEvent(string category, string action, string label, int value)
    {
        if (_class == null)
            return;
        _class.CallStatic("trackEvent", category, action, label, value);
    }

    public void Rate()
    {
        if (_class == null)
            return;
        _class.CallStatic("rate");
    }

    public void ShowNativeAd(string tag, int yPercent)
    {
        if (_class != null)
        {
            _class.CallStatic("showNative", tag, yPercent);
        }
    }

    public void HideNativeAd(string tag)
    {
        if (_class != null)
        {
            _class.CallStatic("hideNative", tag);
        }
    }

    public bool HasNativeAd(string tag)
    {
        if (_class != null)
        {
            return _class.CallStatic<bool>("hasNative", tag);
        }
        else
        {
            return false;
        }
    }

    public void Login()
    {
        if (_class != null)
        {
            _class.CallStatic("login");
        }
    }

    public bool IsLogin()
    {
        if (_class != null)
        {
            return _class.CallStatic<bool>("isLogin");
        }
        else
        {
            return false;
        }
    }

    public void Logout()
    {
        if (_class != null)
        {
            _class.CallStatic("logout");
        }
    }

    public void Invite()
    {
        if (_class != null)
        {
            _class.CallStatic("invite");
        }
    }

    public void Challenge(string title, string message)
    {
        if (_class != null)
        {
            _class.CallStatic("challenge", title, message);
        }
    }

    public string Me()
    {
        if (_class != null)
        {
            return _class.CallStatic<string>("me");
        }
        else
        {
            return "{}";
        }
    }

    public string GetFriends()
    {
        if (_class != null)
        {
            return _class.CallStatic<string>("friends");
        }
        else
        {
            return "[]";
        }
    }

    public void Like()
    {
        if (_class != null)
        {
            _class.CallStatic("like");
        }
    }

    public string GetConfig(int configId)
    {
        if (_class != null)
        {
            return _class.CallStatic<string>("getConfig", configId);
        }
        else
        {
            return "0";
        }
    }

    public string CacheUrl(string url)
    {
        if (_class != null)
        {
            return _class.CallStatic<string>("cacheUrl", url);
        }
        else
        {
            return "";
        }
    }

    public void CacheUrl(int tag, string url)
    {
        if (_class != null)
        {
            _class.CallStatic("cacheUrl", tag, url);
        } 
    }

    public bool HasApp(string packageName)
    {
        if (_class == null)
        {
            return false;
        }
        else
        {
            return _class.CallStatic<bool>("hasApp", packageName);
        }
    }

    public void LaunchApp(string packageName)
    {
        if (_class != null)
        {
            _class.CallStatic("launchApp", packageName);
        }
    }

    public void GetApp(string packageName)
    {
        if (_class != null)
        {
            _class.CallStatic("getApp", packageName);
        }
    }

    public string GetConfig(string packageName, int configId)
    {
        if (_class != null)
        {
            return _class.CallStatic<string>("getConfig", packageName, configId);
        }
        else
        {
            return "";
        }
    }

    public void Alert(string title, string message)
    {
        if (_class != null)
        {
            _class.CallStatic("alert", title, message);
        }
    }

    /**
     * 设置玩家等级
     * @param level
     */
    public void UM_setPlayerLevel(int level)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_setPlayerLevel", level);
        }
    }

    public void UM_onEvent(String eventId)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_onEvent", eventId);
        }
    }

    public void UM_onEvent(String eventId, String eventLabel)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_onEvent", eventId, eventLabel);
        }
    }


    /**
     * 进入某页面
     *
     * @param pageName 页面名称
     */
    public void UM_onPageStart(String pageName)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_onPageStart", pageName);
        }
    }

    /**
     * 离开某页面
     *
     * @param pageName 页面名称
     */
    public void UM_onPageEnd(String pageName)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_onPageEnd", pageName);
        }
    }

    /**
     * 关卡开始
     *
     * @param level 关卡名称
     */
    public void UM_startLevel(String level)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_startLevel", level);
        }
    }

    /**
     * 关卡失败
     *
     * @param level 关卡名称
     */
    public void UM_failLevel(String level)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_failLevel", level);
        }
    }

    /**
     * 关卡结束
     *
     * @param level 关卡名称
     */
    public void UM_finishLevel(String level)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_finishLevel", level);
        }
    }

    /**
     * 游戏内付统计
     *
     * @param money    内付的金额
     * @param itemName 内付购买的商品名称
     * @param number   内付购买的商品数量
     * @param price    内付购买的商品价格
     */
    public void UM_pay(double money, String itemName, int number, double price)
    {
        /**
         * 最后一个参数是支付渠道：
         * 1    Google Play
         * 2    支付宝
         * 3    网银
         * 4    财付通
         * 5    移动通信
         * 6    联通通信
         * 7    电信通信
         * 8    paypal
         */
        if (_class != null)
        {
            _class.CallStatic("UM_pay", money, itemName, number, price);
        }
    }

    /**
     * 购买道具统计
     *
     * @param itemName 购买游戏中道具名称
     * @param count    购买道具数量
     * @param price    购买道具价格
     */
    public void UM_buy(String itemName, int count, double price)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_buy", itemName, count, price);
        }
    }

    /**
     * 使用道具统计
     *
     * @param itemName 使用道具名称
     * @param number   使用道具数量
     * @param price    使用道具价格
     */
    public void UM_use(String itemName, int number, double price)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_use", itemName, number, price);
        }
    }

    /**
     * 额外奖励统计
     *
     * @param itemName 奖励道具名称
     * @param number   奖励道具数量
     * @param price    奖励道具价格
     * @param trigger  触发奖励的事件, 取值在 1~10 之间，“1”已经被预先定义为“系统奖励”， 2~10 需要在网站设置含义
     */
    public void UM_bonus(String itemName, int number, double price, int trigger)
    {
        if (_class != null)
        {
            _class.CallStatic("UM_bonus", itemName, number, price, trigger);
        }
    }

    /*
	public void SubmitScore(string leaderBoardId, long score, string extra) {
		if (_class != null) {
			_class.CallStatic ("submitScore", leaderBoardId, score, extra);
		}
	}

	public void LoadFriendLeaderBoard(string leaderBoardId, int start, int end, string friends) {
		if (_class != null) {
			_class.CallStatic ("loadLeaderBoard", leaderBoardId, start, end, friends);
		}
	}

	public void LoadGlobalLeaderBoard(string leaderBoardId, int start, int end) {
		if (_class != null) {
			_class.CallStatic ("loadGlobalLeaderBoard", leaderBoardId, start, end);
		}
	}

	public void LoadGameData(int version) {
		if (_class != null) {
			_class.CallStatic ("loadExtra", version);
		}
	}

	public void LoadUserData() {
		if (_class != null) {
			_class.CallStatic ("loadData");
		}
	}

	public void SaveUserData(string data) {
		if (_class != null) {
			_class.CallStatic ("saveData", data);
		}
	}

	public void VerifyCode(string code) {
		if (_class != null) {
			_class.CallStatic ("verifyCode", code);
		}
	}

	public void ShowSales(int saleId) {
		if (_class != null) {
			_class.CallStatic ("showSales", saleId);
		}
	}*/
}
