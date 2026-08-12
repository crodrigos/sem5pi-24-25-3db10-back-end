using System.ComponentModel;

namespace dddnet8.Domain.Patients.VO.Name;

public enum Gender
{
    [Description("Masculine")]
    Male,
        
    [Description("Feminine")]
    Female,
        
    [Description("Other")]
    Other,
        
    [Description("Non Specified")]
    Not_Specified // Para lidar com casos em que o gênero não é especificado
}