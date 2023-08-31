using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// 구조체
// class와 거의 같다! 충격적!!
[System.Serializable]
public struct UserInfo
{
    // 전역변수, 함수 만들기 가능
    // but 상속을 못받음!!
    // 변수만 묶는다고 생각하셈
    // 이름 string
    // 나이 int
    // 키 float
    // 성별 (true : 여성, false : 남성) bool
    // 좋아하는 음식 List<string>
    // 취미 List<string>

    public string name;
    public int age;
    public float height;
    public bool gender;
    public List<string> favoritFood;
    // public List<string> hobby;

}

// json을 품은 json
[System.Serializable]
public struct FriendInfo
{
    public List<UserInfo> data;
}

public class JsonStudy : MonoBehaviour
{
    // 나의 정보
    public UserInfo myInfo;


    // User 정보를 여러개 들고있는 변수
    public List<UserInfo> friendList = new List<UserInfo>();


    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            myInfo = new UserInfo();

            myInfo.name = "박동식이" + i; // 댜른 data가 들어가는지 확인차 + i
            myInfo.age = 28;
            myInfo.height = 187;
            myInfo.gender = true;
            myInfo.favoritFood = new List<string>();
            myInfo.favoritFood.Add("편백찜");
            myInfo.favoritFood.Add("마라샹궈");
            myInfo.favoritFood.Add("꿀무지개설기");

            friendList.Add(myInfo);
        }


        // JsonDataTest > 개명 > FriendInfo
        //JsonDataTest test = new JsonDataTest(); // struct는 new 안해줘도 되지만 혼동을 방지해서 해주는게 좋음
        //test.jsonData = myInfo;

        //string s = JsonUtility.ToJson(test, true);
        //print(s);

        FriendInfo info = new FriendInfo(); // friendList의 key값을 만들기위한 구조체
        info.data = friendList; // data가 key, friendList가 value
        string s = JsonUtility.ToJson(info, true);
        print(s);
    }

    // Update is called once per frame
    void Update()
    {
        // 1번 키를 누르면
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            // myInfo를 Json형태로 만들기
            string jsonData = JsonUtility.ToJson(myInfo);
            print(jsonData);

            // jsonData를 파일로 저장
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Create);

            // json string data를 byte 배열로 만듦
            // UTF8이 범용성이 가장 좋음
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            // byteData를 file에 쓰기
            // bateDate 배열의 0부터 byteData.Length까지 저장
            file.Write(byteData, 0, byteData.Length);
            file.Close();
        }

        // 2번 키를 누르면 myInfo.txt를 읽어옴
        // 1번 과정을 반대로 하면 되겠죠?
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            // myInfo.txt 읽기
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Open);

            // file의 크기만큼 byte 배열을 할당(만듦)
            byte[] byteData = new byte[file.Length];

            // byteData에 file의 내용을 읽어오기
            file.Read(byteData, 0, byteData.Length);

            // 파일 닫기
            file.Close();

            // byte data를 string으로 변경
            string jsonData = Encoding.UTF8.GetString(byteData);

            // string의 json data를 myInfo에 parsing한다.
            // 문자열을 하나하나 잘라서 넣는 작업
            myInfo = JsonUtility.FromJson<UserInfo>(jsonData); // jsonData를 UserInfo형으로 받아옴


        }

    }
}
