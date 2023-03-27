public static class KeyGenerator
{
    /// <summary>
    /// Returns key:(public) value:(private) pair that represents a certificate.
    /// </summary>
    /// <returns></returns>
    public static KeyValuePair<string,string> Generate()
    {
        using var rsa = new RSACryptoServiceProvider();
        var publicKey = rsa.ToXmlString(false);
        var privateKey = rsa.ToXmlString(true);
        return new KeyValuePair<string, string>(
            GetPrettyXml(publicKey),
            GetPrettyXml(privateKey)
            );
    }

    private static string GetPrettyXml(string xml)
    {
        try
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}