using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Collections.Generic;

namespace Secs4Net;

internal static class SecsMessageExtensions
{
    public static SecsMessageAssertions Should(this SecsMessage? instance) => new(instance);
}

internal sealed class SecsMessageAssertions : ReferenceTypeAssertions<SecsMessage?, SecsMessageAssertions>
{
    public SecsMessageAssertions(SecsMessage? instance) : base(instance) { }

    protected override string Identifier => "message";

    public AndConstraint<SecsMessageAssertions> BeEquivalentTo(SecsMessage expectation, string because = "", params object[] becauseArgs)
    {
        if (!IsMessageEquals(expectation, Subject))
        {
            new ObjectAssertions(Subject).BeEquivalentTo(expectation,
                options => options.ComparingByMembers<SecsMessage>()
                    .Excluding(a => a.SecsItem)
                    .Excluding(a => a.Name),
                because, becauseArgs);

            if (Subject?.SecsItem is not null)
            {
                Subject.SecsItem.Should().BeEquivalentTo(expectation.SecsItem);
            }
            else if (expectation.SecsItem is not null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected message.SecsItem is not null, but is");
            }
        }

        return new AndConstraint<SecsMessageAssertions>(this);
    }

    public AndConstraint<SecsMessageAssertions> NotBeEquivalentTo(SecsMessage expectation, string because = "", params object[] becauseArgs)
    {
        if (IsMessageEquals(expectation, Subject))
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected message not be equivalent, but they are.");
        }

        return new AndConstraint<SecsMessageAssertions>(this);
    }

    private static bool IsMessageEquals(SecsMessage left, SecsMessage? right)
    {
        if (right is null)
        {
            return false;
        }

        // exclude Name property
        return left.S == right.S
            && left.F == right.F
            && left.ReplyExpected == right.ReplyExpected
            && EqualityComparer<Item>.Default.Equals(left.SecsItem!, right.SecsItem!);
    }
}
