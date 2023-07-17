using UnityEngine;
using Snake.Config;
using Snake.DataAccess;
using TMPro;
using System.Threading.Tasks;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lblPlayer;

    [SerializeField]
    private TextMeshProUGUI lblScore;

    async void Awake()
    {
        ConfigManager.Instance.LoadConfig();
        await PlayerDao.CreateTable();
        await GetCurrentUser();
    }

    private async Task GetCurrentUser()
    {
        var player = await PlayerDao.GetCurrentUser();
        ConfigManager.Instance.LoadCurrentUser(player);
        lblPlayer.text = player.name;
        lblScore.text = player.score.ToString();
    }

}
