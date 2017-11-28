using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class LexicalAnalizerTest
	{
		private TokenDescription[] tokensDescriptionsArray;

		[SetUp]
		public void SetUp()
		{ 
			tokensDescriptionsArray = new[]
			{
				new TokenDescription("Text", null, null, (previous, next)=> TagType.Undefined),

				new TokenDescription("Italics", "_", "<em>", TagTypeDeterminantForItalicsAndBold),

				new TokenDescription("Bold", "__", "<strong>", TagTypeDeterminantForItalicsAndBold)
			};
		}

		[Test]
		public void Analizer_ReturnValidOutput_ForValidInput1()
		{
			var inputTokens = new[]
			{
				new RawToken("Text", tokensDescriptionsArray[0], "haha "),
				new RawToken("Italics", tokensDescriptionsArray[1]),
				new RawToken("Text", tokensDescriptionsArray[0], "go "),
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Text", tokensDescriptionsArray[0], "aw"),
				new RawToken("Italics", tokensDescriptionsArray[1]),
				new RawToken("Text", tokensDescriptionsArray[0], "ay"),
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Text", tokensDescriptionsArray[0], " from"),
				new RawToken("Italics", tokensDescriptionsArray[1])
			};

			var output = new []
			{
				new Token("Text", tokensDescriptionsArray[0],  TagType.Undefined, "haha "),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "go "),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "aw_ay"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, " from"),

				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var lexicalAnalyser = new LexicalAnalyzer("Text", tokensDescriptionsArray[0]);
			lexicalAnalyser.Analyze(inputTokens).ShouldBeEquivalentTo(output);
		}

		[Test]
		public void Analizer_ReturnValidOutput_ForValidInput2()
		{
			var inputTokens = new[]
			{
				new RawToken("Text", tokensDescriptionsArray[0], "haha "),
				new RawToken("Italics", tokensDescriptionsArray[1]),
				new RawToken("Text", tokensDescriptionsArray[0], "go "),
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Text", tokensDescriptionsArray[0], "aw"),
				new RawToken("Italics", tokensDescriptionsArray[1]),
				new RawToken("Text", tokensDescriptionsArray[0], "ay"),
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Text", tokensDescriptionsArray[0], "from"),
				new RawToken("Italics", tokensDescriptionsArray[1])
			};

			var output = new[]
			{
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "haha "),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "go "),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "aw_ay__from"),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var lexicalAnalyzer = new LexicalAnalyzer("Text", tokensDescriptionsArray[0]);
			lexicalAnalyzer.Analyze(inputTokens).ShouldBeEquivalentTo(output);
		}

		[Test]
		public void Analizer_ReturnValidOutput_ForKek()
		{
			var inputTokens = new[]
			{
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Italics", tokensDescriptionsArray[1]),
				new RawToken("Text", tokensDescriptionsArray[0], "kek"),
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Italics", tokensDescriptionsArray[1])
			};

			var output = new[]
			{
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "kek"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var lexicalAnalyzer = new LexicalAnalyzer("Text", tokensDescriptionsArray[0]);
			lexicalAnalyzer.Analyze(inputTokens).ShouldBeEquivalentTo(output);
		}

		private TagType TagTypeDeterminantForItalicsAndBold(char? previousSymbol, char? nextSymbol)
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

			if (char.IsLetterOrDigit((char) previousSymbol) && !char.IsLetterOrDigit((char) nextSymbol))
				return TagType.Closing;
			return TagType.Undefined;
		}
	}
}
