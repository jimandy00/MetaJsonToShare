using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// ����ü
// class�� ���� ����! �����!!
[System.Serializable]
public struct UserInfo
{
    // ��������, �Լ� ����� ����
    // but ����� ������!!
    // ������ ���´ٰ� �����ϼ�
    // �̸� string
    // ���� int
    // Ű float
    // ���� (true : ����, false : ����) bool
    // �����ϴ� ���� List<string>
    // ��� List<string>

    public string name;
    public int age;
    public float height;
    public bool gender;
    public List<string> favoritFood;
    // public List<string> hobby;

}

// json�� ǰ�� json
[System.Serializable]
public struct FriendInfo
{
    public List<UserInfo> data;
}

public class JsonStudy : MonoBehaviour
{
    // ���� ����
    public UserInfo myInfo;


    // User ������ ������ ����ִ� ����
    public List<UserInfo> friendList = new List<UserInfo>();


    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            myInfo = new UserInfo();

            myInfo.name = "�ڵ�����" + i; // ���� data�� ������ Ȯ���� + i
            myInfo.age = 28;
            myInfo.height = 187;
            myInfo.gender = true;
            myInfo.favoritFood = new List<string>();
            myInfo.favoritFood.Add("�����");
            myInfo.favoritFood.Add("���󼧱�");
            myInfo.favoritFood.Add("�ܹ���������");

            friendList.Add(myInfo);
        }


        // JsonDataTest > ���� > FriendInfo
        //JsonDataTest test = new JsonDataTest(); // struct�� new �����൵ ������ ȥ���� �����ؼ� ���ִ°� ����
        //test.jsonData = myInfo;

        //string s = JsonUtility.ToJson(test, true);
        //print(s);

        FriendInfo info = new FriendInfo(); // friendList�� key���� ��������� ����ü
        info.data = friendList; // data�� key, friendList�� value
        string s = JsonUtility.ToJson(info, true);
        print(s);
    }

    // Update is called once per frame
    void Update()
    {
        // 1�� Ű�� ������
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            // myInfo�� Json���·� �����
            string jsonData = JsonUtility.ToJson(myInfo);
            print(jsonData);

            // jsonData�� ���Ϸ� ����
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Create);

            // json string data�� byte �迭�� ����
            // UTF8�� ���뼺�� ���� ����
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            // byteData�� file�� ����
            // bateDate �迭�� 0���� byteData.Length���� ����
            file.Write(byteData, 0, byteData.Length);
            file.Close();
        }

        // 2�� Ű�� ������ myInfo.txt�� �о��
        // 1�� ������ �ݴ�� �ϸ� �ǰ���?
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            // myInfo.txt �б�
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Open);

            // file�� ũ�⸸ŭ byte �迭�� �Ҵ�(����)
            byte[] byteData = new byte[file.Length];

            // byteData�� file�� ������ �о����
            file.Read(byteData, 0, byteData.Length);

            // ���� �ݱ�
            file.Close();

            // byte data�� string���� ����
            string jsonData = Encoding.UTF8.GetString(byteData);

            // string�� json data�� myInfo�� parsing�Ѵ�.
            // ���ڿ��� �ϳ��ϳ� �߶� �ִ� �۾�
            myInfo = JsonUtility.FromJson<UserInfo>(jsonData); // jsonData�� UserInfo������ �޾ƿ�


        }

    }
}
