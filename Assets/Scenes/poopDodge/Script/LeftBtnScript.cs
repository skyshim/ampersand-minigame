using UnityEngine;
using UnityEngine.EventSystems;

public class LeftBtnScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerScript plr;
    public float direction = -1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        plr.moveDirection = direction;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        plr.moveDirection = 0f;
    }
}
