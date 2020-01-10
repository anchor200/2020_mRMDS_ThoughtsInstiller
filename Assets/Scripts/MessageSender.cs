using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSender : MonoBehaviour
{
    public GameObject dialogMaster;
    DialogMaster master;
    // ここにはDialogViewのMasterをアサイン

    // Start is called before the first frame update
    void Start()
    {
        master = dialogMaster.GetComponent<DialogMaster>();
        // socket通信をしてるクラスを取得する
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnClickProceed()
    {
        master.MessageSender("けん玉しようぜ");
    }
}
