#region Using
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion

public sealed class RiseSdk {
	private static RiseSdk _instance = null;
	private AndroidJavaClass _class = null;
    private bool paymentSystemValid = false;

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

	public void SetPaymentSystemValid(bool valid) {
		paymentSystemValid = valid;
	}

	public static RiseSdk Instance {
		get {
			if(null == _instance)
				_instance = new RiseSdk();
			return _instance;
		}
	}

	private RiseSdk() {
	}

	public void Init() {
		if(_class != null)
			return;
		#if UNITY_ANDROID
		try {
			RiseSdkListener.Instance.enabled = true;
			_class = new AndroidJavaClass("com.risesdk.client.Unity");
			if(_class != null) {
				AndroidJNIHelper.debug = true;
				using(AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
					using(AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
						_class.CallStatic("onCreate", context);
					};
				}
			}
		} catch(Exception e) {
			Debug.LogWarning(e.StackTrace);
			_class = null;
		}
		#endif
	}

	public void ShowBanner(int pos) {
		if(_class != null)
			_class.CallStatic("showBanner", pos);
	}

	public void CloseBanner() {
		if(_class != null)
			_class.CallStatic("closeBanner");
	}

	public void ShowAd(String tag) {
		if(_class != null)
			_class.CallStatic("showFullAd", tag);
	}

	public void ShowMore() {
		if(_class != null)
			_class.CallStatic("moreGame");
	}

	public bool HasRewardAd() {
		if(_class != null)
			return _class.CallStatic<bool>("hasRewardAd");
		return false;
	}

	public void ShowRewardAd(int rewardId) {
		if(_class != null)
			_class.CallStatic("showRewardAd", rewardId);
	}

	public void OnResume() {
		if(_class != null)
			_class.CallStatic("onResume");
	}

	public void OnPause() {
		if(_class != null)
			_class.CallStatic("onPause");
	}

	public void OnStart() {
		if(_class != null)
			_class.CallStatic("onStart");
	}

	public void OnStop() {
		if(_class != null)
			_class.CallStatic("onStop");
	}

	public void OnDestroy() {
		if(_class != null)
			_class.CallStatic("onDestroy");
	}

	public void OnExit() {
		if(_class != null)
			_class.CallStatic("onQuit");
	}

	public void HasPaid(int billingId) {
		if(_class != null) {
			_class.CallStatic("query", billingId);
		}
	}

	public bool IsPayEnabled() {
		return paymentSystemValid;
	}

	public void Pay(int billingId) {
		if(_class != null) {
			_class.CallStatic("pay", billingId);
		}
	}

	public void Share() {
		if(_class == null)
			return;
		_class.CallStatic("share");
	}

	public string GetExtraData() {
		if(_class == null)
			return null;
		return _class.CallStatic<string>("getExtraData");
	}

	public void TrackEvent(string category, string action, string label, int value) {
		if(_class == null)
			return;
		_class.CallStatic("trackEvent", category, action, label, value);
	}

	public void Rate() {
		if(_class == null)
			return;
		_class.CallStatic("rate");
	}

  public void ShowNativeAd(string tag, int yPercent) {
    if (_class != null) {
      _class.CallStatic("showNative", tag, yPercent);
    }
  }

  public void HideNativeAd(string tag) {
    if (_class != null) {
      _class.CallStatic("hideNative", tag);
    }
  }

	public bool HasNativeAd(string tag) {
		if (_class != null) {
			return _class.CallStatic<bool> ("hasNative", tag);
		} else {
			return false;
		}
	}

	public void Login(){
		if (_class != null) {
			_class.CallStatic ("login");
		}
	}

	public bool IsLogin() {
		if (_class != null) {
			return _class.CallStatic<bool> ("isLogin");
		} else {
			return false;
		}
	}

	public void Logout() {
		if (_class != null) {
			_class.CallStatic ("logout");
		}
	}

	public void Invite() {
		if (_class != null) {
			_class.CallStatic ("invite");
		}
	}

	public void Challenge(string title, string message) {
		if (_class != null) {
			_class.CallStatic ("challenge", title, message);
		}
	}

	public string Me() {
		if (_class != null) {
			return _class.CallStatic<string> ("me");
		} else {
			return "{}";
		}
	}

	public string GetFriends() {
		if (_class != null) {
			return _class.CallStatic<string> ("friends");
		} else {
			return "[]";
		}
	}

	public void Like() {
		if (_class != null) {
			_class.CallStatic ("like");
		}
	}

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
}
