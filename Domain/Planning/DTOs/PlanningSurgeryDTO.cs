namespace dddnet8.Domain.PlanningModuleNotifications.DTOs;

public class PlanningSurgeryDTO
{
    /// <summary>
    /// The operation type code associated with the surgery.
    /// </summary>
    public string OpTypeCode { get; set; }

    /// <summary>
    /// The estimated time for anesthesia (in HH:mm format).
    /// </summary>
    public string Anesthesia { get; set; }

    /// <summary>
    /// The estimated time for surgery (in HH:mm format).
    /// </summary>
    public string Surgery { get; set; }

    /// <summary>
    /// The estimated time for cleaning (in HH:mm format).
    /// </summary>
    public string Cleaning { get; set; }

    /// <summary>
    /// Constructor for PlanningSurgeryDTO.
    /// </summary>
    /// <param name="opTypeCode">The operation type code associated with the surgery.</param>
    /// <param name="tempoAnestesia">The estimated time for anesthesia (in HH:mm format).</param>
    /// <param name="tCirurgia">The estimated time for surgery (in HH:mm format).</param>
    /// <param name="tLimpeza">The estimated time for cleaning (in HH:mm format).</param>
    public PlanningSurgeryDTO(string opTypeCode, string tempoAnestesia, string tCirurgia, string tLimpeza)
    {
        OpTypeCode = opTypeCode;
        Anesthesia = tempoAnestesia;
        Surgery = tCirurgia;
        Cleaning = tLimpeza;
    }

    /// <summary>
    /// Parameterless constructor for PlanningSurgeryDTO.
    /// </summary>
    public PlanningSurgeryDTO()
    {
    }
}
