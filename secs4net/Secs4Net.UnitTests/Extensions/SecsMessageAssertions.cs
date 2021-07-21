using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Secs4Net.UnitTests.Extensions
{
    public static class SecsMessageExtensions
    {
        public static SecsMessageAssertions Should(this SecsMessage? instance) => new SecsMessageAssertions(instance);
    }

    public sealed class SecsMessageAssertions : ReferenceTypeAssertions<SecsMessage?, SecsMessageAssertions>
    {
        public SecsMessageAssertions(SecsMessage? instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "message";

        public AndConstraint<SecsMessageAssertions> BeEquivalentTo(SecsMessage expectation, string because = "", params object[] becauseArgs)
        {
            if (!expectation.Equals(Subject))
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
            if (expectation.Equals(Subject))
            {
                new ObjectAssertions(Subject).NotBeEquivalentTo(expectation,
                    options => options.Excluding(a => a.SecsItem),
                    because, becauseArgs);

                if (Subject.SecsItem is not null)
                {
                    Subject.SecsItem.Should().NotBeEquivalentTo(expectation.SecsItem);
                }
                else if (expectation.SecsItem is null)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected message.SecsItem is null, but it isn't");
                }
            }
            return new AndConstraint<SecsMessageAssertions>(this);
        }
    }
}
