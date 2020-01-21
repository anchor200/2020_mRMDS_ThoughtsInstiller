using System;
using System.Text;
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

    public GameObject nextPanel;

    // シーンの初期設定**>

    // Start is called before the first frame update
    void Start()
    {
        // <**まずresources以下にあるcsvファイルをすべて読み込む  あとでPCの方だけに変えておこう
        //ConstantsDic.commUNetworkSettings = ConstantsDic.ReadCSV("PRESET/network_setting");
        ConstantsDic.MNetworkSettings = ConstantsDic.ReadCSV("PRESET/network_to_M");

        // 発話の読み込み
        ConstantsDic.mainClaims = ConstantsDic.ReadCSV("TOPICS/main_claims");
        ConstantsDic.SequenceTE = ConstantsDic.ReadCSV("PRESET/001_te_sequence");
        ConstantsDic.TranScriptST = ConstantsDic.ReadCSV("PRESET/000_st");
        ConstantsDic.TranScriptTE = ConstantsDic.ReadCSV("PRESET/001_te");
        ConstantsDic.OnScreenTE = ConstantsDic.ReadCSV("PRESET/001_te_onScreen");
        ConstantsDic.FukidashiTE = ConstantsDic.ReadCSV("PRESET/001_te_Fukidashi");


        string path = "chosen_topics.txt";
        Debug.Log("loading topics " + path);
        List<string[]> tmp = ChoiceImport.ReadCSVFromOutOfBuild(path);

        ConstantsDic.Chosen_topics = tmp[0];

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


        // TcpClientを作成し、サーバーと接続
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

            MessageSender("<ID>:" + Names.ID + "," + Names.YourName + "," + Names.RoboName);

            // 実験処理用の処理用の処理！！！！！！！
            if (ns.CanRead)
            {
                byte[] myReadBuffer = new byte[1024];
                StringBuilder myCompleteMessage = new StringBuilder();
                int numberOfBytesRead = 0;

                // Incoming message may be larger than the buffer size.
                do
                {
                    numberOfBytesRead = ns.Read(myReadBuffer, 0, myReadBuffer.Length);

                    myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                }
                while (ns.DataAvailable);

                // Print out the received message to the console.
                Debug.Log("You received the following message : " +
                                             myCompleteMessage);
                ConstantsDic.Chosen_topics = myCompleteMessage.ToString().Split(',');
            }
            else
            {
                Debug.Log("Sorry.  You cannot read from this NetworkStream.");
            }




            int k = 0;
            string mainclaimer = "<MainClaim>:";
            foreach (string[] clm in ConstantsDic.mainClaims)
            {

                if (k == 0)
                {
                    mainclaimer += "ID,<Topic>,<MainClaim>,<Argument>;";
                }
                else
                {
                    mainclaimer += string.Join(",", clm) + ";";
                }
                k++;
            }
            mainclaimer = mainclaimer.Remove(mainclaimer.Length - 1);

            MessageSender(mainclaimer);

            string[] temp;
            temp = ConstantsDic.SearchUtterance(Names.ID, Scenes[SceneNum], 1, ConstantsDic.TranScriptST);
            string prefix = "<Command>:" + Names.ID + "," + temp[0] + ",";
            MessageSender(prefix + ConstantsDic.FixTranscript(temp[3], Names.ID));

            SceneNum++;

            temp = ConstantsDic.SearchUtterance(Names.ID, Scenes[SceneNum], 1, ConstantsDic.TranScriptTE);
            prefix = "<Command>:" + Names.ID + "," + temp[0] + ",";
            MessageSender(prefix + ConstantsDic.FixTranscript(temp[3], Names.ID));


            // TE01以外を隠す
            /*int i = 0;
            GameObject g;
            foreach (string[] seq in ConstantsDic.SequenceTE)
            {
                if (i == 0)
                {
                    g = GameObject.Find("OpinionInputField");  //コンテナの０がインプットフィールドになってる
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
            SeqContainer[SequenceTENum].SetActive(true);*/
            nextPanel.SetActive(true);


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
        //SeqContainer[SequenceTENum].SetActive(false);  // 古いシーンを切る
        if (SequenceTENum == 1)
        {
            nextPanel.SetActive(false);
        }
        else if (SequenceTENum == 6)
        {
            nextPanel.SetActive(true);
        }

        // 進むボタンを押したときにシーンを更新する、文章を描画しなおす、選択肢ボタンを配置しなおす
        Debug.Log("Now we are:" + Scenes[SceneNum] + SequenceTENum.ToString("D2"));

        if (seqProceedInfo == "False")
        {
            SequenceTENum++;
            choiceControl.PlaceChoice(Scenes[SceneNum], SequenceTENum);
            //SeqContainer[SequenceTENum].SetActive(true);

            sceneDrawer.SetText();

            string[] temp;
            temp = ConstantsDic.SearchUtterance(Names.ID, Scenes[SceneNum], SequenceTENum, ConstantsDic.TranScriptTE);
            Debug.Log(temp[3]);
            string prefix = "<Command>:" + Names.ID + "," + temp[0] + ",";
            MessageSender(prefix + ConstantsDic.FixTranscript(temp[3], Names.ID));

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

                MessageSender("%" + Names.ID + "%" + seqInputInfo + ":" + InputSaver.PlayerInputs[i][1]);

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
        sceneDrawer.SetText();
    }
}
