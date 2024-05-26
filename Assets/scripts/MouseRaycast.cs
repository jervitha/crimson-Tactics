using UnityEngine;
using TMPro;

public class MouseRaycast : MonoBehaviour
{
    public TextMeshProUGUI uiText;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                uiText.text = $"Tile Position: {tileInfo.x}, {tileInfo.y}";
            }
        }
    }
}
