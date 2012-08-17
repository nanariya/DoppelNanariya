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
						"license\tライセンスの表示（外部プログラムも含む）\r\n" +
						"exit\tアプリの終了\r\n" +
						"\r\n");
					break;
				case "license":
					Console.WriteLine(
						"このプログラムについて\r\n" +
						"========\r\n" +
						"DoppelNanariya : Copyright (c) 2012, Nanariya\r\n" +
						"----\r\n" +
						@"Copyright (c) 2012, Nanariya
All rights reserved.
	
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
	
- Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

- Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
	
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ""AS IS"" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE." +
						"\r\n\r\n" +
						"外部プログラムについて\r\n" +
						"========\r\n" +
						"Twitterizer2(Twitterizer2.dll) : Copyright (c) 2010, Patrick \"Ricky\" Smith All rights reserved.\r\n" +
						"----\r\n" +
						@"Copyright (c) 2010, Patrick ""Ricky"" Smith
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

 - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 
 - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in 
   the documentation and/or other materials provided with the distribution.
   
 - Neither the name Twitterizer nor the names of its contributors may be used to endorse or promote products derived from 
   this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ""AS IS"" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE." + 
					"\r\n\r\n" +
					"========\r\n" +
					"Json.NET(Newtonsoft.Json.dll) : Copyright (c) 2007 James Newton-King\r\n" +
					"----\r\n" +
					@"Copyright (c) 2007 James Newton-King

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE." +
					"\r\n\r\n"
						);
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
