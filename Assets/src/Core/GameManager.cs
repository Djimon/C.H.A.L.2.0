using CHAL.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   
    [SerializeField] private GameBalanceConfig config; // optional: Inspector-Zuweisung

    public GameBalanceConfig Config
    {
        get
        {
            if (config == null)
            {
                config = Resources.Load<GameBalanceConfig>("Config/GameBalanceConfig");
            }
            return config;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
