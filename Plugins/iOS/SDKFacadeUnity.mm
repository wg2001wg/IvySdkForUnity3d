//
//  SDKFacadeUnity.m
//  Pods
//
//  Created by 余冰星 on 2017/7/25.
//
//
#ifdef DEBUG
#import "SDKFacadeUnity.h"
#import <QuartzCore/QuartzCore.h>
#import <objc/runtime.h>
#ifdef __cplusplus
extern "C" {
#endif
    void Send(const char* obj, const char* method, const char* msg) {
        UnitySendMessage(obj, method, msg);
    }
#ifdef __cplusplus
} // extern "C"
#endif
@implementation SDKFacadeUnity
- (void)adReward:(NSString *)tag rewardId:(int)rewardId
{
    const char * _msg = [[NSString stringWithFormat:@"%@|%d", tag, rewardId] UTF8String];
    Send("RiseSdkListener", "adReward", _msg);
}

- (void)adLoaded:(NSString *)tag adType:(int)adType
{
    const char * _msg = [[NSString stringWithFormat:@"%@|%d", tag, adType] UTF8String];
    Send("RiseSdkListener", "adLoaded", _msg);
}

-(void)adFailed:(NSString *)tag adType:(int)adType forError:(NSError *)error
{
    const char * _msg = [[NSString stringWithFormat:@"%@|%d", tag, adType] UTF8String];
    Send("RiseSdkListener", "adFailed", _msg);
}

- (void)adDidShown:(NSString *)tag adType:(int)adType
{
    const char * _msg = [[NSString stringWithFormat:@"%@|%d", tag, adType] UTF8String];
    Send("RiseSdkListener", "adDidShown", _msg);
}

-(void)adShowFailed:(NSString *)tag adType:(int)adType
{
    const char * _msg = [[NSString stringWithFormat:@"%@|%d", tag, adType] UTF8String];
    Send("RiseSdkListener", "adShowFailed", _msg);
}

- (void)adDidClose:(NSString *)tag adType:(int)adType
{
    const char * _msg = [[NSString stringWithFormat:@"%@|%d", tag, adType] UTF8String];
    Send("RiseSdkListener", "adDidClose", _msg);
}

-(void)adDidClick:(NSString *)tag adType:(int)adType
{
    const char * _msg = [[NSString stringWithFormat:@"%@|%d", tag, adType] UTF8String];
    Send("RiseSdkListener", "adDidClick", _msg);
}

- (void)onPaymentReady
{
    Send("RiseSdkListener", "onPaymentReady", "");
}

- (void)onPaymentSuccess:(int)paymentId
{
    const char * _msg = [[@(paymentId) stringValue] UTF8String];
    Send("RiseSdkListener", "onPaymentSuccess", _msg);
}

- (void)onPaymentFailure:(int)paymentId forError:(NSString *)error
{
    const char * _msg = [[@(paymentId) stringValue] UTF8String];
    Send("RiseSdkListener", "onPaymentFailure", _msg);
}

- (void)onCheckSubscriptionResult:(int)paymentId remainSeconds:(long)remainSeconds
{
    const char * _msg = [[NSString stringWithFormat:@"%@,%ld", [@(paymentId) stringValue], remainSeconds] UTF8String];
    Send("RiseSdkListener", "onCheckSubscriptionResult", _msg);
}

-(void)onRestoreSuccess:(int)paymentId
{
    const char * _msg = [[@(paymentId) stringValue] UTF8String];
    Send("RiseSdkListener", "onRestoreSuccess", _msg);
}

-(void)onRestoreFailure:(NSString *)error
{
    const char * _msg = error ? [error UTF8String] : "";
    Send("RiseSdkListener", "onRestoreFailure", _msg);
}

-(void)snsShareSuccess
{
    Send("RiseSdkListener", "snsShareSuccess", "");
}

-(void)snsLoginCancel
{
    Send("RiseSdkListener", "snsLoginCancel", "");
}
-(void)snsShareFailure
{
    Send("RiseSdkListener", "snsShareFailure", "");
}

-(void)snsShareCancel
{
    Send("RiseSdkListener", "snsShareCancel", "");
}

-(void)snsLoginSuccess
{
    Send("RiseSdkListener", "snsLoginSuccess", "");
}

-(void)snsLoginFailure:(NSString *)error
{
    const char * _msg = error ? [error UTF8String] : "";
    Send("RiseSdkListener", "snsLoginFailure", _msg);
}
@end

@interface PayHandler : NSObject<UIAlertViewDelegate>
-(instancetype)initWithId:(int)payId;
@end
@implementation PayHandler
{
    @private
    int _payId;
}
-(instancetype)initWithId:(int)payId
{
    self = [super init];
    _payId = payId;
    return self;
}
- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    SDKFacadeUnity *handler = [[SDKFacadeUnity alloc]init];
    switch (buttonIndex) {
        case 0:
            [handler onPaymentSuccess:_payId];
            break;
            
        default:
            [handler onPaymentFailure:_payId forError:nil];
            break;
    }
}

- (void)alertViewCancel:(UIAlertView *)alertView
{
    SDKFacadeUnity *handler = [[SDKFacadeUnity alloc]init];
    [handler onPaymentFailure:_payId forError:nil];
}
@end

