using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SrcForChoice : MonoBehaviour
{
    GameObject OpinionInputField;
    InputControl InputFieldController;
    public Text SelfExpression;
    // Start is called before the first frame update
    void Start()
    {
        OpinionInputField = GameObject.Find("InputField");
        InputFieldController = OpinionInputField.GetComponent<InputControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPress(Text TargetInput)  // ボタンを押したときとかに外から呼び出す
    {
        Debug.Log("pressed " + SelfExpression.text);
        InputFieldController.AddText(SelfExpression);
    }
}
