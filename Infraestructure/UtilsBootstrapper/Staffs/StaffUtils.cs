using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.UtilsBootstrapper.OperationTypes;
using dddnet8.Infraestructure.UtilsBootstrapper.Specializations;
using dddnet8.Infraestructure.UtilsBootstrapper.SystemUsers;
using dddnet8.Infraestructure.UtilsBootstrapper.Timetables;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Infraestructure.UtilsBootstrapper.Staffs;

public class StaffUtils
{
    private readonly IStaffRepository _staffRepository;
    private readonly OperationTypeUtils _operationTypeUtils;
    private readonly TimetableUtils _timetableUtils;
    private readonly SystemUserUtils _systemUserUtils;
    private readonly SpecializationsUtils _specializationsUtils;

    public StaffUtils(IStaffRepository staffRepository, OperationTypeUtils operationTypeUtils,
        SystemUserUtils systemUserUtils, TimetableUtils timetableUtils,
        SpecializationsUtils specializationsUtils){
        _staffRepository = staffRepository;
        _operationTypeUtils = operationTypeUtils;
        _systemUserUtils = systemUserUtils;
        _timetableUtils = timetableUtils;
        _specializationsUtils = specializationsUtils;
    }
    
    public async Task InitializeStaffAsync(){
        var staffs = await _staffRepository.GetAllAsync();

        if (!staffs.Any())
        {
            await SaveStaff(await CreateStaff(
                Name.Create("John"),
                Name.Create("Doe"),
                UserRole.Doctor, 
                new LicenseNumber("D9769"),
                new ContactInfo(PhoneNumber.Create("923923923"), EmailAddress.Create("johndoe@example.com")),
                await _specializationsUtils.GetSpecialization("Orthopedics")),
                
                new TimeSpan(0,8,0, 0), new TimeSpan(22,0,0), "johndoe@example.com");
                
            await SaveStaff(await CreateStaff(
                    Name.Create("Pedro"),
                    Name.Create("Doe"),
                    UserRole.Doctor, 
                    new LicenseNumber("D8290"),
                    new ContactInfo(PhoneNumber.Create("924923923"), EmailAddress.Create("johndoe1@example.com")),
                    await _specializationsUtils.GetSpecialization("Orthopedics")),
                
                new TimeSpan(0,10,0, 0), new TimeSpan(22,0,0), "johndoe1@example.com");

            await SaveStaff(await CreateStaff(
                    Name.Create("Rui"),
                    Name.Create("Doe"),
                    UserRole.Doctor, 
                    new LicenseNumber("D9768"),
                    new ContactInfo(PhoneNumber.Create("923923924"), EmailAddress.Create("johndoe2@example.com")),
                    await _specializationsUtils.GetSpecialization("Orthopedics")),
                
                new TimeSpan(0,8,30, 0), new TimeSpan(22,0,0), "johndoe2@example.com");
            
            
            await SaveStaff(await CreateStaff(
                    Name.Create("Ana"),
                    Name.Create("Doe"),
                    UserRole.Doctor, 
                    new LicenseNumber("D9767"),
                    new ContactInfo(PhoneNumber.Create("923913924"), EmailAddress.Create("johndoe3@example.com")),
                    await _specializationsUtils.GetSpecialization("Anaesthetist")),
                
                new TimeSpan(0,8,0, 0), new TimeSpan(22,0,0), "johndoe3@example.com");
            
            await SaveStaff(await CreateStaff(
                    Name.Create("AP"),
                    Name.Create("Doe"),
                    UserRole.Doctor, 
                    new LicenseNumber("D0719"),
                    new ContactInfo(PhoneNumber.Create("923944954"), EmailAddress.Create("johndoe4@example.com")),
                    await _specializationsUtils.GetSpecialization("Anaesthetist")),
                
                new TimeSpan(0,8,0, 0), new TimeSpan(22,0,0), "johndoe4@example.com");
            
            await SaveStaff(await CreateStaff(
                    Name.Create("DOEE"),
                    Name.Create("Doe"),
                    UserRole.Doctor, 
                    new LicenseNumber("D7282"),
                    new ContactInfo(PhoneNumber.Create("923944954"), EmailAddress.Create("johndoe400@example.com")),
                    await _specializationsUtils.GetSpecialization("Anaesthetist")),
                
                new TimeSpan(0,8,0, 0), new TimeSpan(22,0,0), "johndoe400@example.com");

            
            
            await SaveStaff(await CreateStaff(
                    Name.Create("Ruiana"),
                    Name.Create("Doe"),
                    UserRole.Nurse, 
                    new LicenseNumber("N5177"),
                    new ContactInfo(PhoneNumber.Create("912912912"), EmailAddress.Create("johndoe5@example.com")),
                    await _specializationsUtils.GetSpecialization("Anaesthetist")),
                
                new TimeSpan(0,8,0, 0), new TimeSpan(21,0,0));
            
            await SaveStaff(await CreateStaff(
                    Name.Create("Maria"),
                    Name.Create("Doe"),
                    UserRole.Nurse, 
                    new LicenseNumber("N2519"),
                    new ContactInfo(PhoneNumber.Create("913913913"), EmailAddress.Create("johndoe6@example.com")),
                    await _specializationsUtils.GetSpecialization("Anaesthetist")),
                
                new TimeSpan(0,8,0, 0), new TimeSpan(21,0,0));
            
            
            await SaveStaff(await CreateStaff(
                    Name.Create("Emma"),
                    Name.Create("Doe"),
                    UserRole.Nurse, 
                    new LicenseNumber("N7073"),
                    new ContactInfo(PhoneNumber.Create("913913914"), EmailAddress.Create("johndoe7@example.com")),
                    await _specializationsUtils.GetSpecialization("Instrumenting")),
                
                new TimeSpan(0,8,0, 0), new TimeSpan(20,30,0));

            
            await SaveStaff(await CreateStaff(
                    Name.Create("Janea"),
                    Name.Create("Doe"),
                    UserRole.Nurse, 
                    new LicenseNumber("N7902"),
                    new ContactInfo(PhoneNumber.Create("913913914"), EmailAddress.Create("johndoe8@example.com")),
                    await _specializationsUtils.GetSpecialization("Instrumenting")),
                
                new TimeSpan(0,10,0, 0), new TimeSpan(23,0,0));
            
            await SaveStaff(await CreateStaff(
                    Name.Create("Bro"),
                    Name.Create("Doe"),
                    UserRole.Nurse, 
                    new LicenseNumber("N1238"),
                    new ContactInfo(PhoneNumber.Create("911913914"), EmailAddress.Create("johndoe9@example.com")),
                    await _specializationsUtils.GetSpecialization("Circulating")),
                
                new TimeSpan(0,8,30, 0), new TimeSpan(21,45,0));
            
            
            await SaveStaff(await CreateStaff(
                    Name.Create("Laila"),
                    Name.Create("Doe"),
                    UserRole.Nurse, 
                    new LicenseNumber("N5045"),
                    new ContactInfo(PhoneNumber.Create("911913994"), EmailAddress.Create("johndoe10@example.com")),
                    await _specializationsUtils.GetSpecialization("Circulating")),
                
                new TimeSpan(0,12,0, 0), new TimeSpan(22,0,0));
            
            
            
            await SaveStaff(await CreateStaff(
                    Name.Create("Hombre"),
                    Name.Create("Doe"),
                    UserRole.Technician, 
                    new LicenseNumber("T0534"),
                    new ContactInfo(PhoneNumber.Create("912953994"), EmailAddress.Create("johndoe11@example.com")),
                    await _specializationsUtils.GetSpecialization("Cleaning")),
                
                new TimeSpan(0,8,0, 0), new TimeSpan(22,0,0));
            
            await SaveStaff(await CreateStaff(
                    Name.Create("Female"),
                    Name.Create("Doe"),
                    UserRole.Technician, 
                    new LicenseNumber("T1756"),
                    new ContactInfo(PhoneNumber.Create("911988994"), EmailAddress.Create("johndoe12@example.com")),
                    await _specializationsUtils.GetSpecialization("Cleaning")),
                
                new TimeSpan(0,8,0, 0), new TimeSpan(22,0,0));

        } 
    }

    private async Task SaveStaff(Domain.Staffs.Staff staff, TimeSpan entrance, TimeSpan exit, string? personalEmail = null)
    {
        await _staffRepository.AddStaffAsync(staff);
        
        await _timetableUtils.CreateAndSaveTimetableForStaff(staff.LicenseNumber, entrance, exit);

        if (staff.LicenseNumber.Value.StartsWith("D"))
        {
            await _systemUserUtils.CreateAndSaveUser($"{staff.LicenseNumber.Value}@trelloHospital.com", UserRole.Doctor, personalEmail! );
        }
    }

    private async Task<Domain.Staffs.Staff> CreateStaff(Name firstName, Name lastname, UserRole role, LicenseNumber licenseNumber, ContactInfo contact, Specialization specialization){
        return new Domain.Staffs.Staff(firstName, lastname, specialization, contact, licenseNumber);
    }

    public async Task<Domain.Staffs.Staff?> GetStaff(string licenseNumber)
    {
        Console.WriteLine("LicenseNumber------------> " + licenseNumber);
        return await _staffRepository.GetByLicenseNumberAsync(new LicenseNumber(licenseNumber.ToUpper()));
    }
}