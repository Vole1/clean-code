using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class TagRealizer
	{
		private string TextType { get; }


		public TagRealizer(string textType)
		{
			TextType = textType;
		}

		public string RealizeTokens(Token[] tokens)
		{
			var result = new StringBuilder();
			foreach (var currentToken in tokens)
			{
				if (currentToken.Type == TextType)
				{
					result.Append(currentToken.Value);
					continue;
				}
				var token = currentToken;
				var currentTd = token.Description;
				var valueToAppend = currentToken.TagType == TagType.Opening ? currentTd.OpeningTag : currentTd.ClosingTag;
				result.Append(valueToAppend);
			}
			return result.ToString();
		}
	}
}
