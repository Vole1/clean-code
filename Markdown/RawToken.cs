using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class RawToken
	{
		public string Type { get; }
		public string Value { get; }
		public TokenDescription Description { get; }

		public RawToken(string type, TokenDescription description, string value=null)
		{
			Type = type;
			Value = value;
			Description = description;
		}

	}
}
