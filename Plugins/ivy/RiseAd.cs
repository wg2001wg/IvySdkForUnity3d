using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
using UnityEngine.EventSystems;
#endif

/// <summary>
/// Editor模式下的广告测试类，不可以调用该类的方法，SDK会自动调用。
/// </summary>
public class RiseAd : MonoBehaviour {

    private static RiseAd _instance = null;
    private Rect bannerPos;
    private bool bannerShow = false;
    private string bannerContent = "";
    private bool interstitialShow = false;
    private string interstitialContent = "";
    private bool rewardShow = false;
    private string rewardContent = "";
    private float scaleWidth = 1;
    private float scaleHeight = 1;
    private int originScreenWidth = 1;
    private int originScreenHeight = 1;
    private bool toastShow = false;
    private List<string> toastList = new List<string> ();
    private GUIStyle guiStyle = null;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
    private EventSystem curEvent = null;
#endif

    private const string BANNER_DEFAULT_TXT = "Banner AD: ";
    private const string INTERSTITIAL_DEFAULT_TXT = "\nInterstitial AD Test";
    private const string REWARD_DEFAULT_TXT = "Free Coin AD Test: ";
    private const int SCREEN_WIDTH = 854;
    private const int SCREEN_HEIGHT = 480;
    private const int GUI_DEPTH = -99;

    void Awake () {
        if (_instance == null) {
            _instance = this;
        }
        DontDestroyOnLoad (gameObject);
        if (Screen.width > Screen.height) {
            originScreenWidth = SCREEN_WIDTH;
            originScreenHeight = SCREEN_HEIGHT;
        } else {
            originScreenWidth = SCREEN_HEIGHT;
            originScreenHeight = SCREEN_WIDTH;
        }
        scaleWidth = Screen.width * 1f / originScreenWidth;
        scaleHeight = Screen.height * 1f / originScreenHeight;
        guiStyle = new GUIStyle ();
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.alignment = TextAnchor.MiddleCenter;
    }

    public static RiseAd Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<RiseAd> () == null ? new GameObject ("RiseAd").AddComponent<RiseAd> () : _instance;
            }
            return _instance;
        }
    }

#if UNITY_EDITOR
    void OnGUI () {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
        if (curEvent == null) {
            curEvent = EventSystem.current;
        }
#endif
        GUI.depth = GUI_DEPTH;
        if (bannerShow) {
            GUI.backgroundColor = Color.green;
            if (GUI.Button (bannerPos, bannerContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                if (EventSystem.current != null) {
                    EventSystem.current.enabled = false;
                }
#endif
            }
        }
        if (interstitialShow) {
            GUI.backgroundColor = Color.red;
            if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                interstitialShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                if (EventSystem.current != null) {
                    EventSystem.current.enabled = false;
                }
#endif
            }
            if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), interstitialContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                if (EventSystem.current != null) {
                    EventSystem.current.enabled = false;
                }
#endif
            }
        }
        if (rewardShow) {
            GUI.backgroundColor = Color.blue;
            if (GUI.Button (new Rect (Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight), "Close")) {
                rewardShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                if (EventSystem.current != null) {
                    EventSystem.current.enabled = false;
                }
#endif
            }
            if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), rewardContent)) {
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
                if (EventSystem.current != null) {
                    EventSystem.current.enabled = false;
                }
#endif
            }
        }
        if (toastList.Count > 0) {
            GUI.backgroundColor = Color.black;
            GUI.Label (new Rect ((Screen.width - 200 * scaleWidth) * .5f, Screen.height - 100 * scaleHeight, 200 * scaleWidth, 50 * scaleHeight), toastList [0], guiStyle);
        }
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
        if (EventSystem.current != null) {
            EventSystem.current.enabled = true;
        } else if (curEvent != null) {
            curEvent.enabled = true;
            EventSystem.current = curEvent;
        }
#endif
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            interstitialShow = false;
            rewardShow = false;
#if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
            if (EventSystem.current != null) {
                EventSystem.current.enabled = true;
            } else if (curEvent != null) {
                curEvent.enabled = true;
                EventSystem.current = curEvent;
            }
#endif
        }
    }
