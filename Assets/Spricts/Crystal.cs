using Fungus;
using UnityEngine;

/// <summary>
/// 水晶物件
/// </summary>
public class Crystal : MonoBehaviour, IInteraction
{
    [Header("水晶名稱")]
    public string crystalName;  // "A", "B", "C"

    [Header("Fungus_互動物件說明")]
    [SerializeField] private Flowchart flowchartObject;

    [Header("撿取音效")]
    [SerializeField] private AudioClip soundPickUp;

    private AudioSource aud;
    private Rigidbody rig;
    private Collider col;
    private bool isCollected;

    /// <summary>
    /// 玩家是否已撿到水晶
    /// </summary>
    public bool Collected => isCollected;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        aud = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 互動方法 (IInteraction 介面)
    /// </summary>
    public void Interaction()
    {
        Debug.Log($"<color=#3f3>互動：{crystalName}</color>");
    }

    /// <summary>
    /// 撿取水晶
    /// </summary>
    public void PickUp()
    {
        if (!isCollected)
        {
            isCollected = true;

            // 🔹 先更新 GameData
            if (GameData.Instance != null)
            {
                if (crystalName == "A") GameData.Instance.GetCrystalA();
                else if (crystalName == "B") GameData.Instance.GetCrystalB();
                else if (crystalName == "C") GameData.Instance.GetCrystalC();
            }

            // 🔹 隱藏水晶
            if (rig != null) rig.isKinematic = true;
            if (col != null) col.enabled = false;
            transform.position = new Vector3(0, 0, -200);

            // 🔹 播放音效
            if (aud != null && soundPickUp != null)
                aud.PlayOneShot(soundPickUp);

            // 🔹 通知 UI / Fungus
            if (CrystalUIManager.Instance != null)
                CrystalUIManager.Instance.AddCrystal();

            Debug.Log($"[Crystal] 撿取：{crystalName}");
        }
    }
}