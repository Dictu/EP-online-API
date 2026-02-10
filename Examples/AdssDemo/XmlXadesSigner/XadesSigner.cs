using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace XmlXadesSigner;

internal static class XadesSigner
{
    private const string nsDs = SignedXml.XmlDsigNamespaceUrl;
    private const string nsXades = "http://uri.etsi.org/01903/v1.3.2#";
    private const string nsTns = "http://schemas.ep-online.nl/monitoringbestand";

    /// <summary>
    /// Ondertekent de XML met XAdES-BES en XPath Filter 2.0 (intersect + subtract).
    /// - Reference 1: Type=http://uri.etsi.org/01903#SignedProperties naar xades:SignedProperties (C14N + SHA-256)
    /// - Reference 2: naar het document (URI="") met:
    ///     * Filter2 intersect //tns:Energieprestatie
    ///     * Enveloped signature transform
    ///     * Filter2 subtract /descendant::ds:Signature
    /// </summary>
    public static XmlDocument SignEnergieprestatieWithFilter2(XmlDocument source, X509Certificate2 cert)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (cert is null) throw new ArgumentNullException(nameof(cert));
        if (!cert.HasPrivateKey) throw new ArgumentException("Het certificaat heeft geen private key.", nameof(cert));

        // 0) Registreren van de custom XPath Filter 2.0 transform
        XmlDsigFilter2Transform.Register();

        // 1) SignedXml object opzetten
        var signedXml = new SignedXmlXades(source)
        {
            SigningKey = cert.GetRSAPrivateKey()
        };
        signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigC14NTransformUrl;
        signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

        // 2) XAdES QualifyingProperties/ SignedProperties in ds: Object
        var signatureId = "Signature_" + Guid.NewGuid().ToString("N");
        var signedPropsId = "SignedProperties_" + Guid.NewGuid().ToString("N");
        var signedDataRef = "SignedDataObject_" + Guid.NewGuid().ToString("N");

        var qualifyingProperties = BuildXadesQualifyingProperties(source, signatureId, signedPropsId, cert, signedDataRef);

        var objElement = source.CreateElement("ds", "Object", nsDs);
        objElement.SetAttribute("Id", signedDataRef);
        objElement.SetAttribute("xmlns", nsDs);
        objElement.AppendChild(qualifyingProperties);
        
        var dataObject = new DataObject();
        dataObject.LoadXml(objElement);
        signedXml.AddObject(dataObject);

        // 3) Reference naar SignedProperties
        var rSignedProps = new Reference
        {
            Id = "SignedProperties-Reference",
            Uri = "#" + signedPropsId,
            Type = "http://uri.etsi.org/01903#SignedProperties",
            DigestMethod = SignedXml.XmlDsigSHA256Url
        };
        rSignedProps.AddTransform(new XmlDsigExcC14NTransform()); // XmlDsigC14NTransform()); // canonicalize XAdES
        signedXml.AddReference(rSignedProps);


        // 4) Reference naar het document met Filter 2.0
        var rDoc = new Reference
        {
            Id = "SignedDataObject-Reference",
            Uri = "", // hele document
            DigestMethod = SignedXml.XmlDsigSHA256Url
        };

        // Filter2: Gebruik de //tns:Energieprestatie node om de ondertekening over te berekenen.
        var fIntersect = XmlDsigFilter2Transform.Create(source, "//tns:Energieprestatie", "intersect", ("tns", nsTns));
        rDoc.AddTransform(fIntersect);

        // Standaard enveloped transform (verwijder ds:Signature vóór canonicalization)
        rDoc.AddTransform(new XmlDsigEnvelopedSignatureTransform());

        // Sluit de Signature node uit bij controle van de ondertekening
        var fSubtract = XmlDsigFilter2Transform.Create(source, "/descendant::ds:Signature", "subtract", ("ds", nsDs));
        rDoc.AddTransform(fSubtract);

        signedXml.AddReference(rDoc);

        // 5) KeyInfo met certificaat
        var ki = new KeyInfo();
        ki.AddClause(new KeyInfoX509Data(cert, X509IncludeOption.EndCertOnly));
        signedXml.KeyInfo = ki;

        // 6) Zet een Signature Id en bereken de handtekening.
        signedXml.Signature.Id = signatureId;
        signedXml.ComputeSignature();
        var sigXml = signedXml.GetXml();

        // 8) Signature toevoegen onder .../EPMeta/Tooling
        InsertSignatureAtTooling(source, sigXml);

