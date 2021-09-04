using UnityEngine;

public class DragBehaviour : MonoBehaviour
{   
    Vector3 touchPosition;
    Rigidbody2D rb;
    Vector3 direction;
    float moveSpeed = 10f;

    Vector2 mousePosition;
    float deltaX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMouseDown()
    {
        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
    }

    public void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int xPos = Mathf.RoundToInt(mousePosition.x);
        if (xPos >= -1 && xPos <= 9)
        {
            transform.position = new Vector2(xPos - deltaX, -4);
        }
    }

    public void OnMouseUp()
    {
       
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            direction = touchPosition - transform.position;
            int xPos = Mathf.RoundToInt(direction.x);
            rb.velocity = new Vector2(xPos, 0) * moveSpeed;

            if (touch.phase == TouchPhase.Ended)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

}
