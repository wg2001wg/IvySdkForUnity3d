#region Using
using System;
using UnityEngine;

#endregion

public class RiseSdkListener : MonoBehaviour
{
	public static event Action<int> GetRewardAdSuccessEvent;
	public static event Action<int> OnPaymentSuccessEvent;
	public static event Action<int> OnPaymentFailureEvent;

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

#region MonoBehaviour
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

#endregion
#region Receive Massage Call Back

public void onReceiveReward(string rewardId) {
  int id = int.Parse(rewardId);
  if (GetRewardAdSuccessEvent.GetInvocationList ().Length > 0)
    GetRewardAdSuccessEvent (id);
}

public void onPaymentSuccess(string billId) {
  int id = int.Parse(billId);
  if (OnPaymentSuccessEvent.GetInvocationList ().Length > 0)
    OnPaymentSuccessEvent (id);
}

public void onPaymentFail(string billId) {
  int id = int.Parse(billId);
  if (OnPaymentFailureEvent.GetInvocationList ().Length > 0)
    OnPaymentFailureEvent (id);
}

public void onPaymentSystemValid(string dummy) {
  riseSdk.SetPaymentSystemValid(true);
}
#endregion
}
