using UnityEngine;
using System.Collections.Generic;

public class Demo : MonoBehaviour
{
	public GUISkin guiSkin;
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
	};

	// Use this for initialization
	void Awake ()
	{
		RiseSdk.Instance.Init();
		InitListeners();
	}

	void InitListeners() {
		// Set get free coin event
		RiseSdkListener.GetRewardAdSuccessEvent -= GetFreeCoin;
		RiseSdkListener.GetRewardAdSuccessEvent += GetFreeCoin;
		// On payment result
		RiseSdkListener.OnPaymentSuccessEvent -= OnPaymentSuccess;
		RiseSdkListener.OnPaymentSuccessEvent += OnPaymentSuccess;
		RiseSdkListener.OnPaymentFailureEvent -= OnPaymentFailure;
		RiseSdkListener.OnPaymentFailureEvent += OnPaymentFailure;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home)) {
			RiseSdk.Instance.OnExit();
		}
	}
	
	void OnGUI ()
	{
		GUI.skin = guiSkin;
		float w = Screen.width * .46f;
		float h = Screen.height * .12f;
		float x = 0, y = 0;
		for (int i=0; i<titles.Length; i++) {
			int l = ((int)i / 2) + 1;
			y = 10 + (l - 1) * (h + 2);
			if (i % 2 == 0)
				x = 5;
			else
				x = Screen.width - w - 5;
			if (GUI.Button (new Rect (x, y, w, h), titles [i])) {
				doAction (i);
			}
		}
	}
	
	void doAction (int id)
	{
		switch (id) {
		case 0:
			RiseSdk.Instance.ShowAd(RiseSdk.M_START);
			break;
		case 1:
			RiseSdk.Instance.ShowAd(RiseSdk.M_PAUSE);
			break;
		case 2:
			RiseSdk.Instance.ShowAd(RiseSdk.M_PASSLEVEL);
			break;
		case 3:
			RiseSdk.Instance.ShowAd(RiseSdk.M_CUSTOM);
			break;
		case 4:
			RiseSdk.Instance.ShowBanner(RiseSdk.POS_BANNER_MIDDLE_BOTTOM);
			break;
		case 5:
			RiseSdk.Instance.CloseBanner();
			break;
		case 6:
			RiseSdk.Instance.OnExit();
			break;
		case 7:
			RiseSdk.Instance.Rate ();
			break;
		case 8:
			Debug.LogError("get extra data: " + RiseSdk.Instance.GetExtraData ());
			break;
		case 9:
			RiseSdk.Instance.TrackEvent ("category", "action", "label", 323);
			break;
		case 10:
			RiseSdk.Instance.Pay (1);
			break;
		case 11:
			RiseSdk.Instance.ShowMore();
			break;
		case 12:
			RiseSdk.Instance.ShowRewardAd(1);
			break;
		case 13:
			RiseSdk.Instance.Share();
			break;
		}
	}

	private void OnPaymentSuccess(int billingId) {
		// payment success, do something
		switch(billingId) {
		case 1:// the first billing Id success 
			break;
		case 2:// the second billing Id success
			break;
		case 3:
			break;
		}
		Debug.LogError("On billing success : " + billingId);
	}
	
	private void OnPaymentFailure(int billingId) {
		Debug.LogError("On billing failure : " + billingId);
	}

	// Get Free coin handler
	void GetFreeCoin (int gold)
	{
		Debug.LogError ("free coin: " + gold);
	}
}
