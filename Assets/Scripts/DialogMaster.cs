using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DialogMaster : MonoBehaviour
{
    System.Net.Sockets.NetworkStream ns;
    System.Net.Sockets.TcpClient tcp;
    // Start is called before the first frame update
    void Start()
    {
    
        // まずresources以下にあるcsvファイルをすべて読み込む
        ConstantsDic.commUNetworkSettings = ConstantsDic.ReadCSV("network_setting");


        string ipOrHost = "127.0.0.1";
        int port = 1000;
        for(int i=0; i < 6; i++)
        {
            if(ConstantsDic.commUNetworkSettings[i][0] == Names.ID)
            {
                ipOrHost = ConstantsDic.commUNetworkSettings[i][1];
                port = int.Parse(ConstantsDic.commUNetworkSettings[i][2]);
            }
        }

        if (port == 0)
        {
            SceneManager.LoadScene("InPutName");
            // 間違ったIDを入れたらもう一度入れ直し
        }
        else
        {
            //TcpClientを作成し、サーバーと接続
            try
            {
                tcp = new System.Net.Sockets.TcpClient(ipOrHost, port);
                Debug.Log("サーバー({0}:{1})と接続しました。" +
                    ((System.Net.IPEndPoint)tcp.Client.RemoteEndPoint).Address + "," +
                    ((System.Net.IPEndPoint)tcp.Client.RemoteEndPoint).Port + "," +
                    ((System.Net.IPEndPoint)tcp.Client.LocalEndPoint).Address + "," +
                    ((System.Net.IPEndPoint)tcp.Client.LocalEndPoint).Port);

                //NetworkStreamを取得
                ns = tcp.GetStream();

            }
            catch(SocketException e)
            {
                Debug.Log("接続に失敗しました");
                SceneManager.LoadScene("InPutName");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ns.Close();
            tcp.Close();
            Debug.Log("切断しました。");
        }
    }

    public void MessageSender(string data)
    {
        System.Text.Encoding enc = System.Text.Encoding.UTF8;
        byte[] sendBytes = enc.GetBytes(data + '\n');
        //データを送信する
        ns.Write(sendBytes, 0, sendBytes.Length);
        Debug.Log(data);
    }
}
