#region Using
using System;
using UnityEngine;

#endregion

public class RiseSdkListener : MonoBehaviour
{
	// <rewardId>
	public static event Action<int> OnRewardAdEvent;

	// <success, billId>
	public static event Action<bool, int> OnPaymentEvent;

	// <success, event type, extra data>
	public static event Action<bool, int, int> OnSNSEvent;

	private static RiseSdkListener _instance;
	private static RiseSdk riseSdk;

	// only one IceTimer can exist
	public static RiseSdkListener Instance {
		get {
			if (!_instance) {
				// check if there is a IceTimer instance already available in the scene graph
				_instance = FindObjectOfType (typeof(RiseSdkListener)) as RiseSdkListener;

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

	void OnApplicationPause (bool pauseStatus)
	{
		if (pauseStatus) {
			riseSdk.OnPause ();
		}
	}

	void OnApplicationFocus (bool focusStatus)
	{
		if (focusStatus) {
			riseSdk.OnResume ();
		}
	}

	void OnApplicationQuit ()
	{
			riseSdk.OnStop ();
			riseSdk.OnDestroy ();
	}

	void Awake ()
	{
			riseSdk.OnStart ();
	}

	public void onReceiveReward(string rewardId) {
		int id = int.Parse(rewardId);
		if (OnRewardAdEvent.GetInvocationList ().Length > 0) {
			OnRewardAdEvent (id);
		}
	}

	public void onPaymentSuccess(string billId) {
		int id = int.Parse(billId);
		if (OnPaymentEvent.GetInvocationList ().Length > 0) {
			OnPaymentEvent (true, id);
		}
	}
		
	public void onPaymentFail(string billId) {
		int id = int.Parse(billId);
		if (OnPaymentEvent.GetInvocationList ().Length > 0) {
			OnPaymentEvent (false, id);
		}
	}

	public void onPaymentSystemValid(string dummy) {
		riseSdk.SetPaymentSystemValid(true);
	}

	public void onReceiveLoginResult(string result) {
		int success = int.Parse (result);
		if (OnSNSEvent.GetInvocationList ().Length > 0) {
			OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_LOGIN, 0);
		}
	}

	public void onReceiveInviteResult(string result) {
		int success = int.Parse (result);
		if (OnSNSEvent.GetInvocationList ().Length > 0) {
			OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_INVITE, 0);
		}
	}

	public void onReceiveLikeResult(string result) {
		int success = int.Parse (result);
		if (OnSNSEvent.GetInvocationList ().Length > 0) {
			OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_LIKE, 0);
		}
	}

	public void onReceiveChallengeResult(string result) {
		int count = int.Parse (result);
		if (OnSNSEvent.GetInvocationList ().Length > 0) {
			OnSNSEvent (count > 0, RiseSdk.SNS_EVENT_CHALLENGE, count);
		}
	}
}
