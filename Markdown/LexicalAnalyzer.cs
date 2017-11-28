using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class LexicalAnalyzer
	{
		private string TextType { get; }
		private TokenDescription TextTokenDescription { get; }

		public LexicalAnalyzer(string textType, TokenDescription textTokenDescription)
		{
			TextType = textType;
			TextTokenDescription = textTokenDescription;
		}

		public Token[] Analyze(RawToken[] parsedRawTokens)
		{
			var result = new List<Token>();
			RawToken previousParsedToken = null;
			StringBuilder textTokenValueToAdd = null;
			for (int i = 0; i < parsedRawTokens.Length; i++)
			{
				if (i > 0)
					previousParsedToken = parsedRawTokens[i - 1];
				var currentParsedToken = parsedRawTokens[i];
				var nextParsedToken = i + 1 < parsedRawTokens.Length ? parsedRawTokens[i + 1] : null;


				var currentTokentagType = GetTagType(previousParsedToken, currentParsedToken, nextParsedToken);
				if (currentTokentagType == TagType.Undefined)
				{
					if (textTokenValueToAdd == null)
						textTokenValueToAdd = new StringBuilder(GetTokenValue(currentParsedToken));
					else
						textTokenValueToAdd.Append(GetTokenValue(currentParsedToken));
					continue;
				}

				if (textTokenValueToAdd != null)
				{
					result.Add(new Token(TextType, TextTokenDescription, TagType.Undefined, textTokenValueToAdd.ToString()));
					textTokenValueToAdd = null;
				}
				result.Add(new Token(currentParsedToken.Type, currentParsedToken.Description, currentTokentagType));

			}

			return result.ToArray();
		}

		private TagType GetTagType(RawToken previousToken, RawToken currentToken, RawToken nextToken)
		{
			if (currentToken.Type == TextType)
				return TagType.Undefined;
			var previousSymbol = GetTokenProperChar(previousToken, str => str[str.Length-1]);
			var nextSymbol = GetTokenProperChar(nextToken, str => str[0]);
			var tagTypeDeterminant = currentToken.Description.TagTypeDeterminant;
			return tagTypeDeterminant(previousSymbol, nextSymbol);
		}

		private char? GetTokenProperChar(RawToken token, Func<string, char> selectProperChar)
		{
			if (token == null)
				return null;
			var value = GetTokenValue(token);
			return
				selectProperChar(value);
		}

		private string GetTokenValue(RawToken token)
		{
			if (token.Type == TextType)
				return token.Value;
			
			return token.Description.pattern;
		}
	}
}
