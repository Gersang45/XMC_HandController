using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private GameObject Hand_R;
    [SerializeField]
    private GameObject Hand_L;
    [SerializeField]
    private GameObject cube;


    private SerialPort MySerial = new SerialPort();  // 시리얼 포트 생성
    private const int SIGNAL_CHECK = 0;     // CheckGoodSignal 함수에서 신호의 첫번째 들어오는 값이 정상적으로 들어오고 있는지 비교할 기준값
    private const int RECV_SIZE = 19;      // recv 받을 데이터배열의 크기
    private byte[] recvBuf = new byte[1];   // DAVE에서 보내주는 값들을 1차적으로 저장하는 배열
    private byte[] recvData = new byte[RECV_SIZE]; // DAVE에서 보내주는 값들을 최종적으로 저장할 배열, 앞으로 써먹을 값들

    [Header("COM PORT Config")]
    [SerializeField]
    private string Com_Port_Name;    //컴포트 이름을 저장하는 변수
    [SerializeField]
    private int Com_Port_BaudRate;   //보드레이트 값을 저장하는 변수


    private void Awake()    // 게임 오브젝트를 생성할 때 최초로 실행됨
    {
        //string[] Portname = SerialPort.GetPortNames();    포트 이름 받아오는 함수인데 이젠 안씀


        Serialinit();   // Serial 관련 초기화 함수
        Debug.Log("시작!");   // 시작을 알리는 종소리!!

    }

    void Start()   // Update 시작 직전 한번만 실행
    {
        bool chcek = false;
        chcek = CheckGoodSignal();  // 신호가 제대로 오는지 체크하는 함수
        Debug.Log("Filtering : " + chcek);  // 처리완료 신호
    }

    // Update is called once per frame
    void Update()
    {
        if (MySerial.IsOpen)   // Serial 통신이 열려 있는지 확인
        {
            try
            {
                for (int i = 0; i < RECV_SIZE; i++)     // DAVE에서 보내는 데이터배열의 크기만큼 실행된다.
                {
                    MySerial.Read(recvBuf, 0, 1);    // 데이터 전송받은 갯수, 데이터는 recvBuf에 배열형식으로 저장됨, Byte라서 최대 0~255까지 
                    recvData[i] = recvBuf[0];   // recvData 배열에 저장함, 앞으로 써먹을 값들
                    if (i == 0) //i == 0일때 
                    {
                        if (recvBuf[0] != SIGNAL_CHECK)    // 첫번쨰로 들어오는 값이 Dave에서 SIGNAL_CHECK 값으로, 0을 보내기로 되어있음
                        {
                            Debug.Log("뭐가 이상하게 들어온다?");
                            CheckGoodSignal();
                        }
                    }
                    //Debug.Log("Signal" + i + " : " + recvData[i]);  // 제대로 값이 오고 있는지 주기적으로 확인함
                }
                Hand_R.GetComponent<R_GetSensor>().Get_Value(recvData);     //받아온 데이터를 Hand_R로 전송함
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
        MySerial.DiscardOutBuffer();    // 수신 버퍼에 쌓인 값을 지워줌
        MySerial.Close();   // Serial 통신을 닫음
    }

    private bool CheckGoodSignal()  // 신호가 제대로 오는지 체크하는 함수
    {
        bool BelivData = false;     // 받고있는 신호가 정상적인지 아닌지 체크하는 변수

        if (MySerial.IsOpen)
        {
            Debug.Log("Opened!!");
            while (!BelivData)
            {
                for (int i = 0; i < RECV_SIZE; i++)     // DAVE에서 보내는 데이터배열의 크기만큼 실행된다.
                {
                    MySerial.Read(recvBuf, 0, 1);    // 데이터 전송받은 갯수, 데이터는 recvBuf에 배열형식으로 저장됨
                    if (i == 0) //i == 0일때 
                    {
                        if (recvBuf[0] == SIGNAL_CHECK)    // 첫번쨰로 들어오는 값이 Dave에서 SIGNAL_CHECK 값으로, 0을 보내기로 되어있음
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
                        Debug.Log("Good Signal" + i + " : " + recvBuf[0]);  // 신호가 올바르게 출력되고있음을 보여준다.
                    }
                    else
                    {
                        Debug.Log("Bad Signal" + i + " : " + recvBuf[0]);   // 신호가 개판으로 출력되고있음을 보여준다.
                        i--;
                    }
                }
            }
        }
        return BelivData;
    }

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
    }
}
