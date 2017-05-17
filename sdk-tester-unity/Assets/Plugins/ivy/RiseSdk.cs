#region Using
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
using UnityEngine.EventSystems;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif
#endregion

/// <summary>
/// SDK接口类
/// </summary>
public sealed class RiseSdk {
    private static RiseSdk _instance = null;
    private AndroidJavaClass _class = null;
    private bool paymentSystemValid = false;

    /// <summary>
    /// 计费成功标志常量
    /// </summary>
    public const int PAYMENT_RESULT_SUCCESS = 1;
    /// <summary>
    /// 计费失败标志常量
    /// </summary>
    public const int PAYMENT_RESULT_FAILS = 2;
    /// <summary>
    /// 计费取消标志常量
    /// </summary>
    public const int PAYMENT_RESULT_CANCEL = 3;

    /// <summary>
    /// 在左上角显示banner广告参数常量
    /// </summary>
    public const int POS_BANNER_LEFT_TOP = 1;
    /// <summary>
    /// 在顶部居中显示banner广告参数常量
    /// </summary>
    public const int POS_BANNER_MIDDLE_TOP = 3;
    /// <summary>
    /// 在右上角显示banner广告参数常量
    /// </summary>
    public const int POS_BANNER_RIGHT_TOP = 6;
    /// <summary>
    /// 在中间居中显示banner广告参数常量
    /// </summary>
    public const int POS_BANNER_MIDDLE_MIDDLE = 5;
    /// <summary>
    /// 在左下角显示banner广告参数常量
    /// </summary>
    public const int POS_BANNER_LEFT_BOTTOM = 2;
    /// <summary>
    /// 在底部居中显示banner广告参数常量
    /// </summary>
    public const int POS_BANNER_MIDDLE_BOTTOM = 4;
    /// <summary>
    /// 在右下角显示banner广告参数常量
    /// </summary>
    public const int POS_BANNER_RIGHT_BOTTOM = 7;

    /// <summary>
    /// 游戏打开时显示大屏广告参数常量
    /// </summary>
    public const string M_START = "start";
    /// <summary>
    /// 暂停游戏时显示大屏广告参数常量
    /// </summary>
    public const string M_PAUSE = "pause";
    /// <summary>
    /// 游戏过关时显示大屏广告参数常量
    /// </summary>
    public const string M_PASSLEVEL = "passlevel";
    /// <summary>
    /// 自定义时机显示大屏广告参数常量
    /// </summary>
    public const string M_CUSTOM = "custom";

    /// <summary>
    /// faceboook登陆成功标志常量
    /// </summary>
    public const int SNS_EVENT_LOGIN = 1;
    /// <summary>
    /// faceboook邀请好友成功标志常量
    /// </summary>
    public const int SNS_EVENT_INVITE = 2;
    /// <summary>
    /// faceboook挑战好友成功标志常量
    /// </summary>
    public const int SNS_EVENT_CHALLENGE = 3;
    /// <summary>
    /// faceboook给好友点赞成功标志常量
    /// </summary>
    public const int SNS_EVENT_LIKE = 4;

    /// <summary>
    /// 获取配置的AppId参数常量
    /// </summary>
    public const int CONFIG_KEY_APP_ID = 1;
    /// <summary>
    /// 获取配置的排行榜URL参数常量
    /// </summary>
    public const int CONFIG_KEY_LEADER_BOARD_URL = 2;
    /// <summary>
    /// 获取API Version参数常量
    /// </summary>
    public const int CONFIG_KEY_API_VERSION = 3;
    /// <summary>
    /// 获取本机屏幕宽度参数常量
    /// </summary>
    public const int CONFIG_KEY_SCREEN_WIDTH = 4;
    /// <summary>
    /// 获取本机屏幕高度参数常量
    /// </summary>
    public const int CONFIG_KEY_SCREEN_HEIGHT = 5;
    /// <summary>
    /// 获取本机语言参数常量
    /// </summary>
    public const int CONFIG_KEY_LANGUAGE = 6;
    /// <summary>
    /// 获取本机国家码参数常量
    /// </summary>
    public const int CONFIG_KEY_COUNTRY = 7;
    /// <summary>
    /// 获取应用的版本号参数常量
    /// </summary>
    public const int CONFIG_KEY_VERSION_CODE = 8;
    /// <summary>
    /// 获取应用的版本号名称参数常量
    /// </summary>
    public const int CONFIG_KEY_VERSION_NAME = 9;
    /// <summary>
    /// 获取应用的包名参数常量
    /// </summary>
    public const int CONFIG_KEY_PACKAGE_NAME = 10;

    private String BACK_HOME_ADPOS = M_CUSTOM;
    private bool BACK_HOME_AD_ENABLE = false;
    private double BACK_HOME_AD_TIME = 0;
    private bool canShowBackHomeAd = false;

    /// <summary>
    /// 广告事件类型
    /// </summary>
    public enum AdEventType : int {
        /// <summary>
        /// 大屏广告被关闭
        /// </summary>
        FullAdClosed,
        /// <summary>
        /// 大屏广告被点击
        /// </summary>
        FullAdClicked,
        /// <summary>
        /// 视频广告被关闭
        /// </summary>
        VideoAdClosed,
        /// <summary>
        /// bannner广告被点击
        /// </summary>
        BannerAdClicked,
        /// <summary>
        /// 交叉推广广告被点击
        /// </summary>
        CrossAdClicked
    }
    /*
	public const int SERVER_RESULT_RECEIVE_GAME_DATA = 1;
	public const int SERVER_RESULT_SAVE_USER_DATA = 2;
	public const int SERVER_RESULT_RECEIVE_USER_DATA = 3;
	public const int SERVER_RESULT_VERIFY_CODE = 4;
	public const int SERVER_RESULT_SALES_CLICK = 5;
	*/

