using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Snake.Entities;

namespace Snake.UI
{
    public class MenuComponent : MonoBehaviour
    {

        public Button _btnNewGame, _btnHostGame, _btnJoin, _btnExit;
        public GameObject menu;
        public GameObject food;
        private bool isShowing = true;


        void Start()
        {
            _btnHostGame.onClick.AddListener(HostGame);
            _btnJoin.onClick.AddListener(JoinGame);

            _btnNewGame.onClick.AddListener(StartNewGame);
            _btnExit.onClick.AddListener(ExitGame);
        }

        void StartNewGame()
        {
            isShowing = !isShowing;
            menu.SetActive(isShowing);
        }

        void ExitGame() => Application.Quit();

        void HostGame()
        {
            NetworkManager.Singleton.StartHost();
            StartNewGame();
            GameObject go = Instantiate(food, Vector3.zero, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();

        }

        void JoinGame()
        {
            NetworkManager.Singleton.StartClient();
            StartNewGame();
        }

    }
}