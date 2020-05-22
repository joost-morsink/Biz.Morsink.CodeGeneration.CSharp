using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
#pragma warning disable CS0660, CS0661
        public struct ExpressionBuilder : IStatementBuilder
#pragma warning restore CS0660, CS0661 
        {
            private ExpressionSyntax _expr;

            private ExpressionBuilder(ExpressionSyntax expr)
            {
                _expr = expr;
            }

            public static ExpressionBuilder Create(string text) => new ExpressionBuilder(SF.ParseExpression(text));

            public ExpressionSyntax Build()
                => _expr;

            StatementSyntax IStatementBuilder.Build()
                => SF.ExpressionStatement(_expr);

            public static implicit operator ExpressionBuilder(string text)
                => Create(text);

            public ExpressionBuilder Call(string method, params ExpressionBuilder[] expressions)
                => new ExpressionBuilder(SF.InvocationExpression(SF.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, _expr, SF.IdentifierName(method)),
                    SF.ArgumentList(SF.SeparatedList(expressions.Select(e => SF.Argument(e.Build()))))));
            public ExpressionBuilder Call(string method, IEnumerable<string> typeParameters, params ExpressionBuilder[] expressions)
                => new ExpressionBuilder(SF.InvocationExpression(
                    SF.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, _expr,
                        SF.GenericName(method).WithTypeArgumentList(SF.TypeArgumentList(SF.SeparatedList(typeParameters.Select(tp => ParseType(tp)))))),
                    SF.ArgumentList(SF.SeparatedList(expressions.Select(e => SF.Argument(e.Build()))))));
            public ExpressionBuilder Member(string member)
                => new ExpressionBuilder(SF.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, _expr, SF.IdentifierName(member)));
            public ExpressionBuilder Invoke(params ExpressionBuilder[] expressions)
                => new ExpressionBuilder(SF.InvocationExpression(_expr, SF.ArgumentList(SF.SeparatedList(expressions.Select(e => SF.Argument(e.Build()))))));
            public ExpressionBuilder Binary(SyntaxKind kind, ExpressionBuilder right)
                => new ExpressionBuilder(SF.BinaryExpression(kind, this.Build(), right.Build()));
            public ExpressionBuilder PrefixUnary(SyntaxKind kind)
                => new ExpressionBuilder(SF.PrefixUnaryExpression(kind, _expr));
            public ExpressionBuilder PostfixUnary(SyntaxKind kind)
                => new ExpressionBuilder(SF.PostfixUnaryExpression(kind, _expr));
            public static ExpressionBuilder operator +(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.AddExpression, right);
            public static ExpressionBuilder operator -(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.SubtractExpression, right);
            public static ExpressionBuilder operator *(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.MultiplyExpression, right);
            public static ExpressionBuilder operator /(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.DivideExpression, right);
            public static ExpressionBuilder operator ==(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.EqualsExpression, right);
            public static ExpressionBuilder operator !=(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.NotEqualsExpression, right);
            public static ExpressionBuilder operator <(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.LessThanExpression, right);
            public static ExpressionBuilder operator >(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.GreaterThanExpression, right);
            public static ExpressionBuilder operator <=(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.LessThanOrEqualExpression, right);
            public static ExpressionBuilder operator >=(ExpressionBuilder left, ExpressionBuilder right)
                => left.Binary(SyntaxKind.GreaterThanOrEqualExpression, right);
            public static implicit operator ExpressionBuilder(ExpressionSyntax expressionSyntax)
                => new ExpressionBuilder(expressionSyntax);

            public static ExpressionBuilder Literal(string str)
                => new ExpressionBuilder(SF.LiteralExpression(SyntaxKind.StringLiteralExpression, SF.Literal(str)));
            public static ExpressionBuilder Literal(int i)
                => new ExpressionBuilder(SF.LiteralExpression(SyntaxKind.NumericLiteralExpression, SF.Literal(i)));
            public static ExpressionBuilder Literal(long i)
                => new ExpressionBuilder(SF.LiteralExpression(SyntaxKind.NumericLiteralExpression, SF.Literal(i)));
            public static ExpressionBuilder Literal(decimal i)
                => new ExpressionBuilder(SF.LiteralExpression(SyntaxKind.NumericLiteralExpression, SF.Literal(i)));
            public static ExpressionBuilder Literal(double i)
                => new ExpressionBuilder(SF.LiteralExpression(SyntaxKind.NumericLiteralExpression, SF.Literal(i)));


            public StatementBuilder Return()
                => StatementBuilder.Create(SF.ReturnStatement(Build()));
        }
    }
}
