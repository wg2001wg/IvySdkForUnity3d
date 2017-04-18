
## 1, Add dependencies
Copy Plugins folder to the Assets directory in your Unity3D project, as shown in the following figure:
![](https://github.com/IvySdk/unity3d/raw/master/assets/risesdk-unity-8c095.png)

* if you use proguard to obfuscate your java source code, you should add these rules to your proguard rules file:
```java
-dontwarn com.unity3d.**
-keep class com.android.client.** {
    <methods>;
}

-keep class android.support.** {
    *;
}

-keep class com.core.async.** {
    public *;
}

-keep class com.core.common.** {
    public *;
}

-keep class com.core.network.** {
    public *;
}

-keep class com.core.view.** {
    public *;
}
```

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
This module will make these things done:
* show banner
* close banner
* show full screen ad
* make the player to share the game to his friends
* let the player to give your game a 5-star-rating
* track the player's behaviors for analytics
* Call the functions in need
```csharp
// show full screen ads when game starts
RiseSdk.Instance.ShowAd(RiseSdk.M_START);

// show full screen ads when game pauses
RiseSdk.Instance.ShowAd(RiseSdk.M_PAUSE);

// show full screen ads at the customized position
RiseSdk.Instance.ShowAd(RiseSdk.M_CUSTOM);

// show banner at the middle bottom position of your phone
RiseSdk.Instance.ShowBanner(RiseSdk.POS_BANNER_MIDDLE_BOTTOM);

// close banner
RiseSdk.Instance.CloseBanner();

// show exit page
RiseSdk.Instance.OnExit();

// ask the player to give your game a 5 stars rating
RiseSdk.Instance.Rate ();

// ask the player to share your game with their friends
RiseSdk.Instance.Share();

// show more game to the player
RiseSdk.Instance.ShowMore();
```

### Google Analytics events
```csharp
RiseSdk.Instance.TrackEvent ("your category", "your action", "your label", 1);
```

### Umeng analytics related API
* Track data of player's level
```csharp
int level = 1; //player level
RiseSdk.Instance.UM_setPlayerLevel(level);//Track data of player's level
```
* Track times for entering certain page
```csharp
String pageName = "Shop"; 
RiseSdk.Instance.UM_onPageStart(pageName);//track times for entering Shop page
```
* Track times for exiting certain page
```csharp
String pageName = "Shop";
RiseSdk.Instance.UM_onPageEnd(pageName);//track times for exiting Shop page
```  
* Track event name
```csharp
String eventId = "EnterGame"; //event name
RiseSdk.Instance.UM_onEvent(eventId);
```
* Track event label 
```csharp
String eventId = "EnterGame"; //event name
String eventLabel = "eventLable";//certain event operation label
RiseSdk.Instance.UM_onEvent(eventId, eventLabel);
```
* Detailed grouping content for tracked events
```csharp
HashMap<String, String> map = new HashMap<>(); //detailed grouping content for certain event
map.put("openGift", "roll");
int value = 1;//count statistics, like duration, amount of money
RiseSdk.Instance.UM_onEventValue("EnterGame", map, value);
```
* Track times for level start
```csharp
String level = "Level" + 5;//start from which level
RiseSdk.Instance.UM_startLevel(level);
```
* Track times for level failure
```csharp
String level = "Level" + 5); //which level is failed
RiseSdk.Instance.UM_failLevel(level);
```
* Track times for level completed
```csharp
String level = "Level" + (new Random().nextInt(30) + 10); //which level is completed
RiseSdk.Instance.UM_finishLevel(level);
```
* In-app purchase analytics
```csharp
double money = 5.0; //payment amount
String itemName = "Diamond"; //item name
int number = 10;//amount of the purchased item
double price = 99.0;//price of the purchased item 
RiseSdk.Instance.UM_pay(money,itemName,number,price);
```
* Booster
```csharp
String itemName = "life potion"; //name of the booster to be purchased in game
int number = 10;//amount of purchased boosters
double price = 99.0;//price of purchased booster 
RiseSdk.Instance.UM_buy(itemName,count,price); 
```
* Booster usage
```csharp
String itemName = "life potion"; //name of the used booster
int count = 10;//amount of the used booster
double price = 99.0;//price of the used booster 
RiseSdk.Instance.UM_use(itemName,count,price); 
```
* Extra bonus
```csharp
String itemName = "life potion"; //name of the rewarded booster
int number = 5;//amount of the rewarded booster
double price = 99.0;//price of the reward booster
int trigger = 1;//Events that trigger reward, its value ranges from 1 to 10. “1” is defined as “system reward”. About 2-10，you have to define corresponding rewards respectively
RiseSdk.Instance.UM_bonus(itemName,number,price,trigger);

// get server data in json format for your game if required 
string data = RiseSdk.Instance.GetExtraData ();
```
* Notice: the ad position for banner (POS_BANNER*) and interstitial (M_*) are defined in "namespace RiseSdk", you don’t need to redefine them
```csharp
// position for showBanner
public const int POS_BANNER_LEFT_TOP = 1;
public const int POS_BANNER_MIDDLE_TOP = 3;
public const int POS_BANNER_RIGHT_TOP = 6;
public const int POS_BANNER_MIDDLE_MIDDLE = 5;
public const int POS_BANNER_LEFT_BOTTOM = 2;
public const int POS_BANNER_MIDDLE_BOTTOM = 4;
public const int POS_BANNER_RIGHT_BOTTOM = 7;

// positions to show full screen ads
public const string M_START = "start";
public const string M_PAUSE = "pause";
public const string M_PASSLEVEL = "passlevel";
public const string M_CUSTOM = "custom";
```

## 4, In-App billing
* If you want to use Google checkout service (in-app purchase), you should follow this:
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

* Call Billing Interface
```csharp
RiseSdk.Instance.Pay(billId);
```

## 5, Reward Ads
Reward Ad is video ad that when the player saw it, you will give him some golds/items/diamonds etc.

* If you want to use video reward ads, then you should follow this:
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
//check whether the video ad is loaded or not
bool yes = RiseSdk.Instance.HasRewardAd();
if (yes) {
  setRewardButtonEnable();
} else {
  setRewardButtonDisable();
}

// show reward ad
RiseSdk.Instance.ShowRewardAd(rewardId);
```

## 6, SNS Facebook Related API
This module can make these things done:
* login with facebook
* logout
* like your facebook page
* let the player invite his all friends
* let the player challenge his all friends
* get the player's friend list that have played this game
* get player's profile
* If you want use Facebook related functions, you should follow this:
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
// Facebook Login
RiseSdk.Instance.Login();

// Facebook Logout
RiseSdk.Instance.Logout();

// check whether Facebook account is logged in
RiseSdk.Instance.IsLogin();

// invite facebook friends to play the game
RiseSdk.Instance.Invite ();

// like facebook page
RiseSdk.Instance.Like ();

// challenge your facebook friends
RiseSdk.Instance.Challenge ("your see", "speed coming...");

// get my facebook profile info
string mestring = RiseSdk.Instance.Me ();
// friends is a Hashtable, {"id":"xxx", "name":"xxx", "picture":"/sdcard/.cache/xxxxx"}
object me = MiniJSON.jsonDecode (mestring);
//returned data in json format is as follows:
 {
 "id":"0000000000000000",// my Facebook account id
 "name":"Me is me",// my Facebook account name
 "picture":"/data/empty_not_exists"// absolute path where my Facebook profile picture is saved
 }

// get facebook friend list 
string friendstring = RiseSdk.Instance.GetFriends ();
// friends is an ArrayList, [{"id":"xxx", "name":"xxxx", "picture":"/sdcard/.cache/xxxxx"}, ...]
object friends = MiniJSON.jsonDecode (friendstring);
 //returned data in json format is as follows:
 [
 {
 "id":"0000000000000001",//account id for Facebook Friend 1
 "name":"Friend 1",//account name for Facebook Friend 1
 "picture":"/data/empty_not_exists1"//absolute path where the profile picture of Facebook 1 is saved
 },
 {
  "id":"0000000000000002",//account id for Facebook Friend 2
 "name":"Friend 2",//account name for Facebook Friend 2
 "picture":"/data/empty_not_exists2"//absolute path where the profile picture of Facebook 2 is saved
 },
 {
 "id":"0000000000000003",//account id for Facebook Friend 3
 "name":"Friend 3",//account name for Facebook Friend 3
 "picture":"/data/empty_not_exists3"//absolute path where the profile picture of Facebook 3 is saved
 }
 ]
```

## 7, Native Ads
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

## 8, Others
* download something and cache it (async)
* get system configurations
* query whether installed an app or not
* launch an app
* goto play store for an app

Download a bitmap and cache it (without callback)
```csharp
string path = RiseSdk.Instance.CacheUrl("http://img.google.com/xxxxxx.png");
// do your works, you can query the path whether exists or not after 5 seconds
```

If you want to cache an url and get a callback from system, you can follow this:
* Define callback function
```csharp
const int TAG_BITMAP = 1;
void InitListeners() {
  RiseSdkListener.OnCacheUrlResult -= OnCacheUrl;
  RiseSdkListener.OnCacheUrlResult += OnCacheUrl;
}

void OnCacheUrl(bool result, int tag, string path) {
		Debug.LogError ("cache url result " + result + " tag " + tag + " path: " + path);
}
```

* Download 
```csharp
RiseSdk.Instance.CacheUrl(TAG_BITMAP, "http://img.google.com/xxxxxx.png");
// OnCacheUrl will be called when download completed
```

* other misc 其他
```csharp
// get system configurations
string config = RiseSdk.Instance.GetConfig(RiseSdk.CONFIG_KEY_APP_ID);
int appId = int.Parse(config);

// the configurations are defined in namespace RiseSdk
// you should NOT define these again
public const int CONFIG_KEY_APP_ID = 1;
public const int CONFIG_KEY_LEADER_BOARD_URL = 2;
public const int CONFIG_KEY_API_VERSION = 3;
public const int CONFIG_KEY_SCREEN_WIDTH = 4;
public const int CONFIG_KEY_SCREEN_HEIGHT = 5;
public const int CONFIG_KEY_LANGUAGE = 6;
public const int CONFIG_KEY_COUNTRY = 7;
public const int CONFIG_KEY_VERSION_CODE = 8;
public const int CONFIG_KEY_VERSION_NAME = 9;
public const int CONFIG_KEY_PACKAGE_NAME = 10;

//Check whether an app is installed
string appPackageName = "com.yes.good";
if (RiseSdk.Instance.HasApp(appPackageName)) {
  // launch this app
  RiseSdk.Instance.LaunchApp(appPackageName);
} else {
  // goto play store for this app
  RiseSdk.Instance.GetApp(appPackageName);
}
```

## 9, Congratulations, done.
when you run your game in your android phone or emulator, your will see some toast information.

### We also offer the following API：
* check whether the network is connected
```csharp
boolean isNetworkConnected = RiseSdk.Instance.isNetworkConnected();
```

* Pop up android native toast notification
```csharp
String messageContent="toast content";
RiseSdk.Instance.toast(messageContent);
```

* Pop up android native alert dialog
```csharp
String title = "Title";
String message = "Content";
RiseSdk.Instance.alert(title,message);
```

* If you want to monitor players’ ad behavior, please add ads monitor events：
* Define callback function
```csharp
void OnAdResult (RiseSdk.AdEventType type) {
  switch(type) {
    case RiseSdk.AdEventType.FullAdClosed: //full screen ads closed
      Debug.Log ("OnAdResult, FullAdClosed");
    break;
	case RiseSdk.AdEventType.FullAdClicked: //full screen ads clicked
      Debug.Log ("OnAdResult, FullAdClicked");
    break;
	case RiseSdk.AdEventType.VideoAdClosed: //video ads closed
	  Debug.Log ("OnAdResult, VideoAdClosed");
    break;
	case RiseSdk.AdEventType.BannerAdClicked: //banner ads clicked
	  Debug.Log ("OnAdResult, BannerAdClicked");
    break;
	case RiseSdk.AdEventType.CrossAdClicked: //cross promotion ads clicked
	  Debug.Log ("OnAdResult, CrossAdClicked");
    break;
  }
}

// register callback function during initialization
 void Awake () {
  ...
  RiseSdkListener.OnAdEvent -= OnAdResult;
  RiseSdkListener.OnAdEvent += OnAdResult;
  ...
}
```

## 10，If you have any question about the API document, please contact us via appdev@ivymobile.com. We will reply you as soon as possible! Thank you!


