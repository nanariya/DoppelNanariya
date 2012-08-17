using System;
using System.Net;
using System.Timers;
using System.Collections.Generic;
using Twitterizer;

namespace DoppelNanariya
{
	class MainClass
	{
		private static string SettingFileName { get {return "Setting.xml";}}

		public static void Main (string[] args)
		{
			Array.Reverse(args);
			
			String parameter = null;

			UserConfig userConfig = NanaTwitter.LoadConfig();
			NanaTwitter.Config = NanaTwitter.LoadConfig();
			
			foreach (String arg in args)
			{
				if(arg.Length < 2)
				{
					parameter = arg;
					break;
				}
				
				if(arg[0] == '-')
				{
					switch(arg[1])
					{
					case 'n':
						Console.WriteLine("新規登録します");
						//RegisterApplication();
						return;
					case 'h':
						Console.WriteLine("ヘルプ");
						return;
						
					case 't':
						NanaTwitter.SendTweet(parameter);
						break;
					case 'd':
						DaemonProc(userConfig);
						break;
					default:
						break;
					}
				}else{
					parameter = arg;
				}
			}	

			//通常処理
			Proc(userConfig);

		}
		
		private static void Proc(UserConfig userConfig)
		{
			while(true)
			{
				Console.WriteLine("Waiting for request...");
				String input = Console.ReadLine();

				switch(input)
				{
				case "help":
					Console.WriteLine("\r\n------------------------\r\n" +
						"help\tこのヘルプ\r\n" +
						"show\tホームタイムラインの表示\r\n" +
						"exit\tアプリの終了\r\n");
					break;
				case "show":
					TwitterResponse<TwitterStatusCollection> homeTimeLine = NanaTwitter.ShowHomeTimeline();
					List<TwitterStatus> statuses = new List<TwitterStatus>();

					foreach(TwitterStatus status in homeTimeLine.ResponseObject)
					{
						statuses.Add(status);
						//Console.WriteLine(status.Text);
					}
					statuses.Reverse();
					statuses.ForEach(delegate(TwitterStatus obj){Console.WriteLine(obj.Text);});

					break;
				case "send":
					Console.WriteLine("ツイートする文字打ってね\r\n");
					NanaTwitter.SendTweet(Console.ReadLine());
					break;
				case "test":
					Nana.NanaTool nana = new Nana.NanaTool();
					UserConfig2 conf = new UserConfig2();
					conf.LastId = 10;
					nana.SaveConfig(conf, typeof(UserConfig2), "test.txt");

					UserConfig2 conf2 = (UserConfig2)nana.LoadConfig(typeof(UserConfig2), "test.txt");
					Console.WriteLine(conf2.LastId.ToString());

					break;
				case "q":
				case "exit":
					Environment.Exit(0);
					break;
				default:
					break;
				}
				
			}
		}

		private static void DaemonProc(UserConfig userConfig)
		{
			//擬似デーモンモード Ctrl+C で強制終了できるっぽいので特に何もしない


			Setting setting = null;

			try
			{
				Nana.NanaTool nana = new Nana.NanaTool();
				setting = (Setting)nana.LoadConfig(typeof(Setting), SettingFileName);
			}
			catch(Exception)
			{
				setting = new Setting();
				setting.LoadDefault();
			}

			Timer timer = new Timer();
			timer.Elapsed += new ElapsedEventHandler(EveryMinutes);
			timer.Interval = setting.StatusRefreshInterval;
			timer.AutoReset = true;
			timer.Enabled = true;

			Console.WriteLine("バックグラウンドでのTweet機能が有効です");

			try
			{
				Nana.NanaTool nana = new Nana.NanaTool();
				nana.SaveConfig(setting, typeof(Setting), SettingFileName);
			}
			catch(Exception)
			{/* あきらめろん */}

		}

		public static void EverySeconds(object sender, ElapsedEventArgs e)
		{
			//毎秒の処理
		}	

		public static void EveryMinutes(object sender, ElapsedEventArgs e)
		{
			//毎分の処理

		}

	}
}
