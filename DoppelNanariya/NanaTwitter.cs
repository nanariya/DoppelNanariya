using System;
using System.IO;
using System.Xml;
using System.Net;
using Twitterizer;

namespace DoppelNanariya
{
	public static class NanaTwitter
	{
		private static string ConsumerKey { get {return "";}}
		private static string ConsumerSecret { get {return "";}}
		private static string FileName { get {return "DoppelNanariya.conf";}}
		public static UserConfig Config { get; set; }

		private static OAuthTokenResponse RequestToken { get; set; }
				
		public static bool IsExistUserConfig()
		{
			UserConfig userConfig = LoadConfig();
			if(userConfig == null)
			{
				return false;
			}else{
				Config = userConfig;
				return true;
			}
		}

		public static String CreateRequestTokenUrl()
		{
				OAuthTokenResponse requestToken = OAuthUtility.GetRequestToken(ConsumerKey, ConsumerSecret, "oob");
				Uri uri = OAuthUtility.BuildAuthorizationUri(requestToken.Token);
				return uri.ToString();
		}
		
		public static void SaveConfig(UserConfig conf)
		{
			try
			{
				System.Xml.Serialization.XmlSerializer serializer =
					new System.Xml.Serialization.XmlSerializer(typeof(UserConfig));
				
				System.IO.FileStream fs = new System.IO.FileStream(FileName, FileMode.Create);
				
				serializer.Serialize(fs, conf);
				
				fs.Close();
			}
			catch(Exception)
			{
			}
		}
		public static UserConfig LoadConfig()
		{
			UserConfig userConfig = null;
			
			try
			{
				System.Xml.Serialization.XmlSerializer serializer = 
					new System.Xml.Serialization.XmlSerializer(typeof(UserConfig));
				
				FileStream fs = new FileStream(FileName, FileMode.Open);
				
				userConfig = (UserConfig) serializer.Deserialize(fs);
				
				fs.Close();
			}
			catch(Exception)	
			{}
			
			return userConfig;
		}


		public static Uri RegisterApplicationRequestUrl()
		{
			NanaTwitter.RequestToken = OAuthUtility.GetRequestToken(NanaTwitter.ConsumerKey,NanaTwitter.ConsumerSecret,"oob");
			Console.WriteLine(NanaTwitter.RequestToken.Token.ToString());
			Uri uri = OAuthUtility.BuildAuthorizationUri(NanaTwitter.RequestToken.Token);

			return uri;
		}

		public static void RegisterApplicationWithPinCode(String pinCode)
		{
			OAuthTokenResponse response 
				= OAuthUtility.GetAccessToken(NanaTwitter.ConsumerKey, NanaTwitter.ConsumerSecret, NanaTwitter.RequestToken.Token, pinCode);
			NanaTwitter.Config.AccessToken = response.Token;
			NanaTwitter.Config.AccessTokenSecret = response.TokenSecret;
		}

		public static TwitterResponse<TwitterStatusCollection> ShowHomeTimeline()
		{
			OAuthTokens token = new OAuthTokens();
			token.ConsumerKey = NanaTwitter.ConsumerKey;
			token.ConsumerSecret = NanaTwitter.ConsumerSecret;
			token.AccessToken = Config.AccessToken;
			token.AccessTokenSecret = Config.AccessTokenSecret;

			TwitterResponse<TwitterStatusCollection> status = TwitterTimeline.HomeTimeline(token);

			return status;
			/*
			foreach( TwitterStatus tweet in status.ResponseObject)
			{
				Console.WriteLine(tweet.Text);	
			}
			*/
		}
		public static TwitterResponse<TwitterStatus> SendTweet(String tweet)
		{
			if(Config == null)
			{
				Console.WriteLine("('A`)");
				return null;
			}
			
			OAuthTokens token = new OAuthTokens();
			token.ConsumerKey = NanaTwitter.ConsumerKey;
			token.ConsumerSecret = NanaTwitter.ConsumerSecret;
			token.AccessToken = Config.AccessToken;
			token.AccessTokenSecret = Config.AccessTokenSecret;
			
			TwitterResponse<TwitterStatus> status = TwitterStatus.Update(token, tweet);

			return status;
			/*
			Console.WriteLine(status.Result.ToString());	
			*/
		}
	}
}