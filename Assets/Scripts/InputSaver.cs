using System;
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
}