#endif

    public void ShowBanner (int pos) {
#if UNITY_EDITOR
        bannerContent = BANNER_DEFAULT_TXT + "default";
        bannerShow = true;
        SetBannerPos (pos);
        Toast ("ShowBanner, pos: " + pos);
#endif
    }

    public void ShowBanner (string tag, int pos) {
#if UNITY_EDITOR
        bannerContent = BANNER_DEFAULT_TXT + tag;
        bannerShow = true;
        SetBannerPos (pos);
        Toast ("ShowBanner, tag: " + tag + ", pos: " + pos);
#endif
    }

    public void CloseBanner () {
#if UNITY_EDITOR
        bannerShow = false;
        Toast ("CloseBanner");
#endif
    }

    private void SetBannerPos (int pos) {
#if UNITY_EDITOR
        switch (pos) {
            case RiseSdk.POS_BANNER_LEFT_BOTTOM:
                bannerPos = new Rect (0, Screen.height - 50 * scaleHeight, 320 * scaleWidth, 50 * scaleHeight);
                break;
            case RiseSdk.POS_BANNER_LEFT_TOP:
                bannerPos = new Rect (0, 0, 320 * scaleWidth, 50 * scaleHeight);
                break;
            case RiseSdk.POS_BANNER_MIDDLE_BOTTOM:
                bannerPos = new Rect ((Screen.width - 320 * scaleWidth) * .5f, Screen.height - 50 * scaleHeight, 320 * scaleWidth, 50 * scaleHeight);
                break;
            case RiseSdk.POS_BANNER_MIDDLE_MIDDLE:
                bannerPos = new Rect ((Screen.width - 320 * scaleWidth) * .5f, (Screen.height - 50 * scaleHeight) * .5f, 320 * scaleWidth, 50 * scaleHeight);
                break;
            case RiseSdk.POS_BANNER_MIDDLE_TOP:
                bannerPos = new Rect ((Screen.width - 320 * scaleWidth) * .5f, 0, 320 * scaleWidth, 50 * scaleHeight);
                break;
            case RiseSdk.POS_BANNER_RIGHT_BOTTOM:
                bannerPos = new Rect (Screen.width - 320 * scaleWidth, Screen.height - 50 * scaleHeight, 320 * scaleWidth, 50 * scaleHeight);
                break;
            case RiseSdk.POS_BANNER_RIGHT_TOP:
                bannerPos = new Rect (Screen.width - 320 * scaleWidth, 0, 320 * scaleWidth, 50 * scaleHeight);
                break;
        }
#endif
    }

    public void ShowAd (string tag) {
#if UNITY_EDITOR
        interstitialShow = true;
        interstitialContent = tag + INTERSTITIAL_DEFAULT_TXT;
#endif
    }

    public void ShowRewardAd (int id) {
#if UNITY_EDITOR
        rewardShow = true;
        rewardContent = REWARD_DEFAULT_TXT + "default";
#endif
    }

    public void ShowRewardAd (string tag, int id) {
#if UNITY_EDITOR
        rewardShow = true;
        rewardContent = REWARD_DEFAULT_TXT + tag;
#endif
    }

    public void Pay (int billingId) {
#if UNITY_EDITOR
        switch (EditorUtility.DisplayDialogComplex ("Pay", "Pay: " + billingId, "TRY FAILURE", "NO", "YES")) {
            case 0://TRY FAILURE
                RiseSdkListener.Instance.onPaymentFail (billingId + "");
                break;
            case 1://NO
                RiseSdkListener.Instance.onPaymentCanceled (billingId + "");
                break;
            case 2://YES
                RiseSdkListener.Instance.onPaymentSuccess (billingId + "");
                break;
        }
#endif
    }

    private bool timeCounting = false;
    public void Toast (string msg) {
#if UNITY_EDITOR
        toastList.Add (msg);
        if (!timeCounting) {
            timeCounting = true;
            StartCoroutine (CheckToast ());
        }
#endif
    }

    private IEnumerator CheckToast (float time = 2) {
        yield return new WaitForSeconds (time);
        if (toastList.Count > 0) {
            toastList.RemoveAt (0);
        }
        if (toastList.Count > 0) {
            StartCoroutine (CheckToast ());
        }
        timeCounting = false;
    }

    public void Alert (string title, string msg) {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog (title, msg, "NO", "OK");
#endif
    }

}
