namespace SurgicalManagement.Domain.Domain;

using System.ComponentModel;

public enum UserRole
{
    [Description("Administrator")]
    Admin = 0,

    [Description("Doctor")]
    Doctor = 1,

    [Description("Nurse")]
    Nurse = 2,

    [Description("Technician")]
    Technician = 3,

    [Description("Patient")]
    Patient = 4
}