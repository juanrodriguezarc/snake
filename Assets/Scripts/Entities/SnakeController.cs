using System.Collections.Generic;
using UnityEngine;
using Snake.DataAccess;
using TMPro;
using Snake.Systems;
using Unity.Netcode;

namespace Snake.Entities
{
    public class SnakeController : NetworkBehaviour
    {

        [SerializeField] private bool _serverAuth;
        [SerializeField] private float _cheapInterpolationTime = 0.1f;
        private NetworkVariable<PlayerNetworkState> _playerState;
        private NetworkVariable<int> _netScore = new(0);

        [SerializeField] private TextMeshProUGUI _lblScore;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private Transform _bodyPrefab;
        [SerializeField] private float speed = 10.0f;

        private Transform _rb;                              // Component reference
        private Vector3 _posVel;                            // For SmoothDamp
        private readonly List<Transform> _segments = new(); // Snake body parts
        private Vector2 _direction;                         //Controls direction

        void Start()
        {
            ResetState();
        }

        private void Awake()
        {
            _rb = GetComponent<Transform>();
            _lblScore = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
            var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
            _playerState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
            _netScore.OnValueChanged = OnScoreChange;
        }

        public override void OnDestroy()
        {
            _netScore.OnValueChanged -= OnScoreChange;
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down) _direction = Vector2.up;
            if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left) _direction = Vector2.right;
            if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right) _direction = Vector2.left;
            if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up) _direction = Vector2.down;
            var pos = _rb.position;
            var step = speed * Time.deltaTime;

            if (IsOwner)
            {
                _rb.position = Vector3.MoveTowards(_rb.position, new Vector3(
                 Mathf.Round(pos.x + _direction.x),
                 Mathf.Round(pos.y + _direction.y),
                 0.0f), step);
                TransmitState();
            }
            else
            {
                ConsumeState();
            }

            var offset = IsOwner ? 2 : 0.5f;

            for (int i = _segments.Count - 1; i > 0; i--)
            {
                _segments[i].position = Vector3.Lerp(_segments[i].position, new Vector3(
                Mathf.Round(_segments[i - 1].position.x - (_direction.x / offset)),
                Mathf.Round(_segments[i - 1].position.y - (_direction.y / offset)),
                0.0f), step);
            }
        }

        /// <summary>
        /// Add a snake body prefab to the segments
        /// </summary>
        private void Grow()
        {
            Transform segment = Instantiate(this._bodyPrefab);
            segment.position = new Vector3(_segments[^1].position.x - _direction.x, _segments[^1].position.y - _direction.y, 0.0f);
            _segments.Add(segment);
        }

        /// <summary>
        /// Destroy all prefabs (snake body)
        /// </summary>
        private void ResetState()
        {
            for (int i = _segments.Count - 1; i > 0; i--)
            {
                Destroy(_segments[i].gameObject);
            }
            _segments.Clear();
            _segments.Add(_rb);
            _rb.position = Vector3.zero;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsOwner) return;
            if (other.tag == "Food")
            {
                SoundManager.Instance.PlaySound(_audioClip);
                Grow();
                AddScoreToDB();
                CommitScoreServerRpc(_netScore.Value + 1);
            }
            if (other.tag == "Wall")
            {
                ResetState();
                CommitScoreServerRpc(0);
            }
        }

        private async void AddScoreToDB()
        {
            await PlayerDao.UpdateCurrentScore(_netScore.Value);
        }

        private void OnScoreChange(int prev, int next)
        {
            if (!IsOwner)
            {
                if (next != 0) Grow(); else ResetState();
            }

            if (IsOwner) _lblScore.text = $"{next}";
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

                [ServerRpc]
        private void CommitScoreServerRpc(int value)
        {
            _netScore.Value = value;
        }

        #endregion

        private void ConsumeState()
        {
            _rb.position = Vector3.SmoothDamp(_rb.position, _playerState.Value.Position, ref _posVel, _cheapInterpolationTime);
        }

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