#ifdef __cplusplus
extern "C" {
#endif
    const char *returnStr(const char * src)
    {
        long n = strlen(src) + 1;
        const char *str = (char*)malloc(n + 1);
        memset((void*)str, 0, n);
        memcpy((void*)str, (void*)src, n);
        return str;
    }
    
    char* concat(const char *s1, const char *s2)
    {
        const size_t len1 = strlen(s1);
        const size_t len2 = strlen(s2);
        char *result = (char*)malloc(len1 + len2 + 1); //+1 for the zero-terminator
        //in real code you would check for errors in malloc here
        memcpy(result, s1, len1);
        memcpy(result+len1, s2, len2+1);//+1 to copy the null-terminator
        return result;
    }
    
    void toast(const char * msg)
    {
        UIView *view = [UIApplication sharedApplication].keyWindow.rootViewController.view;
        if(view) {
            [view makeToast:[NSString stringWithUTF8String:msg]];
        }
    }
    
    const char *getPushData()
    {
        return returnStr("{}");
    }
    
    const char *getConfig(int cid)
    {
        return returnStr("");
    }
    
    bool isAdsEnabled()
    {
        return true;
    }
    
    void setAdsEnable(bool enable)
    {
    }
    
    bool hasGdpr() {
        return false;
    }
    
    void resetGdpr() {
        toast("resetGdpr");
    }
    
    int getScreenWidth()
    {
        return [UIScreen mainScreen].currentMode.size.width;
    }
    
    int getScreenHeight()
    {
        return [UIScreen mainScreen].currentMode.size.height;
    }
    
    int getScreenDesignWidth()
    {
        return [UIScreen mainScreen].bounds.size.width;
    }
    
    int getScreenDesignHeight()
    {
        return [UIScreen mainScreen].bounds.size.height;
    }
    
    const char *getExtraData()
    {
        return returnStr("{}");
    }
    
    void sdklog(const char * info)
    {
        NSLog(@"[sdklog]: %@", [NSString stringWithUTF8String:info]);
    }
    
    void onCreate()
    {
        sdklog("onCreate");
    }
    
    bool isBannerAvailable()
    {
        return YES;
    }
    
    bool isBannerAvailableWithTag(const char *tag)
    {
        return YES;
    }
    
    void showBanner(int pos)
    {
        toast("show banner");
    }
    
    void showBannerWithTag(const char *tag, int pos)
    {
        toast(concat("show banner ", tag));
    }
    
    void showBannerCustom(const char *tag, float x, float y, float w, float h)
    {
        toast(concat("show banner ", tag));
    }
    
    void closeBanner()
    {
        toast("close banner");
    }
    
    bool isVideoAvailable()
    {
        return true;
    }
    
    bool isVideoAvailableWithTag(const char *tag)
    {
        return true;
    }
    
    void loadRewardVideo(const char * _Nonnull tag)
    {
        toast(concat("load video : ", tag));
    }
    
    void showRewardVideo(int rewardId)
    {
        toast("show video : default");
    }
    
    void showRewardVideoWithTag(const char *tag, int rewardId)
    {
        toast(concat("show video : ", tag));
    }
    
    bool isInterstitialAvailable(const char *tag)
    {
        return true;
    }
    
    void loadInterstitialAd(const char *tag)
    {
        toast(concat("load interstitial : ", tag));
    }
    
    void showInterstitialAd(const char *tag)
    {
        toast(concat("show interstitial : ", tag));
    }
    
    void showInterstitialAdWithTag(const char *tag, int seconds)
    {
        toast(concat("show interstitial : ", tag));
    }
    
    void showInterstitialAdWithTag2(const char *tag, int seconds, double timeInterval)
    {
        toast(concat("show interstitial : ", tag));
    }
    
    void showIconAd(float width, float xPercent, float yPercent)
    {
        toast("show icon ad");
    }
    
    void showPopupIconAds()
    {
        toast("show popup icon ads");
    }
    
    void hidePopupIconAds()
    {
        toast("hide popup icon ads");
    }
    
    const char * getPopupIconAdsData()
    {
        return "[]";
    }
    
    void closeIconAd()
    {
        toast("close icon ad");
    }
    
    bool isNativeAvailable(const char *tag)
    {
        return false;
    }
    
    const char * _Nullable fetchNativeAdJson(const char * _Nonnull tag)
    {
        return "";
    }
    
    void loadNativeAd(const char * _Nonnull tag)
    {
        toast(concat("load native ad : ", tag));
    }
    
    void closeNativeAd(const char * _Nonnull tag)
    {
        toast(concat("close native ad : ", tag));
    }
    
    void showNativeAd(const char * _Nonnull tag, float x, float y, const char * _Nonnull json)
    {
        toast(concat("show native ad : ", tag));
    }
    
    void showNativeAdWithFrame(const char *_Nonnull tag, float x, float y, float w, float h, const char * _Nonnull json)
    {
        toast(concat("show native ad : ", tag));
    }
    
    bool isDeliciousAdAvailable()
    {
        return YES;
    }
    
    void showDeliciousInterstitialAd(const char * _Nonnull json)
    {
        toast(concat("showDeliciousInterstitialAd : ", json));
    }
    
    void showDeliciousBannerAd(float x, float y, float w, float h, const char * _Nonnull json)
    {
        toast(concat("showDeliciousBannerAd : ", json));
    }
    
    void closeDeliciousBannerAd()
    {
        toast("closeDeliciousBannerAd");
    }
    
    void showDeliciousIconAd(float x, float y, float w, float h, const char * _Nonnull json)
    {
        toast(concat("showDeliciousIconAd : ", json));
    }
    
    void closeDeliciousIconAd()
    {
        toast("closeDeliciousIconAd");
    }
    
    void rateUs()
    {
        toast("rateUs");
    }
    
    void rateInApp()
    {
        toast("rateInApp");
    }
    
    void rateUsWithStar(float star)
    {
        toast("rateUs");
    }
    
    void rateInAppWithStar(float star)
    {
        toast("rateInApp");
    }
    
    bool isNetworkAvailable()
    {
        return true;
    }
    
    void pay(int payId)
    {
        
//        UIAlertView *alertView = [[UIAlertView alloc] initWithTitle:@"Pay" message:[NSString stringWithFormat:@"Pay for : %d", payId] delegate:[[PayHandler alloc] initWithId:payId] cancelButtonTitle:@"Cancel" otherButtonTitles:@"Pay success", @"Pay failure", nil];
//        [alertView show];
        
        // 实例化 UIAlertController 对象
        UIAlertController *alertController = [UIAlertController alertControllerWithTitle:@"Pay"
                                                                                 message:[NSString stringWithFormat:@"Pay for : %d", payId]
                                                                          preferredStyle:UIAlertControllerStyleAlert];
        
        // 创建按钮
        UIAlertAction *cancelAction = [UIAlertAction actionWithTitle:@"Cancel"
                                                               style:UIAlertActionStyleCancel
                                                             handler:nil];
        
        UIAlertAction *okAction = [UIAlertAction actionWithTitle:@"Pay success"
                                                           style:UIAlertActionStyleDefault
                                                         handler:^(UIAlertAction *action){                                                             
                                                             SDKFacadeUnity *handler = [[SDKFacadeUnity alloc]init];
                                                             [handler onPaymentSuccess:payId];
                                                         }];
        UIAlertAction *noAction = [UIAlertAction actionWithTitle:@"Pay failure"
                                                           style:UIAlertActionStyleDestructive
                                                         handler:^(UIAlertAction *action){
                                                             SDKFacadeUnity *handler = [[SDKFacadeUnity alloc]init];
                                                             [handler onPaymentFailure:payId forError:nil];
                                                         }];
        // 向 alertController 上添加按钮
        [alertController addAction:cancelAction];
        [alertController addAction:okAction];
        [alertController addAction:noAction];
        
        // 显示 alertController 视图
        [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:alertController animated:YES completion:nil];
    }
    
    void isSubscriptionActive()
    {
        [[[SDKFacadeUnity alloc] init] onCheckSubscriptionResult:1 remainSeconds:10000000];
    }
    
    void restorePayments()
    {
        toast("restore success");
        [[[SDKFacadeUnity alloc] init] onRestoreSuccess:1];
    }
    
    const char *getPaymentDatas()
    {
        return returnStr("{}");
    }
    
    int* getPurchasedIds()
    {
        return nullptr;
    }
    
    void clearPurchasedIds()
    {
        toast("clearPurchasedIds");
    }
    
    void clearPurchasedId(int paymentId)
    {
        toast("clearPurchasedIds");
    }
    
    bool isGameCenterAvailable()
    {
        return true;
    }
    
    void showLeaderboards()
    {
        toast("showLeaderboards");
    }
    
    void showLeaderboard(int leaderboardId)
    {
        toast("showLeaderboards");
    }
    
    void showAchievements()
    {
        toast("showLeaderboards");
    }
    
    void submitScore(int leaderboardId, long long score)
    {
        toast("submitScore");
    }
    
    long long myHighScore(int leaderboardId)
    {
        return 0;
    }
    
    void submitAchievement(int achievementId, double percent)
    {
        toast("submitAchievement");
    }
    
    double getAchievementProgress(int achievementId)
    {
        return 0;
    }
    
    int getRemoteConfigIntValue(const char * _Nonnull key)
    {
        return 0;
    }
    
    long getRemoteConfigLongValue(const char * _Nonnull key)
    {
        return 0;
    }
    
    double getRemoteConfigDoubleValue(const char * _Nonnull key)
    {
        return 0;
    }
    
    bool getRemoteConfigBoolValue(const char * _Nonnull key)
    {
        return false;
    }
    
    const char * _Nonnull getRemoteConfigStringValue(const char * _Nonnull key)
    {
        return returnStr("null");
    }
    
    void login()
    {
        toast("facebook login");
    }
    
    void logout()
    {
        toast("facebook logout");
    }
    
    bool isLogin()
    {
        return false;
    }
    
    const char *meFirstName()
    {
        return returnStr("FirstName");
    }
    
    const char *meLastName()
    {
        return returnStr("LastName");
    }
    
    const char *meName()
    {
        return returnStr("Me Name");
    }
    
    const char *meId()
    {
        return returnStr("12345678");
    }
    
    const char *me()
    {
        return returnStr("{\"id\":\"12345678\", \"name\":\"hahaha\", \"picture\":\"http://img.qq1234.org/uploads/allimg/141205/3_141205195713_3.jpg\"}");
    }
    
    const char *friends()
    {
        return returnStr("[]");
    }
    
    const char *mePictureURL()
    {
        return returnStr("http://img.qq1234.org/uploads/allimg/141205/3_141205195713_3.jpg");
    }
    
    void fetchFriends(bool invitable)
    {
        toast("facebook fetchFriends");
    }
    
    void fetchScores()
    {
        toast("facebook fetchScores");
    }
    
    void invite()
    {
        toast("facebook invite");
    }
    
    void share()
    {
        toast("facebook share");
    }
    
    void shareContent(const char *contentURL, const char *tag, const char *quote)
    {
        toast(concat("facebook shareContent", contentURL));
    }
    
    void shareSheet(const char *linkURL, const char *tag, const char *quote)
    {
        toast(concat("facebook shareSheet", linkURL));
    }
    
    void shareSheetOS(const char *linkURL, const char *title)
    {
        toast(concat("facebook shareSheetOS", linkURL));
    }
    
    void logEventWithData(const char *eventId, const char *data)
    {
        if(data != nullptr) {
            NSString *objc_name = [NSString stringWithUTF8String:eventId];
            NSMutableDictionary *objc_data = [[NSMutableDictionary alloc] init];
            NSString *_data = [NSString stringWithUTF8String:data];
            NSArray *arr = [_data componentsSeparatedByString:@","];
            if(arr && arr.count > 1) {
                for (int i=0; i<arr.count; i+=2) {
                    NSString *key = [arr objectAtIndex:i];
                    NSString *value = [arr objectAtIndex:i+1];
                    [objc_data setObject:value forKey:key];
                }
                toast(concat("logEvent : ", eventId));
            }
        }
    }
    
    void logPlayerLevel(int levelId)
    {
        char* buffer = (char*)malloc(2);
        sprintf(buffer, "%d", levelId);
        toast(concat("logPlayerLevel : ", buffer));
    }
    
    void logPageStart(char* pageName)
    {
    toast(concat("logPageStart : ", pageName));
    }
    
    void logPageEnd(const char* pageName)
    {
        toast(concat("logPageEnd : ", pageName));
    }
    
    void logEvent(const char* eventId)
    {
        toast(concat("logEvent : ", eventId));
    }
    
    void logEventWithTag(const char* eventId, const char* tag)
    {
        toast(concat("logEventWithTag : ", eventId));
    }
    
    void logEventLikeGA(const char* eventId, const char* action, const char* label, long value)
    {
        toast(concat("logEventLikeGA : ", eventId));
    }
    
    void logStartLevel(const char* level)
    {
        toast(concat("logStartLevel : ", level));
    }
    
    void logFailLevel(const char* level)
    {
        toast(concat("logFailLevel : ", level));
    }
    
    void logFinishLevel(const char* level)
    {
        toast(concat("logFinishLevel : ", level));
    }
    
    void logFinishAchievement(const char* achievement)
    {
        toast(concat("logFinishAchievement : ", achievement));
    }
    
    void logFinishTutorial(const char* tutorial)
    {
        toast(concat("logFinishTutorial : ", tutorial));
    }
    
    void logPay(double money, const char* itemName, int number, const char* currency)
    {
        toast(concat("logPay : ", itemName));
    }
    
    void logBuy(const char* _Nonnull itemName, int count, double price)
    {
        toast(concat("logPay : ", itemName));
    }
    
    void logBuy2(const char* _Nonnull itemName, const char* _Nonnull itemType, int count, double price)
    {
        toast(concat("logPay : ", itemName));
    }
    
    void logUse(const char* itemName, int number, double price)
    {
        toast(concat("logUse : ", itemName));
    }
    
    void logBonus(const char* itemName, int number, double price, int trigger)
    {
        toast(concat("logBonus : ", itemName));
    }
    
    const char *cacheUrl(const char* url)
    {
        return returnStr("");
    }
    
    void cacheUrlWithTag(const char* tag, const char* url)
    {
        NSString *_tag = tag != nullptr ? [NSString stringWithUTF8String:tag] : nil;
        NSString *_url = url != nullptr ? [NSString stringWithUTF8String:url] : nil;
        const char * _msg = [[NSString stringWithFormat:@"%@|%@", _tag, _url] UTF8String];
        Send("RiseSdkListener", "cacheUrlSuccess", _msg);
        Send("RiseSdkListener", "cacheUrlFailure", tag);
    }
    
    void registerPush()
    {
        toast("registerPush");
    }
    
    bool isPushRegistered()
    {
        return false;
    }
    
    void cancelLocalNotification(const char* _Nonnull key)
    {
        toast(concat("cancelLocalNotification : ", key));
    }
    
    void cancelAllLocalNotifications()
    {
        toast("cancelAllLocalNotifications");
    }
    
    const char* _Nullable getLocalNotificationDataJson()
    {
        return returnStr("{}");
    }
    
    void pushLocalNotification(const char* _Nonnull key, const char* _Nonnull title, const char* _Nonnull msg, const char* _Nonnull action, int seconds, int interval, const char* _Nonnull userInfo)
    {
        toast("pushLocalNotification");
    }
    
    void pushLocalNotificationWithDateStr(const char* _Nonnull key, const char* _Nonnull title, const char* _Nonnull msg, const char* _Nonnull action, const char* _Nonnull dateStr, int interval, const char* _Nonnull userInfo)
    {
        toast("pushLocalNotification");
    }
    
    void saveBase64ImageToCameraRoll(const char* _Nonnull base64Image)
    {
        toast("saveBase64ImageToCameraRoll");
    }
    
    void takeScreenshotToCameraRoll()
    {
        toast("takeScreenshotToCameraRoll");
    }
    
    const char * _Nonnull takeScreenshotToDocumentsDirectory()
    {
        toast("takeScreenshotToDocumentsDirectory");
        return returnStr("");
    }
    
    const char * _Nonnull takeScreenshotToDirectoryAtPath(const char* _Nonnull path)
    {
        toast("takeScreenshotToDirectoryAtPath");
        return returnStr(path);
    }
    
    bool isIPhoneX()
    {
        return UIScreen.mainScreen.bounds.size.height / UIScreen.mainScreen.bounds.size.width > 2;;
    }
    
    bool isIPad()
    {
        return (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad);
    }
    
    bool hasNotch()
    {
        return isIPhoneX();
    }
    
    bool justShowFullAd()
    {
        return false;
    }
    
#ifdef __cplusplus
} // extern "C"
#endif

