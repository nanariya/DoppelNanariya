using System;

namespace DoppelNanariya
{
	public class Setting
	{
		public Int32 StatusRefreshInterval { get; set; }

		public Setting ()
		{
		}

		public void LoadDefault()
		{
			this.StatusRefreshInterval = 5 * 60;	// 5 minutes
		}
	}
}

