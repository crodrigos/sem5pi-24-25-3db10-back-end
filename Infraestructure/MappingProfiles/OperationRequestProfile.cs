using AutoMapper;
using dddnet8.Domain.OperationRequests;

namespace dddnet8.Infraestructure.MappingProfiles;

public class OperationRequestProfile : Profile
{
    public OperationRequestProfile()
    {
        // Mapping from CreateOperationRequestDto to OperationRequest
        CreateMap<CreateOperationRequestDto, OperationRequest>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id is generated server-side
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => OperationRequestStatus.Pending)) // Default status
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow)) // Set creation date
            .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow)); // Set last updated date

        // Mapping from OperationRequest to OperationRequestDto
        CreateMap<OperationRequest, OperationRequestDto>()
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
            .ForMember(dest => dest.OperationTypeId, opt => opt.MapFrom(src => src.OperationTypeId))
            .ForMember(dest => dest.DeadlineDate, opt => opt.MapFrom(src => src.DeadlineDate))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
            .ForMember(dest => dest.OperationDescription, opt => opt.MapFrom(src => src.OperationDescription.Value)) // Assuming Description is a value object with a `Value` property
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => src.LastUpdatedDate))
            .ForMember(dest => dest.IsScheduled, opt => opt.MapFrom(src => src.IsScheduled));
        
        CreateMap<OperationRequest, OperationRequestDTOV2>()
            .ForMember(dest => dest.OperationRequestCodeId, opt => opt.MapFrom(src => src.OperationRequestCode._operationRequestCode))
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId.Value))
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId.Value))
            .ForMember(dest => dest.OperationTypeId, opt => opt.MapFrom(src => src.OperationTypeId._OperationTypeCode))
            .ForMember(dest => dest.DeadlineDate, opt => opt.MapFrom(src => src.DeadlineDate.Date))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
            .ForMember(dest => dest.OperationDescription, opt => opt.MapFrom(src => src.OperationDescription.Value)) // Assuming Description is a value object with a `Value` property
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => src.LastUpdatedDate))
            .ForMember(dest => dest.IsScheduled, opt => opt.MapFrom(src => src.IsScheduled));
    }
}