using UnityEngine;
using UnityEngine.EventSystems;

public class DragIngredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform parentTransform;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        parentTransform = transform.parent;
        transform.SetParent(GetComponentInParent<Canvas>().transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsOverCauldron())
        {
            // a�de el ingrediente a la lista del caldero
            CauldronManager.Instance.AddIngredient(gameObject.name);
        }

        // Regresar el ingrediente a la estanter�a
        transform.position = startPosition;
        transform.SetParent(parentTransform);
    }

    private bool IsOverCauldron()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            CauldronManager.Instance.calderoRectTransform,
            Input.mousePosition,
            null,
            out Vector2 localPoint
        );

        return CauldronManager.Instance.calderoRectTransform.rect.Contains(localPoint);
    }
}