    /// <summary>
    /// 配置计费系统的可用状态，SDK自动调用。
    /// </summary>
    /// <param name="valid">要配置的状态</param>
    public void SetPaymentSystemValid (bool valid) {
        paymentSystemValid = valid;
    }

    /// <summary>
    /// 单例对象。
    /// </summary>
    public static RiseSdk Instance {
        get {
            if (null == _instance)
                _instance = new RiseSdk ();
            return _instance;
        }
    }

    private RiseSdk () {
    }

    /// <summary>
    /// 初始化SDK，最好在第一个场景加载时初始化。
    /// </summary>
    public void Init () {
        RiseEditorAd.hasInit = true;
        if (_class != null)
            return;
#if UNITY_ANDROID
        try {
            RiseSdkListener.Instance.enabled = true;
            _class = new AndroidJavaClass ("com.android.client.Unity");
            if (_class != null) {
                AndroidJNIHelper.debug = true;
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
                    using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
                        _class.CallStatic ("onCreate", context);
                    }
                }
            }
        } catch (Exception e) {
            Debug.LogWarning (e.StackTrace);
            _class = null;
        }
#endif
    }

    /// <summary>
    /// 显示bannner广告。
    /// 如果有需要可以添加bannner广告被点击的回调：
    /// RiseSdkListener.OnAdEvent += 
    /// (
    /// RiseSdk.AdEventType type//广告事件类型，需要判断是否为RiseSdk.AdEventType.BannerAdClicked
    /// ) 
    /// => {to do something};
    /// </summary>
    /// <param name="tag">bannner广告tag</param>
    /// <param name="pos">bannner显示的位置，如：POS_BANNER_MIDDLE_BOTTOM为在底部居中显示</param>
    public void ShowBanner (string tag, int pos) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.ShowBanner (tag, pos);
#endif
        if (_class != null) {
            _class.CallStatic ("showBanner", tag, pos);
            Debug.LogWarning ("showBanner");
        }
    }

    /// <summary>
    /// 显示bannner广告。
    /// 如果有需要可以添加bannner广告被点击的回调：
    /// RiseSdkListener.OnAdEvent += 
    /// (
    /// RiseSdk.AdEventType type//广告事件类型，需要判断是否为RiseSdk.AdEventType.BannerAdClicked
    /// ) 
    /// => {to do something};
    /// </summary>
    /// <param name="pos">bannner显示的位置，如：POS_BANNER_MIDDLE_BOTTOM为在底部居中显示</param>
    public void ShowBanner (int pos) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.ShowBanner (pos);
