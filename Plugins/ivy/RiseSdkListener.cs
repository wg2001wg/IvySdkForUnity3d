#region Using
using System;
using System.Collections;
using DG.Tweening.Core;
using UnityEngine;
#endregion

#if Headline
public class RiseSdkListener : MonoBehaviour
{

#if UNITY_ANDROID
    /// <summary>
    /// 大屏和视频广告的回调事件
    /// 1.RiseSdk.AdEventType
    /// 2.rewardId
    /// 3.ad tag
    /// 4.RiseSdk.ADTYPE_
    /// 5.video skipped  //max 4 param limited.
    /// </summary>
    public static event Action<RiseSdk.AdEventType, int, string, int> OnAdEvent;

    /// <summary>
    /// 抖音分享的结果回调事件
    /// </summary>
    public static event Action<RiseSdk.DouYinShareResult> OnShareEvent;

    /// <summary>
    /// 录屏的结果回调事件
    /// </summary>
    public static event Action<RiseSdk.ScreenRecordResult, string> OnRecordEvent;

    /// <summary>
    ///智能的结果回调事件
    /// </summary>
    public static event Action<RiseSdk.AIVideoEditorShareResult, string> OnAIVideoEvnet;

    /// <summary>
    ///获取照片权限的结果回调事件
    /// </summary>
    public static event Action<RiseSdk.RequestPhotoWritepermitResult> OnRequestPhotoWritepermitEvnet;

    private static RiseSdkListener _instance;

    public static RiseSdkListener Instance
    {
        get
        {
            if (!_instance)
            {
                // check if there is a IceTimer instance already available in the scene graph
                _instance = FindObjectOfType(typeof(RiseSdkListener)) as RiseSdkListener;
                // nope, create a new one
                if (!_instance)
                {
                    var obj = new GameObject("RiseSdkListener");
                    _instance = obj.AddComponent<RiseSdkListener>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 显示视频广告的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的结果数据</param>
    public void onReceiveReward(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            bool success = false;
            int id = -1;
            string tag = "Default";
            bool skippedVideo = false;
            if (!string.IsNullOrEmpty(data))
            {
                string[] results = data.Split('|');
                if (results != null && results.Length > 1)
                {
                    success = int.Parse(results[0]) == 0;
                    id = int.Parse(results[1]);
                    if (results.Length > 2)
                    {
                        tag = results[2];
                        if (results.Length > 3)
                        {
                            skippedVideo = int.Parse(results[3]) == 0 ? true : false;
                        }
                    }
                }
            }
            if (success)
            {
                OnAdEvent(RiseSdk.AdEventType.RewardAdShowFinished, id, tag, RiseSdk.ADTYPE_VIDEO);
            }
            else
            {
                OnAdEvent(RiseSdk.AdEventType.RewardAdLoadFailed, id, tag, RiseSdk.ADTYPE_VIDEO);
            }
        }
    }

    /// <summary>
    /// 大屏广告被关闭的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onFullAdClosed(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            if (!string.IsNullOrEmpty(data))
            {
                string[] msg = data.Split('|');
                if (msg != null && msg.Length > 0)
                {
                    tag = msg[0];
                }
            }
            OnAdEvent(RiseSdk.AdEventType.FullAdClosed, -1, tag, RiseSdk.ADTYPE_INTERTITIAL);
        }
    }

    /// <summary>
    /// 大屏广告被点击的回调方法，SDK自动调用。    
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onFullAdClicked(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            if (!string.IsNullOrEmpty(data))
            {
                string[] msg = data.Split('|');
                if (msg != null && msg.Length > 0)
                {
                    tag = msg[0];
                }
            }
            OnAdEvent(RiseSdk.AdEventType.FullAdClicked, -1, tag, RiseSdk.ADTYPE_INTERTITIAL);
        }
    }

