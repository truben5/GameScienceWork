using UnityEngine;

[RequireComponent(typeof(MapReader))]
public abstract class InfrastructureBehaviour : MonoBehaviour
{
    public MapReader map;

    void Awake()
    {
        map = GetComponent<MapReader>();
    }

    public Vector3 GetCenter(OSMWay way)
    {
        Vector3 total = Vector3.zero;

        foreach (var id in way.NodeIDs)
        {
            total += map.nodes[id];
        }

        return total / way.NodeIDs.Count;
    }
}
