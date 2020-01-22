using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System; //Exception
using System.Text;

public static class ConstantsDic
{
    // CommUに接続するときに使うIPアドレス
    public static List<string[]> commUNetworkSettings;
    // 母艦とのアドレス　上のこみゅーのは使わない
    public static List<string[]> MNetworkSettings;

    // ロボットのメインの意見
    public static List<string[]> mainClaims;

    // 推移状態の一覧
    public static List<string[]> SequenceTE;

    // 喋らせる内容
    // public static string[] RoboOnScreen = { "こんな時、<YourName>さんなら何て答える？", "こんな風に言えばいいのかな？　（このまま発言しても違和感がないかどうかチェックしてね）" };
    public static List<string[]> FukidashiTE;
    public static List<string[]> TranScriptST;
    public static List<string[]> TranScriptTE;

    // 画面に表示させる内容
    public static List<string[]> OnScreenTE;

    // トピックの一覧
    public static string[] Chosen_topics;

    public static List<string[]> ReadCSV(string filename)
    {
        List<string[]> tempList = new List<string[]>();

        FileInfo fi = new FileInfo(Application.dataPath + "/ImportData/" + filename + ".csv");
        try
        {
            StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8);

            while (sr.Peek() != -1)
            {
                string line = sr.ReadLine();
                tempList.Add(line.Split(','));
                // Debug.Log(line);
            }
        }
        catch (Exception e)
        {
            string[] err = { "新春シャンソンショー" };
            tempList.Add(err);
        }

        return tempList;

        /*List<string[]> tempList = new List<string[]>();

        TextAsset csvFile;
        csvFile = Resources.Load(filename) as TextAsset; // Resouces下のCSV読み込み
        // Debug.Log(csvFile.text);
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            tempList.Add(line.Split(',')); // , 区切りでリストに追加
        }

        // tempList[行][列]を指定して値を自由に取り出せる
        
        return tempList;*/
    }

    public static string[] SearchUtterance(string YourID, string scene, int sequence, List<string[]> data)
    {
        foreach(string[] c in data)
        {
            if(c[0] == scene + sequence.ToString("D2"))
            {
                if(c[1] == YourID)
                {
                    return c;
                }
            }
        }
        string[] forErorr = { "ERORR", "Z", "よくわかりません。", "よくわかりません。" };
        return forErorr;
    }

    public static string FixTranscript(string Str, string ID)  // 文字列の置き換え
    {
        string[] mainClaimTemp;

        string topic_id = "S";
        switch (ID)
        {
            case "A":
                topic_id = Chosen_topics[0];
                break;
            case "B":
                topic_id = Chosen_topics[1];
                break;
            case "C":
                topic_id = Chosen_topics[2];
                break;
            case "D":
                topic_id = Chosen_topics[3];
                break;
        }

        mainClaimTemp = ConstantsDic.mainClaims[0];
        foreach (string[] c in ConstantsDic.mainClaims)
        {
            if (c[0] == topic_id)
            {
                // Debug.Log("Find your claim for ID:" + ID);
                mainClaimTemp = c;  // cは{ID,<Topic>,<MainClaim>,<Argument>,}
                break;
            }
        }

        int i = 0;
        foreach (string signifiant in ConstantsDic.mainClaims[0])
        {
            // Debug.Log(signifiant + "  :  " + mainClaimTemp[i]);
            Str = Str.Replace(signifiant, mainClaimTemp[i]);
            i++;
        }

        foreach (string[] input in InputSaver.PlayerInputs)
        {
            Str = Str.Replace(input[0], input[1]);
        }

        Str = Str.Replace("#", "\n");
        Str = Str.Replace("<YourName>", Names.YourName).Replace("<RoboName>", Names.RoboName);

        //Str = Regex.Replace(Str, @"(?<=\G.{27}(?!$))|\n", "\n");//Environment.NewLine);

        return Str;
    }

    public static string FixTranscriptLineWidth(string Str)
    {
        Str = Regex.Replace(Str, @"(?<=\G.{26}(?!$))|\n", "\n");//Environment.NewLine);
        return Str;
    }

}
