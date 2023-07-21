
using UnityEngine;
using Unity.Netcode;

namespace Snake.Entities
{
    public class FoodController : NetworkBehaviour
    {
        public BoxCollider2D gridArea;

        void Awake()
        {
            gridArea = GameObject.Find("GridArea").GetComponent<BoxCollider2D>();
        }

        public override void OnNetworkSpawn()
        {
            InitFood();
        }

        /// <summary>
        /// Generates a random position for the food
        /// </summary>
        private Vector3 RandomPosition()
        {
            Bounds bounds = this.gridArea.bounds;
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            return new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);;
        }

        /// <summary>
        /// Executes the RandomizePosition function when the snakes
        /// collides with the food
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                RequestUpdateFoodServerRpc();
            }
        }

        public void UpdateFoodPosition(Vector3 pos)
        {
            this.transform.position = pos;
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestUpdateFoodServerRpc()
        {
            var position = RandomPosition();
            ExecuteUpdateFoodClientRpc(position);
        }

        [ClientRpc]
        private void ExecuteUpdateFoodClientRpc(Vector3 dir)
        {
            UpdateFoodPosition(dir);
        }

        public void InitFood()
        {
            if(IsOwner || IsServer)
            {
                RequestUpdateFoodServerRpc();
            }
        }
    }
}