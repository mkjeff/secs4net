using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;

namespace Secs4Net;

internal static class ItemExtensions
{
    public static ItemAssertions Should(this Item? instance) => new(instance);
}

internal sealed class ItemAssertions : ReferenceTypeAssertions<Item?, ItemAssertions>
{
    public ItemAssertions(Item? instance) : base(instance) { }

    protected override string Identifier => "item";


    public AndConstraint<ItemAssertions> BeEquivalentTo(Item? expectation, string because = "", params object[] becauseArgs)
    {
        if (Subject?.Equals(expectation) != true)
        {
            new ItemValidator().IsEquals(new ItemEquivalencyValidationContext
            {
                Reason = new Reason(because, becauseArgs),
                Subject = Subject,
                Expectation = expectation,
            });
        }

        return new AndConstraint<ItemAssertions>(this);
    }

    public AndConstraint<ItemAssertions> NotBeEquivalentTo(Item expectation, string because = "", params object[] becauseArgs)
    {
        if (expectation.Equals(Subject))
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected item not be equivalent, but they are.");
        }

        return new AndConstraint<ItemAssertions>(this);
    }
}

internal sealed class ItemEquivalencyValidationContext
{
    public Reason Reason { get; init; } = default!;
    public Item? Subject { get; init; } = default!;
    public Item? Expectation { get; init; } = default!;
}

internal sealed class ItemValidator
{
    public bool IsEquals(ItemEquivalencyValidationContext context)
    {
        if (context.Subject is not Item subjectValue ||
            context.Expectation is not Item expectationValue)
        {
            return false;
        }

        return IsMatch(path: "item", subjectValue, expectationValue, context);
    }

    private static bool IsMatch(string path, Item subject, Item expectation, ItemEquivalencyValidationContext context)
    {
        if (subject.Format != expectation.Format)
        {
            Execute.Assertion
                .BecauseOf(context.Reason)
                .FailWith("Expected {0} to be {1}, but found {2}",
                    path + ".Format", expectation.Format, subject.Format);

            return false;
        }

        if (subject.Count != expectation.Count)
        {
            Execute.Assertion
                .BecauseOf(context.Reason)
                .FailWith("Expected {0} to be {1}, but found {2}",
                    path + ".Count", expectation.Count, subject.Count);
            return false;
        }

        if (subject.Count == 0)
        {
            return true;
        }

#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
        return expectation.Format switch
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
        {
            SecsFormat.List => IsMatchList(path, subject, expectation, context),
            SecsFormat.ASCII or SecsFormat.JIS8 => IsMatchStringItem(path, subject, expectation, context),
            SecsFormat.Binary => IsMatchArrayItem<byte>(path, subject, expectation, context),
            SecsFormat.Boolean => IsMatchArrayItem<bool>(path, subject, expectation, context),
            SecsFormat.I8 => IsMatchArrayItem<long>(path, subject, expectation, context),
            SecsFormat.I1 => IsMatchArrayItem<sbyte>(path, subject, expectation, context),
            SecsFormat.I2 => IsMatchArrayItem<short>(path, subject, expectation, context),
            SecsFormat.I4 => IsMatchArrayItem<int>(path, subject, expectation, context),
            SecsFormat.F8 => IsMatchArrayItem<double>(path, subject, expectation, context),
            SecsFormat.F4 => IsMatchArrayItem<float>(path, subject, expectation, context),
            SecsFormat.U8 => IsMatchArrayItem<ulong>(path, subject, expectation, context),
            SecsFormat.U1 => IsMatchArrayItem<byte>(path, subject, expectation, context),
            SecsFormat.U2 => IsMatchArrayItem<ushort>(path, subject, expectation, context),
            SecsFormat.U4 => IsMatchArrayItem<uint>(path, subject, expectation, context),
        };

        bool IsMatchStringItem(string path, Item subject, Item expectation, ItemEquivalencyValidationContext context)
        {
            var subjectString = subject.GetString();
            var expectationString = expectation.GetString();
            if (subjectString.Equals(expectationString, StringComparison.Ordinal))
            {
                return true;
            }

            Execute.Assertion
                .BecauseOf(context.Reason)
                .FailWith("Expected {0} to be {1}, but found {2}",
                    path + ".GetString()", expectationString, subjectString);

            return false;
        }

        bool IsMatchArrayItem<T>(string path, Item subject, Item expectation, ItemEquivalencyValidationContext context) where T : unmanaged, IEquatable<T>
        {
            var subjectSpan = subject.GetMemory<T>().Span;
            var expectationSpan = expectation.GetMemory<T>().Span;
            if (subjectSpan.SequenceEqual(expectationSpan))
            {
                return true;
            }

            Execute.Assertion
                .BecauseOf(context.Reason)
                .FailWith("Expected {0} to be {1}, but found {2}",
                    path + $".GetMemory<{typeof(T).Name}>()",
                    expectationSpan.ToArray(),
                    subjectSpan.ToArray());
            return false;
        }

        bool IsMatchList(string path, Item subjectList, Item expectationList, ItemEquivalencyValidationContext context)
        {
            for (int i = 0, count = subjectList.Count; i < count; i++)
            {
                if (!IsMatch(path + $"[{i}]", subjectList[i], expectationList[i], context))
                {
                    return false;
                }
            }

            return true;
        }
    }

}
