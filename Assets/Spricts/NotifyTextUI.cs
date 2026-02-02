using UnityEngine;
using UnityEngine.UI;

public class NotifyTextUI : MonoBehaviour
{
    [SerializeField] private Text uiText;

    private void Awake()
    {
        uiText.text = "";
    }

    // === 對外呼叫用 ===
    public void ShowA()
    {
        uiText.text = "尋找籃子內掉落的物品(按B詳情)";
    }

    public void ShowB()
    {
        uiText.text = "尋找木屋鑰匙";
    }

    public void ShowC()
    {
        uiText.text = "前往木屋";
    }

    public void ShowD()
    {
        uiText.text = "尋找籃子";
    }

    public void Clear()
    {
        uiText.text = "";
    }
}
