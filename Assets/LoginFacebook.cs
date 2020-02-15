using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class LoginFacebook : MonoBehaviour
{

    public Text txt;
    
    void Awake ()
    {
        if (!FB.IsInitialized) {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        } else {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    
    private void InitCallback ()
    {
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity (bool isGameShown)
    {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void Login()
    {
        var perms = new List<string>(){"public_profile", "email"};
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    
    private void AuthCallback (ILoginResult result) {
        if (FB.IsLoggedIn) {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log("MyApp " + aToken.UserId);
            txt.text = aToken.UserId;
            
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions) {
                Debug.Log("MyApp " + perm);
            }
            FetchFBProfile ();
        } else {
            Debug.Log("MyApp " + "User cancelled login");
        }
    }
    private void FetchFBProfile () {
        FB.API("/me?fields=name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string,string>(){});
    }
    
    private void FetchProfileCallback (IResult result) {
 
        if (result.Error == null) {
            Debug.Log ("MyApp " + result.ResultDictionary ["id"].ToString ());
            Debug.Log ("MyApp " + result.ResultDictionary ["name"].ToString ());
            Debug.Log ("MyApp " + result.ResultDictionary ["email"].ToString ());
            txt.text = txt.text + " " + result.ResultDictionary["email"];
            

        } else {
            Debug.Log ("MyApp " + result.Error);
            txt.text = txt.text + " " + result.Error;

        }
 
    }
}
