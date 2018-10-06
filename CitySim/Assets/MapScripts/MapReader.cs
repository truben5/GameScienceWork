using System.Collections.Generic;
using System.Xml;
using UnityEngine;

class MapReader : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<ulong, OSMNode> nodes;
    [HideInInspector]
    public List<OSMWay> ways;
    [HideInInspector]
    public OSMBounds bounds;

    public string resourceFile;

    public bool IsReady { get; private set; }

	// Use this for initialization
	void Start () {

        nodes = new Dictionary<ulong, OSMNode>();
        ways = new List<OSMWay>();

        var txtAsses = Resources.Load<TextAsset>(resourceFile);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txtAsses.text);


        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        GetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));

        IsReady = true;
	}

    //void Update()
    //{
    //    foreach (OSMWay w in ways)
    //    {
    //        if (w.Visible)
    //        {
    //            Color c = Color.cyan; // cyan for building
    //            if (!w.isBoundary)
    //            {
    //                c = Color.red; // red for roads
    //            }

    //            for (int i = 1; i < w.NodeIDs.Count; i++)
    //            {
    //                OSMNode p1 = nodes[w.NodeIDs[i - 1]];
    //                OSMNode p2 = nodes[w.NodeIDs[i]];

    //                Vector3 v1 = p1 - bounds.Center;
    //                Vector3 v2 = p2 - bounds.Center;

    //                Debug.DrawLine(v1, v2, c);
    //            }
    //        }
    //    }
    //}

    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach(XmlNode node in xmlNodeList)
        {
            OSMWay way = new OSMWay(node);
            ways.Add(way);

        }
    }

    void GetNodes(XmlNodeList xmlNodeList)
    {
        foreach(XmlNode n in xmlNodeList)
        {
            OSMNode node = new OSMNode(n);
            nodes[node.ID] = node;
        }
    }

    void SetBounds(XmlNode xmlNode)
    {
        bounds = new OSMBounds(xmlNode);
    }
}
