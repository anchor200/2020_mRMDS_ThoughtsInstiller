using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class NameSaver : MonoBehaviour
{
    public InputField IDField;
    public InputField yourNameField;
    public InputField roboNameField;

    // Start is called before the first frame update
    void Start()
    {
        IDField = IDField.GetComponent<InputField>();
        yourNameField = yourNameField.GetComponent<InputField>();
        roboNameField = roboNameField.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickConfirm()
    {
        Names.ID = IDField.text;
        Names.YourName = yourNameField.text;
        Names.RoboName = roboNameField.text;
        Names.WriteNamesToText();
        Debug.Log("Names saved! ID: " + Names.ID);
        SceneManager.LoadScene("DialogView");  // 名前を保存したら教授モードへ移行
    }

}