        return source;
    }

    /// <summary>
    /// Verifieert met XadesSignedXml (zodat #SignedProperties in <Object> gevonden wordt).
    /// </summary>

    public static bool VerifyXades(XmlDocument doc)
    {
        if (doc is null) throw new ArgumentNullException(nameof(doc));

        XmlDsigFilter2Transform.Register();

        var ns = new XmlNamespaceManager(doc.NameTable);
        ns.AddNamespace("ds", nsDs);

        var sigNode = (XmlElement?)doc.SelectSingleNode("//ds:Signature", ns)
            ?? throw new InvalidOperationException("Geen <Signature> element gevonden.");

        var sx = new SignedXmlXades(doc);

        //Voeg xmldsig-filter2 toe als veilige vorm van canonisering
        sx.SafeCanonicalizationMethods.Add(XmlDsigFilter2Transform.AlgorithmUri);

        sx.LoadXml(sigNode);
        return sx.CheckSignature();
    }

    /// <summary>
    /// Maakt XAdES QualifyingProperties met SignedProperties, SigningTime en SigningCertificate (SHA-256).
    /// </summary>
    private static XmlElement BuildXadesQualifyingProperties(XmlDocument doc, string signatureId, string signedPropsId, X509Certificate2 cert, string signedDataRef)
    {
        var qp = doc.CreateElement("xades", "QualifyingProperties", nsXades);
        qp.SetAttribute("Target", "#" + signatureId);

        var sp = doc.CreateElement("xades", "SignedProperties", nsXades);
        sp.SetAttribute("Id", signedPropsId);

        var ssp = doc.CreateElement("xades", "SignedSignatureProperties", nsXades);

        var st = doc.CreateElement("xades", "SigningTime", nsXades);
        st.InnerText = DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ssK");
        ssp.AppendChild(st);

        var sc = doc.CreateElement("xades", "SigningCertificate", nsXades);
        var certNode = doc.CreateElement("xades", "Cert", nsXades);

        var certDigest = doc.CreateElement("xades", "CertDigest", nsXades);
        var dm = doc.CreateElement("ds", "DigestMethod", nsDs);
        dm.SetAttribute("Algorithm", SignedXml.XmlDsigSHA256Url);

        var dv = doc.CreateElement("ds", "DigestValue", nsDs);
        using (var sha = SHA256.Create())
        {
            dv.InnerText = Convert.ToBase64String(sha.ComputeHash(cert.RawData));
        }
        certDigest.AppendChild(dm);
        certDigest.AppendChild(dv);

        var issuerSerial = doc.CreateElement("xades", "IssuerSerial", nsXades);
        var xName = doc.CreateElement("ds", "X509IssuerName", nsDs);
        xName.InnerText = cert.Issuer;

        var xSerial = doc.CreateElement("ds", "X509SerialNumber", nsDs);
        xSerial.InnerText = GetSerialNumberDecimalBigEndian(cert);
        issuerSerial.AppendChild(xName);
        issuerSerial.AppendChild(xSerial);

        certNode.AppendChild(certDigest);
        certNode.AppendChild(issuerSerial);
        sc.AppendChild(certNode);

        ssp.AppendChild(sc);

        var sdop = doc.CreateElement("xades", "SignedDataObjectProperties", nsXades);
        var dof = doc.CreateElement("xades", "DataObjectFormat", nsXades);
        dof.SetAttribute("ObjectReference", $"#{signedDataRef}");
        var desc = doc.CreateElement("xades", "Description", nsXades); desc.InnerText = "XML";
        var mime = doc.CreateElement("xades", "MimeType", nsXades); mime.InnerText = "XML";
        dof.AppendChild(desc); dof.AppendChild(mime);
        sdop.AppendChild(dof);

        sp.AppendChild(ssp);
        sp.AppendChild(sdop);

        qp.AppendChild(sp);
        return qp;
    }

    /// <summary>
    /// Converteert X509 serie van little-endian hex naar big-endian decimaal (zoals vaak in XAdES voorbeelden).
    /// </summary>
    private static string GetSerialNumberDecimalBigEndian(X509Certificate2 cert)
    {
        var hexLe = cert.SerialNumber; // little-endian hex string
        var bytesLe = Enumerable.Range(0, hexLe.Length / 2)
            .Select(i => Convert.ToByte(hexLe.Substring(i * 2, 2), 16)).ToArray();
        Array.Reverse(bytesLe); // nu big-endian
        var bi = new BigInteger(bytesLe.Concat(new byte[] { 0x00 }).ToArray()); // force non-negative
        return bi.ToString(); // decimaal
    }

    /// <summary>
    /// Plaatst de ds:Signature onder .../EPMeta/Tooling als die bestaat; anders aan de root.
    /// </summary>
    private static void InsertSignatureAtTooling(XmlDocument doc, XmlElement signature)
    {
        var nsm = new XmlNamespaceManager(doc.NameTable);
        nsm.AddNamespace("tns", nsTns);

        var tooling = doc.SelectSingleNode("/tns:Energieprestatie/*[local-name()='EPMeta']/*[local-name()='Tooling']", nsm);
        if (tooling is XmlNode tNode)
        {
            tNode.AppendChild(doc.ImportNode(signature, true));
        }
        else
        {
            throw new InvalidOperationException("De Tooling node is niet gevonden, kan de handtekening niet plaatsen.");
        }
    }
}