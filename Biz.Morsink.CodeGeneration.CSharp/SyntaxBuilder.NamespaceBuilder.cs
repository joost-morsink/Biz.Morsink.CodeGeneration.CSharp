using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct NamespaceBuilder
        {
            private readonly string _name;
            private readonly ImmutableList<TypeBuilder> _types;

            private NamespaceBuilder(string name, ImmutableList<TypeBuilder> types)
            {
                _name = name;
                _types = types;
            }
            public static NamespaceBuilder Create(string name)
                => new NamespaceBuilder(name, ImmutableList<TypeBuilder>.Empty);
            public NamespaceBuilder Add(params TypeBuilder[] types)
                => Add(types.AsEnumerable());
            public NamespaceBuilder Add(IEnumerable<TypeBuilder> types)
                => new NamespaceBuilder(_name, _types.AddRange(types));
            public NamespaceDeclarationSyntax Build()
                => SF.NamespaceDeclaration(SF.ParseName(_name)).AddMembers(_types.Select(t => t.Build()).ToArray());
        }
    }
}
