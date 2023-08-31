using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// �����ϰ� ��������� ������Ʈ�� ����
public class ObjectInfo 
{
    public int type;
    public Transform tr;

}

// ������ �� �ʿ��� ������
public class SaveInfo
{
    public int type;
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
}

public class jsonList
{

}



public class ObjectSaveLoad : MonoBehaviour
{


    // ������� ������Ʈ���� ���� ����
    public List<ObjectInfo> objectList = new List<ObjectInfo>();


    public List<SaveInfo> data = new List<SaveInfo>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 1��Ű�� ������ ������ ���, ũ��, ��ġ, ȸ���� �� ������Ʈ �����
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            // ����� �����ϰ� ���� (0~3)
            int type = Random.Range(0, 4);

            // type ������� game object ������
            GameObject go = GameObject.CreatePrimitive((PrimitiveType)type); // PrimitiveType.cube ��뵵 ����

            // ũ��, ��ġ, ȸ�� �����ϰ� ����
            go.transform.localScale = Vector3.one * Random.Range(0.5f, 2.1f);
            go.transform.position = Random.insideUnitSphere * Random.Range(1.0f, 20.0f); // �� �ȿ��� ������ ��ġ���� ��ȯ
            go.transform.rotation = Random.rotation;

            // ������� ������Ʈ�� ������ ����Ʈ�� ���
            ObjectInfo info = new ObjectInfo();
            info.type = type;
            info.tr = go.transform;

            objectList.Add(info);
        }

        // 2�� ������ objectList ������ Json�� ����
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            // object list�� ������� ������ ���� ������
            List<SaveInfo> saveInfoLis = new List<SaveInfo>();

            for(int i = 0; i < objectList.Count; i++)
            {
                SaveInfo saveInfo = new SaveInfo();
                saveInfo.type = objectList[i].type;
                saveInfo.pos = objectList[i].tr.position;
                saveInfo.rot = objectList[i].tr.rotation;
                saveInfo.scale = objectList[i].tr.localScale;
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            // myInfo.txt �б�
            FileStream file = new FileStream(Application.dataPath + "/objectInfo.txt", FileMode.Open);

            // file�� ũ�⸸ŭ byte �迭�� �Ҵ�(����)
            byte[] byteData = new byte[file.Length];

            // byteData�� file�� ������ �о����
            file.Read(byteData, 0, byteData.Length);

            // ���� �ݱ�
            file.Close();

            // byteData�� Json ������ ���ڿ��� ������
            string jsonData = Encoding.UTF8.GetString(byteData);

            // jsonData�� �̿��ؼ� JsonList�� Parsing����
            jsonList jsonList = JsonUtility.FromJson<jsonList>(jsonData);

            // jsonList.data�� ���� ��ŭ ������Ʈ�� ��������
            //for(int i = 0; i < jsonList.data.count; i++)
            //{

            //}
        }
    }
}
