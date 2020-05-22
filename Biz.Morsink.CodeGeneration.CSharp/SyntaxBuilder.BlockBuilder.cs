using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct BlockBuilder : IStatementBuilder
        {
            private ImmutableList<IStatementBuilder> _statements;
            public static BlockBuilder Create() => new BlockBuilder(ImmutableList<IStatementBuilder>.Empty);

            private BlockBuilder(ImmutableList<IStatementBuilder> statements)
            {
                _statements = statements;
            }
            public BlockBuilder Add(params IStatementBuilder[] statements)
                => new BlockBuilder(_statements.AddRange(statements));

            public BlockSyntax Build()
                => SF.Block(SF.List(_statements.Select(s => s.Build())));

            StatementSyntax IStatementBuilder.Build()
                => Build();
        }
    }
}