#endif
        if (_class != null) {
            _class.CallStatic ("showBanner", pos);
            Debug.LogWarning ("showBanner");
        }
    }

    /// <summary>
    /// 关闭banner广告。
    /// </summary>
    public void CloseBanner () {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.CloseBanner ();
#endif
        if (_class != null)
            _class.CallStatic ("closeBanner");
    }

    /// <summary>
    /// 显示大屏广告。
    /// 如果有需要可以添加大屏广告被点击和被关闭的回调：
    /// RiseSdkListener.OnAdEvent += 
    /// (
    /// RiseSdk.AdEventType type//广告事件类型，需要判断是否为
    /// //RiseSdk.AdEventType.FullAdClosed（大屏广告被关闭）
    /// //或RiseSdk.AdEventType.FullAdClicked（大屏广告被点击）
    /// ) 
    /// => {to do something};
    /// </summary>
    /// <param name="tag">大屏广告弹出时机，如：M_PAUSE为游戏暂停时弹出</param>
    public void ShowAd (String tag) {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.ShowAd (tag);
#endif
        if (_class != null)
            _class.CallStatic ("showFullAd", tag);
    }

    /// <summary>
    /// 更多游戏接口，跳到推广的游戏列表界面。
    /// </summary>
    public void ShowMore () {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("ShowMore");
#endif
        if (_class != null)
            _class.CallStatic ("moreGame");
    }

    /// <summary>
    /// 检测视频广告是否加载完成。
    /// </summary>
    /// <returns>true完成，false失败</returns>
    public bool HasRewardAd () {
#if UNITY_EDITOR
        return true;
#endif
        if (_class != null)
            return _class.CallStatic<bool> ("hasRewardAd");
        return false;
    }

    /// <summary>
    /// 检测视频广告是否加载完成。
    /// </summary>
    /// <param name="tag">视频广告tag</param>
    /// <returns>true完成，false失败</returns>
    public bool HasRewardAd (string tag) {
#if UNITY_EDITOR
        return true;
#endif
        if (_class != null)
            return _class.CallStatic<bool> ("hasRewardAd", tag);
        return false;
    }

    /// <summary>
    /// 显示视频广告。
    /// 该接口的回调方法为：
    /// RiseSdkListener.OnRewardAdEvent += 
    /// (
    /// bool success,//是否成功显示视频广告
    /// int rewardId//视频广告调用时机
    /// ) 
    /// => {to do something};
    /// 如果有需要可以添加视频广告被关闭的回调：
    /// RiseSdkListener.OnAdEvent += 
    /// (
    /// RiseSdk.AdEventType type//广告事件类型，需要判断是否为RiseSdk.AdEventType.VideoAdClosed
    /// ) 
    /// => {to do something};
    /// </summary>
    /// <param name="rewardId">客户端自己配置的视频广告调用时机</param>
    public void ShowRewardAd (int rewardId) {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.ShowRewardAd (rewardId);
        RiseSdkListener.Instance.onReceiveReward ("true|1");
#endif
        if (_class != null)
            _class.CallStatic ("showRewardAd", rewardId);
    }

    /// <summary>
    /// 显示视频广告。
    /// 该接口的回调方法为：
    /// RiseSdkListener.OnRewardAdEvent += 
    /// (
    /// bool success,//是否成功显示视频广告
    /// int rewardId//视频广告调用时机
    /// ) 
    /// => {to do something};
    /// </summary>
    /// <param name="tag">视频广告tag</param>
    /// <param name="rewardId">客户端自己配置的视频广告调用时机</param>
    public void ShowRewardAd (string tag, int rewardId) {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.ShowRewardAd (tag, rewardId);
#endif
        if (_class != null)
            _class.CallStatic ("showRewardAd", tag, rewardId);
    }

    public void enableBackHomeAd (bool enabled, String adPos) {
        BACK_HOME_ADPOS = adPos;
        BACK_HOME_AD_ENABLE = enabled;
    }

    /// <summary>
    /// 游戏获得焦点，SDK自动调用。
    /// </summary>
    public void OnResume () {
        if (_class != null)
            _class.CallStatic ("onResume");
        if (BACK_HOME_AD_ENABLE) {
            if (canShowBackHomeAd && BACK_HOME_AD_TIME <= 0) {
                canShowBackHomeAd = false;
                ShowAd (BACK_HOME_ADPOS);
            }
        }
    }

    /// <summary>
    /// 游戏失去焦点，SDK自动调用。
    /// </summary>
    public void OnPause () {
        if (_class != null)
            _class.CallStatic ("onPause");
        if (BACK_HOME_AD_ENABLE) {
            double now = GetCurrentTimeInMills ();
            double delta = now - BACK_HOME_AD_TIME;
            canShowBackHomeAd = delta > 500;
            if (canShowBackHomeAd)
                BACK_HOME_AD_TIME = 0;
        }
    }

    /// <summary>
    /// 游戏打开，SDK自动调用。
    /// </summary>
    public void OnStart () {
        if (_class != null)
            _class.CallStatic ("onStart");

    }

    /// <summary>
    /// 游戏退出，SDK自动调用。
    /// </summary>
    public void OnStop () {
        if (_class != null)
            _class.CallStatic ("onStop");
    }

    /// <summary>
    /// 游戏销毁，SDK自动调用。
    /// </summary>
    public void OnDestroy () {
        if (_class != null)
            _class.CallStatic ("onDestroy");
    }

    /// <summary>
    /// 需要退出游戏时调用。
    /// </summary>
    public void OnExit () {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.OnExit ();
#endif
        if (_class != null)
            _class.CallStatic ("onQuit");
    }

    /// <summary>
    /// 检测计费点是否存在。
    /// </summary>
    /// <param name="billingId">计费点id</param>
    public void HasPaid (int billingId) {
        if (_class != null) {
            _class.CallStatic ("query", billingId);
        }
    }

    /// <summary>
    /// 检测计费是否可用。
    /// </summary>
    /// <returns>true可用，false不可用</returns>
    public bool IsPayEnabled () {
        return paymentSystemValid;
    }

    /// <summary>
    /// 支付接口，对计费点进行支付。
    /// 该接口的回调方法为：
    /// RiseSdkListener.OnPaymentEvent += 
    /// (
    /// int result,//计费结果，如：PAYMENT_RESULT_SUCCESS为计费成功
    /// int billId//计费点id
    /// ) 
    /// => {to do something};
    /// </summary>
    /// <param name="billingId">计费点id</param>
    public void Pay (int billingId) {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Pay (billingId);
#endif
        if (_class != null) {
            _class.CallStatic ("pay", billingId);
        }
    }

    /// <summary>
    /// 分享游戏。
    /// </summary>
    public void Share () {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Share");
#endif
        if (_class == null)
            return;
        _class.CallStatic ("share");
    }

    /// <summary>
    /// 获取自定义json数据，该接口最好在SDK初始化完成后3秒或以上再调用。
    /// </summary>
    /// <returns>返回后台配置的自定义json数据，如：{"x":"x", "x":8, "x":{x}, "x":[x]}</returns>
    public string GetExtraData () {
        if (_class == null)
            return null;
        return _class.CallStatic<string> ("getExtraData");
    }

    /// <summary>
    /// 统计数据到Google Analytics。
    /// </summary>
    /// <param name="category">需要统计的数据分类名称</param>
    /// <param name="action">需要统计的数据属性名称</param>
    /// <param name="label">数据的属性值</param>
    /// <param name="value">一般传0</param>
    public void TrackEvent (string category, string action, string label, int value) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Track: " + category + ", action: " + action + ", label: " + label + ", value: " + value);
