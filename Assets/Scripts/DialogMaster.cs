using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class DialogMaster : MonoBehaviour
{
    GameObject master;
    ChoiceControl choiceControl;
    public InputField inputfield;
    SceneDrawer sceneDrawer;

    System.Net.Sockets.NetworkStream ns;
    System.Net.Sockets.TcpClient tcp;

    // <**シーンの初期設定
    public static string[] Scenes = { "ST", "TE" };
    public static int SceneNum = 0; // どのシーンにいるのか
    public static int SequenceTENum = 1; // 今シーンTEでどの遷移状態にいるのか

    public static List<GameObject> SeqContainer = new List<GameObject>();

    // シーンの初期設定**>

    // Start is called before the first frame update
    void Start()
    {
        // <**まずresources以下にあるcsvファイルをすべて読み込む  あとでPCの方だけに変えておこう
        ConstantsDic.commUNetworkSettings = ConstantsDic.ReadCSV("network_setting");
        ConstantsDic.MNetworkSettings = ConstantsDic.ReadCSV("network_to_M");

        // 発話の読み込み
        ConstantsDic.mainClaims = ConstantsDic.ReadCSV("main_claims");
        ConstantsDic.SequenceTE = ConstantsDic.ReadCSV("001_te_sequence");
        ConstantsDic.TranScriptST = ConstantsDic.ReadCSV("000_st");
        ConstantsDic.TranScriptTE = ConstantsDic.ReadCSV("001_te");
        ConstantsDic.OnScreenTE = ConstantsDic.ReadCSV("001_te_onScreen");
        ConstantsDic.FukidashiTE = ConstantsDic.ReadCSV("001_te_Fukidashi");

        // まずresources以下にあるcsvファイルをすべて読み込む**>


        master = GameObject.Find("Master");
        choiceControl = master.GetComponent<ChoiceControl>();
        sceneDrawer = master.GetComponent<SceneDrawer>();

        // <** サーバーに接続
        string ipOrHost = "127.0.0.1";
        int port = 1000;

        ipOrHost = ConstantsDic.MNetworkSettings[0][0];
        port = int.Parse(ConstantsDic.MNetworkSettings[0][1]);

        /*for (int i = 0; i < 6; i++)
        {
            if (ConstantsDic.commUNetworkSettings[i][0] == Names.ID)
            {
                ipOrHost = ConstantsDic.commUNetworkSettings[i][1];
                port = int.Parse(ConstantsDic.commUNetworkSettings[i][2]);
            }
        }*/


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

            //接続出来たらロボットが初めの挨拶をする
            Debug.Log("初期化:" + Scenes[SceneNum] + SequenceTENum.ToString("D2"));
            string[] temp;
            temp = ConstantsDic.SearchUtterance(Names.ID, Scenes[SceneNum], 1, ConstantsDic.TranScriptST);
            MessageSender(ConstantsDic.FixTranscript(temp[3], Names.ID));

            SceneNum++;


            // TE01以外を隠す
            int i = 0;
            GameObject g;
            foreach (string[] seq in ConstantsDic.SequenceTE)
            {
                if (i == 0)
                {
                    g = GameObject.Find("OpinionInputField");
                    SeqContainer.Add(g);
                    i++;
                    continue;
                }
                g = GameObject.Find(seq[0]);
                SeqContainer.Add(g);
                g.SetActive(false);
                i++;
            }
            // TE01だけアクティブにする！
            SeqContainer[SequenceTENum].SetActive(true);


        }
        catch (SocketException e)
        {
            Debug.Log("接続に失敗しました");
            this.Start();
            //SceneManager.LoadScene("InPutName");
        }


        // サーバーに接続**>


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

    public void OnButtonProceed()
    {
        SceneDrawer.IsInConfirmation = false;
        string seqInputInfo = ConstantsDic.SequenceTE[SequenceTENum][2];
        string seqProceedInfo = ConstantsDic.SequenceTE[SequenceTENum][1];
        SeqContainer[SequenceTENum].SetActive(false);

        // 進むボタンを押したときにシーンを更新する、文章を描画しなおす、選択肢ボタンを配置しなおす
        Debug.Log("Now we are:" + Scenes[SceneNum] + SequenceTENum.ToString("D2"));

        if (seqProceedInfo == "False")
        {
            SequenceTENum++;
            choiceControl.PlaceChoice(Scenes[SceneNum], SequenceTENum);
            SeqContainer[SequenceTENum].SetActive(true);

            sceneDrawer.SetText();

            string[] temp;
            temp = ConstantsDic.SearchUtterance(Names.ID, Scenes[SceneNum], SequenceTENum, ConstantsDic.TranScriptTE);
            Debug.Log(temp[3]);
            MessageSender(ConstantsDic.FixTranscript(temp[3], Names.ID));

        }
        else
        {
            SceneManager.LoadScene("AllDataSave");
        }

    }

    public void OnButtonConfirm()
    {
        // 確認ボタンで保存
        string seqInputInfo = ConstantsDic.SequenceTE[SequenceTENum][2];
        string seqProceedInfo = ConstantsDic.SequenceTE[SequenceTENum][1];

        Debug.Log("SavingCandidate: " + inputfield.text);

        int i = 0;
        foreach (string[] saver in InputSaver.PlayerInputs)
        {
            if (saver[0] == seqInputInfo)
            {
                InputSaver.PlayerInputs[i][1] = inputfield.text;
                Debug.Log("ActuallySaved" + seqInputInfo + ": " + InputSaver.PlayerInputs[i][1]);

                MessageSender("%" + seqInputInfo + ": " + InputSaver.PlayerInputs[i][1]);

            }
            i++;
        }
        inputfield.text = "";
        // sceneDrawer.SetText();

    }

    public void OnButtonFix()
    {
        // 確認ウィンドウで、修正をすることにした場合
        SceneDrawer.IsInConfirmation = false;
    }
}
