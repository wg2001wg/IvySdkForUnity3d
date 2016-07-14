# RiseSDK for Unity3D

## 1, Add dependencies
Copy the folder named Plugins into your Unity3D project Assets folder
![Copy](assets/risesdk-unity-8c095.png)

## 2, Initialize
Call the Init function in a gameObject's Awake function in your initialize scene
```csharp
void Awake() {
  RiseSdk.Instance.Init();
  // when you want to use IAP or reward ad
  // then you should define this function
  // see step 4 and 5
  InitListeners();
}
```

## 3, ADs
* Call the functions in need
```csharp
// show start ad when you want
RiseSdk.Instance.ShowAd(RiseSdk.M_START);

// show pause ad when you want
RiseSdk.Instance.ShowAd(RiseSdk.M_PAUSE);

// show custom ad when you want
RiseSdk.Instance.ShowAd(RiseSdk.M_CUSTOM);

// show banner in the bottom center position of your phone
RiseSdk.Instance.ShowBanner(RiseSdk.POS_BANNER_MIDDLE_BOTTOM);

// close banner
RiseSdk.Instance.CloseBanner();

// exit game
RiseSdk.Instance.OnExit();

// ask the player to give your game a 5 stars rating
RiseSdk.Instance.Rate ();

// ask the player to share your game with his friends
RiseSdk.Instance.Share();

// show more game to the player
RiseSdk.Instance.ShowMore();

// track events
RiseSdk.Instance.TrackEvent ("your category", "your action", "your label", 1);

// get server data for your game if needed
string data = RiseSdk.Instance.GetExtraData ();
```

## 4, In-App billing
* When you want to use google checkout, then you should do this:
```csharp
void InitListeners() {
  RiseSdkListener.OnPaymentEvent -= OnPaymentResult;
  RiseSdkListener.OnPaymentEvent += OnPaymentResult;
}

void OnPaymentResult(int resultCode, int billId) {
		switch (resultCode) {
		case RiseSdk.PAYMENT_RESULT_SUCCESS:
			switch (billId) {
			case 1:// the first billing Id success
				break;
			case 2:// the second billing Id success
				break;
			case 3:
				break;
			}
			Debug.LogError("On billing success : " + billId);
			break;

		case RiseSdk.PAYMENT_RESULT_FAILS:
			switch (billId) {
			case 1:
				break;
			}
			Debug.LogError("On billing failure : " + billId);
			break;

		case RiseSdk.PAYMENT_RESULT_CANCEL:
			break;
		}
}
```

* then call Pay function to launch payment flow
```csharp
RiseSdk.Instance.Pay(billId);
```

## 5, Reward Ads
* when you want to use reward ad, then you should do:
```csharp
void InitListeners() {
  RiseSdkListener.OnRewardAdEvent -= GetFreeCoin;
  RiseSdkListener.OnRewardAdEvent += GetFreeCoin;
}

void GetFreeCoin (bool success, int rewardId){
		if (success) {
			switch(rewardId) {
			case 1:
				// you can add random golds, eg. 10
				//player.gold += 10;
				break;
			}
			Debug.LogError ("success: free coin: " + rewardId);
		} else {
			Debug.LogError ("fails: free coin: " + rewardId);
		}
	}
```
* and now you can call
```csharp

// determine whether exists reward ad
bool yes = RiseSdk.Instance.HasRewardAd();
if (yes) {
  setRewardButtonEnable();
} else {
  setRewardButtonDisable();
}

// show reward ad
RiseSdk.Instance.ShowRewardAd(rewardId);
...
```

## 6, SNS
* When you want to use SNS, eg. facebook to login, you should do this:
```csharp
void InitListeners() {
  RiseSdkListener.OnSNSEvent -= OnSNSEvent;
  RiseSdkListener.OnSNSEvent += OnSNSEvent;
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
```
* and then you can do this:
```csharp
// when you want to login
RiseSdk.Instance.Login();

// when you want to log out
RiseSdk.Instance.Logout();

// determine is login
RiseSdk.Instance.IsLogin();

// invite friends
RiseSdk.Instance.Invite ();

// like facebook page
RiseSdk.Instance.Like ();

// challenge your friends
RiseSdk.Instance.Challenge ("your see", "speed coming...");

// get self profile
string mestring = RiseSdk.Instance.Me ();
// friends is a Hashtable, {"id":"xxx", "name":"xxx", "picture":"/sdcard/.cache/xxxxx"}
object me = MiniJSON.jsonDecode (mestring);

// get friend list
string friendstring = RiseSdk.Instance.GetFriends ();
// friends is an ArrayList, [{"id":"xxx", "name":"xxxx", "picture":"/sdcard/.cache/xxxxx"}, ...]
object friends = MiniJSON.jsonDecode (friendstring);
```

