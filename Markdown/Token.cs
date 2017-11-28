using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class Token
	{
		public string Type { get; }
		public TagType TagType { get; }
		public string Value { get; private set; }
		public TokenDescription Description { get; }

		public Token(string type, TokenDescription description, TagType tagType, string value = null)
		{
			Type = type;
			Description = description;
			TagType = tagType;
			Value = value;
		}

		public void AmplifyValue(string addition)
		{
			Value += addition;
		}

	}
}
