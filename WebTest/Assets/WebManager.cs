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
// Login - ���� ��ū�� �ְ� ���Ŀ� ��ū���� ����
// Game Start - DB
// Game Result
// �� ���� ���� ��� ��� ����Ǿ� �ִ� ���� �ƴϱ� ������ ��ū�� ����ϴ� ������� ������ ���ָ� �ȴ�.


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
        //    Debug.Log("TODO : UI �����ϱ�"); //  es)�ٸ� �Լ� ������ �ִ� �ڵ�
        //});

        //SendGetAllRequest("ranking", (uwr) =>
        //{
        //    Debug.Log("TODO : UI �����ϱ�"); 
        //});
    }

    public void OnButton()
    {
        if(inputUserName.text != "")
        {
            res.UserName = inputUserName.text;
            SendPostRequest("ranking", res, (uwr) =>
            {
                Debug.Log("TODO : UI �����ϱ�"); 
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
