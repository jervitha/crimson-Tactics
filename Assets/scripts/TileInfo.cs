using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int x;
    public int y;

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
