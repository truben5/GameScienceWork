using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

class OSMWay : BaseOSM
{
    public ulong ID { get; private set; }

    public bool Visible { get; private set; }

    public List<ulong> NodeIDs { get; private set; }

    public bool isBoundary { get; private set; }

    public bool IsBuilding { get; private set; }

    public bool IsRoad { get; private set; }

    public float Height { get; private set; }

    public string Name { get; private set; }

    public int Lanes { get; private set; }

    public bool IsRailway = false;

    public OSMWay(XmlNode node)
    {
        NodeIDs = new List<ulong>();
        Height = 3.0f;
        Lanes = 1;

        ID = GetAttribute<ulong>("id", node.Attributes);
        Visible = GetAttribute<bool>("visible", node.Attributes);

        XmlNodeList nds = node.SelectNodes("nd");
        foreach (XmlNode n in nds)
        {
            ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
            NodeIDs.Add(refNo);
        }


        if (NodeIDs.Count > 1)
        {
            isBoundary = NodeIDs[0] == NodeIDs[NodeIDs.Count - 1];
        }

        XmlNodeList tags = node.SelectNodes("tag");
        foreach(XmlNode t in tags)
        {
            string key = GetAttribute<string>("k", t.Attributes);
            if (key == "building:levels")
            {
                Height = 3.0f * GetAttribute<float>("v",t.Attributes);
            }
            else if (key == "height")
            {
                Height = .3048f * GetAttribute<float>("v",t.Attributes);
            }
            else if(key == "building")
            {
                IsBuilding = GetAttribute<string>("v", t.Attributes) == "yes";
            }
            else if(key == "highway")
            {
                IsRoad = true;
            }
            else if (key == "lanes")
            {
                Lanes = GetAttribute<int>("v", t.Attributes);
            }
            else if (key == "name")
            {
                Name = GetAttribute<string>("v", t.Attributes);
            }
            else if (key == "railway")
            {
                IsRailway = true;
                //Debug.Log("railway");
            }
        }
    }
}
