using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;

namespace Markdown
{
	[TestFixture]
	class ParserTests
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
		public void Parser_ShouldReturnValidParseResult_ForOrdinaryValidInput()
		{
			var input = "haha _go __away__from_";
			var output = new[]
			{
				new RawToken("Text", tokensDescriptionsArray[0], "haha "),
				new RawToken("Italics", tokensDescriptionsArray[1]),
				new RawToken("Text", tokensDescriptionsArray[0], "go "),
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Text", tokensDescriptionsArray[0], "away"),
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Text", tokensDescriptionsArray[0], "from"),
				new RawToken("Italics", tokensDescriptionsArray[1])
			};

			var parser = new Parser("Text", tokensDescriptionsArray);
			parser.Parse(input).ShouldBeEquivalentTo(output);
		}

		[Test]
		public void Parser_ReturnValidOutput_ForDoubleTagging()
		{
			var input = "___kek___";
			var output = new[]
			{
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Italics", tokensDescriptionsArray[1]),
				new RawToken("Text", tokensDescriptionsArray[0], "kek"),
				new RawToken("Bold", tokensDescriptionsArray[2]),
				new RawToken("Italics", tokensDescriptionsArray[1])
			};

			var parser = new Parser("Text", tokensDescriptionsArray);
			parser.Parse(input).ShouldBeEquivalentTo(output);
		}

	}
}
