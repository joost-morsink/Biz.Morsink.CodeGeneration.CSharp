using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct MethodBuilder : IMemberBuilder
        {
            private readonly ModifierBuilder _modifiers;
            private readonly string _type;
            private readonly string _name;
            private readonly ImmutableList<ParameterBuilder> _parameters;
            private readonly ImmutableList<string> _typeParameters;
            private readonly ExpressionBuilder? _expr;
            private readonly BlockBuilder? _block;

            private MethodBuilder(ModifierBuilder modifiers, string type, string name, ImmutableList<ParameterBuilder>? parameters = null, ImmutableList<string>? typeParameters = null, ExpressionBuilder? expr = default, BlockBuilder? block = default)
            {
                _modifiers = modifiers;
                _type = type;
                _name = name;
                _parameters = parameters ?? ImmutableList<ParameterBuilder>.Empty;
                _typeParameters = typeParameters ?? ImmutableList<string>.Empty;
                _expr = expr;
                _block = block;
            }

            public static MethodBuilder Create(ModifierBuilder modifiers, string type, string name)
                => new MethodBuilder(modifiers, type, name);
            public MethodBuilder With(ExpressionBuilder expr)
                => new MethodBuilder(_modifiers, _type, _name, _parameters, _typeParameters, expr, null);
            public MethodBuilder With(BlockBuilder block)
                => new MethodBuilder(_modifiers, _type, _name, _parameters, _typeParameters, null, block);
            public MethodBuilder AddParameters(params ParameterBuilder[] parameters)
                => new MethodBuilder(_modifiers, _type, _name, _parameters.AddRange(parameters), _typeParameters, _expr, _block);
            public MethodBuilder AddTypeParameters(params string[] parameters)
                => new MethodBuilder(_modifiers, _type, _name, _parameters, _typeParameters.AddRange(parameters), _expr, _block);
            public MethodDeclarationSyntax Build()
            {
                var method = SF.MethodDeclaration(ParseType(_type), _name).AddModifiers(_modifiers.Build().ToArray()).AddParameterListParameters(_parameters.Select(p => p.Build()).ToArray());
                if (_typeParameters.Count > 0)
                    method = method.WithTypeParameterList(SF.TypeParameterList(SF.SeparatedList(_typeParameters.Select(SF.TypeParameter))));
                if (_expr != null)
                    return method.WithExpressionBody(SF.ArrowExpressionClause(_expr.Value.Build())).WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken));
                else if (_block != null)
                    return method.WithBody(_block.Value.Build());
                else
                    return method.WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken));
            }

            MemberDeclarationSyntax IMemberBuilder.Build()
                => Build();
        }
    }
}