NSString * CSToastPositionTop       = @"CSToastPositionTop";
NSString * CSToastPositionCenter    = @"CSToastPositionCenter";
NSString * CSToastPositionBottom    = @"CSToastPositionBottom";

// Keys for values associated with toast views
static const NSString * CSToastTimerKey             = @"CSToastTimerKey";
static const NSString * CSToastDurationKey          = @"CSToastDurationKey";
static const NSString * CSToastPositionKey          = @"CSToastPositionKey";
static const NSString * CSToastCompletionKey        = @"CSToastCompletionKey";

// Keys for values associated with self
static const NSString * CSToastActiveKey            = @"CSToastActiveKey";
static const NSString * CSToastActivityViewKey      = @"CSToastActivityViewKey";
static const NSString * CSToastQueueKey             = @"CSToastQueueKey";

@interface UIView (ToastPrivate)

/**
 These private methods are being prefixed with "cs_" to reduce the likelihood of non-obvious
 naming conflicts with other UIView methods.
 
 @discussion Should the public API also use the cs_ prefix? Technically it should, but it
 results in code that is less legible. The current public method names seem unlikely to cause
 conflicts so I think we should favor the cleaner API for now.
 */
- (void)cs_showToast:(UIView *)toast duration:(NSTimeInterval)duration position:(id)position;
- (void)cs_hideToast:(UIView *)toast;
- (void)cs_hideToast:(UIView *)toast fromTap:(BOOL)fromTap;
- (void)cs_toastTimerDidFinish:(NSTimer *)timer;
- (void)cs_handleToastTapped:(UITapGestureRecognizer *)recognizer;
- (CGPoint)cs_centerPointForPosition:(id)position withToast:(UIView *)toast;
- (NSMutableArray *)cs_toastQueue;

