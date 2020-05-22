using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct AccessorBuilder
        {
            private readonly ModifierBuilder _modifiers;
            private readonly SyntaxKind _kind;
            private readonly ExpressionBuilder? _expr;
            private readonly BlockBuilder? _block;

            public static AccessorBuilder Get(ModifierBuilder modifiers)
               => new AccessorBuilder(modifiers, SyntaxKind.GetAccessorDeclaration);
            public static AccessorBuilder Set(ModifierBuilder modifiers)
                  => new AccessorBuilder(modifiers, SyntaxKind.SetAccessorDeclaration);


            private AccessorBuilder(ModifierBuilder modifiers, SyntaxKind kind, ExpressionBuilder? expr = null, BlockBuilder? block = null)
            {
                _modifiers = modifiers;
                _kind = kind;
                _expr = expr;
                _block = block;
            }
            public AccessorBuilder With(ExpressionBuilder expr)
                => new AccessorBuilder(_modifiers, _kind, expr, null);
            public AccessorBuilder With(BlockBuilder block)
                => new AccessorBuilder(_modifiers, _kind, null, block);

            public AccessorDeclarationSyntax Build()
            {
                var acc = SF.AccessorDeclaration(_kind).AddModifiers(_modifiers.Build().ToArray()); ;
                if (_expr != null)
                    return acc.WithExpressionBody(SF.ArrowExpressionClause(_expr.Value.Build())).WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken));
                else if (_block != null)
                    return acc.WithBody(_block.Value.Build());
                else
                    return acc.WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken));
            }
        }
    }
}
