using System.Collections;
using System.Reflection;

namespace DatabaseEngine.Core.Sql
{
    public interface ILexer
    {
        IEnumerable<IToken> GetTokens();
    }
    public class Lexer : ILexer
    {
        private static readonly Dictionary<string, System.Type> Keywords = Assembly.GetExecutingAssembly().GetTypes().
            Where(t => !t.IsAbstract && typeof(IToken).IsAssignableFrom(t))
            .Select(t => new
            {
                Attribute = t.GetCustomAttribute<KeywordAttribute>(),
                Type = t,
            }).Where(t => t.Attribute != null)
            .ToDictionary(x => x.Attribute!.Value, x => x.Type);

        private string _input;
        private int _position;
        private readonly int _length;

        public Lexer(string input)
        {
            _input = input;
            _length = input.Length;
        }

        public IEnumerable<IToken> GetTokens()
        {
            _position = -1;
            while (HasNextSymbol())
            {
                char? next = GetNextAndIncrement(true);

                if (!next.HasValue)
                    yield break;

                if (char.IsLetter(next!.Value))
                    yield return ReadWord();
                else if (char.IsDigit(next.Value))
                    yield return ReadNumber();
                else
                    yield return ReadSymbol();
            }
        }

        private IToken CreateToken(System.Type type)
        {
            return (IToken)Activator.CreateInstance(type)!;
        }

        private IToken ReadWord()
        {
            int start = _position;
            IncrementWhile(NextSymbolCanBeInWord);

            string word = _input.Substring(start, _position - start + 1).ToLower();
            if (word.Contains('.'))
            {
                var splitResult = word.Split('.');
                if (splitResult.Length == 1)
                    return new Identifier { FirstName = splitResult[0] };
                if (splitResult.Length == 2)
                    return new Identifier { FirstName = splitResult[0], SecondName = splitResult[1] };
                throw new Exception("Identifier can contain only one dot.");
            }
            return Keywords.ContainsKey(word) ? CreateToken(Keywords[word]) : new Identifier { FirstName = word};
        }
        private NumericLiteral ReadNumber()
        {
            int start = _position;
            IncrementWhile(NextSymbolCanBeInNumber);

            string number = _input.Substring(start, _position - start + 1);
            return new NumericLiteral { Value = number };
        }
        private IToken ReadSymbol()
        {
            char symbol = _input[_position];
            return symbol switch
            {
                ',' => new Comma(),
                ';' => new Semicolon(),
                '*' => new Asterisk(),
                '=' => new IsEqual(),
                '(' => new OpenParen(),
                ')' => new CloseParen(),
                '+' => new Plus(),
                '%' => new Percent(),
                '|' => new Concat(),
                '<' => MayBeLonger(new LessThan()),
                '-' => MayBeLonger(new Minus()),
                '>' => MayBeLonger(new GreaterThan()),
                '/' => MayBeLonger(new Slash()),
                '\'' => MayBeLonger(new SingleQuote()),
                _ => throw new Exception($"Unknown symbol: {symbol}")
            };
        }
        private IToken MayBeLonger(IToken firstToken)
        {
            if (!HasNextSymbol()) return firstToken;

            char secondSymbol = _input[_position + 1];
            _position++;

            if (firstToken is LessThan && secondSymbol == '=')
                return new LessThanOrEquals();
            if (firstToken is LessThan && secondSymbol == '>')
                return new NotEquals();
            if (firstToken is GreaterThan && secondSymbol == '=')
                return new GreaterThanOrEquals();
            if (firstToken is Minus && secondSymbol == '-')
                return ReadSingleLineComment();
            if (firstToken is SingleQuote)
                return ReadStringLiteral();

            return --_position == 0 ? firstToken : firstToken;
        }
        private StringLiteral ReadStringLiteral()
        {
            int start = _position;
            IncrementWhile(NextSymbolIsNotSingleQuote);

            if (HasNextSymbol())
                _position++;

            string word = _input.Substring(start, _position - start).ToLower();

            return new StringLiteral { Value = word };
        }
        private SingleLineComment ReadSingleLineComment()
        {
            int start = _position + 1;
            IncrementWhile(NextSymbolIsNotNewLine);

            if (HasNextSymbol())
                _position++;

            var length = _position >= start ? _position - start : 0;
            string word = _input.Substring(start, length).ToLower();

            return new SingleLineComment
            {
                Value = word
            };
        }
        private char? GetNextAndIncrement(bool skipWhiteSpaces = false)
        {
            while (HasNextSymbol())
            {
                char next = _input[++_position];
                if (skipWhiteSpaces && char.IsWhiteSpace(next))
                    continue;

                return next;
            }

            return null;
        }
        private void IncrementWhile(Func<bool> condition)
        {
            while (condition())
            {
                _position++;
            }
        }
        private bool HasNextSymbol() => _position < _length - 1;
        private bool NextSymbolCanBeInWord() => HasNextSymbol() && (char.IsLetterOrDigit(_input[_position + 1]) || _input[_position + 1] == '_' || _input[_position + 1] == '.');
        private bool NextSymbolCanBeInNumber() => HasNextSymbol() && char.IsDigit(_input[_position + 1]);
        private bool NextSymbolIsNotSingleQuote() => HasNextSymbol() && _input[_position + 1] != '\'';
        private bool NextSymbolIsNotNewLine() => HasNextSymbol() && _input[_position + 1] != '\n';
    }
}
