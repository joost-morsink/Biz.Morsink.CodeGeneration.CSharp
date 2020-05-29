using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public static TypeSyntax ParseType(string type)
            => type switch
            {
                "var" => SF.IdentifierName(type),
                "object" => SF.PredefinedType(SF.Token(SyntaxKind.ObjectKeyword)),
                "string" => SF.PredefinedType(SF.Token(SyntaxKind.StringKeyword)),
                "byte" => SF.PredefinedType(SF.Token(SyntaxKind.ByteKeyword)),
                "sbyte" => SF.PredefinedType(SF.Token(SyntaxKind.SByteKeyword)),
                "short" => SF.PredefinedType(SF.Token(SyntaxKind.ShortKeyword)),
                "ushort" => SF.PredefinedType(SF.Token(SyntaxKind.UShortKeyword)),
                "int" => SF.PredefinedType(SF.Token(SyntaxKind.IntKeyword)),
                "uint" => SF.PredefinedType(SF.Token(SyntaxKind.UIntKeyword)),
                "long" => SF.PredefinedType(SF.Token(SyntaxKind.LongKeyword)),
                "ulong" => SF.PredefinedType(SF.Token(SyntaxKind.ULongKeyword)),
                "float" => SF.PredefinedType(SF.Token(SyntaxKind.FloatKeyword)),
                "double" => SF.PredefinedType(SF.Token(SyntaxKind.DoubleKeyword)),
                "decimal" => SF.PredefinedType(SF.Token(SyntaxKind.DecimalKeyword)),
                "void" => SF.PredefinedType(SF.Token(SyntaxKind.VoidKeyword)),
                _ => SF.ParseTypeName(type)
            };


        public static CompilationUnitBuilder CompilationUnit => CompilationUnitBuilder.Empty;
        public static NamespaceBuilder Namespace(string name) => NamespaceBuilder.Create(name);
        public static ModifierBuilder Public() => ModifierBuilder.Create().Public();
        public static ModifierBuilder Private() => ModifierBuilder.Create().Private();
        public static ModifierBuilder Protected() => ModifierBuilder.Create().Protected();
        public static ModifierBuilder Internal() => ModifierBuilder.Create().Internal();
        public static ModifierBuilder Partial() => ModifierBuilder.Create().Partial();
        public static ModifierBuilder Static() => ModifierBuilder.Create().Static();
        public static ModifierBuilder Virtual() => ModifierBuilder.Create().Virtual();
        public static ModifierBuilder Override() => ModifierBuilder.Create().Override();
        public static ModifierBuilder New() => ModifierBuilder.Create().New();
        public static ModifierBuilder Async() => ModifierBuilder.Create().Async();
        public static TypeBuilder Class(string name) => TypeBuilder.Class(default, name);
        public static TypeBuilder Struct(string name) => TypeBuilder.Struct(default, name);
        public static TypeBuilder Interface(string name) => TypeBuilder.Interface(default, name);
        public static GenericDeclarationBuilder Generic(string name) => GenericDeclarationBuilder.Create(name);
        public static PropertyBuilder Property(string type, string name) => PropertyBuilder.Create(default, type, name);
        public static AccessorBuilder Get() => AccessorBuilder.Get(default);
        public static AccessorBuilder Set() => AccessorBuilder.Set(default);
        public static FieldBuilder Field(string type, string name) => FieldBuilder.Create(default, type, name);
        public static MethodBuilder Method(string type, string name) => MethodBuilder.Create(default, type, name);
        public static ConstructorBuilder Constructor(string name) => ConstructorBuilder.Create(default, name);
        public static ExpressionBuilder Expression(string text) => ExpressionBuilder.Create(text);
        public static ExpressionBuilder Literal(string str) => ExpressionBuilder.Literal(str);
        public static ExpressionBuilder Literal(int i) => ExpressionBuilder.Literal(i);
        public static ExpressionBuilder Literal(long i) => ExpressionBuilder.Literal(i);
        public static ExpressionBuilder Literal(decimal i) => ExpressionBuilder.Literal(i);
        public static ExpressionBuilder Literal(double i) => ExpressionBuilder.Literal(i);
        public static ExpressionBuilder New(string type, params ExpressionBuilder[] parameters)
            => New(type, parameters.AsEnumerable());
        public static ExpressionBuilder New(string type, IEnumerable<ExpressionBuilder> parameters)
            => SF.ObjectCreationExpression(ParseType(type), SF.ArgumentList(SF.SeparatedList(parameters.Select(p => SF.Argument(p.Build())))), null);
        public static ExpressionBuilder Await(ExpressionBuilder expr)
            => SF.AwaitExpression(expr.Build());
        public static ExpressionBuilder TypeName(string type)
            => ParseType(type);
        public static StatementBuilder Statement(string text) => StatementBuilder.Create(text);
        public static StatementBuilder If(ExpressionBuilder condition, StatementBuilder then, StatementBuilder? @else)
            => @else == null
                ? StatementBuilder.Create(SF.IfStatement(condition.Build(), then.Build()))
                : StatementBuilder.Create(SF.IfStatement(condition.Build(), then.Build(), SF.ElseClause(@else.Value.Build())));
        public static StatementBuilder Foreach(string type, string name, ExpressionBuilder @in, StatementBuilder @do)
            => StatementBuilder.Create(SF.ForEachStatement(ParseType(type), name, @in.Build(), @do.Build()));
        public static StatementBuilder While(ExpressionBuilder condition, StatementBuilder statement)
            => StatementBuilder.Create(SF.WhileStatement(condition.Build(), statement.Build()));
        public static StatementBuilder For(string type, string name, ExpressionBuilder init, ExpressionBuilder condition, ExpressionBuilder incrementor, StatementBuilder statement)
            => StatementBuilder.Create(SF.ForStatement(
                SF.VariableDeclaration(ParseType(type), SF.SeparatedList(new[] { SF.VariableDeclarator(name).WithInitializer(SF.EqualsValueClause(init.Build())) })),
                SF.SeparatedList<ExpressionSyntax>(),
                condition.Build(),
                SF.SeparatedList(new[] { incrementor.Build() }),
                statement.Build()));
        public static DoBuilder Do(BlockBuilder block)
            => DoBuilder.Create(block);
        public static LambdaBuilder Lambda(params ParameterBuilder[] parameters)
            => Lambda(parameters.AsEnumerable());
        public static LambdaBuilder Lambda(IEnumerable<ParameterBuilder> parameters)
            => LambdaBuilder.Create(parameters);
        public static BlockBuilder Block(params IStatementBuilder[] statements)
            => Block(statements.AsEnumerable());
        public static BlockBuilder Block(IEnumerable<IStatementBuilder> statements)
            => BlockBuilder.Create().Add(statements);
    }
}
