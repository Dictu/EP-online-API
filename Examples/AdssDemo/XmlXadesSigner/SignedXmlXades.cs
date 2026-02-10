using System.Security.Cryptography.Xml;
using System.Xml;

namespace XmlXadesSigner;

/// <summary>
/// SignedXml-variant die GetIdElement uitbreidt zodat ook targets binnen <ds:Object>
/// (en met evt. namespace-geprefixt ID-attribuut) gevonden worden.
/// </summary>
internal sealed class SignedXmlXades : SignedXml
{
    public SignedXmlXades(XmlDocument document) : base(document) { }
    public SignedXmlXades(XmlElement element) : base(element) { }

    public override XmlElement? GetIdElement(XmlDocument? document, string idValue)
    {
        ArgumentNullException.ThrowIfNull(document);

        var found = base.GetIdElement(document, idValue);
        if (found is not null)
            return found;

        var xPath = $"//*[@*[local-name()='Id' or local-name()='ID' or local-name()='id']='{idValue}']";
        var node = document.SelectSingleNode(xPath) as XmlElement;
        if (node is not null)
            return node;

        if (Signature is not null)
        {
            foreach (DataObject obj in Signature.ObjectList)
            {
                if (obj.Data is null) continue;
                foreach (XmlNode n in obj.Data)
                {
                    if (n is XmlElement el)
                    {
                        var target = FindByIdInSubtree(el, idValue);
                        if (target != null) return target;
                    }
                }
            }
        }

        return null;
    }

    private static XmlElement? FindByIdInSubtree(XmlElement root, string idValue)
    {
        if (HasId(root, idValue))
            return root;

        foreach (XmlNode child in root.ChildNodes)
        {
            if (child is XmlElement el)
            {
                var hit = FindByIdInSubtree(el, idValue);
                if (hit is not null)
                    return hit;
            }
        }
        return null;
    }

    private static bool HasId(XmlElement el, string idValue)
    {
        if (el.HasAttribute("Id") && el.GetAttribute("Id") == idValue) return true;
        if (el.HasAttribute("id") && el.GetAttribute("id") == idValue) return true;
        if (el.HasAttribute("ID") && el.GetAttribute("ID") == idValue) return true;

        foreach (XmlAttribute a in el.Attributes)
        {
            if ((a.LocalName is "Id" or "id" or "ID") && a.Value == idValue)
                return true;
        }
        return false;
    }
}