using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct ConstructorBuilder : IMemberBuilder
        {
            private readonly ModifierBuilder _modifiers;
            private readonly string _name;
            private readonly ImmutableList<ParameterBuilder> _parameters;
            private readonly ExpressionBuilder? _expr;
            private readonly BlockBuilder? _block;
            private readonly ConstructorInitializerSyntax? _init;
            private ConstructorBuilder(ModifierBuilder modifiers, string name, ImmutableList<ParameterBuilder>? parameters = null, ConstructorInitializerSyntax? init = null, ExpressionBuilder? expr = default, BlockBuilder? block = default)
            {
                _modifiers = modifiers;
                _name = name;
                _parameters = parameters ?? ImmutableList<ParameterBuilder>.Empty; ;
                _init = init;
                _expr = expr;
                _block = block;
            }

            public static ConstructorBuilder Create(ModifierBuilder modifiers, string name)
                => new ConstructorBuilder(modifiers, name);
            public ConstructorBuilder With(ExpressionBuilder expr)
                => new ConstructorBuilder(_modifiers, _name, _parameters, _init, expr, null);
            public ConstructorBuilder With(BlockBuilder block)
                => new ConstructorBuilder(_modifiers, _name, _parameters, _init, null, block);
            public ConstructorBuilder AddParameters(params ParameterBuilder[] parameters)
                => new ConstructorBuilder(_modifiers, _name, _parameters.AddRange(parameters), _init, _expr, _block);
            public ConstructorBuilder CallBase(params ExpressionBuilder[] parameters)
                => new ConstructorBuilder(_modifiers, _name, _parameters,
                    SF.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer,
                        SF.ArgumentList(SF.SeparatedList(parameters.Select(p => SF.Argument(p.Build()))))),
                    _expr, _block);
            public ConstructorBuilder CallThis(params ExpressionBuilder[] parameters)
                => new ConstructorBuilder(_modifiers, _name, _parameters,
                    SF.ConstructorInitializer(SyntaxKind.ThisConstructorInitializer,
                        SF.ArgumentList(SF.SeparatedList(parameters.Select(p => SF.Argument(p.Build()))))),
                    _expr, _block);

            public ConstructorDeclarationSyntax Build()
            {
                var ctor = SF.ConstructorDeclaration(_name)
                             .AddModifiers(_modifiers.Build().ToArray())
                             .AddParameterListParameters(_parameters.Select(p => p.Build()).ToArray());

                if (_expr != null)
                    return ctor.WithExpressionBody(SF.ArrowExpressionClause(_expr.Value.Build())).WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken));
                else if (_block != null)
                    return ctor.WithBody(_block.Value.Build());
                else
                    return ctor.WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken));
            }

            MemberDeclarationSyntax IMemberBuilder.Build()
                => Build();

        }
    }
}
