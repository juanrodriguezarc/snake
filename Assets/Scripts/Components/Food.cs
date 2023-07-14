
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        RandomizePosition();
    }

    /// <summary>
    /// Generates a random position for the food
    /// and re-position the element in the grid
    /// </summary>
    private void RandomizePosition()
    {
        Bounds bounds =  this.gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x),Mathf.Round(y),0.0f);
    }

    /// <summary>
    /// Executes the RandomizePosition function when the snakes
    /// collides with the food
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            RandomizePosition();
        }
    }
}
