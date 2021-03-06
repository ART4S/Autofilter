using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Model;
using FluentAssertions;
using Xunit;
using static Autofilter.Helpers.PredicateBuilder;

namespace Autofilter.Tests.PredicateBuilder.Types;

public class ByteTests
{
    class TestClass
    {
        public byte Byte { get; set; }
        public byte? NullableByte { get; set; }
    }

    public static IEnumerable<object[]> ByteTestCases => new[]
    {
        new object[] { default(byte), default(byte).ToString(), SearchOperator.Equals, true },
        new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.Equals, true },
        new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.Equals, true },
        new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.Equals, false },
        new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.Equals, false },

        new object[] { default(byte), default(byte).ToString(), SearchOperator.NotEquals, false },
        new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.NotEquals, true },

        new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.Greater, true },
        new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.Greater, false },
        new object[] { default(byte), default(byte).ToString(), SearchOperator.Greater, false },
        new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.Greater, false },
        new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.Greater, false },

        new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object[] { default(byte), default(byte).ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true },

        new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.Less, true },
        new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { default(byte), default(byte).ToString(), SearchOperator.Less, false },
        new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.Less, false },

        new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.LessOrEqual, false },
        new object[] { default(byte), default(byte).ToString(), SearchOperator.LessOrEqual, true },
        new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableByteTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(byte).ToString(), SearchOperator.Equals, false },
        new object?[] { null, byte.MaxValue.ToString(), SearchOperator.Equals, false },
        new object?[] { default(byte), null, SearchOperator.Equals, false },
        new object?[] { byte.MaxValue, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(byte).ToString(), SearchOperator.NotEquals, true },
        new object?[] { null, byte.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object?[] { default(byte), null, SearchOperator.NotEquals, true },
        new object?[] { byte.MaxValue, null, SearchOperator.NotEquals, true },

        new object?[] { null, null, SearchOperator.Greater, false },
        new object?[] { null, default(byte).ToString(), SearchOperator.Greater, false },
        new object?[] { null, string.Empty, SearchOperator.Greater, false },
        new object?[] { null, byte.MinValue.ToString(), SearchOperator.Greater, false },
        new object?[] { default(byte), null, SearchOperator.Greater, false },
        new object?[] { byte.MaxValue, null, SearchOperator.Greater, false },

        new object?[] { null, null, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, default(byte).ToString(), SearchOperator.GreaterOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, byte.MinValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object?[] { default(byte), null, SearchOperator.GreaterOrEqual, false },
        new object?[] { byte.MaxValue, null, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, null, SearchOperator.Less, false },
        new object?[] { null, default(byte).ToString(), SearchOperator.Less, false },
        new object?[] { null, string.Empty, SearchOperator.Less, false },
        new object?[] { null, byte.MaxValue.ToString(), SearchOperator.Less, false },
        new object?[] { default(byte), null, SearchOperator.Less, false },
        new object?[] { byte.MinValue, null, SearchOperator.Less, false },

        new object?[] { null, null, SearchOperator.LessOrEqual, false },
        new object?[] { null, default(byte).ToString(), SearchOperator.LessOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false },
        new object?[] { null, byte.MaxValue.ToString(), SearchOperator.LessOrEqual, false },
        new object?[] { default(byte), null, SearchOperator.LessOrEqual, false },
        new object?[] { byte.MinValue, null, SearchOperator.LessOrEqual, false },

        new object?[] { byte.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { default(byte), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { byte.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { default(byte), null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(ByteTestCases))]
    public void ShouldHandleByte(
        byte propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { Byte = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Byte),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(ByteTestCases))]
    [MemberData(nameof(NullableByteTestCases))]
    public void ShouldHandleNullableByte(
        byte? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { NullableByte = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableByte),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }
}