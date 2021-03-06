﻿using NBitcoin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace NBXplorer
{
    public class NBXplorerDefaultSettings
    {

		public static string GetFolderName(ChainName chainName)
		{
			if (chainName == null)
				throw new ArgumentNullException(nameof(chainName));
			if (chainName == ChainName.Mainnet)
				return "Main";
			if (chainName == ChainName.Testnet)
				return "TestNet";
			if (chainName == ChainName.Regtest)
				return "RegTest";
			return chainName.ToString();
		}

		static Dictionary<ChainName, NBXplorerDefaultSettings> _Settings = new Dictionary<ChainName, NBXplorerDefaultSettings>();
		public string DefaultDataDirectory
		{
			get;
			set;
		}
		public string DefaultConfigurationFile
		{
			get;
			set;
		}
		public string DefaultCookieFile
		{
			get;
			private set;
		}
		public int DefaultPort
		{
			get;
			set;
		}
		public Uri DefaultUrl
		{
			get;
			set;
		}

		public static NBXplorerDefaultSettings GetDefaultSettings(ChainName networkType)
		{
			if (_Settings.TryGetValue(networkType, out var v))
				return v;
			lock (_Settings)
			{
				if (_Settings.TryGetValue(networkType, out v))
					return v;
				var settings = new NBXplorerDefaultSettings();
				settings.DefaultDataDirectory = StandardConfiguration.DefaultDataDirectory.GetDirectory("NBXplorer", GetFolderName(networkType), false);
				settings.DefaultConfigurationFile = Path.Combine(settings.DefaultDataDirectory, "settings.config");
				settings.DefaultCookieFile = Path.Combine(settings.DefaultDataDirectory, ".cookie");
				settings.DefaultPort = (networkType == ChainName.Mainnet ? 24444 :
													  networkType == ChainName.Regtest ? 24446 :
													  networkType == ChainName.Testnet ? 24445 : 24447);
				settings.DefaultUrl = new Uri($"http://127.0.0.1:{settings.DefaultPort}/", UriKind.Absolute);
				_Settings.Add(networkType, settings);
				return settings;
			}
		}
	}
}
