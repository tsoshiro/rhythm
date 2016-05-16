using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// string型の拡張メソッドを管理するクラス
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// スネークケースをアッパーキャメル（パスカル）ケースに変換します
	/// 例）quoted_printable_encode → QuotedPrintableEncode
	/// </summary>
	public static string SnakeToUpperCamel(this string self)
	{
		if (string.IsNullOrEmpty (self)) 
		{
			return self;
		}

		return self
			.Split (new[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries)
			.Select (s => char.ToUpperInvariant (s [0]) + s.Substring (1, s.Length - 1))
			.Aggregate (string.Empty, (s1, s2) => s1 + s2);
	}

	/// <summary>
	/// スネークケースをローワーキャメル（キャメル）ケースに変換します
	/// </summary>
	public static string SnakeToLowerCamel(this string self)
	{
		if (string.IsNullOrEmpty (self)) 
		{
			return self;
		}
		return self.SnakeToUpperCamel ().Insert (0, char.ToLowerInvariant (self [0]).ToString ()).Remove (1, 1);
	}

	/// <summary>
	/// ローワーキャメルケースをスネークケースに変換します
	/// </summary>
	public static string LowerCamelToSnake(this string self)
	{
		if (string.IsNullOrEmpty (self))
			return self;
		string convertStr = "";
		int count = 0;
		foreach (char c in self.ToCharArray()) {
			if (char.IsUpper (c)) {
				if (count != 0) {
					convertStr += "_";
				}
				convertStr += char.ToLowerInvariant(c);
			} else {
				convertStr += c.ToString ();
			}
			count++;
		}
		return convertStr;
	}
}