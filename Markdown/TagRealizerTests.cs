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
	public class TagRealizerTests
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
				new Token("Italics", tokensDescriptionsArray[1], TagType.Closing),
				new Token("Bold", tokensDescriptionsArray[2], TagType.Closing)
			};

			var output = "<strong><em>kek</em></strong>";

			var tagRealizer = new TagRealizer("Text");
			tagRealizer.RealizeTokens(input).ShouldBeEquivalentTo(output);
		}
	}
}
