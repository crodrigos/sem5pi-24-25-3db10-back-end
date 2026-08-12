using NUnit.Framework;
using System;
using dddnet8.Domain.OperationRequests;

namespace dddnet8.Domain.OperationRequests.Tests;

[TestFixture]
public class DescriptionTests
{
    /// <summary>
    /// Tests that a valid description can be successfully created.
    /// </summary>
    [Test]
    public void Create_ValidDescription_ShouldCreateDescription()
    {
        // Arrange
        var descriptionText = "This is a valid description.";

        // Act
        var result = Description.Create(descriptionText);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Value, Is.EqualTo(descriptionText));
    }

    /// <summary>
    /// Tests that creating a description with a null value returns an error.
    /// </summary>
    [Test]
    public void Create_NullDescription_ReturnsError()
    {
        // Act
        var result = Description.Create(null);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.EqualTo("Description cannot be null or empty"));
    }

    /// <summary>
    /// Tests that creating a description with an empty value returns an error.
    /// </summary>
    [Test]
    public void Create_EmptyDescription_ReturnsError()
    {
        // Act
        var result = Description.Create("");

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.EqualTo("Description cannot be null or empty"));
    }

    /// <summary>
    /// Tests that creating a description that exceeds the maximum length returns an error.
    /// </summary>
    [Test]
    public void Create_ExceedingLengthDescription_ReturnsError()
    {
        // Arrange
        var longDescription = new string('A', 513); // 512 characters + 1

        // Act
        var result = Description.Create(longDescription);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.EqualTo("Description cannot exceed 512 characters."));
    }

    /// <summary>
    /// Tests that the ToString method returns the correct value.
    /// </summary>
    [Test]
    public void ToString_ShouldReturnDescriptionValue()
    {
        // Arrange
        var descriptionText = "A description for testing.";
        var description = Description.Create(descriptionText).Value;

        // Act
        string result = description.ToString();

        // Assert
        Assert.That(result, Is.EqualTo(descriptionText));
    }

    /// <summary>
    /// Tests that creating a description from a valid string returns a description object.
    /// </summary>
    [Test]
    public void FromString_ValidString_ShouldCreateDescription()
    {
        // Arrange
        var descriptionText = "A valid description.";

        // Act
        var description = Description.FromString(descriptionText);

        // Assert
        Assert.That(description.Value.Value, Is.EqualTo(descriptionText));
        Assert.That(description.IsSuccess, Is.True);
    }

    /// <summary>
    /// Tests that creating a description from an invalid string returns an error.
    /// </summary>
    [Test]
    public void FromString_InvalidString_ReturnsError()
    {
        // Act
        var description = Description.FromString("");

        // Assert
        Assert.That(description.IsFailure, Is.True);
    }
}