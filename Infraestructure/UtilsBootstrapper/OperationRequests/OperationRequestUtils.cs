using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Infraestructure.UtilsBootstrapper.OperationTypes;
using dddnet8.Infraestructure.UtilsBootstrapper.Patients;
using dddnet8.Infraestructure.UtilsBootstrapper.Staffs;

namespace dddnet8.Infraestructure.UtilsBootstrapper.OperationRequests;

public class OperationRequestUtils
{
    private readonly IOperationRequestRepository _operationRequest;
    private readonly OperationTypeUtils _operationTypeUtils;
    private readonly PatientUtils _patientUtils;
    private readonly StaffUtils _staffUtils;

    public OperationRequestUtils(
        IOperationRequestRepository operationRequest,
        OperationTypeUtils operationTypeUtils,
        PatientUtils patientUtils,
        StaffUtils staffUtils)
    {
        _operationRequest = operationRequest;
        _operationTypeUtils = operationTypeUtils;
        _patientUtils = patientUtils;
        _staffUtils = staffUtils;
    }


    public async Task InitializeOperationRequestAsync()
    {

        var requests = await _operationRequest.GetAllAsync();

        if (!requests.Any())
        {
            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0001"),
                await _operationTypeUtils.GetOperationType("OT0001"),
                await _staffUtils.GetStaff("D0719"),
                await _patientUtils.GetPatient("202411000003"),
                "knee Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));



            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0002"),
                await _operationTypeUtils.GetOperationType("OT0003"),
                await _staffUtils.GetStaff("D0719"),
                await _patientUtils.GetPatient("202411000001"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));


            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0003"),
                await _operationTypeUtils.GetOperationType("OT0002"),
                await _staffUtils.GetStaff("d7282".ToUpper()),
                await _patientUtils.GetPatient("202411000002"),
                "Shoulder Replacement Surgery",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));


            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0004"),
                await _operationTypeUtils.GetOperationType("OT0003"),
                await _staffUtils.GetStaff("d7282".ToUpper()),
                await _patientUtils.GetPatient("202411000004"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));

            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0005"),
                await _operationTypeUtils.GetOperationType("OT0003"),
                await _staffUtils.GetStaff("D0719".ToUpper()),
                await _patientUtils.GetPatient("202411000008"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));


            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0006"),
                await _operationTypeUtils.GetOperationType("OT0001"),
                await _staffUtils.GetStaff("d7282".ToUpper()),
                await _patientUtils.GetPatient("202411000008"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));


            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0007"),
                await _operationTypeUtils.GetOperationType("OT0003"),
                await _staffUtils.GetStaff("D0719".ToUpper()),
                await _patientUtils.GetPatient("202411000008"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));


            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0008"),
                await _operationTypeUtils.GetOperationType("OT0002"),
                await _staffUtils.GetStaff("d7282".ToUpper()),
                await _patientUtils.GetPatient("202411000008"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));


            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0009"),
                await _operationTypeUtils.GetOperationType("OT0002"),
                await _staffUtils.GetStaff("D0719".ToUpper()),
                await _patientUtils.GetPatient("202411000008"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));


            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0010"),
                await _operationTypeUtils.GetOperationType("OT0003"),
                await _staffUtils.GetStaff("D0719".ToUpper()),
                await _patientUtils.GetPatient("202411000008"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));

            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0011"),
                await _operationTypeUtils.GetOperationType("OT0003"),
                await _staffUtils.GetStaff("d7282".ToUpper()),
                await _patientUtils.GetPatient("202411000008"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));

            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0012"),
                await _operationTypeUtils.GetOperationType("OT0002"),
                await _staffUtils.GetStaff("D0719".ToUpper()),
                await _patientUtils.GetPatient("202411000008"),
                "Hip Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));


            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0013"),
                await _operationTypeUtils.GetOperationType("OT0001"),
                await _staffUtils.GetStaff("D0719"),
                await _patientUtils.GetPatient("202411000008"),
                "knee Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));

            await SaveOperationRequest( CreateOperationRequest(
                OperationRequestCode.Create("OR0014"),
                await _operationTypeUtils.GetOperationType("OT0002"),
                await _staffUtils.GetStaff("d7282".ToUpper()),
                await _patientUtils.GetPatient("202411000008"),
                "knee Replacement Surgery Request",
                DateTime.UtcNow.AddMonths(3),
                OperationRequestStatus.Pending,
                OperationRequestPriority.Elective));

        } 
    }

private OperationRequest CreateOperationRequest(
        OperationRequestCode requestCode, 
        OperationType operationType, 
        Domain.Staffs.Staff? staff, 
        Patient? patient,
        string description, 
        DateTime dateTime, 
        OperationRequestStatus status, 
        OperationRequestPriority priority) {
    
    Console.WriteLine("AQUIII ----------------->" + patient.MedicalRecordNumber.Value);
    Console.WriteLine("AQUIII ----------------->" + staff.LicenseNumber.Value);
    Console.WriteLine("AQUIII ----------------->" + operationType.OperationTypeCode._OperationTypeCode);

        return OperationRequest.Create(
            patient!.MedicalRecordNumber,
            staff!.LicenseNumber,
            operationType!.OperationTypeCode,
            dateTime,
            priority,
            description,
            requestCode).Value;
    }


    private async Task SaveOperationRequest(OperationRequest operationRequest){
        await _operationRequest.AddOperationRequestAsync(operationRequest);
    }

    public async Task<OperationRequest?> GetOperationRequest(string code)
    {
        return await _operationRequest.GetByOperationRequestCode(code);
    }
}