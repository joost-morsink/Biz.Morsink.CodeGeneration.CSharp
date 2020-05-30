using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct ModifierBuilder
        {
            private readonly ImmutableList<SyntaxToken> _modifiers;

            public static ModifierBuilder Create() => new ModifierBuilder(ImmutableList<SyntaxToken>.Empty);
            private ModifierBuilder(ImmutableList<SyntaxToken> modifiers)
            {
                _modifiers = modifiers;
            }
            public ModifierBuilder Public() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.PublicKeyword)));
            public ModifierBuilder Private() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.PrivateKeyword)));
            public ModifierBuilder Protected() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.ProtectedKeyword)));
            public ModifierBuilder Internal() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.InternalKeyword)));
            public ModifierBuilder Partial() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.PartialKeyword)));
            public ModifierBuilder Static() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.StaticKeyword)));
            public ModifierBuilder Abstract() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.AbstractKeyword)));
            public ModifierBuilder Virtual() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.VirtualKeyword)));
            public ModifierBuilder Override() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.OverrideKeyword)));
            public ModifierBuilder New() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.NewKeyword)));
            public ModifierBuilder Async() => new ModifierBuilder(_modifiers.Add(SF.Token(SyntaxKind.AsyncKeyword)));
            public TypeBuilder Class(string name) => TypeBuilder.Class(this, name);
            public TypeBuilder Struct(string name) => TypeBuilder.Struct(this, name);
            public TypeBuilder Interface(string name) => TypeBuilder.Interface(this, name);

            public PropertyBuilder Property(string type, string name) => PropertyBuilder.Create(this, type, name);
            public AccessorBuilder Get() => AccessorBuilder.Get(this);
            public AccessorBuilder Set() => AccessorBuilder.Set(this);
            public FieldBuilder Field(string type, string name) => FieldBuilder.Create(this, type, name);
            public MethodBuilder Method(string type, string name) => MethodBuilder.Create(this, type, name);
            public ConstructorBuilder Constructor(string name) => ConstructorBuilder.Create(this, name);

            internal SyntaxTokenList Build()
                => _modifiers == null ? SF.TokenList() : SF.TokenList(_modifiers);
        }
    }
}
