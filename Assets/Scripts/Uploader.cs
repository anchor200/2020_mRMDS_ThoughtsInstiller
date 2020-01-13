using System.Collections;
using System.Net;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Uploader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputSaver.SaveInputToFile();
    }

    void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
    #endif
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Quit();
        if (Input.GetKey(KeyCode.Space)) UpLoader();
    }

    void UpLoader()
    {

        List<string[]> Temp = ChoiceImport.ReadCSVFromOutOfBuild("FTP_ADDR.csv");
        string ADDR = Temp[0][0];

        using (WebClient wc = new WebClient())
        {
            try
            {
                // ログインユーザー名とパスワードを指定
                wc.Credentials = new NetworkCredential("NY", "LGM");

                string name;
                name = "OpnInput" + Names.ID + Names.YourName + ".txt";
                string path = Application.dataPath + "/SavedData/" + name;
                Debug.Log(path);

                // FTPサーバーへアップロードする
                wc.UploadFile("ftp://" + ADDR +"//" + name, path);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

}
