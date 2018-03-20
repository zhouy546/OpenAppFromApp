using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OpenAnapp : MonoBehaviour {
    public Image Black;
    public Text text,DebbugTime;
    public bool OpenOnce;
    public InputField Mins, Hours;
    string Hour, Min;
    bool fail = false;
    string bundleId = "com.alibaba.android.rimet"; //target bundle id for gallery!?

    // Use this for initialization
    void Start () {
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
    public void OpenApp() {
        Debug.Log("Run");
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch (System.Exception e)
        {
            fail = true;
        }

        if (fail)
        { //open app in store
            Application.OpenURL("https://google.com");
        }
        else //I want to open Gallery App? But what activity?
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
    }

	// Update is called once per frame
	void Update () {
         Hour = System.DateTime.Now.Hour.ToString();
         Min = System.DateTime.Now.Minute.ToString();
        DebbugTime.text = "Hour:" + Hour + "Min:" + Min;
       
    }

    public void  Listening() {
        Black.gameObject.SetActive(true);
        StartCoroutine(check());
    }

    public void StopListening() {
        StopAllCoroutines();
        resetTheOpener();
    }


   public IEnumerator check() {
        Debug.Log("check");
        if (Mins.text == Min && Hours.text == Hour)
        {
            if (!OpenOnce)
            {
                OpenOnce = true;
                text.text = "停止打开钉钉";
                OpenApp();
                StopListening();
            }
        }
        
        yield return new WaitForSeconds(1);
        StartCoroutine(check());

    }

    public void resetTheOpener()
    {
        Black.gameObject.SetActive(false);
        OpenOnce = false;
        text.text = "将会打开钉钉";
    }
}
