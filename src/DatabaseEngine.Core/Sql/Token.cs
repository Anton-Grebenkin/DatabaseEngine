
namespace DatabaseEngine.Core.Sql
{
    //public enum TokenType
    //{
    //    [Keyword("select")]
    //    Select,
    //    [Keyword("from")]
    //    From,
    //    [Keyword("where")]
    //    Where,
    //    [Keyword("insert")]
    //    Insert,
    //    [Keyword("into")]
    //    Into,
    //    [Keyword("values")]
    //    Values,
    //    [Keyword("update")]
    //    Update,
    //    [Keyword("set")]
    //    Set,
    //    [Keyword("delete")]
    //    Delete,
    //    [Keyword("create")]
    //    Create,
    //    [Keyword("table")]
    //    Table,
    //    [Keyword("primary")]
    //    Primary,
    //    [Keyword("key")]
    //    Key,
    //    [Keyword("drop")]
    //    Drop,
    //    [Keyword("alter")]
    //    Alter,
    //    [Keyword("add")]
    //    Add,
    //    [Keyword("column")]
    //    Column,
    //    [Keyword("index")]
    //    Index,
    //    [Keyword("join")]
    //    Join,
    //    [Keyword("inner")]
    //    Inner,
    //    [Keyword("left")]
    //    Left,
    //    [Keyword("right")]
    //    Right,
    //    [Keyword("full")]
    //    Full,
    //    [Keyword("outer")]
    //    Outer,
    //    [Keyword("on")]
    //    On,
    //    [Keyword("group")]
    //    Group,
    //    [Keyword("by")]
    //    By,
    //    [Keyword("order")]
    //    Order,
    //    [Keyword("asc")]
    //    Asc,
    //    [Keyword("desc")]
    //    Desc,
    //    [Keyword("union")]
    //    Union,
    //    [Keyword("all")]
    //    All,
    //    [Keyword("distinct")]
    //    Distinct,
    //    [Keyword("limit")]
    //    Limit,
    //    [Keyword("offset")]
    //    Offset,
    //    [Keyword("having")]
    //    Having,
    //    [Keyword("as")]
    //    As,
    //    [Keyword("and")]
    //    And,
    //    [Keyword("or")]
    //    Or,
    //    [Keyword("not")]
    //    Not,
    //    [Keyword("null")]
    //    Null,
    //    [Keyword("is")]
    //    Is,
    //    [Keyword("in")]
    //    In,
    //    [Keyword("between")]
    //    Between,
    //    [Keyword("like")]
    //    Like,
    //    [Keyword("exists")]
    //    Exists,
    //    [Keyword("any")]
    //    Any,
    //    [Keyword("case")]
    //    Case,
    //    [Keyword("when")]
    //    When,
    //    [Keyword("then")]
    //    Then,
    //    [Keyword("else")]
    //    Else,
    //    [Keyword("end")]
    //    End,
    //    [Keyword("default")]
    //    Default,

    //    Int,
    //    Integer,
    //    SmallInt,
    //    TinyInt,
    //    BigInt,
    //    Float,
    //    Real,
    //    Double,
    //    Decimal,
    //    Numeric,
    //    VarChar,
    //    Char,
    //    Text,
    //    Date,
    //    DateTime,
    //    Time,
    //    Timestamp,
    //    Boolean,

    //    Asterisk,
    //    Comma,
    //    Semicolon,
    //    OpenParen,
    //    CloseParen,
    //    Equals,
    //    NotEquals,
    //    LessThan,
    //    GreaterThan,
    //    LessThanOrEquals,
    //    GreaterThanOrEquals,
    //    Plus,
    //    Minus,
    //    Slash,
    //    Percent,
    //    Concat,
    //    SingleQuote,

    //    Identifier,

    //    StringLiteral,
    //    NumericLiteral,
    //    BooleanLiteral,

    //    SingleLineComment,
    //    MultiLineComment,
    //}

    //public record struct Token(TokenType Type, string Value);

    public interface IToken { }


    [Keyword("select")]
    public record struct Select : IToken { }

    [Keyword("from")]
    public record struct From : IToken { }

    [Keyword("where")]
    public record struct Where : IToken { }

    [Keyword("insert")]
    public record struct Insert : IToken { }

    [Keyword("into")]
    public record struct Into : IToken { }

    [Keyword("values")]
    public record struct Values : IToken { }

    [Keyword("update")]
    public record struct Update : IToken { }

    [Keyword("set")]
    public record struct Set : IToken { }

    [Keyword("delete")]
    public record struct Delete : IToken { }

    [Keyword("create")]
    public record struct Create : IToken { }

    [Keyword("table")]
    public record struct Table : IToken { }

