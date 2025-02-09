using DatabaseEngine.Core.Sql;

namespace SqlParser
{
    public abstract class Literal
    {
        public class NumericLiteral : Literal
        {
            public int Value { get; }
            public NumericLiteral(string value) => Value = int.Parse(value);
        }

        public class StringLiteral : Literal
        {
            public string Value { get; }
            public StringLiteral(string value) => Value = value;
        }

        public class IdentifierLiteral : Literal
        {
            public string FirstName { get; }
            public string? SecondName { get; }

            public IdentifierLiteral(string firstName, string? secondName = null)
            {
                FirstName = firstName;
                SecondName = secondName;
            }
        }

        public class FloatLiteral : Literal
        {
            public float Value { get; }
            public FloatLiteral(string value) => Value = float.Parse(value);
        }

        public class BoolLiteral : Literal
        {
            public bool Value { get; }
            public BoolLiteral(string value) => Value = bool.Parse(value);
        }
    }
    public enum NodeType { Leaf, Prefix, Infix }
    public enum Op
    {
        Not,
        DropTable,
        CreateTable,
        ColumnDefinition,
        Select,
        From,
        Where,
        InsertInto,
        ColumnList,
        Values,
        And,
        Or,
        Plus,
        Minus,
        Multiply,
        Divide,
        Equals,
        NotEquals,
        LessThan,
        GreaterThan,
        LessThanOrEquals,
        GreaterThanOrEquals,
        Comma,
        CloseParen
    }

    public class Node
    {
        public NodeType Type { get; set; }
        public Op? Op { get; set; }
        public List<Node>? Children { get; set; }
        public object? Value { get; set; }

        public static Node Leaf(object value) => new Node { Type = NodeType.Leaf, Value = value };
        public static Node Prefix(Op op, List<Node> children) => new Node { Type = NodeType.Prefix, Op = op, Children = children };
        public static Node Infix(Op op, List<Node> children) => new Node { Type = NodeType.Infix, Op = op, Children = children };
    }
    public interface IParser
    {
        Node Parse();
    }
    public class Parser : IParser 
    {
        private static readonly Dictionary<Type, Op> TokenToOpMapping = new Dictionary<Type, Op>
        {
            { typeof(And), Op.And },
            { typeof(Or), Op.Or },
            { typeof(Plus), Op.Plus },
            { typeof(Minus), Op.Minus },
            { typeof(Asterisk), Op.Multiply },
            { typeof(Slash), Op.Divide },
            { typeof(IsEqual), Op.Equals },
            { typeof(NotEquals), Op.NotEquals },
            { typeof(LessThan), Op.LessThan },
            { typeof(GreaterThan), Op.GreaterThan },
            { typeof(LessThanOrEquals), Op.LessThanOrEquals },
            { typeof(GreaterThanOrEquals), Op.GreaterThanOrEquals },
            { typeof(Comma), Op.Comma },
            { typeof(CloseParen), Op.CloseParen }
        };

        private readonly Queue<IToken> _tokens;
        public Parser(ILexer lexer) => _tokens = new Queue<IToken>(lexer.GetTokens());

        public Node Parse() => ParseBp(0);

        private Node ParseBp(int minBp)
        {
            Node lhs;

            var token = _tokens.NextValue();
            if (token == null)
            {
                throw new Exception("Unexpected end of input.");
            }

            if (token is NumericLiteral nl)
            {
                lhs = Node.Leaf(new Literal.NumericLiteral(nl.Value));
            }
            else if (token is StringLiteral sl)
            {
                lhs = Node.Leaf(new Literal.StringLiteral(sl.Value));
            }
            else if (token is Identifier i)
            {
                lhs = Node.Leaf(new Literal.IdentifierLiteral(i.FirstName, i.SecondName));
            }
            else if (token is Not)
            {
                var (_, rightBp) = PrefixOperatorBp(Op.Not);
                var rhs = ParseBp(rightBp);
                lhs = Node.Prefix(Op.Not, new List<Node> { rhs });
            }
            else if (token is OpenParen)
            {
                lhs = ParseBp(0);
                if (_tokens.NextValue() is not CloseParen)
                {
                    throw new Exception("Expected closing parenthesis.");
                }
            }
            else if (token is Select)
            {
                lhs = ParseSelect(minBp);
            }
            else if (token is Create)
            {
                lhs = ParseCreate(minBp);
            }
            else if (token is Drop)
            {
                lhs = ParseDrop(minBp);
            }
            else if (token is Insert)
            {
                lhs = ParseInsert(minBp);
            }
            else
            {
                throw new Exception($"Unexpected token: {token.GetType()}");
            }

            while (true)
            {
                var peek = _tokens.PeekValue();
                if (peek == null)
                    break;

                var op = TokenToOp(peek);
                if (PostfixOperatorBp(op) != null)
                {
                    continue;
                }

                var infixBp = InfixOperatorBp(op);
                if (infixBp != null)
                {
                    var (leftBp, rightBp) = infixBp.Value;
                    if (leftBp < minBp)
                        break;

                    _tokens.NextValue();
                    var rhs = ParseBp(rightBp);
                    lhs = Node.Infix(op, new List<Node> { lhs, rhs });
                    continue;
                }

                break;
            }

            return lhs;
        }