#endif
        if (_class == null)
            return;
        _class.CallStatic ("trackEvent", category, action, label, value);
    }

    /// <summary>
    /// 跳转到游戏评分界面。
    /// </summary>
    public void Rate () {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Rate");
#endif
        if (_class == null)
            return;
        _class.CallStatic ("rate");
    }

    /// <summary>
    /// 显示Native广告，暂时不对外开放。
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="yPercent"></param>
    public void ShowNativeAd (string tag, int yPercent) {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("ShowNativeAd");
#endif
        if (_class != null) {
            _class.CallStatic ("showNative", tag, yPercent);
        }
    }

    /// <summary>
    /// 隐藏Native广告。
    /// </summary>
    /// <param name="tag">广告tag</param>
    public void HideNativeAd (string tag) {
        if (_class != null) {
            _class.CallStatic ("hideNative", tag);
        }
    }

    /// <summary>
    /// 检测Native广告是否加载成功。
    /// </summary>
    /// <param name="tag">广告tag</param>
    /// <returns>true成功， false失败</returns>
    public bool HasNativeAd (string tag) {
        if (_class != null) {
            return _class.CallStatic<bool> ("hasNative", tag);
        } else {
            return false;
        }
    }

    /// <summary>
    /// 登陆faceboook账户。
    /// 该接口的回调方法为：
    /// RiseSdkListener.OnSNSEvent += 
    /// (
    /// bool success,//登陆是否成功
    /// int eventType,//事件类型，需要判断是否为SNS_EVENT_LOGIN
    /// int data//无数据返回
    /// ) 
    /// => {to do something};
    /// </summary>
    public void Login () {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Login");
#endif
        if (_class != null) {
            _class.CallStatic ("login");
        }
    }

    /// <summary>
    /// 检测faceboook是否已经登陆。
    /// </summary>
    /// <returns>true已登陆， false未登陆</returns>
    public bool IsLogin () {
        if (_class != null) {
            return _class.CallStatic<bool> ("isLogin");
        } else {
            return false;
        }
    }

    /// <summary>
    /// 登出faceboook账户。
    /// </summary>
    public void Logout () {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Logout");
#endif
        if (_class != null) {
            _class.CallStatic ("logout");
        }
    }

    /// <summary>
    /// 邀请faceboook好友安装游戏。
    /// 该接口的回调方法为：
    /// RiseSdkListener.OnSNSEvent += 
    /// (
    /// bool success,//发送邀请是否成功
    /// int eventType,//事件类型，这里需要判断是否为SNS_EVENT_INVITE
    /// int data//无数据返回
    /// ) 
    /// => {to do something};
    /// </summary>
    public void Invite () {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Invite");
#endif
        if (_class != null) {
            _class.CallStatic ("invite");
        }
    }

    /// <summary>
    /// 向faceboook好友发起挑战。
    /// 该接口的回调方法为：
    /// RiseSdkListener.OnSNSEvent += 
    /// (
    /// bool success,//发送挑战是否成功
    /// int eventType,//事件类型，这里需要判断是否为SNS_EVENT_CHALLENGE
    /// int data//成功挑战的好友数量
    /// ) 
    /// => {to do something};
    /// </summary>
    /// <param name="title">挑战标题</param>
    /// <param name="message">挑战内容</param>
    public void Challenge (string title, string message) {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Challenge, title: " + title + ", message: " + message);
#endif
        if (_class != null) {
            _class.CallStatic ("challenge", title, message);
        }
    }

    /// <summary>
    /// 获取我的faceboook个人信息。
    /// </summary>
    /// <returns>
    /// 返回json数据，格式为：
    /// {
    /// "id":"0000000000000000",//我的facebook账户id
    /// "name":"Me is me",//我的facebook账户名称 
    /// "picture":"/data/empty_not_exists"//我的facebook账户个人图片本地保存的绝对路径
    /// }
    /// </returns>
    public string Me () {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Me");
#endif
        if (_class != null) {
            return _class.CallStatic<string> ("me");
        } else {
            return "{}";
        }
    }

    /// <summary>
    /// 获取faceook朋友信息列表。
    /// </summary>
    /// <returns>
    /// 返回json数据，格式为：
    /// [
    /// {
    /// "id":"0000000000000001",//我的facebook好友1的账户id
    /// "name":"Friend 1",//我的facebook好友1的账户名称
    /// "picture":"/data/empty_not_exists1"//我的facebook好友1个人头像本地保存的绝对路径
    /// },
    /// {
    /// "id":"0000000000000002",//我的facebook好友2的账户id
    /// "name":"Friend 2",//我的facebook好友2的账户名称
    /// "picture":"/data/empty_not_exists2"//我的facebook好友2个人头像本地保存的绝对路径
    /// },
    /// {
    /// "id":"0000000000000003",//我的facebook好友3的账户id
    /// "name":"Friend 3",//我的facebook好友3的账户名称
    /// "picture":"/data/empty_not_exists3"//我的facebook好友3个人头像本地保存的绝对路径
    /// }
    /// ]
    /// </returns>
    public string GetFriends () {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("GetFriends");
#endif
        if (_class != null) {
            return _class.CallStatic<string> ("friends");
        } else {
            return "[]";
        }
    }

    /// <summary>
    /// facebook点赞接口。
    /// 该接口的回调方法为：
    /// RiseSdkListener.OnSNSEvent += 
    /// (
    /// bool success,//点赞是否成功
    /// int eventType,//事件类型，这里需要判断是否为SNS_EVENT_LIKE
    /// int data//无数据返回
    /// ) 
    /// => {to do something};
    /// </summary>
    public void Like () {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Like");
#endif
        if (_class != null) {
            _class.CallStatic ("like");
        }
    }

    /// <summary>
    /// 获取后台配置信息和Unity无法直接获取的一些信息。
    /// </summary>
    /// <param name="configId">需要获取的信息类型，如：CONFIG_KEY_VERSION_CODE为获取游戏的版本号</param>
    /// <returns>返回请求的信息</returns>
    public string GetConfig (int configId) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("GetConfig, configId: " + configId);