@end

@implementation UIView (Toast)

#pragma mark - Make Toast Methods

- (void)makeToast:(NSString *)message {
    [self makeToast:message duration:[CSToastManager defaultDuration] position:[CSToastManager defaultPosition] style:nil];
}

- (void)makeToast:(NSString *)message duration:(NSTimeInterval)duration position:(id)position {
    [self makeToast:message duration:duration position:position style:nil];
}

- (void)makeToast:(NSString *)message duration:(NSTimeInterval)duration position:(id)position style:(CSToastStyle *)style {
    UIView *toast = [self toastViewForMessage:message title:nil image:nil style:style];
    [self showToast:toast duration:duration position:position completion:nil];
}

- (void)makeToast:(NSString *)message duration:(NSTimeInterval)duration position:(id)position title:(NSString *)title image:(UIImage *)image style:(CSToastStyle *)style completion:(void(^)(BOOL didTap))completion {
    UIView *toast = [self toastViewForMessage:message title:title image:image style:style];
    [self showToast:toast duration:duration position:position completion:completion];
}

#pragma mark - Show Toast Methods

- (void)showToast:(UIView *)toast {
    [self showToast:toast duration:[CSToastManager defaultDuration] position:[CSToastManager defaultPosition] completion:nil];
}

- (void)showToast:(UIView *)toast duration:(NSTimeInterval)duration position:(id)position completion:(void(^)(BOOL didTap))completion {
    // sanity
    if (toast == nil) return;
    
    // store the completion block on the toast view
    objc_setAssociatedObject(toast, &CSToastCompletionKey, completion, OBJC_ASSOCIATION_RETAIN_NONATOMIC);
    
    if ([CSToastManager isQueueEnabled] && [self.cs_activeToasts count] > 0) {
        // we're about to queue this toast view so we need to store the duration and position as well
        objc_setAssociatedObject(toast, &CSToastDurationKey, @(duration), OBJC_ASSOCIATION_RETAIN_NONATOMIC);
        objc_setAssociatedObject(toast, &CSToastPositionKey, position, OBJC_ASSOCIATION_RETAIN_NONATOMIC);
        
        // enqueue
        [self.cs_toastQueue addObject:toast];
    } else {
        // present
        [self cs_showToast:toast duration:duration position:position];
    }
}

