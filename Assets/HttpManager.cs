using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// https://isonplaceholder.typicode.com

// comment�� ����ִ� json �ڷḦ ���� 
// struct�� �־���
[System.Serializable]
public struct CommentInfo
{
    public int postId;
    public int id;
    public string name;
    public string email;
    public string body;

    // ���� �ֵ��� ������ ��ܾ���
    // json list ������
}

[System.Serializable]
public class JsonList<T>
{
    public List<T> jsonList;
}

// �ش� �۾��� �ӽ������� ȸ������ �ǽ��� �ϴ� ��
[Serializable]
public struct signUpInfo
{
    public string userName;
    public string birthDay;
    public int age;
}

public enum RequestType
{
    GET,
    POST,
    PUT,
    DELETE,
    TEXTURE
}


// http ��ũ, GET������� ������ ���� class
public class HttpInfo
{
    public RequestType requestType; // get, set
    public string url = "";
    public string body;
    public Action<DownloadHandler> onRecieve; // �Լ��� ���� �� �ִ� ���� = Action

    public void Set(RequestType type, string u, Action <DownloadHandler> callback, bool userDefaultUrl = true) // userDefaultUrl�� Image���� ����. �⺻ url����..
    {
        requestType = type;

        if(userDefaultUrl) url = "https://jsonplaceholder.typicode.com" + u;
        url += u;
        onRecieve = callback;
    }
}



public class HttpManager : MonoBehaviour
{
    private static HttpManager instance;
    public static HttpManager Get()
    {
        if(instance == null) // gameobject�� null�̴�.
        {
            // instance�� �������?
            // go �����
            GameObject go = new GameObject("HttpStudy"); // go�� hierarchy�� �������
            // go�� http manager component ���̱�
            go.AddComponent<HttpManager>(); // ���Ӱ� ���ÿ� awke �����

            // �������� Awake�� ȣ��Ǿ go�� ���� ����ִ� ����
            // return instance�� �ϸ� ���� instance�� return�Ѵ�.
        }
        return instance;
    }

    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // ��� ������ �ٳ�� �� �͵��� �ִٸ� ���� �������� ������ָ� �ȴ�.
        // �ϳ��� �����̶�� �����ϸ� �ȴ�.
        HttpManager.Get();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // server���� rest api�� ��û(GET, POST, PUT, DELETE)
    public void SendRequest(HttpInfo httpInfo)
    {
        // SERVER���� ��û�� ������ �ڷ�ƾ����
        StartCoroutine(CoSendRequest(httpInfo));
    }

    IEnumerator CoSendRequest(HttpInfo httpInfo)
    {
        
        // ��� �����鼭 �ε��� ���Ⱑ �ִٸ�? ���� �ְ� ������ else �ڿ� ������ ��!!

        // web ����� �Ϸ��� UnityWebRequest class�� Ȱ��
        UnityWebRequest req = null;

        switch(httpInfo.requestType)
        {
            case RequestType.GET:
                // Get������� request�� ���� ����
                //req = UnityWebRequest.Get("https://www.naver.com/");
                req = UnityWebRequest.Get(httpInfo.url);
                break;

            case RequestType.POST:
                req = UnityWebRequest.Post(httpInfo.url, httpInfo.body);
                byte[] byteBody = Encoding.UTF8.GetBytes(httpInfo.body);
                req.uploadHandler = new UploadHandlerRaw(byteBody);
                // post�� header�� ���� �������
                // �̰͵� server�� �˷��ָ� �츮�� ������!
                req.SetRequestHeader("Content-Type", "application/json"); // ������ ���� �̰� ������ ���ָ� ��
                break;

            case RequestType.PUT:
                req = UnityWebRequest.Put(httpInfo.url, httpInfo.body);
                break;

            case RequestType.DELETE:
                req = UnityWebRequest.Delete(httpInfo.url);
                break;

            case RequestType.TEXTURE:
                req = UnityWebRequestTexture.GetTexture(httpInfo.url);
                break;

            
        }



        // server�� ��û�� ������ ������ �ö����� �纸
        yield return req.SendWebRequest();

        // ������ �Դ�? �ȿԴ�?
        // ���࿡ ���信 �����ߴٸ�
        if(req.result == UnityWebRequest.Result.Success)
        {
            // UIManager���� �θ��ϱ� �ּ�ó����
            //print("��Ʈ��ũ ���� : " + req.downloadHandler.text); // ��� �����ϸ� downloadHandler�� data�� ��Ƽ� ��




            if(httpInfo.onRecieve != null)
            {
                httpInfo.onRecieve(req.downloadHandler);
            }
        }
        else
        {
            print("��Ʈ��ũ ����" + req.error);
        }
    }
}
