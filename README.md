# RiseSDK for Unity3D
## Version: 3.3

## 1, Add dependencies
完全复制Plugins文件夹到你的Unity工程Assets目录下
Copy the folder named Plugins into your Unity3D project Assets folder
![Copy](assets/risesdk-unity-8c095.png)

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

// Google Analytics events
RiseSdk.Instance.TrackEvent ("your category", "your action", "your label", 1);

** 友盟统计相关接口
* 统计玩家等级
```csharp
int level = 1; //玩家等级
RiseSdk.Instance.UM_setPlayerLevel(level);//统计玩家等级
```
* 统计进入某页面
```csharp
String pageName = "Shop"; 
RiseSdk.Instance.UM_onPageStart(pageName);//统计进入商店页面
```
* 统计离开某页面
```csharp
String pageName = "Shop";
RiseSdk.Instance.UM_onPageEnd(pageName);//统计离开商店页面
```  
* 统计事件名称
```csharp
String eventId = "EnterGame"; //事件名称
RiseSdk.Instance.UM_onEvent(eventId);
```
* 统计事件标签操作
```csharp
String eventId = "EnterGame"; //事件名称
String eventLabel = "eventLable";//事件的某个操作标签
RiseSdk.Instance.UM_onEvent(eventId, eventLabel);
```
* 统计事件详细分组内容
```csharp
HashMap<String, String> map = new HashMap<>(); //事件详细分组内容
map.put("openGift", "roll");
int value = 1;//计数统计值，比如持续时间，每次付款金额
RiseSdk.Instance.UM_onEventValue("EnterGame", map, value);
```
* 统计关卡开始
```csharp
String level = "Level" + 5;//level ,开始哪个关卡
RiseSdk.Instance.UM_startLevel(level);
```
* 统计关卡失败
```csharp
String level = "Level" + 5); //level ,哪个关卡失败
RiseSdk.Instance.UM_failLevel(level);
```
* 关卡结束
```csharp
String level = "Level" + (new Random().nextInt(30) + 10); //level,完成哪个关卡
RiseSdk.Instance.UM_finishLevel(level);
```
* 游戏内付统计
```csharp
double money = 5.0; //内付的金额
String itemName = "钻石"; //内付购买的商品名称
int number = 10;//内付购买的商品数量
double price = 99.0;//内付购买的商品价格
RiseSdk.Instance.UM_pay(money,itemName,number,price);
```
* 购买道具统计
```csharp
String itemName = "血瓶"; //购买游戏中道具名称
int number = 10;//购买道具数量
double price = 99.0;//购买道具价格
RiseSdk.Instance.UM_buy(itemName,count,price); 
```
* 使用道具统计
```csharp
String itemName = "血瓶"; //使用道具名称
int count = 10;//使用道具数量
double price = 99.0;//使用道具价格
RiseSdk.Instance.UM_use(itemName,count,price); 
```
* 额外奖励统计
```csharp
String itemName = "血瓶"; //奖励道具名称
int number = 5;//奖励道具数量
double price = 99.0;//奖励道具价格
int trigger = 1;//触发奖励的事件, 取值在 1~10 之间，“1”已经被预先定义为“系统奖励”， 2~10 需要在网站设置含义
RiseSdk.Instance.UM_bonus(itemName,number,price,trigger);

// get server data for your game if needed
string data = RiseSdk.Instance.GetExtraData ();
```
* Notice
POS_BANNER* and M_* are defined in namespace RiseSdk
you should NOT define these again
```csharp
//position for showBanner
public const int POS_BANNER_LEFT_TOP = 1;
public const int POS_BANNER_MIDDLE_TOP = 3;
public const int POS_BANNER_RIGHT_TOP = 6;
public const int POS_BANNER_MIDDLE_MIDDLE = 5;
public const int POS_BANNER_LEFT_BOTTOM = 2;
public const int POS_BANNER_MIDDLE_BOTTOM = 4;
public const int POS_BANNER_RIGHT_BOTTOM = 7;

// tag for showAd
public const string M_START = "start";
public const string M_PAUSE = "pause";
public const string M_PASSLEVEL = "passlevel";
public const string M_CUSTOM = "custom";
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
Reward Ad is a video ad that when the player saw it, you will give him some golds/items/diamonds etc.

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
This module can make these things done:
* login with facebook
* logout
* like your facebook page
* let the player invite his all friends
* let the player challenge his all friends
* get the player's friend list that have played this game
* get player's profile

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

## 8, Misc
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

If you want to cache an url and let the system give you a callback, you can do this
* define callback
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

* download
```csharp
RiseSdk.Instance.CacheUrl(TAG_BITMAP, "http://img.google.com/xxxxxx.png");
// the result will be called in function OnCacheUrl
```

* other misc
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

// query an app whether installed or not
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
when you run your game in your android phone or emulator, your will see some toast information like this:
<center>![toast](assets/risesdk-unity-1fcfc.png)</center>

** 我们额外还提供以下接口：
* 判断网络是否连接
```csharp
boolean isNetworkConnected = RiseSdk.Instance.isNetworkConnected();
```
* 弹出android原生toast提示
```csharp
String messageContent="我是toast消息内容";
RiseSdk.Instance.toast(messageContent);
```
* 弹出android原生alert dialog
```csharp
String title = "我是标题";
String message = "我是内容";
RiseSdk.Instance.alert(title,message);
```
* 游戏，应用程序退出
```csharp
AndroidSdk.onQuit();
```
* 缓存文件
```csharp
String url = "http://xxxx.png";//文件下载连接
String path = RiseSdk.Instance.cacheUrl(url); //返回保存文件的绝对路径（/sdcard/0/.cache/383292918283483291）
```

* 如果你想缓存一个url并且让系统给你一个回调，你应该这样做：
* 定义回调函数
```csharp
const int TAG_PNG = 1;
void OnCacheUrlResult(bool success, int tag, string path) {
  switch(tag) {
    case TAG_PNG:
      Debug.Log ("download png result, success ? " + success + ", path: " + path);
    break;
  }
}

