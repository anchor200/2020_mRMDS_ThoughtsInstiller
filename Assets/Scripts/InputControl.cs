using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputControl : MonoBehaviour
{
    public InputField TargetInputField;
    public Text TextToConfirm;
    public Text LengthCount;

    GameObject master;
    SceneDrawer sceneDrawer;

    void Start()
    {
        TargetInputField.text = "";
        // TargetInputField.placeholder.GetComponent<Text>().text = "プレースフォルダ";
        TextToConfirm.text = "                      ";

        master = GameObject.Find("Master");
        sceneDrawer = master.GetComponent<SceneDrawer>();

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
        sceneDrawer.SetText();
        TextToConfirm.text = ConstantsDic.FixTranscript(SceneDrawer.SearchFukidashi("TE" + DialogMaster.SequenceTENum.ToString("D2"), ConstantsDic.OnScreenTE)[1], Names.ID).Replace("#", "\n") + TargetInputField.text;
    }

    public void OnValueChange()
    {
        LengthCount.text = TargetInputField.text.Length + "文字 (最小4字、最大70字)";
    }

}