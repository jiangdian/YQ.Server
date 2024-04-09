using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace YuanQi.Tool
{
	/// <summary>
	/// 枚举拓展方法
	/// </summary>
	public static class EnumExtension
	{
		/// <summary>
		/// 获取枚举变量值的 Description 属性
		/// </summary>
		/// <param name="obj">枚举变量</param>
		/// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
		public static string GetDescription(this System.Enum obj)
		{
			return GetDescription(obj, false);
		}

		/// <summary>
		/// 获取枚举变量值的 Description 属性
		/// </summary>
		/// <param name="obj">枚举变量</param>
		/// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
		/// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
		public static string GetDescription(this System.Enum obj, bool isTop)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			try
			{
				Type enumType = obj.GetType();
				DescriptionAttribute dna = null!;
				if (isTop)
				{
					dna = (DescriptionAttribute)Attribute.GetCustomAttribute(enumType, typeof(DescriptionAttribute))!;
				}
				else
				{
					FieldInfo fi = enumType.GetField(System.Enum.GetName(enumType, obj)!)!;
					dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
					   fi, typeof(DescriptionAttribute))!;
				}
				if ((dna != null)
					&& (string.IsNullOrEmpty(dna.Description) == false))
				{
					return dna.Description;
				}
			}
			catch
			{
			}
			return obj.ToString();
		}

		/// <summary>
		/// 获取枚举变量值的 Description 属性
		/// </summary>
		/// <param name="obj">枚举变量</param>
		/// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
		public static string GetDescription<TEnum>(this TEnum obj)
		{
			return GetDescription(obj, false);
		}

		/// <summary>
		/// 获取枚举变量值的 Description 属性
		/// </summary>
		/// <param name="obj">枚举变量</param>
		/// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
		/// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
		public static string GetDescription<TEnum>(this TEnum obj, bool isTop)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			try
			{
				Type enumType = obj.GetType();
				DescriptionAttribute dna = null!;
				if (isTop)
				{
					dna = (DescriptionAttribute)Attribute.GetCustomAttribute(enumType, typeof(DescriptionAttribute))!;
				}
				else
				{
					FieldInfo fi = enumType.GetField(System.Enum.GetName(enumType, obj)!)!;
					dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
					   fi, typeof(DescriptionAttribute))!;
				}
				if ((dna != null)
					&& (string.IsNullOrEmpty(dna.Description) == false))
				{
					return dna.Description;
				}
			}
			catch
			{
			}
			return obj.ToString()!;
		}

		/// <summary>
		/// 获取字段Description
		/// </summary>
		/// <param name="fieldInfo">FieldInfo</param>
		/// <returns>DescriptionAttribute[] </returns>
		public static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
		{
			if (fieldInfo != null)
			{
				return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
			}
			return null!;
		}

		/// <summary>
		/// 根据Description获取枚举
		/// 说明：
		/// 单元测试-->通过
		/// </summary>
		/// <typeparam name="T">枚举类型</typeparam>
		/// <param name="description">枚举描述</param>
		/// <returns>枚举</returns>
		public static T GetEnumName<T>(this string description)
		{
			Type _type = typeof(T);
			foreach (FieldInfo field in _type.GetFields())
			{
				DescriptionAttribute[] _curDesc = field.GetDescriptAttr();
				if (_curDesc != null && _curDesc.Length > 0)
				{
					if (_curDesc[0].Description == description)
						return (T)field.GetValue(null)!;
				}
				else
				{
					if (field.Name == description)
						return (T)field.GetValue(null)!;
				}
			}
			throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.错误枚举{1}", description, _type.Name), "Description");
		}

		/// <summary>
		/// 将枚举转换为ArrayList
		/// 说明：
		/// 若不是枚举类型，则返回NULL
		/// 单元测试-->通过
		/// </summary>
		/// <param name="type">枚举类型</param>
		/// <returns>ArrayList</returns>
		public static ArrayList ToArrayList(this Type type)
		{
			if (type.IsEnum)
			{
				ArrayList _array = new ArrayList();
				Array _enumValues = System.Enum.GetValues(type);
				foreach (System.Enum value in _enumValues)
				{
					_array.Add(new KeyValuePair<System.Enum, string>(value, GetDescription(value)));
				}
				return _array;
			}
			return default!;
		}

		/// <summary>
		/// 将枚举转换为Dictionary
		/// 说明：
		/// 若不是枚举类型，则返回NULL
		/// 单元测试-->通过
		/// </summary>
		/// <param name="type">枚举类型</param>
		/// <returns>ArrayList</returns>
		public static Dictionary<System.Enum, string> ToDictionary(this Type type)
		{
			if (type.IsEnum)
			{
				Dictionary<System.Enum, string> _array = new Dictionary<System.Enum, string>();
				Array _enumValues = System.Enum.GetValues(type);
				foreach (System.Enum value in _enumValues)
				{
					_array.Add(value, GetDescription(value));
				}
				return _array;
			}
			return default!;
		}
	}
}