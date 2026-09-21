using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class PuzzleTileClick : MonoBehaviour
{
    [SerializeField] private PuzzleGridController grid;
    [SerializeField] private int slotIndex;

    private void OnPointerClick(PointerEventData eventData)
    {
        grid.OnTileClicked(slotIndex);
    }
}
