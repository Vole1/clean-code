using System.Diagnostics;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
			string textType = "Text";
			var tokenDescriptions = Initializer.GetTokenDescriptions();
			var textTokenTypeDescription = tokenDescriptions.Single(td => td.Type == textType);

			var parser = new Parser(textType, tokenDescriptions);
			var rawTokens = parser.Parse(markdown).ToArray();

			var lexicalAnalyzer = new LexicalAnalyzer(textType, textTokenTypeDescription);
			var parsedTokens = lexicalAnalyzer.Analyze(rawTokens);

			var tokenAnalyzer = new TokenAnalyzer(textType, tokenDescriptions);
			var finalTokens = tokenAnalyzer.Analyze(parsedTokens);

			var tagRealizer = new TagRealizer(textType);
			return tagRealizer.RealizeTokens(finalTokens);

		}
	}

	[TestFixture]
	public class MdSpeedTests
	{
		[TestCase(100000, 500)]
		[TestCase(1000000, 1000)]
		public void MdSpeedTest(int n, int milliseconds)
		{
			var inputStringBuilder = new StringBuilder();
			for (int i = 0; i < n; i++)
			{
				inputStringBuilder.Append("_'__" + i + "__'_");
			}
			var stringInput = inputStringBuilder.ToString();

			var stopWatch = new Stopwatch();
			stopWatch.Start();
			var md = new Md();
			md.RenderToHtml(stringInput);
			stopWatch.Stop();
			stopWatch.Elapsed.Milliseconds.Should().BeLessOrEqualTo(milliseconds);
		}
	}
}