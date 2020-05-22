using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct StatementBuilder : IStatementBuilder
        {
            private StatementSyntax _stat;
            public static StatementBuilder Create(string text)
                => new StatementBuilder(SF.ParseStatement(text));
            public static StatementBuilder Create(StatementSyntax statement)
                => new StatementBuilder(statement);
            private StatementBuilder(StatementSyntax stat)
            {
                _stat = stat;
            }

            public StatementSyntax Build()
                => _stat;

            public static implicit operator StatementBuilder(ExpressionBuilder builder)
                => Create(SF.ExpressionStatement(builder.Build()));
            public static implicit operator StatementBuilder(StatementSyntax statement)
                => Create(statement);
        }
    }
}
