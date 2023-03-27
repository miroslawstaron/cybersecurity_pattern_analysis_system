#include <CkCert.h>
#include <CkPrivateKey.h>

void ChilkatSample(void)
    {
    CkCert cert;

    // Load from the PFX file
    const char *pfxFilename = "/Users/chilkat/testData/pfx/chilkat_ssl_pwd_is_test.pfx";
    const char *pfxPassword = "test";

    // A PFX typically contains certificates in the chain of authentication.
    // The Chilkat cert object will choose the certificate w/
    // private key farthest from the root authority cert.
    // To access all the certificates in a PFX, use the 
    // Chilkat certificate store object instead.
    bool success = cert.LoadPfxFile(pfxFilename,pfxPassword);
    if (success != true) {
        std::cout << cert.lastErrorText() << "\r\n";
        return;
    }

    // Get the private key...
    CkPrivateKey *privKey = 0;
    privKey = cert.ExportPrivateKey();
    if (cert.get_LastMethodSuccess() == false) {
        std::cout << cert.lastErrorText() << "\r\n";
        return;
    }

    // Export to various formats:

    const char *password = "secret";
    const char *path = 0;

    // PKCS8 Encrypted DER
    path = "/Users/chilkat/testData/privkeys/chilkat_pkcs8_enc.der";
    success = privKey->SavePkcs8EncryptedFile(password,path);
    if (success != true) {
        std::cout << privKey->lastErrorText() << "\r\n";
        delete privKey;
        return;
    }

    // PKCS8 Encrypted PEM
    path = "/Users/chilkat/testData/privkeys/chilkat_pkcs8_enc.pem";
    success = privKey->SavePkcs8EncryptedPemFile(password,path);
    if (success != true) {
        std::cout << privKey->lastErrorText() << "\r\n";
        delete privKey;
        return;
    }

    // PKCS8 Unencrypted DER
    path = "/Users/chilkat/testData/privkeys/chilkat_pkcs8.der";
    success = privKey->SavePkcs8File(path);
    if (success != true) {
        std::cout << privKey->lastErrorText() << "\r\n";
        delete privKey;
        return;
    }

    // PKCS8 Unencrypted PEM
    path = "/Users/chilkat/testData/privkeys/chilkat_pkcs8.pem";
    success = privKey->SavePkcs8PemFile(path);
    if (success != true) {
        std::cout << privKey->lastErrorText() << "\r\n";
        delete privKey;
        return;
    }

    //  RSA DER (unencrypted)
    path = "/Users/chilkat/testData/privkeys/chilkat_rsa.der";
    success = privKey->SavePkcs1File(path);
    if (success != true) {
        std::cout << privKey->lastErrorText() << "\r\n";
        delete privKey;
        return;
    }

    // RSA PEM (unencrypted)
    path = "/Users/chilkat/testData/privkeys/chilkat_rsa.pem";
    success = privKey->SavePemFile(path);
    if (success != true) {
        std::cout << privKey->lastErrorText() << "\r\n";
        delete privKey;
        return;
    }

    // XML (unencrypted)
    path = "/Users/chilkat/testData/privkeys/chilkat.xml";
    success = privKey->SaveXmlFile(path);
    if (success != true) {
        std::cout << privKey->lastErrorText() << "\r\n";
        delete privKey;
        return;
    }

    delete privKey;

    std::cout << "Private key exported to all formats." << "\r\n";
    }