using dddnet8.Domain.Shared;
using dddnet8.Domain.OperationTypes.Names;
using dddnet8.Domain.Specializations;

namespace dddnet8.Domain.OperationTypes;

// TODO: IMPLEMENTAR HISTORICAL DATA DE OPERATION TYPE
public class OperationType : Entity<Guid>
{
    public Name Name { get; set; }
    
    public OperationTypeCode OperationTypeCode { get; set; }
    public Status Status { get; set; }
    public EstimatedDuration EstimatedDuration { get; set; }
    
    public Specialization SpecializationRequired { get; set; } //AC: "The system validates that the operation type matches the doctor’s specialization"


    public OperationType(Guid id, Name name, Status status, EstimatedDuration estimatedDuration, OperationTypeCode operationTypeCode) : base(id)
    {
        this.Name = name;
        this.Status = status;
        this.EstimatedDuration = estimatedDuration;
        this.OperationTypeCode = operationTypeCode;
    }
    
   

    protected OperationType() : base(Guid.NewGuid())
    {
    }
}