    /// <summary>
    /// 大屏广告展示成功的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onAdShow(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            int type = RiseSdk.ADTYPE_INTERTITIAL;
            if (!string.IsNullOrEmpty(data))
            {
                string[] msg = data.Split('|');
                if (msg != null && msg.Length > 1)
                {
                    int.TryParse(msg[0], out type);
                    tag = msg[1];
                }
            }
            RiseSdk.AdEventType eventType = RiseSdk.AdEventType.FullAdClicked;
            switch (type)
            {
                case RiseSdk.ADTYPE_INTERTITIAL:
                    eventType = RiseSdk.AdEventType.FullAdShown;
                    break;
                case RiseSdk.ADTYPE_VIDEO:
                    eventType = RiseSdk.AdEventType.RewardAdShown;
                    break;
                case RiseSdk.ADTYPE_BANNER:
                case RiseSdk.ADTYPE_ICON:
                case RiseSdk.ADTYPE_NATIVE:
                    eventType = RiseSdk.AdEventType.AdShown;
                    break;
            }
            OnAdEvent(eventType, -1, tag, type);
        }
    }

    /// <summary>
    /// 大屏广告被点击的回调方法，SDK自动调用。    
    /// </summary
    /// <param name="data">返回的数据</param>
    public void onAdClicked(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            int adType = RiseSdk.ADTYPE_INTERTITIAL;
            if (!string.IsNullOrEmpty(data))
            {
                string[] msg = data.Split('|');
                if (msg != null && msg.Length > 1)
                {
                    int.TryParse(msg[0], out adType);
                    tag = msg[1];
                }
            }
            RiseSdk.AdEventType eventType = RiseSdk.AdEventType.FullAdClicked;
            switch (adType)
            {
                case RiseSdk.ADTYPE_INTERTITIAL:
                    eventType = RiseSdk.AdEventType.FullAdClicked;
                    break;
                case RiseSdk.ADTYPE_VIDEO:
                    eventType = RiseSdk.AdEventType.RewardAdClicked;
                    break;
                case RiseSdk.ADTYPE_BANNER:
                    eventType = RiseSdk.AdEventType.BannerAdClicked;
                    break;
                case RiseSdk.ADTYPE_ICON:
                    eventType = RiseSdk.AdEventType.IconAdClicked;
                    break;
                case RiseSdk.ADTYPE_NATIVE:
                    eventType = RiseSdk.AdEventType.NativeAdClicked;
                    break;
            }
            //OnAdEvent (RiseSdk.AdEventType.AdClicked, -1, tag, adType);
            OnAdEvent(eventType, -1, tag, adType);
        }
    }

    /// <summary>
    /// 视频广告被关闭的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onVideoAdClosed(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            if (!string.IsNullOrEmpty(data))
            {
                string[] msg = data.Split('|');
                if (msg != null && msg.Length > 0)
                {
                    tag = msg[0];
                }
            }
            OnAdEvent(RiseSdk.AdEventType.RewardAdClosed, -1, tag, RiseSdk.ADTYPE_VIDEO);
        }
    }

    /// <summary>
    /// banner广告被点击的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onBannerAdClicked(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            if (!string.IsNullOrEmpty(data))
            {
                string[] msg = data.Split('|');
                if (msg != null && msg.Length > 0)
                {
                    tag = msg[0];
                }
            }
            OnAdEvent(RiseSdk.AdEventType.BannerAdClicked, -1, tag, RiseSdk.ADTYPE_BANNER);
        }
    }

    /// <summary>
    /// 交叉推广广告被点击的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onCrossAdClicked(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            if (!string.IsNullOrEmpty(data))
            {
                string[] msg = data.Split('|');
                if (msg != null && msg.Length > 0)
                {
                    tag = msg[0];
                }
            }
            OnAdEvent(RiseSdk.AdEventType.CrossAdClicked, -1, tag, RiseSdk.ADTYPE_OTHER);
        }
    }

    public void onRecordingStoped(string data)
    {
        if (OnRecordEvent != null && OnRecordEvent.GetInvocationList().Length > 0)
        {
            if (!string.IsNullOrEmpty(data))
                OnRecordEvent(RiseSdk.ScreenRecordResult.Success, data);
            else
                OnRecordEvent(RiseSdk.ScreenRecordResult.Fail, data);
        }
    }

    //public void onRecordingProgress(string data)
    //{
    //    if (OnAIVideoEvnet != null && OnAIVideoEvnet.GetInvocationList().Length > 0)
    //    {
    //        Debug.LogError("onRecordingProgress:" + Time.frameCount);
    //        OnRecordEvent(RiseSdk.ScreenRecordResult.EditingProgress, data);
    //    }
    //}

    public void onEditVideoComplete(string data)
    {
        if (OnAIVideoEvnet != null && OnAIVideoEvnet.GetInvocationList().Length > 0)
        {
            OnAIVideoEvnet(RiseSdk.AIVideoEditorShareResult.Success, data);
        }
    }

    public void onSharePhotosSuccess(string data)
    {
        if (OnShareEvent != null && OnShareEvent.GetInvocationList().Length > 0)
        {
            OnShareEvent(RiseSdk.DouYinShareResult.Success);
        }
    }

    public void onSharePhotosFailure(string data)
    {
        if (OnShareEvent != null && OnShareEvent.GetInvocationList().Length > 0)
        {
            OnShareEvent(RiseSdk.DouYinShareResult.Fail);
        }
    }

    public void onShareVideosSuccess(string data)
    {
        if (OnShareEvent != null && OnShareEvent.GetInvocationList().Length > 0)
        {
            OnShareEvent(RiseSdk.DouYinShareResult.Success);
        }
    }

    public void onShareVideosFailure(string data)
    {
        if (OnShareEvent != null && OnShareEvent.GetInvocationList().Length > 0)
        {
            OnShareEvent(RiseSdk.DouYinShareResult.Fail);
        }
    }

    public void onRecordVideoEditComplete(string data)
    {
        if (OnAIVideoEvnet != null && OnAIVideoEvnet.GetInvocationList().Length > 0)
        {
            OnAIVideoEvnet(RiseSdk.AIVideoEditorShareResult.RecordVideoEditSuccess, data);
        }
    }

    public void requestPhotoWritePermissionResult(string data)
    {
        Debug.LogError("requestPhotoWritepermitResult====" + data);
        if (OnRequestPhotoWritepermitEvnet != null && OnRequestPhotoWritepermitEvnet.GetInvocationList().Length > 0)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (data.Equals("0"))
                {
                    OnRequestPhotoWritepermitEvnet(RiseSdk.RequestPhotoWritepermitResult.Fail);
                }
                else if (data.Equals("1"))
                {
                    OnRequestPhotoWritepermitEvnet(RiseSdk.RequestPhotoWritepermitResult.Success);
                }
            }
        }
    }
#endif

