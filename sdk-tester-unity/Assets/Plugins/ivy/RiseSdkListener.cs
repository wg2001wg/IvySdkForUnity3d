#region Using
using System;
using UnityEngine;

#endregion

public class RiseSdkListener : MonoBehaviour {
    /// <rewardId>
    public static event Action<bool, int> OnRewardAdEvent;

    /// <success, billId>
    public static event Action<int, int> OnPaymentEvent;

    /// <success, event type, extra data>
    public static event Action<bool, int, int> OnSNSEvent;

    public static event Action<bool, int, string> OnCacheUrlResult;

    /// <submit or load, success, leader board id, extra data>
    public static event Action<bool, bool, string, string> OnLeaderBoardEvent;

    public static event Action<int, bool, string> OnReceiveServerResult;

    public static event Action<string> OnReceiveServerExtra;

    public static event Action<string> OnReceiveNotificationData;

    private static RiseSdkListener _instance;
    private static RiseSdk riseSdk;

    // only one IceTimer can exist
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

    public void onReceiveReward (string data) {
        string [] results = data.Split ('|');
        bool success = int.Parse (results [0]) == 0;
        int id = int.Parse (results [1]);
        if (OnRewardAdEvent != null && OnRewardAdEvent.GetInvocationList ().Length > 0) {
            OnRewardAdEvent (success, id);
        }
    }

    public void onPaymentSuccess (string billId) {
        int id = int.Parse (billId);
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PAYMENT_RESULT_SUCCESS, id);
        }
    }

    public void onPaymentFail (string billId) {
        int id = int.Parse (billId);
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PAYMENT_RESULT_FAILS, id);
        }
    }

    public void onPaymentCanceled (string bill) {
        int id = int.Parse (bill);
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PAYMENT_RESULT_CANCEL, id);
        }
    }

    public void onPaymentSystemValid (string dummy) {
        riseSdk.SetPaymentSystemValid (true);
    }

    public void onReceiveLoginResult (string result) {
        int success = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_LOGIN, 0);
        }
    }

    public void onReceiveInviteResult (string result) {
        int success = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_INVITE, 0);
        }
    }

    public void onReceiveLikeResult (string result) {
        int success = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_LIKE, 0);
        }
    }

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

    public void onReceiveServerExtra (string data) {
        if (OnReceiveServerExtra != null && OnReceiveServerExtra.GetInvocationList ().Length > 0) {
            OnReceiveServerExtra (data);
        }
    }

    public void onReceiveNotificationData (string data) {
        if (OnReceiveNotificationData != null && OnReceiveNotificationData.GetInvocationList ().Length > 0) {
            OnReceiveNotificationData (data);
        }
    }
}
