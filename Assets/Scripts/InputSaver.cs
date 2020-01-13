using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputSaver
{
    // public static List<string> PlayerInputs;
    // public static string[] dataList = { "<Example>", "<Detail>", "<Refute>", "<Rerefu>", "<Perspec>" };
    public static List<string[]> PlayerInputs = new List<string[]>() {
    new string[]{ "<Example>", "" },
    new string[]{ "<Detail>", "" },
    new string[]{ "<Refute>", "" },
    new string[]{ "<Rerefu>", "" },
    new string[]{ "<Perspec>", "" }
};


    public static void OnClickConfirm()
    {
        string filename;
        filename = "userInputs" + Names.ID + ".txt";
        var content = String.Join(",", PlayerInputs);
        Names.CreateTextFile(filename, content);

        Debug.Log("Inputs saved! ID: " + Names.ID);
    }

    public static void SaveInputToFile()
    {
        string content = "";
        int i = 0;
        foreach (string[] line in PlayerInputs)
        {
            if (i != 0)
            {
                content += "\n";
            }
            content += line[0] + ":" + line[1];
            i++;
        }

        string name;
        name = "OpnInput" + Names.ID + Names.YourName + ".txt";
        string path = Application.dataPath + "/SavedData/" + name;
        StreamWriter sw = File.CreateText(path);
        sw.Write(content);
        sw.Close();
        Debug.Log("Path : " + Application.dataPath);
    } 
}