#if UNITY_IOS
    /// <summary>
    /// 大屏和视频广告的回调事件
    /// 1.RiseSdk.AdEventType
    /// 2.rewardId
    /// 3.ad tag
    /// 4.RiseSdk.ADTYPE_
    /// 5.video skipped  //max 4 param limited.
    /// </summary>
    public static event Action<RiseSdk.AdEventType, int, string, int> OnAdEvent;
 
    /// <summary>
    /// 抖音分享的结果回调事件
    /// </summary>
    public static event Action<RiseSdk.DouYinShareResult> OnShareEvent;

    /// <summary>
    /// 录屏的结果回调事件
    /// </summary>
    public static event Action<RiseSdk.ScreenRecordResult, string> OnRecordEvent;

    /// <summary>
    ///智能的结果回调事件
    /// </summary>
    public static event Action<RiseSdk.AIVideoEditorShareResult, string> OnAIVideoEvnet;

	/// <summary>
	///获取照片权限的结果回调事件
	/// </summary>
	public static event Action<RiseSdk.RequestPhotoWritepermitResult> OnRequestPhotoWritepermitEvnet;

    private static RiseSdkListener _instance;
 
    public static RiseSdkListener Instance
    {
        get
        {
            if (!_instance)
            {
                // check if there is a IceTimer instance already available in the scene graph
                _instance = FindObjectOfType(typeof(RiseSdkListener)) as RiseSdkListener;
                // nope, create a new one
                if (!_instance)
                {
                    var obj = new GameObject("RiseSdkListener");
                    _instance = obj.AddComponent<RiseSdkListener>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    public void adReward(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            int placementId = -1;
            if (!string.IsNullOrEmpty(data))
            {
                string[] str = data.Split('|');
                if (str.Length == 1)
                {
                    tag = str[0];
                }
                else if (str.Length >= 2)
                {
                    tag = str[0];
                    int.TryParse(str[1], out placementId);
                }
            }
            OnAdEvent(RiseSdk.AdEventType.RewardAdShowFinished, placementId, tag, RiseSdk.ADTYPE_VIDEO);
        }
    }

    public void adLoaded(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty(data))
            {
                string[] str = data.Split('|');
                if (str.Length == 1)
                {
                    tag = str[0];
                }
                else if (str.Length >= 2)
                {
                    tag = str[0];
                    int.TryParse(str[1], out adType);
                }
            }
            OnAdEvent(RiseSdk.AdEventType.AdLoadCompleted, -1, tag, adType);
        }
    }
		

    public void adShowFailed(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty(data))
            {
                string[] str = data.Split('|');
                if (str.Length == 1)
                {
                    tag = str[0];
                }
                else if (str.Length >= 2)
                {
                    tag = str[0];
                    int.TryParse(str[1], out adType);
                }
            }
            OnAdEvent(RiseSdk.AdEventType.AdLoadFailed, -1, tag, adType);
        }
    }

    public void adDidShown(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty(data))
            {
                string[] str = data.Split('|');
                if (str.Length == 1)
                {
                    tag = str[0];
                }
                else if (str.Length >= 2)
                {
                    tag = str[0];
                    int.TryParse(str[1], out adType);
                }
            }
            OnAdEvent(RiseSdk.AdEventType.AdShown, -1, tag, adType);
        }
    }

    public void adDidClose(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty(data))
            {
                string[] str = data.Split('|');
                if (str.Length == 1)
                {
                    tag = str[0];
                }
                else if (str.Length >= 2)
                {
                    tag = str[0];
                    int.TryParse(str[1], out adType);
                }
            }
            OnAdEvent(RiseSdk.AdEventType.AdClosed, -1, tag, adType);
        }
    }

    public void adDidClick(string data)
    {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
        {
            string tag = "Default";
            int adType = RiseSdk.ADTYPE_INTERTITIAL;
            if (!string.IsNullOrEmpty(data))
            {
                string[] str = data.Split('|');
                if (str.Length == 1)
                {
                    tag = str[0];
                }
                else if (str.Length >= 2)
                {
                    tag = str[0];
                    int.TryParse(str[1], out adType);
                }
            }
            RiseSdk.AdEventType eventType = RiseSdk.AdEventType.FullAdClicked;
            switch (adType)
            {
                case RiseSdk.ADTYPE_INTERTITIAL:
                    eventType = RiseSdk.AdEventType.FullAdClicked;
                    break;
                case RiseSdk.ADTYPE_VIDEO:
                    eventType = RiseSdk.AdEventType.RewardAdClick;
                    break;
                case RiseSdk.ADTYPE_BANNER:
                    eventType = RiseSdk.AdEventType.BannerAdClicked;
                    break;
                case RiseSdk.ADTYPE_ICON:
                    eventType = RiseSdk.AdEventType.IconAdClicked;
                    break;
                case RiseSdk.ADTYPE_NATIVE:
                    eventType = RiseSdk.AdEventType.NativeAdClicked;
                    break;
            }
            OnAdEvent(eventType, -1, tag, adType);
        }
    }

    public void onRecordingStoped(string data)
    {
        if (OnRecordEvent != null && OnRecordEvent.GetInvocationList().Length > 0)
        {
            if (!string.IsNullOrEmpty(data))
                OnRecordEvent(RiseSdk.ScreenRecordResult.Success, data);
            else
                OnRecordEvent(RiseSdk.ScreenRecordResult.Fail, data);
        }
	}
        

        public void onEditVideoComplete(string data)
        {
            if (OnAIVideoEvnet != null && OnAIVideoEvnet.GetInvocationList().Length > 0)
            {
			OnAIVideoEvnet(RiseSdk.AIVideoEditorShareResult.Success, data);
            }
        }

        public void onSharePhotosSuccess(string data)
        {
            if (OnShareEvent != null && OnShareEvent.GetInvocationList().Length > 0)
            {
			OnShareEvent(RiseSdk.DouYinShareResult.Success);
            }
        }

        public void onSharePhotosFailure(string data)
        {
            if (OnShareEvent != null && OnShareEvent.GetInvocationList().Length > 0)
            {
			OnShareEvent(RiseSdk.DouYinShareResult.Fail);
            }
        }

        public void onShareVideosSuccess(string data)
        {
            if (OnShareEvent != null && OnShareEvent.GetInvocationList().Length > 0)
            {
			OnShareEvent(RiseSdk.DouYinShareResult.Success);
            }
        }

        public void onShareVideosFailure(string data)
        {
            if (OnShareEvent != null && OnShareEvent.GetInvocationList().Length > 0)
            {
			OnShareEvent(RiseSdk.DouYinShareResult.Fail);
            }
        }

        public void onRecordVideoEditComplete(string data)
        {
            if (OnAIVideoEvnet != null && OnAIVideoEvnet.GetInvocationList().Length > 0)
            {
			OnAIVideoEvnet(RiseSdk.AIVideoEditorShareResult.RecordVideoEditSuccess, data);
            }
        }

	public void requestPhotoWritePermissionResult(string data)
	{

		Debug.LogError ("requestPhotoWritepermitResult====" + data);
		if (OnRequestPhotoWritepermitEvnet != null && OnRequestPhotoWritepermitEvnet.GetInvocationList().Length > 0)
		{

			if (!string.IsNullOrEmpty(data))
			{
				if (data.Equals ("0")) {
					OnRequestPhotoWritepermitEvnet(RiseSdk.RequestPhotoWritepermitResult.Fail);
				}
				else if(data.Equals("1"))
				{
					OnRequestPhotoWritepermitEvnet(RiseSdk.RequestPhotoWritepermitResult.Success);
				}
			}
		}
	}
#endif
}
#else
/// <summary>
/// SDK接口回调类
/// </summary>
public class RiseSdkListener : MonoBehaviour {

    public static void ClearOnAdEvent()
    {
        if (OnAdEvent != null)
        {
            var list = OnAdEvent.GetInvocationList();
            for (var i = list.Length - 1; i >= 0; --i)
            {
                OnAdEvent -= list[i] as Action<RiseSdk.AdEventType, int, string, int>;
            }
        }
    }
#if UNITY_ANDROID
    /// <summary>
    /// 支付的结果回调事件
    /// </summary>
    public static event Action<RiseSdk.PaymentResult, int> OnPaymentEvent;
    
    /// <summary>
    /// 有payload的支付结果回调
    /// </summary>
    public static event Action<RiseSdk.PaymentResult, int, string> OnPaymentWithPayloadEvent;

    /// <summary>
    /// 网络状态变化回调：android返回StatusNotReachable或StatusReachable
    /// </summary>
    public static event Action<RiseSdk.NetworkStatus> OnNetworkChangedEvent;

    /// <summary>
    /// facebook相关操作的结果回调事件
    /// </summary>
    public static event Action<RiseSdk.SnsEventType, int> OnSNSEvent;

    /// <summary>
    /// 下载文件的结果回调事件
    /// </summary>
    public static event Action<bool, int, string> OnCacheUrlResult;

    /// <submit or load, success, leader board id, extra data>
    public static event Action<bool, bool, string, string> OnLeaderBoardEvent;

    public static event Action<int, bool, string> OnReceiveServerResult;

    public static event Action<string> OnReceivePaymentsPrice;

    /// <summary>
    /// 获取后台自定义json数据的结果回调事件
    /// </summary>
    public static event Action<string> OnReceiveServerExtra;

    /// <summary>
    /// 获取通知栏消息的结果回调事件
    /// </summary>
    public static event Action<string> OnReceiveNotificationData;