#pragma mark - Hide Toast Method

- (void)hideToasts {
    for (UIView *toast in [self cs_activeToasts]) {
        [self hideToast:toast];
    }
}

- (void)hideToast:(UIView *)toast {
    // sanity
    if (!toast || ![[self cs_activeToasts] containsObject:toast]) return;
    
    NSTimer *timer = (NSTimer *)objc_getAssociatedObject(toast, &CSToastTimerKey);
    [timer invalidate];
    
    [self cs_hideToast:toast];
}

#pragma mark - Private Show/Hide Methods

- (void)cs_showToast:(UIView *)toast duration:(NSTimeInterval)duration position:(id)position {
    toast.center = [self cs_centerPointForPosition:position withToast:toast];
    toast.alpha = 0.0;
    
    if ([CSToastManager isTapToDismissEnabled]) {
        UITapGestureRecognizer *recognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(cs_handleToastTapped:)];
        [toast addGestureRecognizer:recognizer];
        toast.userInteractionEnabled = YES;
        toast.exclusiveTouch = YES;
    }
    
    [[self cs_activeToasts] addObject:toast];
    
    [self addSubview:toast];
    
    [UIView animateWithDuration:[[CSToastManager sharedStyle] fadeDuration]
                          delay:0.0
                        options:(UIViewAnimationOptionCurveEaseOut | UIViewAnimationOptionAllowUserInteraction)
                     animations:^{
                         toast.alpha = 1.0;
                     } completion:^(BOOL finished) {
                         NSTimer *timer = [NSTimer timerWithTimeInterval:duration target:self selector:@selector(cs_toastTimerDidFinish:) userInfo:toast repeats:NO];
                         [[NSRunLoop mainRunLoop] addTimer:timer forMode:NSRunLoopCommonModes];
                         objc_setAssociatedObject(toast, &CSToastTimerKey, timer, OBJC_ASSOCIATION_RETAIN_NONATOMIC);
                     }];
}

