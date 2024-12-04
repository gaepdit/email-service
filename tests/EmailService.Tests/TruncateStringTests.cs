using GaEpd.EmailService.Utilities;

namespace EmailService.Tests;

public class TruncateStringTests
{
    private const string Value = "abc";

    [Test]
    public void Truncate_WithNegativeMaxLength_Throws()
    {
        var func = () => Value.Truncate(maxLength: -1);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Truncate_WithNullValue_ReturnsNull()
    {
        var result = ((string?)null).Truncate(maxLength: 1);
        result.Should().BeNull();
    }

    [Test]
    public void Truncate_WithEmptyValue_ReturnsEmptyString()
    {
        var result = string.Empty.Truncate(maxLength: 1);
        result.Should().Be(string.Empty);
    }

    [Test]
    public void Truncate_WithZeroMaxLength_ReturnsEmptyString()
    {
        var result = Value.Truncate(maxLength: 0);
        result.Should().Be(string.Empty);
    }

    [Test]
    public void Truncate_ValueShorterThanMaxLength_ReturnsOriginalValue()
    {
        var result = Value.Truncate(maxLength: 4);
        result.Should().Be(Value);
    }

    [Test]
    public void Truncate_ValueEqualToMaxLength_ReturnsOriginalValue()
    {
        var result = Value.Truncate(maxLength: Value.Length);
        result.Should().Be(Value);
    }

    [Test]
    public void Truncate_ValueLongerThanMaxLength_ReturnsTruncatedValue()
    {
        var result = Value.Truncate(maxLength: 2);
        result.Should().Be("a…");
    }
}