    public static event Action<string> OnDeepLinkReceivedEvent;
    /// <summary>
    /// 1.RiseSdk.AdEventType
    /// 2.rewardId
    /// 3.ad tag
    /// 4.RiseSdk.ADTYPE_
    /// 5.video skipped  //max 4 param limited.
    /// </summary>
    public static event Action<RiseSdk.AdEventType, int, string, int> OnAdEvent;

    public static event Action OnResumeAdEvent;

    public static event Action OnLogoutInGame;

    /// <summary>
    /// 静默登录谷歌回调：GoogleEventType
    /// </summary>
    public static event Action<RiseSdk.GoogleEventType> OnSilentLoginGoogleEvent;
    /// <summary>
    /// 主动登录谷歌回调：GoogleEventType
    /// </summary>
    public static event Action<RiseSdk.GoogleEventType,string> OnLoginGoogleEvent;

    /// <summary>
    /// 登出谷歌回调：GoogleEventType
    /// </summary>
    public static event Action<RiseSdk.GoogleEventType> OnLogoutGoogleEvent;
    /// <summary>
    /// 更新排行榜回调：GoogleEventType，LeaderBoard Id
    /// </summary>
    public static event Action<RiseSdk.GoogleEventType, string> OnUpdateLeaderBoardEvent;
    /// <summary>
    /// 更新成就回调：GoogleEventType，Achievement Id
    /// </summary>
    public static event Action<RiseSdk.GoogleEventType, string> OnUpdateAchievementEvent;
    /// <summary>
    /// 实名认证回调：Age
    /// </summary>
    public static event Action<int> OnReceiveIdCardVerifiedResultEvent;

    public static event Action<bool> OnPrivacyAcceptEvent;
    private static RiseSdkListener _instance;

    /// <summary>
    /// 单例对象
    /// </summary>
    public static RiseSdkListener Instance {
        get {
            if (_instance == null) {
                // check if there is a IceTimer instance already available in the scene graph
                _instance = FindObjectOfType (typeof (RiseSdkListener)) as RiseSdkListener;

                // nope, create a new one
                if (_instance == null) {
                    GameObject obj = new GameObject ("RiseSdkListener");
                    _instance = obj.AddComponent<RiseSdkListener> ();
                    DontDestroyOnLoad (obj);
                }
            }

            return _instance;
        }
    }

    void OnApplicationPause (bool pauseStatus) {
        if (pauseStatus) {
            RiseSdk.Instance.OnPause ();
        }
    }

    void OnApplicationFocus (bool focusStatus) {
        if (focusStatus) {
            RiseSdk.Instance.OnResume ();
        }
    }

    void OnApplicationQuit () {
        RiseSdk.Instance.OnStop ();
        RiseSdk.Instance.OnDestroy ();
    }

    void Awake () {
        RiseSdk.Instance.OnStart ();
        
    }

