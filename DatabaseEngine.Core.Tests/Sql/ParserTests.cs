
using DatabaseEngine.Core.Sql;
using FluentAssertions;
using SqlParser;

namespace DatabaseEngine.Core.Tests.Sql
{
    

    internal class ParserTests
    {
        internal static IEnumerable<TestCaseData> ParseShouldMatchExpectedTestCases()
        {
            yield return new TestCaseData("word", Node.Leaf(new Literal.IdentifierLiteral("word")));
            yield return new TestCaseData("1 + 1", Node.Infix(Op.Plus, new List<Node>
            {
                Node.Leaf(new Literal.NumericLiteral("1")),
                Node.Leaf(new Literal.NumericLiteral("1"))
            }));
        }

        [Test, TestCaseSource(nameof(ParseShouldMatchExpectedTestCases))]
        public void Parse_Should_MatchExpected(string input, Node expectedResult)
        {
            //Arrange
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);

            //Act

            //Assert
            parser.Parse().Should().BeEquivalentTo(expectedResult);
        }
    }
}
