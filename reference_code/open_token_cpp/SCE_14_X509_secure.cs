using Psd2CertificateGenerator.Asn1;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Psd2CertificateGenerator
{
    public class X509QcStatmentExtension : X509Extension
    {
        private const string OID_qcStatements = "1.3.6.1.5.5.7.1.3";
        private const string OID_QcsCompliance = "0.4.0.1862.1.1";
        private const string OID_QcRetentionPeriod = "0.4.0.1862.1.3";
        private const string OID_QcsQcSSCD = "0.4.0.1862.1.4";
        private const string OID_QcType = "0.4.0.1862.1.6";
        private const string OID_PSD2qcStatement = "0.4.0.19495.2";
        private const string OID_QSealC_eseal = "0.4.0.1862.1.6.2";
        private const string OID_QWAC_web = "0.4.0.1862.1.6.3";
        private const string OID_PSP_AS = "0.4.0.19495.1.1";
        private const string OID_PSP_PI = "0.4.0.19495.1.2";
        private const string OID_PSP_AI = "0.4.0.19495.1.3";
        private const string OID_PSP_IC = "0.4.0.19495.1.4";

        private static Dictionary<PSD2Roles, string> SupportedRoles = new Dictionary<PSD2Roles, string> {
            { PSD2Roles.PSP_AS, OID_PSP_AS },
            { PSD2Roles.PSP_PI, OID_PSP_PI },
            { PSD2Roles.PSP_AI, OID_PSP_AI },
            { PSD2Roles.PSP_IC, OID_PSP_IC },
        };

        public X509QcStatmentExtension(PSD2Roles roles, PSD2CertificateType certificateType, byte retentionPeriod, string ncaName, string ncaId, bool critical = false) : 
            base(OID_qcStatements, EncodePSD2QcStatmentExtension(roles, certificateType, retentionPeriod, ncaName, ncaId), critical)
        {
        }

        private static byte[] EncodePSD2QcStatmentExtension(PSD2Roles roles, PSD2CertificateType certType, byte retentionPeriod, string ncaName, string ncaId)
        {
            return Asn1Encoder.Sequence(
                Asn1Encoder.Sequence(
                    Asn1Encoder.ObjectIdentifier(OID_QcsCompliance) //  this certificate is issued as a Qualified Certificate
                ),
                Asn1Encoder.Sequence(
                    Asn1Encoder.ObjectIdentifier(OID_QcRetentionPeriod), // number of years after the validity period the certificate will be stored in the issuer's archive
                    Asn1Encoder.IntegerBigEndian(new[] { retentionPeriod })
                ),
                Asn1Encoder.Sequence(
                    Asn1Encoder.ObjectIdentifier(OID_QcsQcSSCD) // CAs claiming to issue certificates where the private key related to the certified public key resides in a Secure Signature Creation Device(SSCD)
                ),
                Asn1Encoder.Sequence(
                    Asn1Encoder.ObjectIdentifier(OID_QcType),
                    Asn1Encoder.Sequence(
                        Asn1Encoder.ObjectIdentifier(certType == PSD2CertificateType.QWAC ? OID_QWAC_web : OID_QSealC_eseal)
                    )
                ),
                Asn1Encoder.Sequence(
                    Asn1Encoder.ObjectIdentifier(OID_PSD2qcStatement),
                    Asn1Encoder.Sequence(
                        Asn1Encoder.Sequence(SupportedRoles.Keys.Aggregate(new List<byte[]>(), (acc, role) =>
                        {
                            if (roles.HasFlag(role)) 
                                acc.Add(
                                    Asn1Encoder.Sequence(
                                        Asn1Encoder.ObjectIdentifier(SupportedRoles[role]), 
                                        Asn1Encoder.Utf8String(role.ToString())
                                    )
                                );
                            return acc;
                        }).ToArray()),
                        Asn1Encoder.Utf8String(ncaName),
                        Asn1Encoder.Utf8String(ncaId)
                    )
                )
            );
        }
    }
}