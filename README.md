# Readme
This repository contains a helper library (fluent API) to easily create '_Roslyn_' syntax trees.
An excellent intermediate representation for either on-the-fly compilation or outputting (generated) source code.

This library is by no means complete, and I will accept pull requests that add to the functionality of this library.

## Instructions

Put the following line on top of your source file:

```csharp
using static Biz.Morsink.CodeGeneration.CSharp.SyntaxBuilder;
```

You can have a look at the test code how to construct a source file.

