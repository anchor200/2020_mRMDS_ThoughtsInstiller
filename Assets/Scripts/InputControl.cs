﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputControl : MonoBehaviour
{
    public InputField TargetInputField;
    public Text TextToConfirm;
    public Text LengthCount;

    void Start()
    {
        TargetInputField.text = "";
        // TargetInputField.placeholder.GetComponent<Text>().text = "プレースフォルダ";
        TextToConfirm.text = "                      ";
    }

    public void AddText(Text TargetInput)  // ボタンを押したときとかに外から呼び出す
    {
        // Debug.Log("pressed " + TargetInput.text);
        TargetInputField.text = TargetInput.text;
        SceneDrawer.IsInConfirmation = false;
    }

    public void EnterText()
    {
        // 確定ボタンを押したときに呼ばれる
        // 確認ウィンドウを出す。
        SceneDrawer.IsInConfirmation = true;
        TextToConfirm.text = TargetInputField.text;
    }

    public void OnValueChange()
    {
        LengthCount.text = TargetInputField.text.Length + "文字 (最小6字、最大120字)";
    }

}