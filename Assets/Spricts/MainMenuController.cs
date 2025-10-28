using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelButton
{
    public Button button;
    public Image lockImage;
    public string unlockKey; // 對應 GameData 變數名稱
}

public class MainMenuController : MonoBehaviour
{
    public LevelButton[] levelButtons;

    void Start()
    {
        if (GameData.Instance == null)
        {
            Debug.LogError("GameData 尚未建立！");
            return;
        }

        foreach (var level in levelButtons)
        {
            bool unlocked = CheckUnlock(level.unlockKey);
            SetButtonState(level, unlocked);
        }
    }

    private bool CheckUnlock(string key)
    {
        switch (key)
        {
            case "conch": return GameData.Instance.conch;
            case "candy": return GameData.Instance.candy;
            default:
                return true; // 第一關預設可玩
        }
    }

    private void SetButtonState(LevelButton levelButton, bool unlocked)
    {
        levelButton.button.interactable = unlocked;

        if (levelButton.lockImage != null)
            levelButton.lockImage.gameObject.SetActive(!unlocked);
    }

}
