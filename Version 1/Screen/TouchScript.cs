using UnityEngine;
using UnityEngine.EventSystems;

public class TouchScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public Vector2 TouchDist;
    [SerializeField] public Vector2 PointerOld;
    [SerializeField] protected int PointerId;
    [SerializeField] public bool Pressed;

    private void Update()
    {
        if (Pressed)
        {
            if (Input.touchCount == 1)
            {
                if (PointerId >= 0 && PointerId < Input.touches.Length)
                {
                    TouchDist = Input.touches[PointerId].position - PointerOld;
                    PointerOld = Input.touches[PointerId].position;
                }
                else
                {
                    TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                    PointerOld = Input.mousePosition;
                }
            }    
        }
        else
        {
            TouchDist = new Vector2();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}