    private void Start()
    {
        var channel = RiseSdk.Instance.GetConfig(RiseSdk.CONFIG_KEY_CHANNEL);
        if (!string.IsNullOrEmpty(channel) && (channel.Equals("xiaomi")|| channel.Equals("uc")))
        {
            mDoCheckLogic_ = false;
        }
    }
    bool mDoCheckLogic_ = false;
    [System.Runtime.InteropServices.DllImport("c+")]
    private static extern int E77ITgnNHHS(IntPtr ptr, IntPtr obj);
    private float checkTimer = 0;
    void Update()
    {
        if(!mDoCheckLogic_)
        {
            return;
        }
        
        if (checkTimer < 100)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > 30)
            {
                checkTimer = 666;
                Check();
            }
        }
        else if (checkTimer < 1000)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > 678)
            {
                checkTimer = 6666;
                if (!signatureCheckOk)
                {
                    Application.Quit();
                }
            }
        }
    }

    public static bool signatureCheckOk = false;
    private void Check()
    {
#if !UNITY_EDITOR
        try
        {
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            if (unityClass == null)
            {
                Debug.LogError(":::UnityPlayer Class Is Null:::");
                return;
            }
            AndroidJavaObject jObj = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            if (jObj == null)
            {
                Debug.LogError(":::Unity Activity Class Is Null:::");
                return;
            }
            jObj = jObj.Call<AndroidJavaObject>("getApplicationContext");
            if (jObj == null)
            {
                Debug.LogError(":::Application Is Null:::");
                return;
            }
            try
            {
                if (E77ITgnNHHS(IntPtr.Zero, jObj.GetRawObject()) != 1)
                {
                    Application.Quit();
                }
                else
                {
                    signatureCheckOk = true;
                }
            }
            catch (System.Exception ex)
            {
                Application.Quit();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError ("RiseSdkListener Check Error:::\n" + e.Message + "\n" + e.StackTrace);
        }
#endif
    }

    [System.Runtime.InteropServices.DllImport("c+")]
    private static extern int FC0CDB0966DB7790908AD837E325C388(IntPtr ptr, IntPtr obj);
    public bool CheckDeviceRootState() {
#if !UNITY_EDITOR
        try
        {
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            if (unityClass == null)
            {
                Debug.LogError(":::UnityPlayer Class Is Null:::");
                return false;
            }
            AndroidJavaObject jObj = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            if (jObj == null)
            {
                Debug.LogError(":::Unity Activity Class Is Null:::");
                return false;
            }
            jObj = jObj.Call<AndroidJavaObject>("getApplicationContext");
            if (jObj == null)
            {
                Debug.LogError(":::Application Is Null:::");
                return false;
            }
            try {
                return FC0CDB0966DB7790908AD837E325C388(IntPtr.Zero, jObj.GetRawObject()) == 1;
            }
            catch (System.Exception ex)
            {
                Debug.LogError ("RiseSdkListener CheckDeviceRootState Error:::\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError ("RiseSdkListener CheckDeviceRootState Error:::\n" + e.Message + "\n" + e.StackTrace);
        }
#endif
        return false;
    }

    public void OnResumeAd()
    {
        if (OnResumeAdEvent != null)
            OnResumeAdEvent();
    }
    //wsq 再营销广告回调
    public void onDeepLinkReceived(string url)
    {
        if(OnDeepLinkReceivedEvent!=null&&OnDeepLinkReceivedEvent.GetInvocationList().Length>0)
        {
            OnDeepLinkReceivedEvent(url);
        }
    }

    /// <summary>
    /// 网络状态变化的回调
    /// </summary>
    /// <param name="data">返回0或1</param>
    public void onNetworkChanged(string data) {
        if (OnNetworkChangedEvent != null && OnNetworkChangedEvent.GetInvocationList ().Length > 0) {
            int status = 0;
            if (int.TryParse(data, out status)) {
                RiseSdk.NetworkStatus networkStatus = status > 0 ? RiseSdk.NetworkStatus.StatusReachable : RiseSdk.NetworkStatus.StatusNotReachable;
                OnNetworkChangedEvent (networkStatus);
            }
        }
    }
    //实名认证接口
    public void onReceiveIdCardVerifiedResult(string age)
    {
        if (OnReceiveIdCardVerifiedResultEvent != null && OnReceiveIdCardVerifiedResultEvent.GetInvocationList().Length > 0)
        {
            OnReceiveIdCardVerifiedResultEvent(int.Parse(age));
        }
    }

    public void onPrivacyAccept(string accept)
    {
        if (OnPrivacyAcceptEvent != null && OnPrivacyAcceptEvent.GetInvocationList().Length > 0)
        {
            int success = int.Parse(accept);
            OnPrivacyAcceptEvent(success==0?true:false);
        }
    }
    //目前是针对37玩 需要再浮动框中登出游戏使用
    public void onLogoutInGame()
    {
        if (OnLogoutInGame != null && OnLogoutInGame.GetInvocationList().Length > 0)
        {
            OnLogoutInGame();
        }
    }

    /// <summary>
    /// 支付成功结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="billId">计费点Id</param>
    public void onPaymentSuccess (string billId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            int id = int.Parse (billId);
            OnPaymentEvent (RiseSdk.PaymentResult.Success, id);
        }
    }
    
    /// <summary>
    /// payload支付成功结果回调，SDK自动调用。
    /// </summary>
    /// <param name="data"></param>
    public void onPaymentSuccessWithPayload(string data) {
        if (OnPaymentWithPayloadEvent != null && OnPaymentWithPayloadEvent.GetInvocationList().Length > 0) {
            if (!string.IsNullOrEmpty(data)) {
                string[] strArray = data.Split('|');
                int id = 0;
                if (strArray.Length > 1 && int.TryParse(strArray[0], out id)) {
                    OnPaymentWithPayloadEvent(RiseSdk.PaymentResult.Success, id, strArray[1]);
                }
            }
        }
    }

    /// <summary>
    /// 支付失败结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="billingId">计费点Id</param>
    public void onPaymentFail (string billingId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PaymentResult.Failed, int.Parse (billingId));
        }
        if (OnPaymentWithPayloadEvent != null && OnPaymentWithPayloadEvent.GetInvocationList ().Length > 0) {
            OnPaymentWithPayloadEvent (RiseSdk.PaymentResult.Failed, int.Parse (billingId), null);
        }
    }
    
    /// <summary>
    /// 支付取消结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="billingId">计费点Id</param>
    public void onPaymentCanceled (string billingId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PaymentResult.Cancel, int.Parse (billingId));
        }
        if (OnPaymentWithPayloadEvent != null && OnPaymentWithPayloadEvent.GetInvocationList ().Length > 0) {
            OnPaymentWithPayloadEvent (RiseSdk.PaymentResult.Cancel, int.Parse (billingId), null);
        }
    }

    /// <summary>
    /// 设置支付系统状态，SDK自动调用。
    /// </summary>
    public void onPaymentSystemError (string data) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            RiseSdk.Instance.SetPaymentSystemValid(false);
            OnPaymentEvent (RiseSdk.PaymentResult.PaymentSystemError, -1);
        }
    }

    /// <summary>
    /// 设置支付系统状态，SDK自动调用。
    /// </summary>
    public void onPaymentSystemValid(string data)
    {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList().Length > 0)
        {
            RiseSdk.Instance.SetPaymentSystemValid(true);
            OnPaymentEvent(RiseSdk.PaymentResult.PaymentSystemValid, -1);
        }
    }

    public void onReceiveBillPrices (string data) {
        if (OnReceivePaymentsPrice != null && OnReceivePaymentsPrice.GetInvocationList ().Length > 0) {
            OnReceivePaymentsPrice (data);
        }
    }

    /// <summary>
    /// 登陆faceboook账户的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="result">返回的结果数据</param>
    public void onReceiveLoginResult (string result) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            int success = int.Parse (result);
            OnSNSEvent (success == 0 ? RiseSdk.SnsEventType.LoginSuccess : RiseSdk.SnsEventType.LoginFailed, 0);
        }
    }

    /// <summary>
    /// 邀请faceboook好友的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="result">返回的结果数据</param>
    public void onReceiveInviteResult (string result) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            int success = int.Parse (result);
            OnSNSEvent (success == 0 ? RiseSdk.SnsEventType.InviteSuccess : RiseSdk.SnsEventType.InviteFailed, 0);
        }
    }

    /// <summary>
    /// faceboook点赞的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="result">返回的结果数据</param>
    public void onReceiveLikeResult (string result) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            int success = int.Parse (result);
            OnSNSEvent (success == 0 ? RiseSdk.SnsEventType.LikeSuccess : RiseSdk.SnsEventType.LikeFailed, 0);
        }
    }

    /// <summary>
    /// faceboook发起挑战的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="result">返回的结果数据</param>
    public void onReceiveChallengeResult (string result) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            int count = int.Parse (result);
            OnSNSEvent (count > 0 ? RiseSdk.SnsEventType.ChallengeSuccess : RiseSdk.SnsEventType.ChallengeFailed, count);
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
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            string[] results = data.Split ('|');
            OnLeaderBoardEvent (false, true, results[0], results[1]);
        }
    }

    public void onLoadFailure (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (false, false, leaderBoardTag, "");
        }
    }

    public void onServerResult (string data) {
        if (OnReceiveServerResult != null && OnReceiveServerResult.GetInvocationList ().Length > 0) {
            string[] results = data.Split ('|');
            int resultCode = int.Parse (results[0]);
            bool success = int.Parse (results[1]) == 0;
            OnReceiveServerResult (resultCode, success, results[2]);
        }
    }

    /// <summary>
    /// 下载文件结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onCacheUrlResult (string data) {
        if (OnCacheUrlResult != null && OnCacheUrlResult.GetInvocationList ().Length > 0) {
            //tag,success,name
            string[] results = data.Split ('|');
            int tag = int.Parse (results[0]);
            bool success = int.Parse (results[1]) == 0;
            if (success) {
                OnCacheUrlResult (true, tag, results[2]);
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
    /// 获取到通知栏消息数据的回调。当点击通知栏消息打开应用时，会自动调用该方法，如果需要可以提前添加监听。
    /// </summary>
    /// <param name="data">后台配置的数据</param>
    public void onReceiveNotificationData (string data) {
        if (OnReceiveNotificationData != null && OnReceiveNotificationData.GetInvocationList ().Length > 0) {
            OnReceiveNotificationData (data);
        }
    }

    /// <summary>
    /// 显示视频广告的结果回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的结果数据</param>
    public void onReceiveReward (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            bool success = false;
            int id = -1;
            string tag = "Default";
            bool skippedVideo = false;
            if (!string.IsNullOrEmpty (data)) {
                string[] results = data.Split ('|');
                if (results != null && results.Length > 1) {
                    success = int.Parse (results[0]) == 0;
                    id = int.Parse (results[1]);
                    if (results.Length > 2) {
                        tag = results[2];
                        if (results.Length > 3) {
                            skippedVideo = int.Parse (results[3]) == 0 ? true : false;
                        }
                    }
                }
            }
            Debug.Log($"**** onReceiveReward {data}");
            Debug.Log($"**** onReceiveReward {success}");
            if (success) {
                OnAdEvent (RiseSdk.AdEventType.RewardAdShowFinished, id, tag, RiseSdk.ADTYPE_VIDEO);
            } else {
                OnAdEvent (RiseSdk.AdEventType.RewardAdShowFailed, id, tag, RiseSdk.ADTYPE_VIDEO);
            }
        }
    }

    /// <summary>
    /// 大屏广告被关闭的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onFullAdClosed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.FullAdClosed, -1, tag, RiseSdk.ADTYPE_INTERTITIAL);
        }
    }

    /// <summary>
    /// 大屏广告被点击的回调方法，SDK自动调用。    
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onFullAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.FullAdClicked, -1, tag, RiseSdk.ADTYPE_INTERTITIAL);
        }
    }

    /// <summary>
    /// 大屏广告展示成功的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onAdShow (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int type = RiseSdk.ADTYPE_INTERTITIAL;
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 1) {
                    int.TryParse (msg[0], out type);
                    tag = msg[1];
                }
            }
            RiseSdk.AdEventType eventType = RiseSdk.AdEventType.FullAdClicked;
            switch (type)
            {
                case RiseSdk.ADTYPE_INTERTITIAL:
                    eventType = RiseSdk.AdEventType.FullAdShown;
                    break;
                case RiseSdk.ADTYPE_VIDEO:
                    eventType = RiseSdk.AdEventType.RewardAdShown;
                    break;
                case RiseSdk.ADTYPE_BANNER:
                case RiseSdk.ADTYPE_ICON:
                case RiseSdk.ADTYPE_NATIVE:
                    eventType = RiseSdk.AdEventType.AdShown;
                    break;
            }
            OnAdEvent (eventType, -1, tag, type);
        }
    }

    /// <summary>
    /// 大屏广告被点击的回调方法，SDK自动调用。    
    /// </summary
    /// <param name="data">返回的数据</param>
    public void onAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = RiseSdk.ADTYPE_INTERTITIAL;
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 1) {
                    int.TryParse (msg[0], out adType);
                    tag = msg[1];
                }
            }
            RiseSdk.AdEventType eventType = RiseSdk.AdEventType.FullAdClicked;
            switch (adType) {
                case RiseSdk.ADTYPE_INTERTITIAL:
                    eventType = RiseSdk.AdEventType.FullAdClicked;
                    break;
                case RiseSdk.ADTYPE_VIDEO:
                    eventType = RiseSdk.AdEventType.RewardAdClicked;
                    break;
                case RiseSdk.ADTYPE_BANNER:
                    eventType = RiseSdk.AdEventType.BannerAdClicked;
                    break;
                case RiseSdk.ADTYPE_ICON:
                    eventType = RiseSdk.AdEventType.IconAdClicked;
                    break;
                case RiseSdk.ADTYPE_NATIVE:
                    eventType = RiseSdk.AdEventType.NativeAdClicked;
                    break;
            }
            //OnAdEvent (RiseSdk.AdEventType.AdClicked, -1, tag, adType);
            OnAdEvent (eventType, -1, tag, adType);
        }
    }

    /// <summary>
    /// 视频广告被关闭的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onVideoAdClosed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.RewardAdClosed, -1, tag, RiseSdk.ADTYPE_VIDEO);
        }
    }

    /// <summary>
    /// banner广告被点击的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onBannerAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.BannerAdClicked, -1, tag, RiseSdk.ADTYPE_BANNER);
        }
    }

    /// <summary>
    /// 交叉推广广告被点击的回调方法，SDK自动调用。
    /// </summary>
    /// <param name="data">返回的数据</param>
    public void onCrossAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.CrossAdClicked, -1, tag, RiseSdk.ADTYPE_OTHER);
        }
    }

	/// <summary>
	/// 视频加载的回调方法，SDK自动调用。
	/// </summary>
	/// <param name="data">返回的数据</param>
	public void adLoaded(string data)
	{
		if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
		{
			string tag = "Default";
			int adType = -1;
			if (!string.IsNullOrEmpty(data))
			{
				string[] str = data.Split('|');
				if (str.Length == 1)
				{
					tag = str[0];
				}
				else if (str.Length >= 2)
				{
					tag = str[0];
					int.TryParse(str[1], out adType);
				}
			}
			OnAdEvent(RiseSdk.AdEventType.AdLoadCompleted, -1, tag, adType);
		}
	}

    public void onSilentLoginGoogle(string data)
    {
        if (OnSilentLoginGoogleEvent != null && OnSilentLoginGoogleEvent.GetInvocationList().Length > 0)
        {
            OnSilentLoginGoogleEvent(string.IsNullOrEmpty(data) || data.Equals("1") ? RiseSdk.GoogleEventType.SilentLoginFailed : RiseSdk.GoogleEventType.SilentLoginSuccess);
        }
    }

    public void onLoginGoogleSuccess(string data)
    {
        Debug.Log("onLoginGoogleSuccess:::" + data);
        if (OnLoginGoogleEvent != null && OnLoginGoogleEvent.GetInvocationList().Length > 0)
        {
            OnLoginGoogleEvent(RiseSdk.GoogleEventType.LoginSuccess,data);
        }
    }

    public void onLoginGoogleFailure() {
        Debug.Log("onLoginGoogleFailure");
        if (OnLoginGoogleEvent != null && OnLoginGoogleEvent.GetInvocationList().Length > 0)
        {
            OnLoginGoogleEvent(RiseSdk.GoogleEventType.LoginFailed,"");
        }
    }

    public void onLogoutGoogle(string data)
    {
        if (OnLogoutGoogleEvent != null && OnLogoutGoogleEvent.GetInvocationList().Length > 0)
        {
            OnLogoutGoogleEvent(string.IsNullOrEmpty(data) || data.Equals("1") ? RiseSdk.GoogleEventType.LogoutFailed : RiseSdk.GoogleEventType.LogoutSuccess);
        }
    }

    public void onUpdateLeaderBoard(string data)
    {
        if (OnUpdateLeaderBoardEvent != null && OnUpdateLeaderBoardEvent.GetInvocationList().Length > 0)
        {
            if (string.IsNullOrEmpty(data))
            {
                OnUpdateLeaderBoardEvent(RiseSdk.GoogleEventType.UpdateLeaderBoardFailed, "-1");
            }
            else
            {
                string[] msg = data.Split('|');
                if (msg != null && msg.Length > 1)
                {
                    OnUpdateLeaderBoardEvent(string.IsNullOrEmpty(msg[1]) || msg[1].Equals("1") ? RiseSdk.GoogleEventType.UpdateLeaderBoardFailed : RiseSdk.GoogleEventType.UpdateLeaderBoardSuccess, msg[0]);
                }
                else
                {
                    OnUpdateLeaderBoardEvent(RiseSdk.GoogleEventType.UpdateLeaderBoardFailed, "-1");
                }
            }
        }
    }

    public void onUpdateAchievement(string data)
    {
        if (OnUpdateAchievementEvent != null && OnUpdateAchievementEvent.GetInvocationList().Length > 0)
        {
            if (string.IsNullOrEmpty(data))
            {
                OnUpdateAchievementEvent(RiseSdk.GoogleEventType.UpdateLeaderBoardFailed, "-1");
            }
            else
            {
                string[] msg = data.Split('|');
                if (msg != null && msg.Length > 1)
                {
                    OnUpdateAchievementEvent(string.IsNullOrEmpty(msg[1]) || msg[1].Equals("1") ? RiseSdk.GoogleEventType.UpdateAchievementFailed : RiseSdk.GoogleEventType.UpdateAchievementSuccess, msg[0]);
                }
                else
                {
                    OnUpdateAchievementEvent(RiseSdk.GoogleEventType.UpdateLeaderBoardFailed, "-1");
                }
            }
        }
    }
