using System;

namespace DoppelNanariya
{
	public class UserConfig
	{
		public string RequestToken { get; set; }
		public string RequestTokenSecret { get; set; }
		public string AccessToken { get; set; }
		public string AccessTokenSecret { get; set; }
		
		public UserConfig ()
		{
		}
	}
}