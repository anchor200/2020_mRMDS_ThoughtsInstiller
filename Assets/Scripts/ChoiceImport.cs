using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System; //Exception
using System.Text;

public static class ChoiceImport
{
    public static string TOPIC = "LOVE";
    public static List<string[]> CurrentChoice;



    public static List<string[]> ReadCSVFromOutOfBuild(string path)
    {
        List<string[]> tempList = new List<string[]>();

        FileInfo fi = new FileInfo(Application.dataPath + "/ImportData/" + path);
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
        }

        return tempList;
    }

}
