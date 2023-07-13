using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector2.down;
        }
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        var pos = this.transform.position;
        this.transform.position = new Vector3(
            Mathf.Round(pos.x + _direction.x),
            Mathf.Round(pos.y + _direction.y),
            0.0f);
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
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

}