#endif
        if (_class != null) {
            return _class.CallStatic<string> ("getConfig", configId);
        } else {
            return "0";
        }
    }

    /// <summary>
    /// 下载文件，该接口无回调方法，直接返回文件保存路径。
    /// </summary>
    /// <param name="url">需要下载的文件的url地址</param>
    /// <returns>返回该文件保存在本地的绝对路径</returns>
    public string CacheUrl (string url) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("CacheUrl, url: " + url);
#endif
        if (_class != null) {
            return _class.CallStatic<string> ("cacheUrl", url);
        } else {
            return "";
        }
    }

    /// <summary>
    /// 下载文件。
    /// 该接口的回调方法为：
    /// RiseSdkListener.OnCacheUrlResult += 
    /// (
    /// bool success,//下载是否成功
    /// int tag,//客户端添加的tag
    /// string data//文件保存在本地的绝对路径
    /// ) 
    /// => {to do something};
    /// </summary>
    /// <param name="tag">客户端添加的tag</param>
    /// <param name="url">需要下载的文件的url地址</param>
    public void CacheUrl (int tag, string url) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("CacheUrl, tag: " + tag + ", url: " + url);
#endif
        if (_class != null) {
            _class.CallStatic ("cacheUrl", tag, url);
        }
    }

    /// <summary>
    /// 检测本机是否安装了app。
    /// </summary>
    /// <param name="packageName">需要检测的app的包名</param>
    /// <returns>true已安装， false未安装</returns>
    public bool HasApp (string packageName) {
        if (_class == null) {
            return false;
        } else {
            return _class.CallStatic<bool> ("hasApp", packageName);
        }
    }

    /// <summary>
    /// 打开本机上已安装的app。
    /// </summary>
    /// <param name="packageName">需要打开的app的包名</param>
    public void LaunchApp (string packageName) {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
        if (_class != null) {
            _class.CallStatic ("launchApp", packageName);
        }
    }

    /// <summary>
    /// 前往商店下载app。
    /// </summary>
    /// <param name="packageName">需要下载的app的包名</param>
    public void GetApp (string packageName) {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
        if (_class != null) {
            _class.CallStatic ("getApp", packageName);
        }
    }

    /// <summary>
    /// 获取指定app的后台配置信息和Unity无法直接获取的一些信息。
    /// </summary>
    /// <param name="packageName"></param>
    /// <param name="configId">需要获取的信息类型，如：CONFIG_KEY_VERSION_CODE为获取游戏的版本号</param>
    /// <returns>返回请求的信息</returns>
    public string GetConfig (string packageName, int configId) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("GetConfig, packageName: " + packageName + ", configId: " + configId);
#endif
        if (_class != null) {
            return _class.CallStatic<string> ("getConfig", packageName, configId);
        } else {
            return "";
        }
    }

    /// <summary>
    /// 系统的确认对话框。
    /// </summary>
    /// <param name="title">对话框标题</param>
    /// <param name="message">对话框内容</param>
    public void Alert (string title, string message) {
        BACK_HOME_AD_TIME = GetCurrentTimeInMills ();
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Alert (title, message);
#endif
        if (_class != null) {
            _class.CallStatic ("alert", title, message);
        }
    }

    /// <summary>
    /// 系统的toast提示信息。
    /// </summary>
    /// <param name="message">提示内容</param>
    public void Toast (string message) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast (message);
