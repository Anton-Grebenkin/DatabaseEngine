using DatabaseEngine.Core.Sql;
using FluentAssertions;

namespace DatabaseEngine.Core.Tests.Sql
{
    internal class LexerTests
    {
        internal static IEnumerable<TestCaseData> GetTokensShouldMatchExpectedTestCases()
        {
            yield return new TestCaseData("", new List<IToken>() { });
            yield return new TestCaseData("word", new List<IToken>() { new Identifier { FirstName = "word"} });
            yield return new TestCaseData("   word   ", new List<IToken>() { new Identifier { FirstName = "word" } });
            yield return new TestCaseData(
                @"select 
                    * 
                from TableName", 
                new List<IToken>() 
                { 
                    new Select(),
                    new Asterisk(),
                    new From (),
                    new Identifier{ FirstName = "tablename" }
                }
            );
            yield return new TestCaseData("insert into table TableName(Id, Name) values(1, 'Name')", new List<IToken>()
            {
                new Insert(),
                new Into(),
                new Table(),
                new Identifier { FirstName = "tablename" },
                new OpenParen(),
                new Identifier { FirstName = "id" },
                new Comma(),
                new Identifier { FirstName = "name" },
                new CloseParen(),
                new Values(),
                new OpenParen(),
                new NumericLiteral { Value = "1" },
                new Comma(),
                new StringLiteral{ Value = "name" },
                new CloseParen()
            });
            yield return new TestCaseData("''", new List<IToken>() { new StringLiteral { Value = "" } });
            yield return new TestCaseData("'", new List<IToken>() { new SingleQuote() });
            yield return new TestCaseData("--comment\nselect", new List<IToken>() { new SingleLineComment { Value = "comment"}, new Select() });
            yield return new TestCaseData("--", new List<IToken>() { new SingleLineComment { Value = ""} });
        }

        internal static IEnumerable<TestCaseData> GetTokensShouldThrowExceptionTestCases()
        {
            yield return new TestCaseData("#", new Exception("Unknown symbol: #"));
        }

        [Test, TestCaseSource(nameof(GetTokensShouldMatchExpectedTestCases))]
        public void GetTokens_Should_MatchExpected(string input, IEnumerable<IToken> expectedResult)
        {
            //Arrange
            var lexer = new Lexer(input);

            //Act

            //Assert
            lexer.GetTokens().Should().Equal(expectedResult);
        }

        [Test, TestCaseSource(nameof(GetTokensShouldThrowExceptionTestCases))]
        public void GetTokens_Should_ThrowException(string input, Exception exception)
        {
            //Arrange
            var lexer = new Lexer(input);

            //Act
            var tokens = () => lexer.GetTokens().ToList();

            //Assert
            tokens.Should().Throw<Exception>().Which.GetType().Should().Be(exception.GetType());
            tokens.Should().Throw<Exception>().Which.Message.Should().Be(exception.Message);
        }
    }
}
