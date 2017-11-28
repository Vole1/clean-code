using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public static class Initializer
	{
		public static TokenDescription[] GetTokenDescriptions()
		{
			return new []
			{
				new TokenDescription("Text", null, null, (previous, next)=> TagType.Undefined),
				new TokenDescription("Italics", "_", "<em>", TagTypeDeterminantForItalicsAndBold),
				new TokenDescription("Strong", "__", "<strong>", TagTypeDeterminantForItalicsAndBold)
			};
		}

		private static TagType TagTypeDeterminantForItalicsAndBold(char? previousSymbol, char? nextSymbol)
		{
			if (previousSymbol == '\\')
				return TagType.Undefined;


			if (previousSymbol == null || previousSymbol == ' ')
			{
				if (nextSymbol == null)
					return TagType.Undefined;
				if (nextSymbol != ' ')
					return TagType.Opening;

			}

			if (nextSymbol == null || nextSymbol == ' ')
			{
				if (previousSymbol == null)
					return TagType.Undefined;
				if (previousSymbol != ' ')
					return TagType.Closing;
			}

			if (!char.IsLetterOrDigit((char)previousSymbol) && char.IsLetterOrDigit((char)nextSymbol))
				return TagType.Opening;

			if (char.IsLetterOrDigit((char)previousSymbol) && !char.IsLetterOrDigit((char)nextSymbol))
				return TagType.Closing;
			return TagType.Undefined;
		}
	}
}