#elif UNITY_IOS || PLATFORM_IOS
    /// <summary>
    /// 实名认证回调：Age
    /// </summary>
    public static event Action<int> OnReceiveIdCardVerifiedResultEvent;
    
    public static event Action<string> OnDeepLinkReceivedEvent;

    public static event Action<bool> OnPrivacyAcceptEvent;
    /// <summary>
    /// 大屏和视频广告的回调事件
    /// 1.RiseSdk.AdEventType
    /// 2.rewardId
    /// 3.ad tag
    /// 4.RiseSdk.ADTYPE_
    /// 5.video skipped  //max 4 param limited.
    /// </summary>
    public static event Action<RiseSdk.AdEventType, int, string, int> OnAdEvent;

    

    ///// <summary>
    ///// 视频广告的回调事件
    ///// </summary>
    //public static event Action<RiseSdk.AdEventType, int, string> OnRewardAdEvent;

    /// <summary>
    /// 支付的回调事件
    /// </summary>
    public static event Action<RiseSdk.PaymentResult, int> OnPaymentEvent;

    /// <summary>
    /// 有payload的支付结果回调
    /// </summary>
    public static event Action<RiseSdk.PaymentResult, int, string> OnPaymentWithPayloadEvent;

    /// <summary>
    /// 网络状态变化回调：ios返回StatusUnknown或StatusNotReachable或StatusReachableViaWWAN或StatusReachableViaWiFi
    /// </summary>
    public static event Action<RiseSdk.NetworkStatus> OnNetworkChangedEvent;

    public static event Action<int, long> OnCheckSubscriptionResult;
    public static event Action OnRestoreFailureEvent;
    public static event Action<int> OnRestoreSuccessEvent;
    public static event Action<RiseSdk.SnsEventType, int> OnSNSEvent;
    /// <summary>
    /// 获取后台自定义json数据的结果回调事件
    /// </summary>
    public static event Action<string> OnReceiveServerExtra;

    public static event Action<string> OnGameCenterLoginSuccessEvent;
    public static event Action OnGameCenterLoginFailureEvent;
    /// <summary>
    /// 获取通知栏消息的结果回调事件
    /// </summary>
    public static event Action<string> OnReceiveNotificationData;
    private static RiseSdkListener _instance;
    public static event Action OnResumeAdEvent;

    /// <summary>
    /// 苹果登陆结果回调事件
    /// </summary>
    public static event Action OnSignInAppleSuccessEvent;
    public static event Action OnSignInAppleFailureEvent;

    public static RiseSdkListener Instance {
        get {
            if (!_instance) {
                // check if there is a IceTimer instance already available in the scene graph
                _instance = FindObjectOfType (typeof (RiseSdkListener)) as RiseSdkListener;
                // nope, create a new one
                if (!_instance) {
                    var obj = new GameObject ("RiseSdkListener");
                    _instance = obj.AddComponent<RiseSdkListener> ();
                    DontDestroyOnLoad (obj);
                }
            }
            return _instance;
        }
    }

    public void adReward (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {

            string tag = "Default";
            int placementId = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out placementId);
                }
            }
            OnAdEvent(RiseSdk.AdEventType.RewardAdShowFinished, placementId, tag, RiseSdk.ADTYPE_VIDEO);
            Debug.Log("adReward : " + data);
        }
    }

    public void adLoaded (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out adType);
                }
            }
            OnAdEvent (RiseSdk.AdEventType.AdLoadCompleted, -1, tag, adType);
        }
    }

    public void adShowFailed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out adType);
                }
            }
            OnAdEvent (RiseSdk.AdEventType.AdLoadFailed, -1, tag, adType);
        }
    }

    public void adDidShown (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out adType);
                }
            }
            OnAdEvent (RiseSdk.AdEventType.AdShown, -1, tag, adType);
        }
    }

    public void adDidClose (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out adType);
                }
            }
            OnAdEvent (RiseSdk.AdEventType.AdClosed, -1, tag, adType);
        }
    }

    public void adDidClick (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = RiseSdk.ADTYPE_INTERTITIAL;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str[0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out adType);
                }
            }
            RiseSdk.AdEventType eventType = RiseSdk.AdEventType.FullAdClicked;
            switch (adType) {
                case RiseSdk.ADTYPE_INTERTITIAL:
                    eventType = RiseSdk.AdEventType.FullAdClicked;
                    break;
                case RiseSdk.ADTYPE_VIDEO:
                    eventType = RiseSdk.AdEventType.RewardAdClicked;
                    break;
                case RiseSdk.ADTYPE_BANNER:
                    eventType = RiseSdk.AdEventType.BannerAdClicked;
                    break;
                case RiseSdk.ADTYPE_ICON:
                    eventType = RiseSdk.AdEventType.IconAdClicked;
                    break;
                case RiseSdk.ADTYPE_NATIVE:
                    eventType = RiseSdk.AdEventType.NativeAdClicked;
                    break;
            }
            OnAdEvent (eventType, -1, tag, adType);
        }
    }

    public void onInitialized(string msg)
    {
        Debug.Log("wsq=== onInitialized!");
    }

    /// <summary>
    /// 网络状态变化的回调
    /// </summary>
    /// <param name="data">返回-1或0或1或2</param>
    public void onNetworkChanged(string data) {
        Debug.Log("wsq=== onNetworkChanged : " + data);
        if (OnNetworkChangedEvent != null && OnNetworkChangedEvent.GetInvocationList ().Length > 0) {
            int status = 0;
            if (int.TryParse(data, out status)) {
                RiseSdk.NetworkStatus networkStatus = RiseSdk.NetworkStatus.StatusNotReachable;
                try {
                    networkStatus = (RiseSdk.NetworkStatus) status;
                } catch (Exception e) {
                    Debug.LogError("parse network status error:::" + e.StackTrace);
                } finally {
                    OnNetworkChangedEvent (networkStatus);
                }
            }
        }
    }

    public void onPaymentSuccess (string billingId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PaymentResult.Success, int.Parse (billingId));
        }
    }

    /// <summary>
    /// payload支付成功结果回调，SDK自动调用。
    /// </summary>
    /// <param name="data"></param>
    public void onPaymentSuccessWithPayload(string data) {
        if (OnPaymentWithPayloadEvent != null && OnPaymentWithPayloadEvent.GetInvocationList().Length > 0) {
            if (!string.IsNullOrEmpty(data)) {
                string[] strArray = data.Split('|');
                int id = 0;
                if (strArray.Length > 1 && int.TryParse(strArray[0], out id)) {
                    OnPaymentWithPayloadEvent(RiseSdk.PaymentResult.Success, id, strArray[1]);
                }
            }
        }
    }

    public void onPaymentFailure (string billingId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PaymentResult.Failed, int.Parse (billingId));
        }
        if (OnPaymentWithPayloadEvent != null && OnPaymentWithPayloadEvent.GetInvocationList ().Length > 0) {
            OnPaymentWithPayloadEvent (RiseSdk.PaymentResult.Failed, int.Parse (billingId), null);
        }
    }

    public void onCheckSubscriptionResult (string data) {
        if (OnCheckSubscriptionResult != null && OnCheckSubscriptionResult.GetInvocationList ().Length > 0) {
            int billingId = -1;
            long remainSeconds = 0;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split (',');
                if (str.Length >= 2) {
                    billingId = int.Parse (str[0]);
                    remainSeconds = long.Parse (str[1]);
                }
            }
            OnCheckSubscriptionResult (billingId, remainSeconds);
        }
    }

    public void onRestoreFailure (string error) {
        if (OnRestoreFailureEvent != null && OnRestoreFailureEvent.GetInvocationList ().Length > 0) {
            OnRestoreFailureEvent ();
        }
    }

    public void onRestoreSuccess (string billingId) {
        if (OnRestoreSuccessEvent != null && OnRestoreSuccessEvent.GetInvocationList ().Length > 0) {
            OnRestoreSuccessEvent (int.Parse (billingId));
        }
    }
    //wsq 用于国内ios登陆使用
    public void onReceiveLoginResult (string result) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            int success = int.Parse (result);
            OnSNSEvent (success == 0 ? RiseSdk.SnsEventType.LoginSuccess : RiseSdk.SnsEventType.LoginFailed, 0);
        }
    }

    public void snsShareSuccess (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.ShareSuccess, 0);
        }
    }

    public void snsShareFailure (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.ShareFailed, 0);
        }
    }

    public void snsShareCancel (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.ShareCancel, 0);
        }
    }

    public void snsLoginSuccess (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.LoginSuccess, 0);
        }
    }

    public void snsLoginFailure (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.LoginFailed, 0);
        }
    }
     /// <summary>
    /// 获取后台配置的自定义json数据的回调。当SDK初始化完成，第一次取到数据后会自动调用该方法，如果需要可以提前添加监听。
    /// </summary>
    /// <param name="data">后台配置的自定义json数据，如：{"x":"x", "x":8, "x":{x}, "x":[x]}</param>
    public void onReceiveServerExtra (string data) {
        if (OnReceiveServerExtra != null && OnReceiveServerExtra.GetInvocationList ().Length > 0) {
            OnReceiveServerExtra (data);
        }
    }

    /// <summary>
    /// 获取到通知栏消息数据的回调。当点击通知栏消息打开应用时，会自动调用该方法，如果需要可以提前添加监听。
    /// </summary>
    /// <param name="data">后台配置的数据</param>
    public void onReceiveNotificationData (string data) {
        if (OnReceiveNotificationData != null && OnReceiveNotificationData.GetInvocationList ().Length > 0) {
            OnReceiveNotificationData (data);
        }
    }

    public void onGameCenterLoginSuccess(string playerId)
    {
        if (OnGameCenterLoginSuccessEvent != null && OnGameCenterLoginSuccessEvent.GetInvocationList ().Length > 0) {
            OnGameCenterLoginSuccessEvent (playerId);
        }
    }
    
    public void onGameCenterLoginFailure(string msg)
    {
        if (OnGameCenterLoginFailureEvent != null && OnGameCenterLoginFailureEvent.GetInvocationList ().Length > 0) {
            OnGameCenterLoginFailureEvent ();
        }
    }
    
    //实名认证接口
    public void onReceiveIdCardVerifiedResult(string age)
    {
        if (OnReceiveIdCardVerifiedResultEvent != null && OnReceiveIdCardVerifiedResultEvent.GetInvocationList().Length > 0)
        {
            OnReceiveIdCardVerifiedResultEvent(int.Parse(age));
        }
    }

    public void onPrivacyAccept(string accept)
    {
        if (OnPrivacyAcceptEvent != null && OnPrivacyAcceptEvent.GetInvocationList().Length > 0)
        {
            int success = int.Parse(accept);
            OnPrivacyAcceptEvent(success==0?true:false);
        }
    }
    
    //wsq 再营销广告回调
    public void onDeepLinkReceived(string url)
    {
        if(OnDeepLinkReceivedEvent!=null&&OnDeepLinkReceivedEvent.GetInvocationList().Length>0)
        {
            OnDeepLinkReceivedEvent(url);
        }
    }

    public void signInAppleSuccess(string msg)
    {
        if(OnSignInAppleSuccessEvent != null && OnSignInAppleSuccessEvent.GetInvocationList().Length>0)
        {
            OnSignInAppleSuccessEvent();
        }
    }

    public void signInAppleFailure(string msg)
    {
        if(OnSignInAppleFailureEvent != null && OnSignInAppleFailureEvent.GetInvocationList().Length>0)
        {
            OnSignInAppleFailureEvent();
        }
    }
#endif
}
#endif