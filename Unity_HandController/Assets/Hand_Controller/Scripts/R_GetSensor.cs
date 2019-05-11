using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_GetSensor : MonoBehaviour {

    private byte[] sensorValue = new byte[10];
    private GameObject Armature_01, hand_r, index_r, middle_r, pinky_palm_r, ring_r, thumb_palm_r, index_r_01, index_r_02, index_r_end;
    private GameObject middle_r_01, middle_r_02, middle_r_end, pinky_r, pinky_r_01, pinky_r_02, pinky_r_end, ring_r_01, ring_r_02, ring_r_end;
    private GameObject thumb_r, thumb_r_01, thumb_r_02, thumb_r_end;

    // Use this for initialization
    void Start () {
        ChildInit();
    }   
	
	// Update is called once per frame
	void Update () {

    }

    public void Get_Value(byte[] sesnsor)
    {
        for(int i = 0; i < 5; i++)
        {
            sensorValue[i] = sesnsor[i+1];
            if(i == 0)
            {
                Debug.Log("잘 받고있다" + i + " : " + sensorValue[i]);
            }
        }
    }

    private void ChildInit()    //각각의 게임오브젝트에 알맞게 설정해줌
    {
        Armature_01 = transform.GetChild(0).gameObject;
        hand_r = Armature_01.transform.GetChild(0).gameObject;

        /////////////////검지/////////////////
        index_r = hand_r.transform.GetChild(0).gameObject;
        index_r_01 = index_r.transform.GetChild(0).gameObject;
        index_r_02 = index_r_01.transform.GetChild(0).gameObject;
        index_r_end = index_r_02.transform.GetChild(0).gameObject;
        /////////////////검지/////////////////


        /////////////////중지/////////////////
        middle_r = hand_r.transform.GetChild(1).gameObject;
        middle_r_01 = middle_r.transform.GetChild(0).gameObject;
        middle_r_02 = middle_r_01.transform.GetChild(0).gameObject;
        middle_r_end = middle_r_02.transform.GetChild(0).gameObject;
        /////////////////중지/////////////////

        /////////////////약지/////////////////
        ring_r = hand_r.transform.GetChild(3).gameObject;
        ring_r_01 = ring_r.transform.GetChild(0).gameObject;
        ring_r_02 = ring_r_01.transform.GetChild(0).gameObject;
        ring_r_end = ring_r_02.transform.GetChild(0).gameObject;
        /////////////////약지/////////////////

        /////////////////소지/////////////////
        pinky_palm_r = hand_r.transform.GetChild(2).gameObject;
        pinky_r = pinky_palm_r.transform.GetChild(0).gameObject;
        pinky_r_01 = pinky_r.transform.GetChild(0).gameObject;
        pinky_r_02 = pinky_r_01.transform.GetChild(0).gameObject;
        pinky_r_end = pinky_r_02.transform.GetChild(0).gameObject;
        /////////////////소지/////////////////

        /////////////////엄지/////////////////
        thumb_palm_r = hand_r.transform.GetChild(4).gameObject;
        thumb_r = thumb_palm_r.transform.GetChild(0).gameObject;
        thumb_r_01 = thumb_r.transform.GetChild(0).gameObject;
        thumb_r_02 = thumb_r_01.transform.GetChild(0).gameObject;
        thumb_r_end = thumb_r_02.transform.GetChild(0).gameObject;
        /////////////////엄지/////////////////
    }

    private void DebugChilde()      //각각의 게임오브젝트 변수에 올바르게 들어가있는지 체크하는 함수
    {
        Debug.Log("Armature == " + Armature_01.name);
        Debug.Log("hand_r == " + hand_r.name);
        Debug.Log("index_r == " + index_r.name);
        Debug.Log("middle_r == " + middle_r.name);
        Debug.Log("pinky_palm_r == " + pinky_palm_r.name);
        Debug.Log("ring_r == " + ring_r.name);
        Debug.Log("thumb_palm_r == " + thumb_palm_r.name);

        Debug.Log("index_r_01 == " + index_r_01.name);
        Debug.Log("index_r_02 == " + index_r_02.name);
        Debug.Log("index_r_end == " + index_r_end.name);

        Debug.Log("middle_r_01 == " + middle_r_01.name);
        Debug.Log("middle_r_02 == " + middle_r_02.name);
        Debug.Log("middle_r_end == " + middle_r_end.name);

        Debug.Log("ring_r_01 == " + ring_r_01.name);
        Debug.Log("ring_r_02 == " + ring_r_02.name);
        Debug.Log("ring_r_end == " + ring_r_end.name);

        Debug.Log("pinky_r == " + pinky_r.name);
        Debug.Log("pinky_r_01 == " + pinky_r_01.name);
        Debug.Log("pinky_r_02 == " + pinky_r_02.name);
        Debug.Log("pinky_r_end == " + pinky_r_end.name);

        Debug.Log("thumb_r == " + thumb_r.name);
        Debug.Log("thumb_r_01 == " + thumb_r_01.name);
        Debug.Log("thumb_r_02 == " + thumb_r_02.name);
        Debug.Log("thumb_r_end == " + thumb_r_end.name);
    }
}
