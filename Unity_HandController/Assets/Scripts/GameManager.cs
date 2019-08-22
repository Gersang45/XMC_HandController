using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject Hand_R;
    [SerializeField]
    private GameObject Hand_L;
    [SerializeField]
    private GameObject cube;

    private const int RECV_SIZE = 19;      // recv 받을 데이터배열의 크기
    private const int START_CHECK = 0;     // 신호의 첫번째 들어오는 값이 정상적으로 들어오고 있는지 비교할 기준값
    private const byte START_TX_SIGNAL = 0x30;


    private SerialPort MySerial = new SerialPort();  // 시리얼 포트 생성    
    private byte[] Rx_Buf = new byte[1];   // DAVE에서 보내주는 값들을 1차적으로 저장하는 배열
    private byte[] Rx_Data = new byte[RECV_SIZE]; // DAVE에서 보내주는 값들을 최종적으로 저장할 배열, 앞으로 써먹을 값들
    private byte[] Tx_Check = { START_TX_SIGNAL };
    private byte[] Rx_Check = { START_TX_SIGNAL };

    [Header("COM PORT Config")]
    [SerializeField]
    private string Com_Port_Name;    //컴포트 이름을 저장하는 변수
    [SerializeField]
    private int Com_Port_BaudRate;   //보드레이트 값을 저장하는 변수


    private void Awake()    // 게임 오브젝트를 생성할 때 최초로 실행됨
    {
        Serialinit();   // Serial 관련 초기화 함수
        Debug.Log("시작!");   // 시작을 알리는 종소리!!
    }

    void Start()   // Update 시작 직전 한번만 실행
    {
        
        //CheckGoodSignal();    // 신호가 제대로 오는지 체크하는 함수
    }

    // Update is called once per frame
    void Update()
    {
        if (MySerial.IsOpen)   // Serial 통신이 열려 있는지 확인
        {
            MySerial.DiscardInBuffer();     // 수신 버퍼에 쌓인 값을 지워줌
            Start_Singal();       // DAVE에게 Unity 로 데이터전송을 시작하라는 신호를 보내는 함수
            try
            {
                MySerial.Read(Rx_Buf, 0, 1);    // 데이터 전송받은 갯수, 데이터는 recvBuf에 배열형식으로 저장됨
                if (Rx_Buf[0] != START_CHECK) // 시작 데이터가 들어왔는지 체크하는 조건문
                {
                    Rx_Data[0] = Rx_Buf[0]; // 신호의 첫 데이터를 저장함.
                    for (int i = 0; i < RECV_SIZE - 2; i++) // 총 16개의 신호를 받을것임.
                    {
                        MySerial.Read(Rx_Buf, 0, 1);    // 데이터 전송받은 갯수, 데이터는 recvBuf에 배열형식으로 저장됨
                        Rx_Data[i + 1] = Rx_Buf[0]; // 각 신호들의 값을 데이터에 저장함.
                        Hand_R.GetComponent<R_GetSensor>().Get_Value(Rx_Data);     //받아온 데이터를 Hand_R로 전송함
                        //Debug.Log("Signal" + i + " : " + Rx_Data[i]);  // 제대로 값이 오고 있는지 주기적으로 확인함
                    }
                }
                
            }
            catch (TimeoutException e)   // Timeout 에러를 죽일때 사용
            {
                Debug.Log(e);
            }
        }
        else
        {
            Debug.Log("Not Open!!");
        }
    }

    private void OnApplicationQuit()    // App이 꺼지기 전에 실행됨
    {
        MySerial.DiscardInBuffer();     // 수신 버퍼에 쌓인 값을 지워줌
        MySerial.DiscardOutBuffer();    // 송신 버퍼에 쌓인 값을 지워줌
        MySerial.Close();   // Serial 통신을 닫음
    }

    private void Start_Singal()  // DAVE에게 Unity 로 데이터전송을 시작하라는 신호를 보내는 함수
    {
        if (MySerial.IsOpen)
        {
            MySerial.Write(Tx_Check, 0, 1);   // 신호 보내라아~
        }
    }
/*
    private void CheckGoodSignal()  // 신호가 제대로 오는지 체크하는 함수      // 이젠 안씀
    {
        bool BelivData = false;     // 받고있는 신호가 정상적인지 아닌지 체크하는 변수

        MySerial.DiscardInBuffer();     // 수신 버퍼에 쌓인 값을 지워줌

        while (!BelivData)
        {
            for (int i = 0; i < RECV_SIZE; i++)     // DAVE에서 보내는 데이터배열의 크기만큼 실행된다.
            {
                MySerial.Read(Rx_Buf, 0, 1);    // 데이터 전송받은 갯수, 데이터는 recvBuf에 배열형식으로 저장됨
                if (i == 0) //i == 0일때 
                {
                    if (Rx_Buf[0] == START_CHECK)    // 첫번쨰로 들어오는 값이 Dave에서 SIGNAL_CHECK 값으로, 0을 보내기로 되어있음
                    {
                        BelivData = true;
                    }
                    else
                    {
                        BelivData = false;
                    }
                }
                if (BelivData)
                {
                    Debug.Log("Good Signal" + i + " : " + Rx_Buf[0]);  // 신호가 올바르게 출력되고있음을 보여준다.
                }
                else
                {
                    Debug.Log("Bad Signal" + i + " : " + Rx_Buf[0]);   // 신호가 개판으로 출력되고있음을 보여준다.
                    i--;
                }
            }
        }
    }
*/
    private void Serialinit()   //Serial 관련 초기화 함수
    {
        MySerial.PortName = "" + Com_Port_Name;     // 사용자가 사용할 COM 을 미리 입력해줘서 수정가능 내블루투스 : COM7, 민수형블루투스 : COM8
        MySerial.BaudRate = Com_Port_BaudRate;      // 사용자가 사용할 BaudRate를 수정가능 내블루투스 : 9600, 민수형블루투스 : 115200
        MySerial.Parity = Parity.None;
        MySerial.DataBits = 8;
        MySerial.StopBits = StopBits.One;
        MySerial.ReadTimeout = 3000;

        MySerial.Open();
        Debug.Log("OPEN");  // OPEN 신호
        MySerial.DiscardInBuffer();     // 수신 버퍼에 쌓인 값을 지워줌
        MySerial.DiscardOutBuffer();    // 송신 버퍼에 쌓인 값을 지워줌
    }
}