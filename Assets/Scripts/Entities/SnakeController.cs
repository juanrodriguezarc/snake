using System.Collections.Generic;
using UnityEngine;
using Snake.DataAccess;
using TMPro;
using Snake.Systems;
using Unity.Netcode;

namespace Snake.Entities
{
    public class SnakeController : MonoBehaviour
    {
        private readonly List<Transform> _segments = new();
        private int score = 0;
        private Vector2 _direction;
        public Transform _bodyPrefab;
        public float speed = 10.0f;

        [SerializeField]
        private TextMeshProUGUI _lblScore;
        [SerializeField]
        private AudioClip _audioClip;

        private void Start()
        {
            ResetState();
            _lblScore = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Handle snake directional (control assignment)
        /// </summary>
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down) _direction = Vector2.up;
            if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left) _direction = Vector2.right;
            if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right) _direction = Vector2.left;
            if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up) _direction = Vector2.down;
            var pos = this.transform.position;
            var step = speed * Time.deltaTime;

            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(
             Mathf.Round(pos.x + _direction.x),
             Mathf.Round(pos.y + _direction.y),
             0.0f), step);

            for (int i = _segments.Count - 1; i > 0; i--)
            {
                _segments[i].position = Vector3.MoveTowards(_segments[i].position, new Vector3(
                Mathf.Round(_segments[i - 1].position.x - (_direction.x / 2)),
                Mathf.Round(_segments[i - 1].position.y - (_direction.y / 2)),
                0.0f), step);
            }
        }

        /// <summary>
        /// Reposition the snake and all the body
        /// </summary>
        private void FixedUpdate()
        {

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
            _segments.Add(this.transform);
            this.transform.position = Vector3.zero;
        }

        /// <summary>
        /// Execute functions according to the collition
        /// Food => Will grow the snake
        /// Wall & Body => Restart game
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Collition");
            if (other.tag == "Food")
            {
                SoundManager.Instance.PlaySound(_audioClip);
                Grow();
                AddScore();
            }
            else if (other.tag == "Wall")
            {
                ResetState();
            }
            else if (other.tag == "Body")
            {
                // ResetState();
            }
        }

        private async void AddScore()
        {
            score += 100;
            await PlayerDao.UpdateCurrentScore(score);
            _lblScore.text = $"{score}";
        }


    }
}