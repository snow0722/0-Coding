using UnityEngine;
using Fungus;

public class Fairy : MonoBehaviour
{
    [Header("Fungus Flowchart")]
    public Flowchart flowchart;       // 指向 Fungus Flowchart
    public string message = "鑰匙有無判定"; // 發送給 Fungus 的訊息名稱

    [Header("音效 (可選)")]
    public AudioClip soundEffect;
    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // 確保 Collider 是 Trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    // 當玩家進入碰撞範圍
    private void OnTriggerEnter(Collider other)
    {
        // 假設玩家有 tag "Player"
        if (other.CompareTag("Player"))
        {
            // 播放音效
            if (aud != null && soundEffect != null)
                aud.PlayOneShot(soundEffect);

            // 發送 Fungus 訊息
            if (flowchart != null && !string.IsNullOrEmpty(message))
                flowchart.SendFungusMessage(message);

            // 可選：精靈被觸碰後消失
            // gameObject.SetActive(false);
        }
    }
}