// 在初始化的时候注册你的回调函数
 void Awake () {
  ...
  RiseSdkListener.OnCacheUrlResult -= OnCacheUrlResult;
  RiseSdkListener.OnCacheUrlResult += OnCacheUrlResult;
  ...
}
```

* 如果你想在玩家对广告进行操作后做处理，你可以添加广告事件的监听：
* 定义回调函数
```csharp
void OnAdResult (RiseSdk.AdEventType type) {
  switch(type) {
    case RiseSdk.AdEventType.FullAdClosed: //大屏广告被关闭
      Debug.Log ("OnAdResult, FullAdClosed");
    break;
	case RiseSdk.AdEventType.FullAdClicked: //大屏广告被点击
      Debug.Log ("OnAdResult, FullAdClicked");
    break;
	case RiseSdk.AdEventType.VideoAdClosed: //视频广告被关闭
	  Debug.Log ("OnAdResult, VideoAdClosed");
    break;
	case RiseSdk.AdEventType.BannerAdClicked: //bannner广告被点击
	  Debug.Log ("OnAdResult, BannerAdClicked");
    break;
	case RiseSdk.AdEventType.CrossAdClicked: //交叉推广广告被点击
	  Debug.Log ("OnAdResult, CrossAdClicked");
    break;
  }
}

// 在初始化的时候注册你的回调函数
 void Awake () {
  ...
  RiseSdkListener.OnAdEvent -= OnAdResult;
  RiseSdkListener.OnAdEvent += OnAdResult;
  ...
}
```

## 10，RiseSdkApi中有对接口的详细注释，如您看过RiseSdkApi后还有不明白之处，可发送邮件到appdev@ivymobile.com，我们会尽快给您回复！谢谢！