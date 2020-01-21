using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDrawer : MonoBehaviour
{

    // 文字制限等をしないといけないので
    public InputField TargetInputField;


    // 発話内で置き換えるべきものたち
    private string[] transferList = { "<MainClaim>", "<Argument>", "<Point>", "<Relation>" };


    // ロボットが画面上でいうこと

    // 各遷移状況で画面に表示されるセリフを格納するもの
    /*public Text textTE01no1;
    public Text textTE02no1;
    public Text textTE02no2;
    public Text textTE03no1;
    public Text textTE03no2;
    public Text textTE04no1;
    public Text textTE05no1;
    public Text textTE05no2;
    public Text textTE05no3;
    public Text textTE06no1;
    public Text textTE07no1;*/
    public Text Fukidashi;
    //public Text FukidashiConfirm;


    public static bool IsInConfirmation; // 確認ウィンドウを出してるかどうか
    public GameObject ConfirmWindow;

    // Start is called before the first frame update
    void Start()
    {
        IsInConfirmation = false;
        this.SetText();

    }

    public void SetText()
    {
        // RoboQuing.text = ConstantsDic.RoboOnScreen[0].Replace("<YourName>", Names.YourName);
        // RoboConfirm.text = ConstantsDic.RoboOnScreen[1];

        /*textTE01no1.text = ConstantsDic.FixTranscript(ConstantsDic.SearchUtterance(Names.ID, DialogMaster.Scenes[DialogMaster.SceneNum], 1, ConstantsDic.TranScriptTE)[2], Names.ID);
        textTE02no1.text = ConstantsDic.FixTranscriptLineWidth(ConstantsDic.FixTranscript(SearchLineOnScreen("TE02", "0", ConstantsDic.OnScreenTE)[3], Names.ID));
        textTE02no2.text = ConstantsDic.FixTranscriptLineWidth(ConstantsDic.FixTranscript(SearchLineOnScreen("TE02", "1", ConstantsDic.OnScreenTE)[3], Names.ID));
        textTE03no1.text = ConstantsDic.FixTranscriptLineWidth(ConstantsDic.FixTranscript(SearchLineOnScreen("TE03", "0", ConstantsDic.OnScreenTE)[3], Names.ID));
        textTE03no2.text = ConstantsDic.FixTranscriptLineWidth(ConstantsDic.FixTranscript(SearchLineOnScreen("TE03", "1", ConstantsDic.OnScreenTE)[3], Names.ID));
        textTE04no1.text = ConstantsDic.FixTranscriptLineWidth(ConstantsDic.FixTranscript(SearchLineOnScreen("TE04", "0", ConstantsDic.OnScreenTE)[3], Names.ID));
        textTE05no1.text = ConstantsDic.FixTranscriptLineWidth(ConstantsDic.FixTranscript(SearchLineOnScreen("TE05", "0", ConstantsDic.OnScreenTE)[3], Names.ID));
        textTE05no2.text = ConstantsDic.FixTranscriptLineWidth(ConstantsDic.FixTranscript(SearchLineOnScreen("TE05", "1", ConstantsDic.OnScreenTE)[3], Names.ID));
        textTE05no3.text = ConstantsDic.FixTranscriptLineWidth(ConstantsDic.FixTranscript(SearchLineOnScreen("TE05", "2", ConstantsDic.OnScreenTE)[3], Names.ID));
        textTE06no1.text = ConstantsDic.FixTranscriptLineWidth(ConstantsDic.FixTranscript(SearchLineOnScreen("TE06", "0", ConstantsDic.OnScreenTE)[3], Names.ID));
        textTE07no1.text = ConstantsDic.FixTranscript(ConstantsDic.SearchUtterance(Names.ID, DialogMaster.Scenes[DialogMaster.SceneNum], 7, ConstantsDic.TranScriptTE)[2], Names.ID);
        Fukidashi.text = ConstantsDic.FixTranscript(SearchFukidashi("TE" + DialogMaster.SequenceTENum.ToString("D2"), ConstantsDic.FukidashiTE)[1], Names.ID).Replace("#", "\n");
        FukidashiConfirm.text = ConstantsDic.FixTranscript(SearchFukidashi("TE" + DialogMaster.SequenceTENum.ToString("D2"), ConstantsDic.FukidashiTE)[2], Names.ID).Replace("#", "\n");*/
        Fukidashi.text = ConstantsDic.FixTranscript(ConstantsDic.SearchUtterance(Names.ID, DialogMaster.Scenes[DialogMaster.SceneNum], DialogMaster.SequenceTENum, ConstantsDic.TranScriptTE)[2], Names.ID).Replace("#", "\n");
        if (IsInConfirmation == true)
        {
            Fukidashi.text = ConstantsDic.FixTranscript(SearchFukidashi("TE" + DialogMaster.SequenceTENum.ToString("D2"), ConstantsDic.FukidashiTE)[2], Names.ID).Replace("#", "\n");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsInConfirmation == false)
        {
            if(ConfirmWindow.activeSelf == true)
            {
                ConfirmWindow.SetActive(false);
            }
        }
        if (IsInConfirmation == true)
        {
            if (ConfirmWindow.activeSelf == false)
            {
                if (TargetInputField.text.Length > 5 && TargetInputField.text.Length <= 120 )
                {
                    ConfirmWindow.SetActive(true);
                }
                else
                {
                    IsInConfirmation = false;
                }

            }
        }
    }

    public void SequenceTransfer(string test)  // seqenceを遷移させたときに呼んで、テキストを更新する
    {
        // どっかのGameObjectに張り付けて、GetComponentからこの関数を呼ばせる
        // 時間がないのでとりあえず手作業で分岐を…
        Debug.Log("Hello");
    }

    private string[] SearchLineOnScreen(string Sequence, string NumInSeq, List<string[]> data)
    {
        foreach (string[] c in data)
        {
            if (c[0] == Sequence)
            {
                if (c[1] == NumInSeq)
                {
                    return c;
                }
            }
        }
        return data[0];
    }

    private string[] SearchFukidashi(string Sequence, List<string[]> data)
    {
        foreach (string[] c in data)
        {
            if (c[0] == Sequence)
            {
                return c;
            }
        }
        return data[0];
    }

    /*public string FixTranscript(string Str, string ID)  // 文字列の置き換え
    {
        string[] mainClaimTemp;
        mainClaimTemp = ConstantsDic.mainClaims[0];
        foreach (string[] c in ConstantsDic.mainClaims)
        {
            if (c[0] == ID)
            {
                // Debug.Log("Find your claim for ID:" + ID);
                mainClaimTemp = c;  // cは{話者ID,<MainClaim>,<Argument>,<Point>,<Relation>}
                break;
            }
        }

        int i = 0;
        foreach (string signifiant in ConstantsDic.mainClaims[0])
        {
            Debug.Log(signifiant + "  :  " + mainClaimTemp[i]);
            Str = Str.Replace(signifiant, mainClaimTemp[i]);
            i++;
        }

        foreach (string[] input in InputSaver.PlayerInputs)
        {
            Str = Str.Replace(input[0], input[1]);
        }

        Str = Str.Replace("<YourName>", Names.YourName).Replace("<RoboName>", Names.RoboName);

        Str = Regex.Replace(Str, @"(?<=\G.{27}(?!$))|\n", "\n");//Environment.NewLine);


        return Str;
    }*/
}
