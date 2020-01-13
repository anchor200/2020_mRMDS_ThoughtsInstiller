using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceControl : MonoBehaviour
{
    GameObject inputfield;
    InputControl inputcontrol;
    public GameObject ChoiceField;
    Transform choiceTransform;
    public GameObject ChoicePrefab;


    // Start is called before the first frame update
    void Start()
    {
        inputfield = GameObject.Find("InputField");
        inputcontrol = inputfield.GetComponent<InputControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // 選択肢を配置する
    public void PlaceChoice(string Scene, int SequenceNum)
    {
        string path = ChoiceImport.TOPIC + "/" + ConstantsDic.SequenceTE[DialogMaster.SequenceTENum][0] + ".csv";
        Debug.Log("loading " + path);
        ChoiceImport.CurrentChoice = ChoiceImport.ReadCSVFromOutOfBuild(path);
        // Debug.Log(ChoiceImport.CurrentChoice[1][0]);


        choiceTransform = ChoiceField.transform;
        choiceTransform.DetachChildren();  // 過去の子供を全員抹消
        foreach (string[] choice in ChoiceImport.CurrentChoice)
        {
            //プレハブからボタンを生成
            GameObject listChoice = Instantiate(ChoicePrefab) as GameObject;
            //Vertical Layout Group の子にする
            listChoice.transform.SetParent(choiceTransform, false);
            //適当にボタンのラベルを変える
            //※ Textコンポーネントを扱うには using UnityEngine.UI; が必要
            listChoice.transform.Find("Text").GetComponent<Text>().text = choice[0];
        }
    }

    /*public void PushJudge(GameObject obj)　// 押したボタンに応じてテキストフィールドの文字を変える
    {
        inputcontrol.AddText(obj.GetComponentInChildren<Text>());
    }*/
}
