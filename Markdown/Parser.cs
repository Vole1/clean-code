using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class Parser
	{
		private string TextType { get; set; }
		private TokenDescription[] TokensDescriptions { get; set; }

		public Parser(string textType, TokenDescription[] tokenDescriptions)
		{
			TextType = textType;
			TokensDescriptions = tokenDescriptions;
		}

		public IEnumerable<RawToken> Parse(string input)
		{
			var patternsLenghts = TokensDescriptions.Where(td => !string.IsNullOrEmpty(td.pattern)).Select(td => td.pattern.Length).OrderByDescending(x => x).ToArray();

			var previousTolkenStart = 0;
			for (int currentIndexOnInput = 0; currentIndexOnInput < input.Length; currentIndexOnInput++)
			{
				foreach (var currentPattersLength in patternsLenghts)
				{
					if (currentIndexOnInput + currentPattersLength > input.Length)
						continue;

					var matchTokenDescription = TokensDescriptions.FirstOrDefault(td => td.pattern == input.Substring(currentIndexOnInput, currentPattersLength));

					if (matchTokenDescription != null)
					{
						var tokenValue = input.Substring(previousTolkenStart, currentIndexOnInput - previousTolkenStart);
						if (previousTolkenStart < currentIndexOnInput)
							yield return new RawToken(TextType, TokensDescriptions.Single(td=>td.Type==TextType), tokenValue);
						yield return new RawToken(matchTokenDescription.Type, matchTokenDescription);
						previousTolkenStart = currentIndexOnInput + currentPattersLength;
						currentIndexOnInput += currentPattersLength-1;
						break;
					}
				}
			}
		}

	}
}