#endif
        if (_class != null) {
            _class.CallStatic ("toast", message);
        }
    }

    /// <summary>
    /// 检测网络是否可用。
    /// </summary>
    /// <returns>true可用， false不可用</returns>
    public bool IsNetworkConnected () {
        if (_class != null) {
            return _class.CallStatic<bool> ("isNetworkConnected");
        }
        return false;
    }

    #region Umeng

    /// <summary>
    /// 友盟统计，设置玩家等级。
    /// </summary>
    /// <param name="level">等级</param>
    public void UM_setPlayerLevel (int level) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, setPlayerLevel: " + level);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_setPlayerLevel", level);
        }
    }

    /// <summary>
    /// 友盟统计，自定义事件统计。
    /// </summary>
    /// <param name="eventId">事件id，要与在友盟后台添加的保持一致</param>
    public void UM_onEvent (String eventId) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, onEvent: " + eventId);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_onEvent", eventId);
        }
    }

    /// <summary>
    /// 友盟统计，自定义事件统计。
    /// </summary>
    /// <param name="eventId">事件id，要与在友盟后台添加的保持一致</param>
    /// <param name="eventLabel">事件标签</param>
    public void UM_onEvent (String eventId, String eventLabel) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, onEvent: " + eventId + ", label: " + eventLabel);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_onEvent", eventId, eventLabel);
        }
    }

    /// <summary>
    /// 友盟统计，自定义事件统计。
    /// </summary>
    /// <param name="eventId">事件id，要与在友盟后台添加的保持一致</param>
    /// <param name="mapStr">要统计的事件标签的键值对</param>
    public void UM_onEventValue (string eventId, Dictionary<string, string> mapStr) {
        if (_class != null) {
            AndroidJavaObject map = null;
            if (mapStr != null) {
                try {
                    map = new AndroidJavaObject ("java.util.Map");
                    foreach (KeyValuePair<string, string> pair in mapStr) {
                        map.Call<string> ("put", pair.Key, pair.Value);
                    }
                } catch (System.Exception ex) {
                    Debug.LogError ("UM_onEventValue Exception msg:\n" + ex.StackTrace);
                }
            }
            _class.CallStatic ("UM_onEventValue", map, 1);
        }
    }

    /// <summary>
    /// 友盟统计，进入某页面。
    /// </summary>
    /// <param name="pageName">页面名称</param>
    public void UM_onPageStart (String pageName) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, onPageStart: " + pageName);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_onPageStart", pageName);
        }
    }

    /// <summary>
    /// 友盟统计，离开某页面。
    /// </summary>
    /// <param name="pageName">页面名称</param>
    public void UM_onPageEnd (String pageName) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, onPageEnd: " + pageName);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_onPageEnd", pageName);
        }
    }

    /// <summary>
    /// 友盟统计，关卡开始。
    /// </summary>
    /// <param name="level">关卡名称</param>
    public void UM_startLevel (String level) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, startLevel: " + level);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_startLevel", level);
        }
    }

    /// <summary>
    /// 友盟统计，关卡失败。
    /// </summary>
    /// <param name="level">关卡名称</param>
    public void UM_failLevel (String level) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, failLevel: " + level);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_failLevel", level);
        }
    }

    /// <summary>
    /// 友盟统计，关卡胜利或结束。
    /// </summary>
    /// <param name="level">关卡名称</param>
    public void UM_finishLevel (String level) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, finishLevel: " + level);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_finishLevel", level);
        }
    }

    /// <summary>
    /// 友盟统计，游戏内付统计。
    /// </summary>
    /// <param name="money">内付的金额</param>
    /// <param name="itemName">内付购买的商品名称</param>
    /// <param name="number">内付购买的商品数量</param>
    /// <param name="price">内付购买的商品价格</param>
    public void UM_pay (double money, String itemName, int number, double price) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, pay, money: " + money + ", item: " + itemName + ", number: " + number + ", price: " + price);