    [Keyword("primary")]
    public record struct Primary : IToken { }

    [Keyword("key")]
    public record struct Key : IToken { }

    [Keyword("drop")]
    public record struct Drop : IToken { }

    [Keyword("alter")]
    public record struct Alter : IToken { }

    [Keyword("add")]
    public record struct Add : IToken { }

    [Keyword("column")]
    public record struct Column : IToken { }

    [Keyword("index")]
    public record struct Index : IToken { }

    [Keyword("join")]
    public record struct Join : IToken { }

    [Keyword("inner")]
    public record struct Inner : IToken { }

    [Keyword("left")]
    public record struct Left : IToken { }

    [Keyword("right")]
    public record struct Right : IToken { }

    [Keyword("full")]
    public record struct Full : IToken { }

    [Keyword("outer")]
    public record struct Outer : IToken { }

    [Keyword("on")]
    public record struct On : IToken { }

    [Keyword("group")]
    public record struct Group : IToken { }

    [Keyword("by")]
    public record struct By : IToken { }

    [Keyword("order")]
    public record struct Order : IToken { }

    [Keyword("asc")]
    public record struct Asc : IToken { }

    [Keyword("desc")]
    public record struct Desc : IToken { }

    [Keyword("union")]
    public record struct Union : IToken { }

    [Keyword("all")]
    public record struct All : IToken { }

    [Keyword("distinct")]
    public record struct Distinct : IToken { }

    [Keyword("limit")]
    public record struct Limit : IToken { }

    [Keyword("offset")]
    public record struct Offset : IToken { }

    [Keyword("having")]
    public record struct Having : IToken { }

    [Keyword("as")]
    public record struct As : IToken { }

    [Keyword("and")]
    public record struct And : IToken { }

    [Keyword("or")]
    public record struct Or : IToken { }

    [Keyword("not")]
    public record struct Not : IToken { }

    [Keyword("null")]
    public record struct Null : IToken { }

    [Keyword("is")]
    public record struct Is : IToken { }

    [Keyword("in")]
    public record struct In : IToken { }

    [Keyword("between")]
    public record struct Between : IToken { }

    [Keyword("like")]
    public record struct Like : IToken { }

    [Keyword("exists")]
    public record struct Exists : IToken { }

    [Keyword("any")]
    public record struct Any : IToken { }

    [Keyword("case")]
    public record struct Case : IToken { }

    [Keyword("when")]
    public record struct When : IToken { }

    [Keyword("then")]
    public record struct Then : IToken { }

    [Keyword("else")]
    public record struct Else : IToken { }

    [Keyword("end")]
    public record struct End : IToken { }

    [Keyword("default")]
    public record struct Default : IToken { }

    public record struct Int : IToken { }
    public record struct Integer : IToken { }
    public record struct SmallInt : IToken { }
    public record struct TinyInt : IToken { }
    public record struct BigInt : IToken { }
    public record struct Float : IToken { }
    public record struct Real : IToken { }
    public record struct Double : IToken { }
    public record struct Decimal : IToken { }
    public record struct Numeric : IToken { }
    public record struct VarChar : IToken { }
    public record struct Char : IToken { }
    public record struct Text : IToken { }
    public record struct Date : IToken { }
    public record struct DateTime : IToken { }
    public record struct Time : IToken { }
    public record struct Timestamp : IToken { }
    public record struct Boolean : IToken { }

    public record struct Asterisk : IToken { }
    public record struct Comma : IToken { }
    public record struct Semicolon : IToken { }
    public record struct OpenParen : IToken { }
    public record struct CloseParen : IToken { }
    public record struct IsEqual : IToken { }
    public record struct NotEquals : IToken { }
    public record struct LessThan : IToken { }
    public record struct GreaterThan : IToken { }
    public record struct LessThanOrEquals : IToken { }
    public record struct GreaterThanOrEquals : IToken { }
    public record struct Plus : IToken { }
    public record struct Minus : IToken { }
    public record struct Slash : IToken { }
    public record struct Percent : IToken { }
    public record struct Concat : IToken { }
    public record struct SingleQuote : IToken { }

    public record struct Identifier : IToken 
    {
        public string FirstName { get; init; }
        public string SecondName { get; init; }
    }

    public record struct StringLiteral : IToken
    {
        public string Value { get; init; }
    }
    public record struct NumericLiteral : IToken
    {
        public string Value { get; init; }
    }
    public record struct BooleanLiteral : IToken
    {
        public bool Value { get; init; }
    }

    public record struct SingleLineComment : IToken
    {
        public string Value { get; init; }
    }
    public record struct MultiLineComment : IToken
    {
        public string Value { get; init; }
    }
}

