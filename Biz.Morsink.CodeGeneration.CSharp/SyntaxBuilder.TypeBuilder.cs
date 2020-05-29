using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct TypeBuilder : IMemberBuilder
        {
            public enum Kind
            {
                Class, Struct, Interface
            }
            private readonly Kind _kind;
            private readonly ModifierBuilder _modifiers;
            private readonly string _name;
            private readonly ImmutableList<IMemberBuilder> _members;
            private readonly ImmutableList<GenericDeclarationBuilder> _generics;
            public static TypeBuilder Class(ModifierBuilder modifiers, string name)
                => new TypeBuilder(Kind.Class, modifiers, name);
            public static TypeBuilder Struct(ModifierBuilder modifiers, string name)
                => new TypeBuilder(Kind.Struct, modifiers, name);
            public static TypeBuilder Interface(ModifierBuilder modifiers, string name)
                => new TypeBuilder(Kind.Interface, modifiers, name);
            private TypeBuilder(Kind kind, ModifierBuilder modifiers, string name, ImmutableList<IMemberBuilder>? members = null, ImmutableList<GenericDeclarationBuilder>? generics = null)
            {
                _kind = kind;
                _modifiers = modifiers;
                _name = name;
                _members = members ?? ImmutableList<IMemberBuilder>.Empty;
                _generics = generics ?? ImmutableList<GenericDeclarationBuilder>.Empty;
            }

            public TypeBuilder Add(params IMemberBuilder[] builders)
                => Add(builders.AsEnumerable());
            public TypeBuilder Add(IEnumerable<IMemberBuilder> builders)
                => new TypeBuilder(_kind, _modifiers, _name, _members.AddRange(builders), _generics);
            public TypeBuilder WithGenerics(params GenericDeclarationBuilder[] generics)
                => WithGenerics(generics.AsEnumerable());
            public TypeBuilder WithGenerics(IEnumerable<GenericDeclarationBuilder> generics)
                => new TypeBuilder(_kind, _modifiers, _name, _members, _generics.AddRange(generics));
            public TypeDeclarationSyntax Build()
            {
                var res = (_kind switch
                {
                    Kind.Class => (TypeDeclarationSyntax)SF.ClassDeclaration(_name),
                    Kind.Interface => SF.InterfaceDeclaration(_name),
                    Kind.Struct => SF.StructDeclaration(_name),
                    _ => throw new InvalidOperationException()
                }).WithModifiers(_modifiers.Build())
                  .WithMembers(SF.List(_members.Select(m => m.Build())));
                if (_generics.Count > 0)
                    res = res.WithTypeParameterList(SF.TypeParameterList(SF.SeparatedList(_generics.Select(g => g.Build()))));
                return res;
            }

            MemberDeclarationSyntax IMemberBuilder.Build()
                => Build();
        }
    }
}
