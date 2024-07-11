using UnityEngine;
using UnityEngine.UI;

public class MouseRaycast : MonoBehaviour
{
    public Text tileInfoText;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GridCubeInfo cubeInfo = hit.transform.GetComponent<GridCubeInfo>();
            if (cubeInfo != null)
            {
                tileInfoText.text = $"Tile Position: ({cubeInfo.x}, {cubeInfo.y})";
            }
        }
    }
}
