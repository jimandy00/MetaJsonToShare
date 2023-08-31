using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// https://isonplaceholder.typicode.com

// comment에 들어있는 json 자료를 보고 
// struct에 넣어줌
[System.Serializable]
public struct CommentInfo
{
    public int postId;
    public int id;
    public string name;
    public string email;
    public string body;

    // 위에 애들이 여러개 담겨야해
    // json list 가져와
}

[System.Serializable]
public class JsonList<T>
{
    public List<T> jsonList;
}

// 해당 작업은 임시적으로 회원가입 실습을 하는 것
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


// http 링크, GET방식인지 정보를 담은 class
public class HttpInfo
{
    public RequestType requestType; // get, set
    public string url = "";
    public string body;
    public Action<DownloadHandler> onRecieve; // 함수를 담을 수 있는 변수 = Action

    public void Set(RequestType type, string u, Action <DownloadHandler> callback, bool userDefaultUrl = true) // userDefaultUrl은 Image때매 넣음. 기본 url때매..
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
        if(instance == null) // gameobject가 null이다.
        {
            // instance가 비었으면?
            // go 만들고
            GameObject go = new GameObject("HttpStudy"); // go가 hierarchy에 만들어짐
            // go에 http manager component 붙이기
            go.AddComponent<HttpManager>(); // 붙임과 동시에 awke 실행됨

            // 이제부터 Awake가 호출되어서 go에 무언가 들어있는 상태
            // return instance를 하면 가진 instance를 return한다.
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
        // 계속 가지고 다녀야 할 것들이 있다면 위의 로직으로 만들어주면 된다.
        // 하나의 공식이라고 생각하면 된다.
        HttpManager.Get();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // server에게 rest api로 요청(GET, POST, PUT, DELETE)
    public void SendRequest(HttpInfo httpInfo)
    {
        // SERVER에게 요청을 보낼땐 코루틴으로
        StartCoroutine(CoSendRequest(httpInfo));
    }

    IEnumerator CoSendRequest(HttpInfo httpInfo)
    {
        
        // 통신 보내면서 로딩바 돌기가 있다면? 여기 넣고 끝나면 else 뒤에 넣으면 됨!!

        // web 통신을 하려면 UnityWebRequest class를 활용
        UnityWebRequest req = null;

        switch(httpInfo.requestType)
        {
            case RequestType.GET:
                // Get방식으로 request에 정보 셋팅
                //req = UnityWebRequest.Get("https://www.naver.com/");
                req = UnityWebRequest.Get(httpInfo.url);
                break;

            case RequestType.POST:
                req = UnityWebRequest.Post(httpInfo.url, httpInfo.body);
                byte[] byteBody = Encoding.UTF8.GetBytes(httpInfo.body);
                req.uploadHandler = new UploadHandlerRaw(byteBody);
                // post는 header를 설정 해줘야함
                // 이것도 server가 알려주면 우리가 착착착!
                req.SetRequestHeader("Content-Type", "application/json"); // 여러개 들어가면 이거 여러개 써주면 됨
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



        // server에 요청을 보내고 응답이 올때까지 양보
        yield return req.SendWebRequest();

        // 응답이 왔다? 안왔다?
        // 만약에 응답에 성공했다면
        if(req.result == UnityWebRequest.Result.Success)
        {
            // UIManager에서 부르니까 주석처리함
            //print("네트워크 응답 : " + req.downloadHandler.text); // 통신 성공하면 downloadHandler에 data를 담아서 줌




            if(httpInfo.onRecieve != null)
            {
                httpInfo.onRecieve(req.downloadHandler);
            }
        }
        else
        {
            print("네트워크 에러" + req.error);
        }
    }
}
