﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.CodeGeneration
{
    public class DefaultDocumentWriterTest
    {
        [Fact]
        public void WriteDocument_Empty_WritesChecksumAndMarksAutoGenerated()
        {
            // Arrange
            var document = new DocumentIntermediateNode();

            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var writer = new DefaultDocumentWriter(target, options);

            // Act
            var result = writer.WriteDocument(codeDocument, document);

            // Assert
            var csharp = result.GeneratedCode;
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
// <auto-generated/>
#pragma warning disable 1591
#pragma warning restore 1591
", 
                csharp, 
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_Empty_SuppressChecksumTrue_DoesnotWriteChecksum()
        {
            // Arrange
            var document = new DocumentIntermediateNode();

            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var optionsBuilder = new DefaultRazorCodeGenerationOptionsBuilder(designTime: false)
            {
                SuppressChecksum = true
            };
            var options = optionsBuilder.Build();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var writer = new DefaultDocumentWriter(target, options);

            // Act
            var result = writer.WriteDocument(codeDocument, document);

            // Assert
            var csharp = result.GeneratedCode;
            Assert.Equal(
@"// <auto-generated/>
#pragma warning disable 1591
#pragma warning restore 1591
",
                csharp,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_WritesNamespace()
        {
            // Arrange
            var document = new DocumentIntermediateNode();
            var builder = IntermediateNodeBuilder.Create(document);
            builder.Add(new NamespaceDeclarationIntermediateNode()
            {
                Content = "TestNamespace",
            });

            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var writer = new DefaultDocumentWriter(target, options);

            // Act
            var result = writer.WriteDocument(codeDocument, document);

            // Assert
            var csharp = result.GeneratedCode;
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
// <auto-generated/>
#pragma warning disable 1591
namespace TestNamespace
{
    #line hidden
}
#pragma warning restore 1591
", 
                csharp, 
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_WritesClass()
        {
            // Arrange
            var document = new DocumentIntermediateNode();
            var builder = IntermediateNodeBuilder.Create(document);
            builder.Add(new ClassDeclarationIntermediateNode()
            {
                Modifiers =
                {
                    "internal"
                },
                BaseType = "TestBase",
                Interfaces = new List<string> { "IFoo", "IBar", },
                ClassName = "TestClass",
            });

            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var writer = new DefaultDocumentWriter(target, options);

            // Act
            var result = writer.WriteDocument(codeDocument, document);

            // Assert
            var csharp = result.GeneratedCode;
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
// <auto-generated/>
#pragma warning disable 1591
internal class TestClass : TestBase, IFoo, IBar
{
}
#pragma warning restore 1591
",
                csharp,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_WritesMethod()
        {
            // Arrange
            var document = new DocumentIntermediateNode();
            var builder = IntermediateNodeBuilder.Create(document);
            builder.Add(new MethodDeclarationIntermediateNode()
            {
                Modifiers =
                {
                    "internal",
                    "virtual",
                    "async",
                },
                MethodName = "TestMethod",
                ReturnType = "string",
            });

            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var writer = new DefaultDocumentWriter(target, options);

            // Act
            var result = writer.WriteDocument(codeDocument, document);

            // Assert
            var csharp = result.GeneratedCode;
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 1998
internal virtual async string TestMethod()
{
}
#pragma warning restore 1998
#pragma warning restore 1591
",
                csharp,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_WritesField()
        {
            // Arrange
            var document = new DocumentIntermediateNode();
            var builder = IntermediateNodeBuilder.Create(document);
            builder.Add(new FieldDeclarationIntermediateNode()
            {
                Modifiers =
                {
                    "internal",
                    "readonly",
                },
                FieldName = "_foo",
                FieldType = "string",
            });

            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var writer = new DefaultDocumentWriter(target, options);

            // Act
            var result = writer.WriteDocument(codeDocument, document);

            // Assert
            var csharp = result.GeneratedCode;
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
// <auto-generated/>
#pragma warning disable 1591
internal readonly string _foo;
#pragma warning restore 1591
",
                csharp,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_WritesProperty()
        {
            // Arrange
            var document = new DocumentIntermediateNode();
            var builder = IntermediateNodeBuilder.Create(document);
            builder.Add(new PropertyDeclarationIntermediateNode()
            {
                Modifiers =
                {
                    "internal",
                    "virtual",
                },
                PropertyName = "Foo",
                PropertyType = "string",
            });

            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var writer = new DefaultDocumentWriter(target, options);

            // Act
            var result = writer.WriteDocument(codeDocument, document);

            // Assert
            var csharp = result.GeneratedCode;
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
// <auto-generated/>
#pragma warning disable 1591
internal virtual string Foo { get; set; }
#pragma warning restore 1591
",
                csharp,
                ignoreLineEndingDifferences: true);
        }
    }
}
