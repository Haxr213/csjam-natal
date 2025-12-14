using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    public bool isOccupied;

    public void Occupy()
    {
        isOccupied = true;
    }

    public void Free()
    {
        isOccupied = false;
    }
}
