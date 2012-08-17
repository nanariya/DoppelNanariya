using System;
using System.IO;
using System.Xml;

namespace Nana
{
	public class NanaTool
	{
		/// <summary>
		/// Saves the config.
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		/// <param name='objType'>
		/// Object type.
		/// </param>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		public void SaveConfig(Object obj, Type objType, String fileName)
		{
			try
			{
				System.Xml.Serialization.XmlSerializer serializer =
					new System.Xml.Serialization.XmlSerializer(objType);
				
				System.IO.FileStream fs = new System.IO.FileStream(fileName, FileMode.Create);
				
				serializer.Serialize(fs, obj);
				
				fs.Close();
			}
			catch(Exception e)
			{
				throw new Exception("（´A'）コンフィグ[" + fileName + "]に書けね", e);
			}
		}
		/// <summary>
		/// Loads the config.
		/// </summary>
		/// <returns>
		/// The config.
		/// </returns>
		/// <param name='objType'>
		/// Object type.
		/// </param>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		public Object LoadConfig(Type objType, String fileName)
		{			
			Object obj = null;
			try
			{
				System.Xml.Serialization.XmlSerializer serializer = 
					new System.Xml.Serialization.XmlSerializer(objType);
				
				FileStream fs = new FileStream(fileName, FileMode.Open);
				
				obj = serializer.Deserialize(fs);
				
				fs.Close();


			}
			catch(Exception e)	
			{
				throw new Exception("（´A'）コンフィグ[" + fileName + "]が読めねぇ・・・", e);
			}
			
			return obj;
		}
	}
}

