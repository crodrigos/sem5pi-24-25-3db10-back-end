namespace dddnet8.Domain.Specializations.DTO;

/// <summary>
/// DTO representing a Specialization.
/// </summary>
public class SpecializationDto {

    /// <summary>
    /// Gets or sets the name of the specialization.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the specialization.
    /// </summary>
    public string Description { get; set; }
    
    public string SpecializationCode { get; set; }

    /// <summary>
    ///  Initializes a new instance of the <see cref="SpecializationDto"/> class.
    ///  Required for serialization.
    ///  Default constructor.
    /// <summary>
    public SpecializationDto(string name, string description, string specializationCode)
    {
        Name = name;
        Description = description;
        SpecializationCode = specializationCode;
    }
}