- (void)cs_hideToast:(UIView *)toast {
    [self cs_hideToast:toast fromTap:NO];
}

- (void)cs_hideToast:(UIView *)toast fromTap:(BOOL)fromTap {
    [UIView animateWithDuration:[[CSToastManager sharedStyle] fadeDuration]
                          delay:0.0
                        options:(UIViewAnimationOptionCurveEaseIn | UIViewAnimationOptionBeginFromCurrentState)
                     animations:^{
                         toast.alpha = 0.0;
                     } completion:^(BOOL finished) {
                         [toast removeFromSuperview];
                         
                         // remove
                         [[self cs_activeToasts] removeObject:toast];
                         
                         // execute the completion block, if necessary
                         void (^completion)(BOOL didTap) = objc_getAssociatedObject(toast, &CSToastCompletionKey);
                         if (completion) {
                             completion(fromTap);
                         }
                         
                         if ([self.cs_toastQueue count] > 0) {
                             // dequeue
                             UIView *nextToast = [[self cs_toastQueue] firstObject];
                             [[self cs_toastQueue] removeObjectAtIndex:0];
                             
                             // present the next toast
                             NSTimeInterval duration = [objc_getAssociatedObject(nextToast, &CSToastDurationKey) doubleValue];
                             id position = objc_getAssociatedObject(nextToast, &CSToastPositionKey);
                             [self cs_showToast:nextToast duration:duration position:position];
                         }
                     }];
}

#pragma mark - View Construction

- (UIView *)toastViewForMessage:(NSString *)message title:(NSString *)title image:(UIImage *)image style:(CSToastStyle *)style {
    // sanity
    if(message == nil && title == nil && image == nil) return nil;
    
    // default to the shared style
    if (style == nil) {
        style = [CSToastManager sharedStyle];
    }
    
    // dynamically build a toast view with any combination of message, title, & image
    UILabel *messageLabel = nil;
    UILabel *titleLabel = nil;
    UIImageView *imageView = nil;
    
    UIView *wrapperView = [[UIView alloc] init];
    wrapperView.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin | UIViewAutoresizingFlexibleBottomMargin);
    wrapperView.layer.cornerRadius = style.cornerRadius;
    
    if (style.displayShadow) {
        wrapperView.layer.shadowColor = style.shadowColor.CGColor;
        wrapperView.layer.shadowOpacity = style.shadowOpacity;
        wrapperView.layer.shadowRadius = style.shadowRadius;
        wrapperView.layer.shadowOffset = style.shadowOffset;
    }
    
    wrapperView.backgroundColor = style.backgroundColor;
    
    if(image != nil) {
        imageView = [[UIImageView alloc] initWithImage:image];
        imageView.contentMode = UIViewContentModeScaleAspectFit;
        imageView.frame = CGRectMake(style.horizontalPadding, style.verticalPadding, style.imageSize.width, style.imageSize.height);
    }
    
    CGRect imageRect = CGRectZero;
    
    if(imageView != nil) {
        imageRect.origin.x = style.horizontalPadding;
        imageRect.origin.y = style.verticalPadding;
        imageRect.size.width = imageView.bounds.size.width;
        imageRect.size.height = imageView.bounds.size.height;
    }
    
    if (title != nil) {
        titleLabel = [[UILabel alloc] init];
        titleLabel.numberOfLines = style.titleNumberOfLines;
        titleLabel.font = style.titleFont;
        titleLabel.textAlignment = style.titleAlignment;
        titleLabel.lineBreakMode = NSLineBreakByTruncatingTail;
        titleLabel.textColor = style.titleColor;
        titleLabel.backgroundColor = [UIColor clearColor];
        titleLabel.alpha = 1.0;
        titleLabel.text = title;
        
        // size the title label according to the length of the text
        CGSize maxSizeTitle = CGSizeMake((self.bounds.size.width * style.maxWidthPercentage) - imageRect.size.width, self.bounds.size.height * style.maxHeightPercentage);
        CGSize expectedSizeTitle = [titleLabel sizeThatFits:maxSizeTitle];
        // UILabel can return a size larger than the max size when the number of lines is 1
        expectedSizeTitle = CGSizeMake(MIN(maxSizeTitle.width, expectedSizeTitle.width), MIN(maxSizeTitle.height, expectedSizeTitle.height));
        titleLabel.frame = CGRectMake(0.0, 0.0, expectedSizeTitle.width, expectedSizeTitle.height);
    }
    
    if (message != nil) {
        messageLabel = [[UILabel alloc] init];
        messageLabel.numberOfLines = style.messageNumberOfLines;
        messageLabel.font = style.messageFont;
        messageLabel.textAlignment = style.messageAlignment;
        messageLabel.lineBreakMode = NSLineBreakByTruncatingTail;
        messageLabel.textColor = style.messageColor;
        messageLabel.backgroundColor = [UIColor clearColor];
        messageLabel.alpha = 1.0;
        messageLabel.text = message;
        
        CGSize maxSizeMessage = CGSizeMake((self.bounds.size.width * style.maxWidthPercentage) - imageRect.size.width, self.bounds.size.height * style.maxHeightPercentage);
        CGSize expectedSizeMessage = [messageLabel sizeThatFits:maxSizeMessage];
        // UILabel can return a size larger than the max size when the number of lines is 1
        expectedSizeMessage = CGSizeMake(MIN(maxSizeMessage.width, expectedSizeMessage.width), MIN(maxSizeMessage.height, expectedSizeMessage.height));
        messageLabel.frame = CGRectMake(0.0, 0.0, expectedSizeMessage.width, expectedSizeMessage.height);
    }
    
    CGRect titleRect = CGRectZero;
    
    if(titleLabel != nil) {
        titleRect.origin.x = imageRect.origin.x + imageRect.size.width + style.horizontalPadding;
        titleRect.origin.y = style.verticalPadding;
        titleRect.size.width = titleLabel.bounds.size.width;
        titleRect.size.height = titleLabel.bounds.size.height;
    }
    
    CGRect messageRect = CGRectZero;
    
    if(messageLabel != nil) {
        messageRect.origin.x = imageRect.origin.x + imageRect.size.width + style.horizontalPadding;
        messageRect.origin.y = titleRect.origin.y + titleRect.size.height + style.verticalPadding;
        messageRect.size.width = messageLabel.bounds.size.width;
        messageRect.size.height = messageLabel.bounds.size.height;
    }
    
    CGFloat longerWidth = MAX(titleRect.size.width, messageRect.size.width);
    CGFloat longerX = MAX(titleRect.origin.x, messageRect.origin.x);
    
    // Wrapper width uses the longerWidth or the image width, whatever is larger. Same logic applies to the wrapper height.
    CGFloat wrapperWidth = MAX((imageRect.size.width + (style.horizontalPadding * 2.0)), (longerX + longerWidth + style.horizontalPadding));
    CGFloat wrapperHeight = MAX((messageRect.origin.y + messageRect.size.height + style.verticalPadding), (imageRect.size.height + (style.verticalPadding * 2.0)));
    
    wrapperView.frame = CGRectMake(0.0, 0.0, wrapperWidth, wrapperHeight);
    
    if(titleLabel != nil) {
        titleLabel.frame = titleRect;
        [wrapperView addSubview:titleLabel];
    }
    
    if(messageLabel != nil) {
        messageLabel.frame = messageRect;
        [wrapperView addSubview:messageLabel];
    }
    
    if(imageView != nil) {
        [wrapperView addSubview:imageView];
    }
    
    return wrapperView;
}