        private Node ParseDrop(int minBp)
        {
            var token = _tokens.NextValue();
            if (token is Table)
            {
                return ParseDropTable(minBp);
            }

            throw new Exception("Unexpected token in DROP statement.");
        }

        private Node ParseDropTable(int minBp)
        {
            var token = _tokens.NextValue();
            if (token is Identifier i)
            {
                var identifier = Node.Leaf(new Literal.IdentifierLiteral(i.FirstName, i.SecondName));
                return Node.Prefix(Op.DropTable, new List<Node> { identifier });
            }

            throw new Exception("Expected identifier in DROP TABLE.");
        }

        private Node ParseCreate(int minBp)
        {
            var token = _tokens.NextValue();
            if (token is Table)
            {
                return ParseCreateTable(minBp);
            }

            throw new Exception("Unexpected token in CREATE statement.");
        }

        private Node ParseCreateTable(int minBp)
        {
            var token = _tokens.NextValue();
            if (token is not Identifier i)
                throw new Exception("Expected table name in CREATE TABLE.");

            var tableName = Node.Leaf(new Literal.IdentifierLiteral(i.FirstName, i.SecondName));
            var columns = new List<Node>();

            if (_tokens.NextValue() is not OpenParen)
                throw new Exception("Expected opening parenthesis in CREATE TABLE.");

            while (true)
            {
                var columnToken = _tokens.NextValue();
                if (columnToken is Identifier)
                {
                    var columnName = Node.Leaf(new Literal.IdentifierLiteral(i.FirstName, i.SecondName));
                    if (_tokens.NextValue() is Int)
                    {
                        columns.Add(Node.Infix(Op.ColumnDefinition, new List<Node> { columnName, Node.Leaf("INT") }));
                    }
                    else
                    {
                        throw new Exception("Expected type for column.");
                    }

                    var next = _tokens.Peek();
                    if (next != null && next is Comma)
                    {
                        _tokens.NextValue();
                        continue;
                    }

                    if (next != null && next is CloseParen)
                    {
                        _tokens.NextValue();
                        break;
                    }
                }

                if (columnToken is CloseParen)
                    break;
            }

            return Node.Prefix(Op.CreateTable, new List<Node> { tableName, Node.Infix(Op.Comma, columns) });
        }

        private Node ParseSelect(int minBp) 
        {
            throw new NotImplementedException();
        }
        private Node ParseInsert(int minBp) 
        { 
            throw new NotImplementedException();
        }

        private static (int, int)? InfixOperatorBp(Op op) =>
            op switch
            {
                Op.Or => (1, 2),
                Op.And => (3, 4),
                Op.Equals or Op.NotEquals or Op.LessThan or Op.GreaterThan or Op.LessThanOrEquals or Op.GreaterThanOrEquals => (4, 5),
                Op.Plus or Op.Minus => (6, 7),
                Op.Multiply or Op.Divide => (8, 9),
                _ => null
            };

        private static (int, int) PrefixOperatorBp(Op op) =>
            op switch
            {
                Op.Not => (7, 7),
                _ => throw new Exception($"Unexpected prefix operator: {op}")
            };

        private static (int, int)? PostfixOperatorBp(Op op) => null;

        private static Op TokenToOp(IToken type)
        {
            if (TokenToOpMapping.TryGetValue(type.GetType(), out var op))
            {
                return op;
            }
            throw new Exception($"Unexpected token type: {type}");
        }
    }
}
