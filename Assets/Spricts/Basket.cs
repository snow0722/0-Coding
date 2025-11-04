using Fungus;
using UnityEngine;

/// <summary>
/// 籃子
/// </summary>
public class Basket : MonoBehaviour, IInteraction
{
    [SerializeField, Header("Fungus_互動物件說明")]
    private Flowchart FlowchartObject;
    [SerializeField, Header("撿取音效")]
    private AudioClip soundPickUp;
    [Header("是否取得籃子")]
    public bool basket; // 是否取得漂浮能力（取得籃子）

    private string flowchartMessage = "漂浮教學";
    private AudioSource aud;
    private Rigidbody rig;
    private Collider col;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        aud = GetComponent<AudioSource>();
    }

    public void Interaction()
    {
        print($"<color=#3f3>互動：{name}</color>");
    }

    public void PickUp()
    {
        print($"<color=#37f>撿取：{name}</color>");
        // 設為已經撿取，剛體設定為運動學(不會動)，關閉碰撞，設定座標
        basket = true;
        GameData.Instance.GetBasket();    // 更新全局狀態
        rig.isKinematic = true;
        col.enabled = true;
        transform.position = new Vector3(0, 0, -200);
        aud.PlayOneShot(soundPickUp);
        FlowchartObject.SendFungusMessage(flowchartMessage);
    }
}