#pragma mark - Storage

- (NSMutableArray *)cs_activeToasts {
    NSMutableArray *cs_activeToasts = objc_getAssociatedObject(self, &CSToastActiveKey);
    if (cs_activeToasts == nil) {
        cs_activeToasts = [[NSMutableArray alloc] init];
        objc_setAssociatedObject(self, &CSToastActiveKey, cs_activeToasts, OBJC_ASSOCIATION_RETAIN_NONATOMIC);
    }
    return cs_activeToasts;
}

- (NSMutableArray *)cs_toastQueue {
    NSMutableArray *cs_toastQueue = objc_getAssociatedObject(self, &CSToastQueueKey);
    if (cs_toastQueue == nil) {
        cs_toastQueue = [[NSMutableArray alloc] init];
        objc_setAssociatedObject(self, &CSToastQueueKey, cs_toastQueue, OBJC_ASSOCIATION_RETAIN_NONATOMIC);
    }
    return cs_toastQueue;
}

#pragma mark - Events

- (void)cs_toastTimerDidFinish:(NSTimer *)timer {
    [self cs_hideToast:(UIView *)timer.userInfo];
}

- (void)cs_handleToastTapped:(UITapGestureRecognizer *)recognizer {
    UIView *toast = recognizer.view;
    NSTimer *timer = (NSTimer *)objc_getAssociatedObject(toast, &CSToastTimerKey);
    [timer invalidate];
    
    [self cs_hideToast:toast fromTap:YES];
}

#pragma mark - Activity Methods

- (void)makeToastActivity:(id)position {
    // sanity
    UIView *existingActivityView = (UIView *)objc_getAssociatedObject(self, &CSToastActivityViewKey);
    if (existingActivityView != nil) return;
    
    CSToastStyle *style = [CSToastManager sharedStyle];
    
    UIView *activityView = [[UIView alloc] initWithFrame:CGRectMake(0.0, 0.0, style.activitySize.width, style.activitySize.height)];
    activityView.center = [self cs_centerPointForPosition:position withToast:activityView];
    activityView.backgroundColor = style.backgroundColor;
    activityView.alpha = 0.0;
    activityView.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin | UIViewAutoresizingFlexibleBottomMargin);
    activityView.layer.cornerRadius = style.cornerRadius;
    
    if (style.displayShadow) {
        activityView.layer.shadowColor = style.shadowColor.CGColor;
        activityView.layer.shadowOpacity = style.shadowOpacity;
        activityView.layer.shadowRadius = style.shadowRadius;
        activityView.layer.shadowOffset = style.shadowOffset;
    }
    
    UIActivityIndicatorView *activityIndicatorView = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
    activityIndicatorView.center = CGPointMake(activityView.bounds.size.width / 2, activityView.bounds.size.height / 2);
    [activityView addSubview:activityIndicatorView];
    [activityIndicatorView startAnimating];
    
    // associate the activity view with self
    objc_setAssociatedObject (self, &CSToastActivityViewKey, activityView, OBJC_ASSOCIATION_RETAIN_NONATOMIC);
    
    [self addSubview:activityView];
    
    [UIView animateWithDuration:style.fadeDuration
                          delay:0.0
                        options:UIViewAnimationOptionCurveEaseOut
                     animations:^{
                         activityView.alpha = 1.0;
                     } completion:nil];
}