#endif
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
        if (_class != null) {
            _class.CallStatic ("UM_pay", money, itemName, number, price);
        }
    }

    /// <summary>
    /// 友盟统计，购买道具统计。
    /// </summary>
    /// <param name="itemName">购买游戏中道具名称</param>
    /// <param name="count">购买道具数量</param>
    /// <param name="price">购买道具价格</param>
    public void UM_buy (String itemName, int count, double price) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, buy, item: " + itemName + ", count: " + count + ", price: " + price);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_buy", itemName, count, price);
        }
    }

    /// <summary>
    /// 友盟统计，使用道具统计。
    /// </summary>
    /// <param name="itemName">使用道具名称</param>
    /// <param name="number">使用道具数量</param>
    /// <param name="price">使用道具价格</param>
    public void UM_use (String itemName, int number, double price) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, use, item: " + itemName + ", number: " + number + ", price: " + price);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_use", itemName, number, price);
        }
    }

    /// <summary>
    /// 友盟统计，额外奖励统计。
    /// </summary>
    /// <param name="itemName">奖励道具名称</param>
    /// <param name="number">奖励道具数量</param>
    /// <param name="price">奖励道具价格</param>
    /// <param name="trigger">触发奖励的事件, 取值在 1~10 之间，“1”已经被预先定义为“系统奖励”， 2~10 需要在网站设置含义</param>
    public void UM_bonus (String itemName, int number, double price, int trigger) {
#if UNITY_EDITOR
        RiseEditorAd.EditorAdInstance.Toast ("Umeng, bonus, item: " + itemName + ", number: " + number + ", price: " + price + ", trigger: " + trigger);
#endif
        if (_class != null) {
            _class.CallStatic ("UM_bonus", itemName, number, price, trigger);
        }
    }

    #endregion

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

    public static double GetCurrentTimeInMills () {
        TimeSpan span = DateTime.Now.Subtract (new DateTime (1970, 1, 1, 0, 0, 0));
        return span.TotalMilliseconds;
    }


    /// <summary>
    /// Editor模式下的广告测试类，不可以调用该类的方法。
    /// </summary>
    private class RiseEditorAd : MonoBehaviour {

        private static RiseEditorAd _editorAdInstance = null;
        public static bool hasInit = false;
        private Rect bannerPos;
        private bool bannerShow = false;
        private string bannerContent = "";
        private bool interstitialShow = false;
        private string interstitialContent = "";
        private bool rewardShow = false;
        private string rewardContent = "";
        private float scaleWidth = 1;
        private float scaleHeight = 1;
        private int originScreenWidth = 1;
        private int originScreenHeight = 1;
        private bool toastShow = false;
        private List<string> toastList = new List<string> ();
        private GUIStyle toastStyle = null;
        private string rewardAdId = NONE_REWARD_ID;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
        private EventSystem curEvent = null;
#endif

        private const string NONE_REWARD_ID = "None";
        private const string BANNER_DEFAULT_TXT = "Banner AD: ";
        private const string INTERSTITIAL_DEFAULT_TXT = "\nInterstitial AD Test";
        private const string REWARD_DEFAULT_TXT = "Free Coin AD Test: ";
        private const int SCREEN_WIDTH = 854;
        private const int SCREEN_HEIGHT = 480;
        private const int GUI_DEPTH = -99;
        private const int BANNER_WIDTH = 320;
        private const int BANNER_HEIGHT = 50;

        void Awake () {
            if (_editorAdInstance == null) {
                _editorAdInstance = this;
            }
            DontDestroyOnLoad (gameObject);
            if (Screen.width > Screen.height) {
                originScreenWidth = SCREEN_WIDTH;
                originScreenHeight = SCREEN_HEIGHT;
            } else {
                originScreenWidth = SCREEN_HEIGHT;
                originScreenHeight = SCREEN_WIDTH;
            }
            scaleWidth = Screen.width * 1f / originScreenWidth;
            scaleHeight = Screen.height * 1f / originScreenHeight;
            toastStyle = new GUIStyle ();
            toastStyle.fontStyle = FontStyle.Bold;
            toastStyle.alignment = TextAnchor.MiddleCenter;
            toastStyle.fontSize = 30;
        }

        public static RiseEditorAd EditorAdInstance {
            get {
                if (_editorAdInstance == null) {
                    _editorAdInstance = FindObjectOfType<RiseEditorAd> () == null ? new GameObject ("RiseEditorAd").AddComponent<RiseEditorAd> () : _editorAdInstance;
                }
                if (!hasInit) {
                    Debug.LogError ("Fatal Error: \nNeed Call RiseSdk.Instance.Init () First At Initialize Scene");
                }
                return _editorAdInstance;
            }
        }

#if UNITY_EDITOR
        void OnGUI () {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
            if (curEvent == null) {
                curEvent = EventSystem.current;
            }
#endif
            GUI.depth = GUI_DEPTH;
            if (bannerShow) {
                GUI.backgroundColor = Color.green;
                GUI.color = Color.red;
                if (GUI.Button (bannerPos, bannerContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                GUI.backgroundColor = Color.green;
                if (GUI.Button (bannerPos, bannerContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                GUI.backgroundColor = Color.green;
                if (GUI.Button (bannerPos, bannerContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                GUI.backgroundColor = Color.green;
                if (GUI.Button (bannerPos, bannerContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
            }
            if (interstitialShow) {
                GUI.backgroundColor = Color.black;
                //GUI.backgroundColor = new Color (0, 0, 0, 1);
                //GUI.color = new Color (1, 0, 0, 1);
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    interstitialShow = false;
                    Instance.OnResume ();
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), interstitialContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), interstitialContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), interstitialContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), interstitialContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                GUI.backgroundColor = Color.red;
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    interstitialShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    interstitialShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    interstitialShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    interstitialShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
            }
            if (rewardShow) {
                GUI.backgroundColor = Color.black;
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    rewardShow = false;
                    Instance.OnResume ();
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
                    RewardAdCallBack ();
#endif
                }
                if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), rewardContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), rewardContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), rewardContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), rewardContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
#endif
                }
                GUI.backgroundColor = Color.red;
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    rewardShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
                    RewardAdCallBack ();
#endif
                }
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    rewardShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
                    RewardAdCallBack ();
#endif
                }
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    rewardShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
                    RewardAdCallBack ();
#endif
                }
                if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                    rewardShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                    if (EventSystem.current != null) {
                        EventSystem.current.enabled = false;
                    }
                    RewardAdCallBack ();
#endif
                }
            }
            if (toastList.Count > 0) {
                GUI.backgroundColor = Color.black;
                GUI.color = Color.red;
                //GUI.contentColor = Color.red;
                GUI.Button (new Rect ((Screen.width - 400 * scaleWidth) * .5f, Screen.height - 100 * scaleHeight, 400 * scaleWidth, 50 * scaleHeight), toastList [0]);
                GUI.Button (new Rect ((Screen.width - 400 * scaleWidth) * .5f, Screen.height - 100 * scaleHeight, 400 * scaleWidth, 50 * scaleHeight), toastList [0]);
                //GUI.Label (new Rect ((Screen.width - 200 * scaleWidth) * .5f, Screen.height - 100 * scaleHeight, 200 * scaleWidth, 50 * scaleHeight), toastList [0], toastStyle);
            }
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
            if (EventSystem.current != null) {
                EventSystem.current.enabled = true;
            } else if (curEvent != null) {
                curEvent.enabled = true;
                EventSystem.current = curEvent;
            }
#endif
        }

        void Update () {
            if (Input.GetKeyDown (KeyCode.Escape)) {
                interstitialShow = false;
                rewardShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                if (EventSystem.current != null) {
                    EventSystem.current.enabled = true;
                } else if (curEvent != null) {
                    curEvent.enabled = true;
                    EventSystem.current = curEvent;
                }
#endif
            }
        }

        private void RewardAdCallBack () {
            if (!rewardAdId.Equals (NONE_REWARD_ID)) {
                RiseSdkListener.Instance.onReceiveReward ("0|" + rewardAdId);
            }
            rewardAdId = NONE_REWARD_ID;
        }
