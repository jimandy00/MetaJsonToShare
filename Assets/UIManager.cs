using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickGet()
    {
        // todos 가져오기
        HttpInfo info = new HttpInfo();
        //info.Set(RequestType.GET, "/todos", onReceiveGet);
        // 위 코드 익명함수로 나타내기
        info.Set(RequestType.GET, "/todos", (DownloadHandler downloadHandler) =>
        {
            print("onReceiveGet : " + downloadHandler.text);
        });

        // info의 정보로 요청을 보내자
        // HttpManager.Get().SendRequest();
        HttpManager.Get().SendRequest(info);


    }

    void onReceiveGet(DownloadHandler downloadHandler)
    {
        print("onReceiveGet : " + downloadHandler.text);
    }


    public List<CommentInfo> comments;
    public void OnClickComment()
    {
        HttpInfo info = new HttpInfo();

        info.Set(RequestType.GET, "/comments", (DownloadHandler downloadHandler) =>
        {
            print("코멘트 리스트 : " + downloadHandler.text);

            string jsonData = "{\"jsonList\" : " + downloadHandler.text + "}";

            // 응답받은 jsonData를 변수에 parsing
            JsonList<CommentInfo> commentList = JsonUtility.FromJson<JsonList<CommentInfo>>(jsonData);
            comments = commentList.jsonList;
        });

        // 요청
        HttpManager.Get().SendRequest(info);

    }


    public void PostTest()
    {
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.POST, "/sign_up", (DownloadHandler downloadHandler) =>
        {
            // post data 전송했을 때 서버로부터 응답이 옵니다!
        });

        signUpInfo signUpInfo = new signUpInfo();
        signUpInfo.userName = "dongsik";
        signUpInfo.birthDay = "1650-10-23";
        signUpInfo.age = 17;

        string jsonData = JsonUtility.ToJson(signUpInfo); // 구조체에 담긴 데이터가 json형태로 담긴다.

        info.body = jsonData;

        HttpManager.Get().SendRequest(info);

        // get과 다른건 body setting이 추가된 것, header랑
    }



    // Image 가져오기
    // https://i.pinimg.com/originals/14/41/ce/1441ce4b9a6b6fda20f00f8eb04174e9.jpg
    public void OnClickDownloadImage()
    {
        // 요청 할때마다 HttpInfo 만든다고 생각하면 됨
        HttpInfo info = new HttpInfo();

        info.Set(
            RequestType.TEXTURE,
            "https://i.pinimg.com/originals/14/41/ce/1441ce4b9a6b6fda20f00f8eb04174e9.jpg",
            OnCompleteDownloadTexture, // lamda로 안할때
            false
            );

        HttpManager.Get().SendRequest(info);

        // download
        void OnCompleteDownloadTexture(DownloadHandler downloadHandler)
        {
            // 다운로드된 image 데이터를 sprite로 만든다.
            // 1. textrue2D
            // 2. sprite
            Texture2D texture = ((DownloadHandlerTexture)downloadHandler).texture;
            downloadImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            
          
        }

    }
}