- (void)hideToastActivity {
    UIView *existingActivityView = (UIView *)objc_getAssociatedObject(self, &CSToastActivityViewKey);
    if (existingActivityView != nil) {
        [UIView animateWithDuration:[[CSToastManager sharedStyle] fadeDuration]
                              delay:0.0
                            options:(UIViewAnimationOptionCurveEaseIn | UIViewAnimationOptionBeginFromCurrentState)
                         animations:^{
                             existingActivityView.alpha = 0.0;
                         } completion:^(BOOL finished) {
                             [existingActivityView removeFromSuperview];
                             objc_setAssociatedObject (self, &CSToastActivityViewKey, nil, OBJC_ASSOCIATION_RETAIN_NONATOMIC);
                         }];
    }
}

#pragma mark - Helpers

- (CGPoint)cs_centerPointForPosition:(id)point withToast:(UIView *)toast {
    CSToastStyle *style = [CSToastManager sharedStyle];
    
    if([point isKindOfClass:[NSString class]]) {
        if([point caseInsensitiveCompare:CSToastPositionTop] == NSOrderedSame) {
            return CGPointMake(self.bounds.size.width/2, (toast.frame.size.height / 2) + style.verticalPadding);
        } else if([point caseInsensitiveCompare:CSToastPositionCenter] == NSOrderedSame) {
            return CGPointMake(self.bounds.size.width / 2, self.bounds.size.height / 2);
        }
    } else if ([point isKindOfClass:[NSValue class]]) {
        return [point CGPointValue];
    }
    
    // default to bottom
    return CGPointMake(self.bounds.size.width/2, (self.bounds.size.height - (toast.frame.size.height / 2)) - style.verticalPadding);
}

@end

@implementation CSToastStyle

#pragma mark - Constructors

- (instancetype)initWithDefaultStyle {
    self = [super init];
    if (self) {
        self.backgroundColor = [[UIColor blackColor] colorWithAlphaComponent:0.8];
        self.titleColor = [UIColor whiteColor];
        self.messageColor = [UIColor whiteColor];
        self.maxWidthPercentage = 0.8;
        self.maxHeightPercentage = 0.8;
        self.horizontalPadding = 10.0;
        self.verticalPadding = 10.0;
        self.cornerRadius = 10.0;
        self.titleFont = [UIFont boldSystemFontOfSize:16.0];
        self.messageFont = [UIFont systemFontOfSize:16.0];
        self.titleAlignment = NSTextAlignmentLeft;
        self.messageAlignment = NSTextAlignmentLeft;
        self.titleNumberOfLines = 0;
        self.messageNumberOfLines = 0;
        self.displayShadow = NO;
        self.shadowOpacity = 0.8;
        self.shadowRadius = 6.0;
        self.shadowOffset = CGSizeMake(4.0, 4.0);
        self.imageSize = CGSizeMake(80.0, 80.0);
        self.activitySize = CGSizeMake(100.0, 100.0);
        self.fadeDuration = 0.2;
    }
    return self;
}

- (void)setMaxWidthPercentage:(CGFloat)maxWidthPercentage {
    _maxWidthPercentage = MAX(MIN(maxWidthPercentage, 1.0), 0.0);
}

- (void)setMaxHeightPercentage:(CGFloat)maxHeightPercentage {
    _maxHeightPercentage = MAX(MIN(maxHeightPercentage, 1.0), 0.0);
}

- (instancetype)init NS_UNAVAILABLE {
    return nil;
}

@end

@interface CSToastManager ()

@property (strong, nonatomic) CSToastStyle *sharedStyle;
@property (assign, nonatomic, getter=isTapToDismissEnabled) BOOL tapToDismissEnabled;
@property (assign, nonatomic, getter=isQueueEnabled) BOOL queueEnabled;
@property (assign, nonatomic) NSTimeInterval defaultDuration;
@property (strong, nonatomic) id defaultPosition;

@end

@implementation CSToastManager

#pragma mark - Constructors

+ (instancetype)sharedManager {
    static CSToastManager *_sharedManager = nil;
    static dispatch_once_t oncePredicate;
    dispatch_once(&oncePredicate, ^{
        _sharedManager = [[self alloc] init];
    });
    
    return _sharedManager;
}

- (instancetype)init {
    self = [super init];
    if (self) {
        self.sharedStyle = [[CSToastStyle alloc] initWithDefaultStyle];
        self.tapToDismissEnabled = YES;
        self.queueEnabled = YES;
        self.defaultDuration = 3.0;
        self.defaultPosition = CSToastPositionBottom;
    }
    return self;
}

#pragma mark - Singleton Methods

+ (void)setSharedStyle:(CSToastStyle *)sharedStyle {
    [[self sharedManager] setSharedStyle:sharedStyle];
}

+ (CSToastStyle *)sharedStyle {
    return [[self sharedManager] sharedStyle];
}

+ (void)setTapToDismissEnabled:(BOOL)tapToDismissEnabled {
    [[self sharedManager] setTapToDismissEnabled:tapToDismissEnabled];
}

+ (BOOL)isTapToDismissEnabled {
    return [[self sharedManager] isTapToDismissEnabled];
}

+ (void)setQueueEnabled:(BOOL)queueEnabled {
    [[self sharedManager] setQueueEnabled:queueEnabled];
}

+ (BOOL)isQueueEnabled {
    return [[self sharedManager] isQueueEnabled];
}

+ (void)setDefaultDuration:(NSTimeInterval)duration {
    [[self sharedManager] setDefaultDuration:duration];
}

+ (NSTimeInterval)defaultDuration {
    return [[self sharedManager] defaultDuration];
}

+ (void)setDefaultPosition:(id)position {
    if ([position isKindOfClass:[NSString class]] || [position isKindOfClass:[NSValue class]]) {
        [[self sharedManager] setDefaultPosition:position];
    }
}

+ (id)defaultPosition {
    return [[self sharedManager] defaultPosition];
}

@end
#endif
