using UnityEngine;

public class MultiplayerSetting : MonoBehaviour
{
    public static MultiplayerSetting multiplayerSetting;
    public bool IsDelayStart { get; set; }
    public static int MaxPlayers { get; set; }
    public int MenuScene { get; set; }
    public int MultiPlayerScene { get; set; }

    void Awake()
    {
        if (MultiplayerSetting.multiplayerSetting == null)
        {
            MultiplayerSetting.multiplayerSetting = this;
        }
        else{
            if (MultiplayerSetting.multiplayerSetting != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
