using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Phobos.Api.Infrastructure.Configuration
{
	public interface IConfig
	{
		string GoogleClientId { get; }

		string GoogleClientSecret { get; }
	}

	public class Config : IConfig
	{
		private readonly Lazy<XDocument> ConfigDocumentCache;

		private readonly Lazy<string> googleClientIdCache;
		private readonly Lazy<string> googleClientSecretCache;

		public Config()
		{
			ConfigDocumentCache = new Lazy<XDocument>(() => ConfigDocument(), true);

			googleClientIdCache = new Lazy<string>(() => LoadGoogleClientId(), true);
			googleClientSecretCache = new Lazy<string>(() => LoadGoogleClientSecret(), true);
		}

		public string GoogleClientId
		{
			get
			{
				return googleClientIdCache.Value;
			}
		}

		public string GoogleClientSecret
		{
			get
			{
				return googleClientSecretCache.Value;
			}
		}


		private string LoadGoogleClientId()
		{
			if (ConfigDocumentCache.Value != null)
				return ConfigDocumentCache.Value.XPathSelectElements("/configuration/externalLogin/google").Select(x => x.Attribute("clientId").Value).First();

			return string.Empty;
		}

		private string LoadGoogleClientSecret()
		{
			if (ConfigDocumentCache.Value != null)
				return ConfigDocumentCache.Value.XPathSelectElements("/configuration/externalLogin/google").Select(x => x.Attribute("clientSecret").Value).First();

			return string.Empty;
		}

		private XDocument ConfigDocument()
		{
			string localConfigFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", ""), @"..\..\..\")) + "config.local";

			if (File.Exists(localConfigFilePath))
				return XDocument.Load(localConfigFilePath);

			return null;
		}
	}
}