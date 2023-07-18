using System.Collections.Generic;
using UnityEngine;
using Snake.DataAccess;
using TMPro;
using Snake.Systems;

public class MainSnake : MonoBehaviour
{
    private readonly List<Transform> _segments = new();
    private int score = 0;
    private Vector2 _direction;
    public Transform segmentPrefab;
    public float speed = 10.0f;

    [SerializeField]
    private TextMeshProUGUI _lblScore;
    [SerializeField]
    private AudioClip _audioClip;


    private void Start()
    {
        ResetState();
        AddFirstSegments();
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
    }

    /// <summary>
    /// Reposition the snake and all the body
    /// </summary>
    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--) _segments[i].position = _segments[i - 1].position;

        var vec = new Vector3(
            Mathf.Round(_direction.x),
            Mathf.Round(_direction.y),
            0.0f);
        this.transform.Translate(vec*speed*Time.deltaTime);
    }

    /// <summary>
    /// Add a snake body prefab to the segments
    /// </summary>
    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[^1].position;
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

    private void AddFirstSegments()
    {
        var pos = this.transform.position;
        for (var count = 1.0f; count < 4.0f; count++)
        {
            Transform segment = Instantiate(this.segmentPrefab);
            segment.position = _segments[^1].position;
            segment.position = new Vector3(Mathf.Round(-count + pos.x), Mathf.Round(pos.y), 0.0f);
            _segments.Add(segment);
        }
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
            ResetState();
        }
    }

    private async void AddScore()
    {
        score += 100;
        await PlayerDao.UpdateCurrentScore(score);
        _lblScore.text = $"{score}";
    }

}
