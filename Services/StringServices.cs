using System.Collections.Generic;
using System.Text;

namespace Mangary.Services
{
	public static class StringServices
	{
		public static string Cleaner(string DirtyString)
		{
			HashSet<char> removeChars = new HashSet<char>("?&^$#@!()+-,:;<>’\'-_*");
			StringBuilder result = new StringBuilder(DirtyString.Length);
			foreach(char c in DirtyString) 
				if(!removeChars.Contains(c)) result.Append(c);
			return result.ToString();
		}
	}
}