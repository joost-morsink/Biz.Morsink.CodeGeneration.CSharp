using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct PropertyBuilder : IMemberBuilder
        {
            private readonly ModifierBuilder _modifiers;
            private readonly string _type;
            private readonly string _name;
            private readonly ImmutableList<AccessorBuilder> _accessors;

            public static PropertyBuilder Create(ModifierBuilder modifiers, string type, string name)
                => new PropertyBuilder(modifiers, type, name, ImmutableList<AccessorBuilder>.Empty);
            private PropertyBuilder(ModifierBuilder modifiers, string type, string name, ImmutableList<AccessorBuilder> accessors)
            {
                _modifiers = modifiers;
                _type = type;
                _name = name;
                _accessors = accessors;
            }

            public PropertyBuilder Add(params AccessorBuilder[] accessors)
                => new PropertyBuilder(_modifiers, _type, _name, _accessors.AddRange(accessors));

            public PropertyDeclarationSyntax Build()
                => SF.PropertyDeclaration(ParseType(_type), _name)
                    .AddModifiers(_modifiers.Build().ToArray())
                    .WithAccessorList(SF.AccessorList(SF.List(_accessors.Select(a => a.Build()))));

            MemberDeclarationSyntax IMemberBuilder.Build()
                => Build();
        }
    }
}
