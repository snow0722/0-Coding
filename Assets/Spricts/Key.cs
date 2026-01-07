using Fungus;
using UnityEngine;

/// <summary>
/// 鑰匙
/// </summary>
public class Key : MonoBehaviour, IInteraction
{
    [SerializeField, Header("Fungus_互動物件說明")]
    private Flowchart FlowchartObject;
    [SerializeField, Header("撿取音效")]
    private AudioClip soundPickUp;

    private string flowchartMessage = "本關鑰匙";
    private AudioSource aud;
    private Rigidbody rig;
    private Collider col;
    private bool isPickUp;

    /// <summary>
    /// 是否撿到鑰匙
    /// </summary>
    public bool pickedUp => isPickUp;

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
        GameData.Instance.GetKey();
        isPickUp = true;
        rig.isKinematic = true;
        col.enabled = true;
        transform.position = new Vector3(0, 0, -200);
        aud.PlayOneShot(soundPickUp);
        FlowchartObject.SendFungusMessage(flowchartMessage);
    }
}