using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// 랜덤하게 만들어지는 오브젝트의 정보
public class ObjectInfo 
{
    public int type;
    public Transform tr;

}

// 저장할 때 필요한 데이터
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


    // 만들어진 오브젝트들을 담을 변수
    public List<ObjectInfo> objectList = new List<ObjectInfo>();


    public List<SaveInfo> data = new List<SaveInfo>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 1번키를 누르면 랜덤한 모양, 크기, 위치, 회전이 된 오프젝트 만들기
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 모양을 랜덤하게 뽑자 (0~3)
            int type = Random.Range(0, 4);

            // type 모양으로 game object 만들자
            GameObject go = GameObject.CreatePrimitive((PrimitiveType)type); // PrimitiveType.cube 사용도 가능

            // 크기, 위치, 회전 랜덤하게 하자
            go.transform.localScale = Vector3.one * Random.Range(0.5f, 2.1f);
            go.transform.position = Random.insideUnitSphere * Random.Range(1.0f, 20.0f); // 구 안에서 랜덤한 위치값을 반환
            go.transform.rotation = Random.rotation;

            // 만들어진 오브젝트의 정보를 리스트에 담기
            ObjectInfo info = new ObjectInfo();
            info.type = type;
            info.tr = go.transform;

            objectList.Add(info);
        }

        // 2번 누르면 objectList 정보를 Json에 저장
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            // object list를 기반으로 저장할 정보 빼오기
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
            // myInfo.txt 읽기
            FileStream file = new FileStream(Application.dataPath + "/objectInfo.txt", FileMode.Open);

            // file의 크기만큼 byte 배열을 할당(만듦)
            byte[] byteData = new byte[file.Length];

            // byteData에 file의 내용을 읽어오기
            file.Read(byteData, 0, byteData.Length);

            // 파일 닫기
            file.Close();

            // byteData를 Json 형태의 문자열로 만들자
            string jsonData = Encoding.UTF8.GetString(byteData);

            // jsonData를 이용해서 JsonList에 Parsing하자
            jsonList jsonList = JsonUtility.FromJson<jsonList>(jsonData);

            // jsonList.data의 갯수 만큼 오브젝트를 생성하자
            //for(int i = 0; i < jsonList.data.count; i++)
            //{

            //}
        }
    }
}
