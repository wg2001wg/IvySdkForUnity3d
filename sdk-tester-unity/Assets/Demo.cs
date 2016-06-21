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
	};

	// Use this for initialization
	void Awake ()
	{
		RiseSdk.Instance.Init();
		InitListeners();
	}

	void InitListeners() {
		// Set get free coin event
		RiseSdkListener.OnRewardAdEvent -= GetFreeCoin;
		RiseSdkListener.OnRewardAdEvent += GetFreeCoin;
		// On payment result
		RiseSdkListener.OnPaymentEvent -= OnPaymentResult;
		RiseSdkListener.OnPaymentEvent += OnPaymentResult;

		RiseSdkListener.OnSNSEvent -= OnSNSEvent;
		RiseSdkListener.OnSNSEvent += OnSNSEvent;

		RiseSdkListener.OnLeaderBoardEvent -= OnLeaderBoardResult;
		RiseSdkListener.OnLeaderBoardEvent += OnLeaderBoardResult;
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
		float h = Screen.height * .06f;
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

		case 14:
			RiseSdk.Instance.Login ();
			break;

		case 15:
			Debug.LogError("is login: " + RiseSdk.Instance.IsLogin ());
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

		case 22:
			RiseSdk.Instance.SubmitScore ("endless", 1234, "userName: haha");
			break;

		case 23:
			RiseSdk.Instance.LoadFriendLeaderBoard ("endless", 1, 32, "");
			break;

		case 24:
			RiseSdk.Instance.LoadGlobalLeaderBoard ("endless", 1, 32);
			break;

		case 25:
			RiseSdk.Instance.ShowNativeAd ("lock_pre", 20);
			break;

		case 26:
			RiseSdk.Instance.HideNativeAd ("lock_pre");
			break;
		}
	}

	void OnPaymentResult(bool success, int billId) {
		if (success) {
			switch (billId) {
			case 1:// the first billing Id success 
				break;
			case 2:// the second billing Id success
				break;
			case 3:
				break;
			}
			Debug.LogError("On billing success : " + billId);
		} else {
			switch (billId) {
			case 1:
				break;
			}
			Debug.LogError("On billing failure : " + billId);
		}
	}

	void OnSNSEvent(bool success, int eventType, int extra) {
		switch (eventType) {
		case RiseSdk.SNS_EVENT_LOGIN:
			Debug.LogError ("login: " + success);
			break;

		case RiseSdk.SNS_EVENT_INVITE:
			Debug.LogError ("invite: " + success);
			break;

		case RiseSdk.SNS_EVENT_LIKE:
			Debug.LogError ("like success? " + success);
			break;

		case RiseSdk.SNS_EVENT_CHALLENGE:
			int friendsCount = extra;
			Debug.LogError ("challenge: " + friendsCount);
			break;
		}
	}

	// Get Free coin handler
	void GetFreeCoin (int rewardId){
		switch(rewardId) {
		case 1:
			// you can add random golds, eg. 10
			//player.gold += 10;
			break;
		}
		Debug.LogError ("free coin: " + rewardId);
	}

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
}
