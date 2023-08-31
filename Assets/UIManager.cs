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
        // todos ��������
        HttpInfo info = new HttpInfo();
        //info.Set(RequestType.GET, "/todos", onReceiveGet);
        // �� �ڵ� �͸��Լ��� ��Ÿ����
        info.Set(RequestType.GET, "/todos", (DownloadHandler downloadHandler) =>
        {
            print("onReceiveGet : " + downloadHandler.text);
        });

        // info�� ������ ��û�� ������
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
            print("�ڸ�Ʈ ����Ʈ : " + downloadHandler.text);

            string jsonData = "{\"jsonList\" : " + downloadHandler.text + "}";

            // ������� jsonData�� ������ parsing
            JsonList<CommentInfo> commentList = JsonUtility.FromJson<JsonList<CommentInfo>>(jsonData);
            comments = commentList.jsonList;
        });

        // ��û
        HttpManager.Get().SendRequest(info);

    }


    public void PostTest()
    {
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.POST, "/sign_up", (DownloadHandler downloadHandler) =>
        {
            // post data �������� �� �����κ��� ������ �ɴϴ�!
        });

        signUpInfo signUpInfo = new signUpInfo();
        signUpInfo.userName = "dongsik";
        signUpInfo.birthDay = "1650-10-23";
        signUpInfo.age = 17;

        string jsonData = JsonUtility.ToJson(signUpInfo); // ����ü�� ��� �����Ͱ� json���·� ����.

        info.body = jsonData;

        HttpManager.Get().SendRequest(info);

        // get�� �ٸ��� body setting�� �߰��� ��, header��
    }



    // Image ��������
    // https://i.pinimg.com/originals/14/41/ce/1441ce4b9a6b6fda20f00f8eb04174e9.jpg
    public void OnClickDownloadImage()
    {
        // ��û �Ҷ����� HttpInfo ����ٰ� �����ϸ� ��
        HttpInfo info = new HttpInfo();

        info.Set(
            RequestType.TEXTURE,
            "https://i.pinimg.com/originals/14/41/ce/1441ce4b9a6b6fda20f00f8eb04174e9.jpg",
            OnCompleteDownloadTexture, // lamda�� ���Ҷ�
            false
            );

        HttpManager.Get().SendRequest(info);

        // download
        void OnCompleteDownloadTexture(DownloadHandler downloadHandler)
        {
            // �ٿ�ε�� image �����͸� sprite�� �����.
            // 1. textrue2D
            // 2. sprite
            Texture2D texture = ((DownloadHandlerTexture)downloadHandler).texture;
            downloadImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            
          
        }

    }
}
