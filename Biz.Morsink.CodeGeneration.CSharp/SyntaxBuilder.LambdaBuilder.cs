using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct LambdaBuilder : IExpressionBuilder
        {
            private readonly ImmutableList<ParameterBuilder> _parameters;
            private readonly ExpressionBuilder? _expr;
            private readonly BlockBuilder? _block;

            private LambdaBuilder(ImmutableList<ParameterBuilder> parameters, ExpressionBuilder? expr = null, BlockBuilder? block = null)
            {
                _parameters = parameters;
                _expr = expr;
                _block = block;
            }
            public static LambdaBuilder Create(params ParameterBuilder[] parameters)
                => Create(parameters.AsEnumerable());
            public static LambdaBuilder Create(IEnumerable<ParameterBuilder> parameters)
                => new LambdaBuilder(parameters.ToImmutableList());
            public LambdaBuilder With(ExpressionBuilder expr)
                => new LambdaBuilder(_parameters, expr, null);
            public LambdaBuilder With(BlockBuilder block)
                => new LambdaBuilder(_parameters, null, block);

            public LambdaExpressionSyntax Build()
                => _parameters.Count != 1
                    ? (LambdaExpressionSyntax)SF.ParenthesizedLambdaExpression(SF.ParameterList(SF.SeparatedList(_parameters.Select(p => p.Build()).ToArray())), _block.HasValue ? _block.Value.Build() : null, _expr.HasValue ? _expr.Value.Build() : null)
                    : SF.SimpleLambdaExpression(_parameters[0].Build(), _block.HasValue ? _block.Value.Build() : null, _expr.HasValue ? _expr.Value.Build() : null);

            ExpressionSyntax IExpressionBuilder.Build()
                => Build();
        }
    }
}
