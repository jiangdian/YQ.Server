using System.Configuration;

namespace YQ.Tool
{
	/// <summary>
	/// 配置文件读取
	/// </summary>
	public class ConfigHelper
	{
		private readonly static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

		/// <summary>
		/// 获取默认数据库链接字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetConnectionString(string Conn = "Conn")
		{
			try
			{
				return ConfigurationManager.ConnectionStrings[Conn].ToString();

			}
			catch
			{
				return string.Empty;
			}
		}

		public static string GetValue(string keyName)
		{
			try
			{
				return ConfigurationManager.AppSettings[keyName].ToString();
			}
			catch
			{
				return string.Empty;
			}
		}
		public static bool SetValue(string keyName, string content)
		{
			try
			{
				config.AppSettings.Settings[keyName].Value = content;
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
				return true;
			}
			catch
			{
				return false;
			}
		}
		public static bool AddValue(string keyName, string value)
		{
			try
			{
				config.AppSettings.Settings.Add(keyName, value);
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
				return true;
			}
			catch
			{
				return false;
			}
		}
		public static bool DelleteValue(string keyName)
		{
			try
			{
				config.AppSettings.Settings.Remove(keyName);
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
