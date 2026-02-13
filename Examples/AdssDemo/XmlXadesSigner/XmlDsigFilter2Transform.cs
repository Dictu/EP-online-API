using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace XmlXadesSigner;

/// <summary>
/// Custom XPath Filter 2.0 transform voor SignedXml.
/// - Serialiseert <XPath> zonder overbodige xmlns:*
/// - Bindt tijdens evaluatie de gebruikte prefixen via XmlNamespaceManager (runtime)
/// </summary>
public sealed class XmlDsigFilter2Transform : Transform
{
    public const string AlgorithmUri = "http://www.w3.org/2002/06/xmldsig-filter2";

    private readonly List<(string Filter, string XPath, Dictionary<string, string> Ns)> items = [];
    private XmlDocument? inputDoc;

    // Detecteer prefixen in de XPath (bijv. tns: / ds:)
    private static readonly Regex s_prefixRegex = new Regex(@"(?<!:)\b([A-Za-z_][\w\-.]*)\:(?!:)", RegexOptions.Compiled);


    public XmlDsigFilter2Transform()
    {
        Algorithm = AlgorithmUri;
    }

    public override Type[] InputTypes => [typeof(Stream), typeof(XmlDocument)];
    public override Type[] OutputTypes => [typeof(XmlDocument)];

    /// <summary>
    /// Registreert deze transform bij CryptoConfig zodat SignedXml hem kan vinden.
    /// </summary>
    public static void Register()
    {
        if (CryptoConfig.CreateFromName(AlgorithmUri) is null)
            CryptoConfig.AddAlgorithm(typeof(XmlDsigFilter2Transform), AlgorithmUri);
    }

    /// <summary>
    /// Alternatieve helper: zelf namespaces bijzetten op de <XPath>-node.
    /// </summary>
    public static XmlDsigFilter2Transform Create(XmlDocument owner, string xpath, string filter, params (string prefix, string ns)[] requiredNs)
    {
        var t = new XmlDsigFilter2Transform { Algorithm = AlgorithmUri };

        var frag = owner.CreateDocumentFragment();
        var e = owner.CreateElement("XPath", AlgorithmUri);
        e.SetAttribute("Filter", filter);
        e.InnerText = xpath;

        // Alleen expliciet gevraagde mappings serialiseren
        foreach (var (pfx, uri) in requiredNs)
        {
            if (string.IsNullOrWhiteSpace(pfx))
                e.SetAttribute("xmlns", uri);
            else
                e.SetAttribute($"xmlns:{pfx}", uri);
        }

        frag.AppendChild(e);
        t.LoadInnerXml(frag.ChildNodes);
        return t;
    }

    public override void LoadInput(object obj)
    {
        if (obj is XmlDocument document)
        {
            inputDoc = document;
        }
        else if (obj is Stream s)
        {
            var streamDocument = new XmlDocument { PreserveWhitespace = true };
            using var sr = new StreamReader(s, leaveOpen: true);
            streamDocument.LoadXml(sr.ReadToEnd());
            inputDoc = streamDocument;
        }
        else
        {
            throw new CryptographicException("Unsupported input type for Filter2 transform.");
        }
    }