## 7, Leaderboard
When you want to use leaderboard, you should do this:
* Define leaderboard call back
```csharp
void InitListeners() {
  RiseSdkListener.OnLeaderBoardEvent -= OnLeaderBoardResult;
  RiseSdkListener.OnLeaderBoardEvent += OnLeaderBoardResult;
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
```
* and you can use leaderboard now, do this:
```csharp
// submit score to the leaderboard named "endless", the extra data is your game data that you want to sumbit
RiseSdk.Instance.SubmitScore ("endless", 1234, "{userName: haha}");

// load first 32 records of the friends leaderboard
RiseSdk.Instance.LoadFriendLeaderBoard ("endless", 1, 32, "friend_1,friend_2");

// load first 32 records of the leaderboard for all player
RiseSdk.Instance.LoadGlobalLeaderBoard ("endless", 1, 32);
```

## 8, Native Ads
When you want to show some ads in your loading stage or pause game stage, you can use this type of ad. This Ad will show in screen position that measured by percentage of the screen height that you want. see blow:
```csharp
// show native ad in screen with y position of 45 percent of screen height
RiseSdk.Instance.ShowNativeAd ("loading", 45);

// hide native ad
RiseSdk.Instance.HideNativeAd ("loading");

// check whether exists any native ad
if (RiseSdk.Instance.HasNativeAd ("loading")) {
  // show loading with native ad
} else {
  // show common loading
}
```

## 9, Server Data
When you want to use these functions with our servers
* store player data
* sales promotion
* notice, extra game data

you can do these
* init server result listener
```csharp
void InitListeners() {
		RiseSdkListener.OnReceiveServerResult -= OnServerResult;
		RiseSdkListener.OnReceiveServerResult += OnServerResult;
}

void OnServerResult(int resultCode, bool success, string data) {
		switch (resultCode) {
		case RiseSdk.SERVER_RESULT_RECEIVE_GAME_DATA:
			if (success) {
        // you can see the data format from:
        // http://restartad.com/cloud/server/Mobile.sv.php?v=1&a=data&appid=13&version=1
				Debug.LogError ("load extra: " + data);
			} else {
				Debug.LogError ("load extra fails");
			}
			break;

		case RiseSdk.SERVER_RESULT_SALES_CLICK:
			if (success) {
        int saleId = int.Parse(data);
				Debug.LogError ("sales click");
			} else {
				// do nothing...
			}
			break;

		case RiseSdk.SERVER_RESULT_VERIFY_CODE:
			if (success) {
				Debug.LogError ("verify code success: " + data);
			} else {
				// fails
			}
			break;

      ...
		}
	}
```

the resultCode are defined in RiseSdk, they are below:
```csharp
public const int SERVER_RESULT_RECEIVE_GAME_DATA = 1;
public const int SERVER_RESULT_SAVE_USER_DATA = 2;
public const int SERVER_RESULT_RECEIVE_USER_DATA = 3;
public const int SERVER_RESULT_VERIFY_CODE = 4;
public const int SERVER_RESULT_SALES_CLICK = 5;
```
* now you can do what you want:
```csharp
// save user data
string userData = ...
RiseSdk.Instance.SaveUserData(userData);

// load user data now, you will receive the result in function you have defined that named OnServerResult
RiseSdk.Instance.LoadUserData();

// load game data, you will receive the result in function OnServerResult
int VERSION = 1;// the default value of VERSION is 1
RiseSdk.Instance.LoadGameData(VERSION);

// show sales promotion, if the player clicked the sales, you will receive SERVER_RESULT_SALES_CLICK in function OnServerResult
// the saleId is defined by the Operator, contact your group leader for more details
int saleId = ...
RiseSdk.Instance.ShowSales(saleId);

// verify the activity code, you will receive the result in function OnServerResult, the code is provided by the player
string code = ...
RiseSdk.Instance.VerifyCode(code);
```

## 10, Congratulations, done.
when you run your game in your android phone or emulator, your will see some toast information like this:
<center>![toast](assets/risesdk-unity-1fcfc.png)</center>
