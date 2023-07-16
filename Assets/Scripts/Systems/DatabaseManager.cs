using UnityEngine;
using snake.Config;
using TMPro;
using System.Threading.Tasks;

public class InitDB : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lblPlayer;
    
    [SerializeField]
    private TextMeshProUGUI lblScore;
    
    async void Awake()
    {
        ConfigManager.Instance.LoadConfig();
        await PlayerDao.InitDB();
        await GetCurrentUser();
    }

    private async Task GetCurrentUser()
    {
        var player = await PlayerDao.GetCurrentUser();
        lblPlayer.text = player.name;
        lblScore.text = player.score.ToString();
    }

}
