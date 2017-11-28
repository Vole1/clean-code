using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class TokenAnalizerTests
	{

		private TokenDescription[] tokensDescriptionsArray;

		[SetUp]
		public void SetUp()
		{
			tokensDescriptionsArray = new[]
			{
				new TokenDescription("Text", null, null, null),
				new TokenDescription("Italics", "_", "<em>", null),
				new TokenDescription("Bold", "__", "<strong>", null)
			};
		}


		[Test]
		public void TokenAnalizer_ReturnValidOutput_ForDoubleTagging()
		{
			var input = new[]
			{
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "kek"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var output = new[]
			{
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "kek"),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing)
			};

			var tokenAnalyzer = new TokenAnalyzer("Text", tokensDescriptionsArray);
			tokenAnalyzer.Analyze(input).ShouldBeEquivalentTo(output);
		}

		[Test]
		public void TokenAnalizer_ReturnValidOutput_ForOrdinaryValidInput()
		{
			var input = new[]
			{
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "haha "),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "go "),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "aw_ay"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, " from"),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var output = new[]
			{
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "haha "),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "go "),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "aw_ay"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, " from"),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var tokenAnalyzer = new TokenAnalyzer("Text", tokensDescriptionsArray);
			tokenAnalyzer.Analyze(input).ShouldBeEquivalentTo(output);
		}

		[Test]
		public void TokenAnalizer_ReturnValidOutput_ForValidInputWithExtraClosingTag()
		{
			var input = new[]
			{
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "haha "),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "go "),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "aw_ay"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, " from"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var output = new[]
			{
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "haha "),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "go "),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "aw_ay"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, " from__"),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var tokenAnalyzer = new TokenAnalyzer("Text", tokensDescriptionsArray);
			tokenAnalyzer.Analyze(input).ShouldBeEquivalentTo(output);
		}


		[Test]
		[Ignore("Functionality isn't realized")]
		public void TokenAnalizer_ReturnValidOutput_ForValidInputWithExtraOpeningTag()
		{
			var input = new[]
			{
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "haha "),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "go "),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "aw_ay"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, " from"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var output = new[]
			{
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "haha "),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Opening),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Opening),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, "go __aw_ay"),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing),
				new Token("Text", tokensDescriptionsArray[0], TagType.Undefined, " from"),
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing)
			};

			var tokenAnalyzer = new TokenAnalyzer("Text", tokensDescriptionsArray);
			tokenAnalyzer.Analyze(input).ShouldBeEquivalentTo(output);
		}

	}
}