#endif

        public void ShowBanner (int pos) {
#if UNITY_EDITOR
            bannerContent = BANNER_DEFAULT_TXT + "default";
            bannerShow = true;
            SetBannerPos (pos);
            Toast ("ShowBanner, pos: " + pos);
#endif
        }

        public void ShowBanner (string tag, int pos) {
#if UNITY_EDITOR
            bannerContent = BANNER_DEFAULT_TXT + tag;
            bannerShow = true;
            SetBannerPos (pos);
            Toast ("ShowBanner, tag: " + tag + ", pos: " + pos);
#endif
        }

        public void CloseBanner () {
#if UNITY_EDITOR
            bannerShow = false;
            Toast ("CloseBanner");
#endif
        }

        private void SetBannerPos (int pos) {
#if UNITY_EDITOR
            switch (pos) {
                case RiseSdk.POS_BANNER_LEFT_BOTTOM:
                    bannerPos = new Rect (0, Screen.height - BANNER_HEIGHT * scaleHeight, BANNER_WIDTH * scaleWidth, BANNER_HEIGHT * scaleHeight);
                    break;
                case RiseSdk.POS_BANNER_LEFT_TOP:
                    bannerPos = new Rect (0, 0, BANNER_WIDTH * scaleWidth, BANNER_HEIGHT * scaleHeight);
                    break;
                case RiseSdk.POS_BANNER_MIDDLE_BOTTOM:
                    bannerPos = new Rect ((Screen.width - BANNER_WIDTH * scaleWidth) * .5f, Screen.height - BANNER_HEIGHT * scaleHeight, BANNER_WIDTH * scaleWidth, BANNER_HEIGHT * scaleHeight);
                    break;
                case RiseSdk.POS_BANNER_MIDDLE_MIDDLE:
                    bannerPos = new Rect ((Screen.width - BANNER_WIDTH * scaleWidth) * .5f, (Screen.height - BANNER_HEIGHT * scaleHeight) * .5f, BANNER_WIDTH * scaleWidth, BANNER_HEIGHT * scaleHeight);
                    break;
                case RiseSdk.POS_BANNER_MIDDLE_TOP:
                    bannerPos = new Rect ((Screen.width - BANNER_WIDTH * scaleWidth) * .5f, 0, BANNER_WIDTH * scaleWidth, BANNER_HEIGHT * scaleHeight);
                    break;
                case RiseSdk.POS_BANNER_RIGHT_BOTTOM:
                    bannerPos = new Rect (Screen.width - BANNER_WIDTH * scaleWidth, Screen.height - BANNER_HEIGHT * scaleHeight, BANNER_WIDTH * scaleWidth, BANNER_HEIGHT * scaleHeight);
                    break;
                case RiseSdk.POS_BANNER_RIGHT_TOP:
                    bannerPos = new Rect (Screen.width - BANNER_WIDTH * scaleWidth, 0, BANNER_WIDTH * scaleWidth, BANNER_HEIGHT * scaleHeight);
                    break;
            }
#endif
        }

        public void ShowAd (string tag) {
#if UNITY_EDITOR
            interstitialShow = true;
            interstitialContent = tag + INTERSTITIAL_DEFAULT_TXT;
            Instance.OnPause ();
#endif
        }

        public void ShowRewardAd (int id) {
#if UNITY_EDITOR
            rewardShow = true;
            rewardContent = REWARD_DEFAULT_TXT + "default";
            rewardAdId = id + "";
            Instance.OnPause ();
#endif
        }

        public void ShowRewardAd (string tag, int id) {
#if UNITY_EDITOR
            rewardShow = true;
            rewardContent = REWARD_DEFAULT_TXT + tag;
            rewardAdId = id + "";
            Instance.OnPause ();
#endif
        }

        public void Pay (int billingId) {
#if UNITY_EDITOR
            switch (EditorUtility.DisplayDialogComplex ("Pay", "Pay: " + billingId, "TRY FAILURE", "NO", "YES")) {
                case 0://TRY FAILURE
                    RiseSdkListener.Instance.onPaymentFail (billingId + "");
                    break;
                case 1://NO
                    RiseSdkListener.Instance.onPaymentCanceled (billingId + "");
                    break;
                case 2://YES
                    RiseSdkListener.Instance.onPaymentSuccess (billingId + "");
                    break;
            }
#endif
        }

        private bool timeCounting = false;

        public void Toast (string msg) {
#if UNITY_EDITOR
            toastList.Add (msg);
            if (!timeCounting) {
                timeCounting = true;
                StartCoroutine (CheckToast ());
            }
#endif
        }

        private IEnumerator CheckToast (float time = 2) {
            yield return new WaitForSeconds (time);
            if (toastList.Count > 0) {
                toastList.RemoveAt (0);
            }
            if (toastList.Count > 0) {
                StartCoroutine (CheckToast ());
            } else {
                timeCounting = false;
            }
        }

        public void Alert (string title, string msg) {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog (title, msg, "NO", "OK");
#endif
        }

        public void OnExit () {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog ("Exit", "Are you sure to exit?", "YES", "NO")) {
                EditorApplication.isPlaying = false;
            }
#endif
        }

    }

}
