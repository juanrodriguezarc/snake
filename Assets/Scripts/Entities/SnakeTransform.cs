using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Snake.Entities
{
    /// <summary>
    /// An example network serializer with both server and owner authority.
    /// </summary>
    public class SnakeTransform : NetworkBehaviour
    {

        /// <summary>
        /// A toggle to test the difference between owner and server auth.
        /// </summary>
        [SerializeField] private bool _serverAuth;
        [SerializeField] private float _cheapInterpolationTime = 0.1f;
        private NetworkVariable<PlayerNetworkState> _playerState;
        private Transform _rb;

        private void Awake()
        {
            _rb = GetComponent<Transform>();
            var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
            _playerState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) Destroy(transform.GetComponent<SnakeController>());
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsOwner) TransmitState();
            else ConsumeState();
        }

        #region Transmit State

        private void TransmitState()
        {
            var state = new PlayerNetworkState { Position = _rb.position };

            if (IsServer || !_serverAuth)
                _playerState.Value = state;
            else
                TransmitStateServerRpc(state);
        }

        [ServerRpc]
        private void TransmitStateServerRpc(PlayerNetworkState state)
        {
            _playerState.Value = state;
        }

        #endregion


        #region Interpolate State

        private Vector3 _posVel;

        private void ConsumeState()
        {
            // Basic interpolation
            _rb.position = Vector3.SmoothDamp(_rb.position, _playerState.Value.Position, ref _posVel, _cheapInterpolationTime);
        }

        #endregion

        private struct PlayerNetworkState : INetworkSerializable
        {
            private float _posX, _posY;

            internal Vector3 Position
            {
                get => new(_posX, _posY, 0);
                set
                {
                    _posX = value.x;
                    _posY = value.y;
                }
            }


            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref _posX);
                serializer.SerializeValue(ref _posY);
            }
        }

    }
}