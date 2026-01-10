using UnityEngine;
using Fungus;

public class HiddenItemManager : MonoBehaviour
{
    [Header("小精靈與鑰匙")]
    public GameObject fairy;
    public GameObject key;     // 鑰匙

    [Header("Fungus Flowchart")]
    public Flowchart flowchart;       // 指向 Fungus Flowchart
    public string message = "出現精靈"; // 發送給 Fungus 的訊息名稱

    private void Start()
    {
        // 一開始先隱藏精靈（如果還沒出現過）
        if (fairy != null)
        fairy.SetActive(false);

        if (key != null)
        key.SetActive(false);

        // 判斷是否應該顯示精靈
        CheckFairyCondition();
        KeyCondition();
    }

    /// <summary>
    /// 判斷是否顯示精靈（只用 GameData）
    /// </summary>
    public void CheckFairyCondition()
    {
        if (GameData.Instance == null || fairy == null) return;

        // 如果精靈已經出現過，直接顯示
        if (GameData.Instance.fairyShown)
        {
            fairy.SetActive(true);
            return;
        }

        // 判斷條件：蘋果、牛奶、麵包都取得
        if (GameData.Instance.apple &&
            GameData.Instance.milk &&
            GameData.Instance.bread)
        {
            fairy.SetActive(true);
            flowchart.SendFungusMessage(message);

            // 記錄已經出現過
            GameData.Instance.fairyShown = true;
        }
    }

    public void KeyCondition()
    {
        if (GameData.Instance == null || key == null) return;

        // 如果鑰匙已經出現過，但沒被拿，直接顯示
        if (GameData.Instance.keyShown && !key)
        {
            key.SetActive(true);
            return;
        } 
    }

    public void Keyshow()
    {
        key.SetActive(true);
        // 記錄已經出現過
        GameData.Instance.keyShown = true;
    }

}