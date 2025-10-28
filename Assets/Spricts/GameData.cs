using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public bool conch;
    public bool candy;
    public bool basket;
    public bool crystal;
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
    public void GetCrystal() { crystal = true; Debug.Log("玩家獲得 Crystal！"); }
    public void GetKey() { key = true; Debug.Log("玩家獲得 Key！"); }
    public void GetApple() { apple = true; Debug.Log("玩家獲得 Apple！"); }
    public void GetMilk() { milk = true; Debug.Log("玩家獲得 Milk！"); }
    public void GetBread() { bread = true; Debug.Log("玩家獲得 Bread！"); }
}

