using System;
using System.Xml;

public class BaseOSM
{
        public T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
        {
            // TODO:  We are going to assume 'attrName' exists in colection
            string strValue = attributes[attrName].Value;
            return (T)Convert.ChangeType(strValue, typeof(T));
    }
}
