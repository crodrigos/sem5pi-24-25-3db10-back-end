using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Specializations.DTO;


namespace dddnet8.Domain.Specializations;

/// <summary>
/// Represents a specialization entity with name, description, and code.
/// </summary>
public class Specialization : Entity<Guid>, IAggregateRoot
{
    /// <summary>
    /// Gets the name of the specialization.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Gets the creation date of the specialization.
    /// </summary>
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    /// Gets the unique code of the specialization.
    /// </summary>
    public SpecializationCode Code { get; private set; }

    /// <summary>
    /// Gets the description of the specialization.
    /// </summary>
    public Description Description { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Specialization"/> class.
    /// </summary>
    /// <param name="name">The name of the specialization.</param>
    /// <param name="description">The description of the specialization.</param>
    /// <param name="code">The unique code of the specialization.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public Specialization(Name name, Description description, SpecializationCode code) 
        : base(Guid.NewGuid())
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Name cannot be null.");
        Description = description ?? throw new ArgumentNullException(nameof(description), "Description cannot be null.");
        Code = code ?? throw new ArgumentNullException(nameof(code), "Code cannot be null.");
        CreatedOn = DateTime.UtcNow;
    }
    
    protected Specialization() : base(Guid.NewGuid()) {}

    /// <summary>
    /// Updates the name of the specialization.
    /// </summary>
    /// <param name="name">The new name for the specialization.</param>
    public void UpdateName(Name name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Name cannot be null.");
    }

    /// <summary>
    /// Updates the description of the specialization.
    /// </summary>
    /// <param name="description">The new description for the specialization.</param>
    public void UpdateDescription(Description description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description), "Description cannot be null.");
    }

    /// <summary>
    /// Returns a string representation of the specialization.
    /// </summary>
    /// <returns>A string representation of the specialization.</returns>
    public override string ToString()
    {
        return $"{Name.Value} - {Description.Value} - {Code.Code}";
    }

    /// <summary>
    /// Factory method to create a <see cref="Specialization"/> instance from a string.
    /// </summary>
    /// <param name="specializationString">The string representation of the specialization.</param>
    /// <returns>A new <see cref="Specialization"/> instance.</returns>
    public static Specialization FromString(string specializationString)
    {
        if (string.IsNullOrWhiteSpace(specializationString))
        {
            throw new ArgumentNullException(nameof(specializationString), "Specialization string cannot be null or empty.");
        }

        var parts = specializationString.Split('-');
        if (parts.Length != 3)
        {
            // Log do valor que causou o erro
            Console.WriteLine($"Invalid specialization string: '{specializationString}'");
            throw new ArgumentException("Specialization must be in the format 'Name - Description - Code'.");
        }

        return new Specialization(
            Name.Create(parts[0].Trim()),
            Description.Create(parts[1].Trim()).Value,
            SpecializationCode.Create(parts[2].Trim())
        );
    }
    
    public void UpdateSpecialization(SpecializationByCriteriaDTO specializationCriteria)
    {
        if (specializationCriteria.Name != null) {UpdateName(Name.Create(specializationCriteria.Name));}

        if (specializationCriteria.Description != null) {UpdateDescription(Description.Create(specializationCriteria.Description).Value);}
    }
}
