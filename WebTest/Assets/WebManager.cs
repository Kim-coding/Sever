using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameResult
{
    public string UserName;
    public int Score;
}
// Login - 인증 토큰을 주고 이후에 토큰으로 인증
// Game Start - DB
// Game Result
// 웹 서버 같은 경우 계속 연결되어 있는 것이 아니기 때문에 토큰을 사용하는 방식으로 인증을 해주면 된다.


public class WebManager : MonoBehaviour
{
    string _baseUrl = "https://localhost:7106/api";
    public TMP_InputField inputUserName;
    public GameResult res = new GameResult();
    void Start()
    {
        res.Score = 999;

        //SendPostRequest("ranking", res, (uwr) =>
        //{
        //    Debug.Log("TODO : UI 갱신하기"); //  es)다른 함수 연결해 주는 코드
        //});

        //SendGetAllRequest("ranking", (uwr) =>
        //{
        //    Debug.Log("TODO : UI 갱신하기"); 
        //});
    }

    public void OnButton()
    {
        if(inputUserName.text != "")
        {
            res.UserName = inputUserName.text;
            SendPostRequest("ranking", res, (uwr) =>
            {
                Debug.Log("TODO : UI 갱신하기"); 
            });
        }
    }

        public void SendPostRequest(string url, object obj, Action<UnityWebRequest> callback)
    {
        StartCoroutine(CoSendWebRequest(url, "POST", obj, callback));
    }
    public void SendGetAllRequest(string url, Action<UnityWebRequest> callback)
    {
        StartCoroutine(CoSendWebRequest(url, "GET", null, callback));
    }

    IEnumerator CoSendWebRequest(string url, string method, object obj, Action<UnityWebRequest>callback)
    {
        string sendUrl = $"{_baseUrl}/{url}/";

        byte[] jsonBytes = null;

        if(obj != null)
        {
            string jsonStr = JsonUtility.ToJson(obj);
            jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
        }

        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log("Recy " + uwr.downloadHandler.text);
            callback.Invoke(uwr);
        }
    }

    
}
