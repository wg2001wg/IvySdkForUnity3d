#region Using
using System;
using UnityEngine;

#endregion

/// <summary>
/// SDK接口回调类
/// </summary>
public class RiseSdkListener : MonoBehaviour {
    /// <summary>
    /// 显示视频广告的结果回调事件
    /// </summary>
    public static event Action<bool, int> OnRewardAdEvent;

    /// <summary>
    /// 支付的结果回调事件
    /// </summary>
    public static event Action<int, int> OnPaymentEvent;

    /// <summary>
    /// facebook相关操作的结果回调事件
    /// </summary>
    public static event Action<bool, int, int> OnSNSEvent;

    /// <summary>
    /// 下载文件的结果回调事件
    /// </summary>
    public static event Action<bool, int, string> OnCacheUrlResult;

    /// <submit or load, success, leader board id, extra data>
    public static event Action<bool, bool, string, string> OnLeaderBoardEvent;

    public static event Action<int, bool, string> OnReceiveServerResult;

    /// <summary>
    /// 获取后台自定义json数据的结果回调事件
    /// </summary>
    public static event Action<string> OnReceiveServerExtra;

    /// <summary>
    /// 获取后台通知栏消息的结果回调事件
    /// </summary>
    public static event Action<string> OnReceiveNotificationData;

    private static event Action<RiseSdk.AdEventType> OnAdEvent;

    private static RiseSdkListener _instance;
    private static RiseSdk riseSdk;

    /// <summary>
    /// 单例对象
    /// </summary>
    public static RiseSdkListener Instance {
        get {
            if (!_instance) {
                // check if there is a IceTimer instance already available in the scene graph
                _instance = FindObjectOfType (typeof (RiseSdkListener)) as RiseSdkListener;

                // nope, create a new one
                if (!_instance) {
                    var obj = new GameObject ("RiseSdkListener");
                    riseSdk = RiseSdk.Instance;
                    _instance = obj.AddComponent<RiseSdkListener> ();
                    DontDestroyOnLoad (obj);
                }
            }

            return _instance;
        }
    }

    void OnApplicationPause (bool pauseStatus) {
        if (pauseStatus) {
            riseSdk.OnPause ();
        }
    }

    void OnApplicationFocus (bool focusStatus) {
        if (focusStatus) {
            riseSdk.OnResume ();
        }
    }

    void OnApplicationQuit () {
        riseSdk.OnStop ();
        riseSdk.OnDestroy ();
    }

    void Awake () {
        riseSdk.OnStart ();
    }

    /// <summary>
    /// 显示视频广告的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的结果数据</param>
    public void onReceiveReward (string data) {
        string [] results = data.Split ('|');
        bool success = int.Parse (results [0]) == 0;
        int id = int.Parse (results [1]);
        if (OnRewardAdEvent != null && OnRewardAdEvent.GetInvocationList ().Length > 0) {
            OnRewardAdEvent (success, id);
        }
    }

    /// <summary>
    /// 支付成功结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="billId">计费点Id</param>
    public void onPaymentSuccess (string billId) {
        int id = int.Parse (billId);
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PAYMENT_RESULT_SUCCESS, id);
        }
    }

    /// <summary>
    /// 支付失败结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="billId">计费点Id</param>
    public void onPaymentFail (string billId) {
        int id = int.Parse (billId);
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PAYMENT_RESULT_FAILS, id);
        }
    }

    /// <summary>
    /// 支付取消结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="billId">计费点Id</param>
    public void onPaymentCanceled (string billId) {
        int id = int.Parse (billId);
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PAYMENT_RESULT_CANCEL, id);
        }
    }

    /// <summary>
    /// 设置支付系统状态，SDK自动调用。
    /// </summary>
    /// <param name="dummy"></param>
    public void onPaymentSystemValid (string dummy) {
        riseSdk.SetPaymentSystemValid (true);
    }

    /// <summary>
    /// 登陆faceboook账户的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="result">返回的结果数据</param>
    public void onReceiveLoginResult (string result) {
        int success = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_LOGIN, 0);
        }
    }

    /// <summary>
    /// 邀请faceboook好友的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="result">返回的结果数据</param>
    public void onReceiveInviteResult (string result) {
        int success = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_INVITE, 0);
        }
    }

    /// <summary>
    /// faceboook点赞的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="result">返回的结果数据</param>
    public void onReceiveLikeResult (string result) {
        int success = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_LIKE, 0);
        }
    }

    /// <summary>
    /// faceboook发起挑战的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="result">返回的结果数据</param>
    public void onReceiveChallengeResult (string result) {
        int count = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (count > 0, RiseSdk.SNS_EVENT_CHALLENGE, count);
        }
    }

    public void onSubmitSuccess (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (true, true, leaderBoardTag, "");
        }
    }

    public void onSubmitFailure (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (true, false, leaderBoardTag, "");
        }
    }

    public void onLoadSuccess (string data) {
        string [] results = data.Split ('|');
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (false, true, results [0], results [1]);
        }
    }

    public void onLoadFailure (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (false, false, leaderBoardTag, "");
        }
    }

    public void onServerResult (string data) {
        string [] results = data.Split ('|');
        int resultCode = int.Parse (results [0]);
        bool success = int.Parse (results [1]) == 0;
        if (OnReceiveServerResult != null && OnReceiveServerResult.GetInvocationList ().Length > 0) {
            OnReceiveServerResult (resultCode, success, results [2]);
        }
    }

    /// <summary>
    /// 下载文件结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onCacheUrlResult (string data) {
        //tag,success,name
        string [] results = data.Split ('|');
        int tag = int.Parse (results [0]);
        bool success = int.Parse (results [1]) == 0;
        if (OnCacheUrlResult != null && OnCacheUrlResult.GetInvocationList ().Length > 0) {
            if (success) {
                OnCacheUrlResult (true, tag, results [2]);
            } else {
                OnCacheUrlResult (false, tag, "");
            }
        }
    }

    /// <summary>
    /// 获取后台配置的自定义json数据的回调。当SDK初始化完成，第一次取到数据后会自动调用该方法，如果需要可以提前添加监听。
    /// </summary>
    /// <param name="data">返回后台配置的自定义json数据，如：{"x":"x", "x":8, "x":{x}, "x":[x]}</param>
    public void onReceiveServerExtra (string data) {
        if (OnReceiveServerExtra != null && OnReceiveServerExtra.GetInvocationList ().Length > 0) {
            OnReceiveServerExtra (data);
        }
    }

    /// <summary>
    /// 获取到通知栏消息数据的回调。当SDK初始化完成，第一次取到数据后会自动调用该方法，如果需要可以提前添加监听。
    /// </summary>
    /// <param name="data">后台配置的数据</param>
    public void onReceiveNotificationData (string data) {
        if (OnReceiveNotificationData != null && OnReceiveNotificationData.GetInvocationList ().Length > 0) {
            OnReceiveNotificationData (data);
        }
    }

    /// <summary>
    /// 大屏广告被关闭的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onFullAdClosed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.FullAdClosed);
        }
    }

    /// <summary>
    /// 大屏广告被点击的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onFullAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.FullAdClicked);
        }
    }

    /// <summary>
    /// 视频广告被关闭的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onVideoAdClosed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.VideoAdClosed);
        }
    }

    /// <summary>
    /// banner广告被点击的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onBannerAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.BannerAdClicked);
        }
    }

    /// <summary>
    /// 交叉推广广告被点击的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onCrossAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.CrossAdClicked);
        }
    }
}
