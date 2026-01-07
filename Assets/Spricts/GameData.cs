using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    // 🔹 新增：只儲存水晶數量
    public int crystalCount;
    public bool conch;
    public bool candy;
    public bool basket;
    public bool crystala;
    public bool crystalb;
    public bool crystalc;
    public bool key;
    public bool apple;
    public bool milk;
    public bool bread;
    // 可以繼續新增其他物品布林

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void GetConch() { conch = true; Debug.Log("玩家獲得 Conch！"); }
    public void GetCandy() { candy = true; Debug.Log("玩家獲得 Candy！"); }
    public void GetBasket() { basket = true; Debug.Log("玩家獲得 Basket！"); }
    public void GetCrystalA() { crystala = true; Debug.Log("玩家獲得 CrystalA！"); }
    public void GetCrystalB() { crystalb = true; Debug.Log("玩家獲得 CrystalB！"); }
    public void GetCrystalC() { crystalc = true; Debug.Log("玩家獲得 CrystalC！"); }
    public void GetKey() { key = true; Debug.Log("玩家獲得 Key！"); }
    public void GetApple() { apple = true; Debug.Log("玩家獲得 Apple！"); }
    public void GetMilk() { milk = true; Debug.Log("玩家獲得 Milk！"); }
    public void GetBread() { bread = true; Debug.Log("玩家獲得 Bread！"); }

    public void AddCrystal()
    {
        // 🔹 增加水晶數量
        crystalCount++;

        // 🔹 可以在這裡打印方便除錯
        Debug.Log("[GameData] 水晶數量：" + crystalCount);
    }

    public void ResetCrystals()
    {
        crystala = false;
        crystalb = false;
        crystalc = false;

        crystalCount = 0; // 🔹 將水晶數量歸零
        Debug.Log("水晶已重置！");
    }

    // 🔹 新增：重置所有道具 & 水晶數量
    public void ResetAll()
    {
        Debug.Log("GameData Instance ID = " + GameData.Instance.GetInstanceID());

        // 重置布林道具
        conch = false;
        candy = false;
        basket = false;
        crystala = false;
        crystalb = false;
        crystalc = false;
        key = false;
        apple = false;
        milk = false;
        bread = false;

        // 重置水晶數量
        crystalCount = 0;

        Debug.Log("[GameData] 所有道具與水晶數量已重置！");
    }
}

