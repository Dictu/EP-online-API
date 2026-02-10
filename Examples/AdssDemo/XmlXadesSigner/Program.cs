using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace XmlXadesSigner;

internal class Program
{
    private const string certPath = @"C:\Temp\certificate.pfx";
    private const string certPassword = "[password here]";
    private const string inputPath = @"C:\Temp\tosign.xml";
    private const string outputPath = @"C:\Temp\signed.xml";

    static int Main(string[] args)
    {
        // Trace source logging voor debug doeleinden.
        TraceConfiguration.Register();

        const string signedXmlSource = "System.Security.Cryptography.Xml";

        var traceSwitch = new SourceSwitch("signedXmlSwitch", "Verbose")
        {
            Level = SourceLevels.Verbose
        };

        Trace.AutoFlush = true;

        var sxTrace = new TraceSource(signedXmlSource)
        {
            Switch = traceSwitch
        };

        sxTrace.Switch.Level = SourceLevels.Verbose;

        Trace.WriteLine("=== SignedXml tracing enabled ===");

        try
        {
            var cert = X509CertificateLoader.LoadPkcs12FromFile(
                certPath,
                certPassword,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable
            );

            if (!cert.HasPrivateKey)
            {
                Console.Error.WriteLine("Het PFX-bestand bevat geen private key.");
                return 3;
            }

            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.Load(inputPath);

            var signed = XadesSigner.SignEnergieprestatieWithFilter2(doc, cert);
            signed.Save(outputPath);

            Console.WriteLine($"OK: ondertekend bestand geschreven naar: {outputPath}");

            XadesSigner.VerifyXades(doc);

            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Fout tijdens ondertekenen:");
            Console.Error.WriteLine(ex);
            return 1;
        }
    }
}