using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct DoBuilder
        {
            private readonly BlockBuilder _body;

            private DoBuilder(BlockBuilder body)
            {
                _body = body;
            }
            public static DoBuilder Create(BlockBuilder body)
                => new DoBuilder(body);

            public StatementBuilder While(ExpressionBuilder condition)
                => StatementBuilder.Create(SF.DoStatement(_body.Build(), condition.Build()));
        }
    }
}