    public override object GetOutput()
    {
        if (inputDoc is null)
            throw new CryptographicException("No input for Filter2 transform.");

        // Werkdocument: begin met de volledige input
        var document = new XmlDocument { PreserveWhitespace = true };
        document.LoadXml(inputDoc.OuterXml);

        foreach (var (filter, xpath, nsMap) in items)
        {
            var nsm = new XmlNamespaceManager(document.NameTable);

            // Nodig in subtract: ds:Signature
            nsm.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);

            // Voeg expliciet meegegeven mappings toe (alleen als je de 'uitgebreide' Create(...) gebruikt)
            foreach (var kv in nsMap)
            {
                if (!string.IsNullOrEmpty(kv.Key) && nsm.LookupNamespace(kv.Key) is null)
                    nsm.AddNamespace(kv.Key, kv.Value);
            }

            // Detecteer prefixen uit de XPath en bind ze aan in-scope namespaces uit het document
            EnsureNsBindings(nsm, document, xpath);

            if (filter.Equals("intersect", StringComparison.OrdinalIgnoreCase))
            {
                var nodes = document.SelectNodes(xpath, nsm);
                if (nodes is { Count: > 0 } && nodes[0] is XmlElement keep)
                {
                    var kept = new XmlDocument { PreserveWhitespace = true };
                    kept.AppendChild(kept.ImportNode(keep, deep: true));
                    document = kept;
                }
                else
                {
                    document = new XmlDocument { PreserveWhitespace = true };
                    document.AppendChild(document.CreateElement("empty"));
                }
            }
            else if (filter.Equals("subtract", StringComparison.OrdinalIgnoreCase))
            {
                var nodes = document.SelectNodes(xpath, nsm);
                if (nodes is { Count: > 0 })
                {
                    var toRemove = nodes.Cast<XmlNode>().ToList();
                    foreach (var n in toRemove)
                    {
                        n.ParentNode?.RemoveChild(n);
                    }
                }
            }
            else
            {
                throw new CryptographicException($"Onbekende Filter2 filter '{filter}'.");
            }
        }

        return document;
    }

    public override object GetOutput(Type type) => GetOutput();

    public override void LoadInnerXml(XmlNodeList nodeList)
    {
        items.Clear();
        foreach (XmlNode n in nodeList)
        {
            if (n is XmlElement e && e.LocalName == "XPath" && e.NamespaceURI == AlgorithmUri)
            {
                var filter = e.GetAttribute("Filter"); // intersect|subtract
                var xpath = e.InnerText;

                var ns = new Dictionary<string, string>(StringComparer.Ordinal);
                foreach (XmlAttribute a in e.Attributes)
                {
                    if (a.Prefix == "xmlns" && !string.IsNullOrWhiteSpace(a.LocalName))
                        ns[a.LocalName] = a.Value;
                    else if (a.Name == "xmlns")
                        ns[string.Empty] = a.Value;
                }

                items.Add((filter, xpath, ns));
            }
        }
    }

    /// <summary>
    /// Serialiseer de opgebouwde <XPath>-items terug
    /// </summary>
    protected override XmlNodeList? GetInnerXml()
    {
        var doc = new XmlDocument();
        var frag = doc.CreateDocumentFragment();
        foreach (var (filter, xpath, ns) in items)
        {
            var e = doc.CreateElement("XPath", AlgorithmUri);
            e.SetAttribute("Filter", filter);
            foreach (var kv in ns)
            {
                if (string.IsNullOrEmpty(kv.Key))
                    e.SetAttribute("xmlns", kv.Value);
                else
                    e.SetAttribute($"xmlns:{kv.Key}", kv.Value);
            }
            e.InnerText = xpath;
            frag.AppendChild(e);
        }
        return frag.SelectNodes(".");
    }

    /// <summary>
    /// Bindt alle in de XPath gebruikte prefixen (bv. tns, ds) aan de namespace‑URI's
    /// die in het document in scope zijn; serialiseert géén xmlns‑attributes.
    /// </summary>
    private static void EnsureNsBindings(XmlNamespaceManager nsm, XmlDocument doc, string xpath)
    {
        var prefixes = s_prefixRegex.Matches(xpath)
            .Cast<Match>()
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList();

        foreach (var prefix in prefixes)
        {
            if (prefix == "xml") 
                continue; 

            if (prefix == "ds")
            {
                if (nsm.LookupNamespace("ds") is null)
                    nsm.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                continue;
            }

            string? uri = doc.DocumentElement?.GetNamespaceOfPrefix(prefix);
            if (string.IsNullOrEmpty(uri))
            {
                var attr = doc.DocumentElement?.Attributes[$"xmlns:{prefix}"];
                uri = attr?.Value;
            }

            if (!string.IsNullOrEmpty(uri))
            {
                if (nsm.LookupNamespace(prefix) is null)
                    nsm.AddNamespace(prefix, uri);
            }
            else
            {
                throw new XPathException($"Namespace prefix '{prefix}' is not defined.");
            }
        }
    }
}
