﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.IO;

public static class Names
{
    public static string ID = "9";
    public static string YourName = "hiroshi";
    public static string RoboName = "geminoid";


    public static void WriteNamesToText()
    {
        string filename;
        filename = "userProf" + ID + ".txt";
        string content;
        content = ID + "," + YourName + ";" + RoboName;
        CreateTextFile(filename, content);
    }

    private static void CreateTextFile(string name, string content)
    {
        string path = Application.dataPath + "/SavedData/" + name;
        StreamWriter sw = File.CreateText(path);
        sw.Write(content);
        sw.Close();
        Debug.Log("Path : " + Application.dataPath);
    }
}