using dddnet8.AuditLog.Entities;
using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.OperationRequests;
using Microsoft.EntityFrameworkCore;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Patients.DataModel;
using dddnet8.Domain.RequiredStaffs;
using dddnet8.Domain.RoomCoordinates.Domain;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Staffs;
using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Appointments;
using dddnet8.Infraestructure.AuditLog.Patients;
using dddnet8.Infraestructure.AuditLog.Staffs;
using dddnet8.Infraestructure.OperationRequests;
using dddnet8.Infraestructure.OperationTypes;
using dddnet8.Infraestructure.Patients;
using dddnet8.Infraestructure.RequiredStaffs;
using dddnet8.Infraestructure.RoomCoordinates;
using dddnet8.Infraestructure.Specializations;
using dddnet8.Infraestructure.Staff;
using dddnet8.Infraestructure.Staffs;
using dddnet8.Infraestructure.SurgeryRooms.TypeConfiguration;
using dddnet8.Infraestructure.SystemUsers;
using dddnet8.Infraestructure.Timetable;
using dddnet8.Infrastructure.Configurations;
using YourNamespace.GDPR.Entities;

namespace dddnet8.Infraestructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<SystemUser> SystemUser { get; set; }

        public DbSet<PatientDataModel> Patients { get; set; }

        // TODO: Replace DbSet<OperationRequest> with DbSet<OperationRequestDataModel>
        public DbSet<OperationRequest> OperationRequests { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }

        public DbSet<RequiredStaff> RequiredStaff { get; set; }
        
        public DbSet<PatientLog> PatientLogs { get; set; }
        
        public DbSet<Specialization> Specializations { get; set; }
                
        public DbSet<Domain.Staffs.Staff> Staff { get; set; }
        
        public DbSet<StaffLog> StaffLog { get; set; }
        
        public DbSet<SurgeryRoom> SurgeryRoom { get; set; }
        
        public DbSet<MaintenanceSlot> MaintenanceSlot { get; set; }
        
        public DbSet<EquipmentAssignment> EquipmentAssignment { get; set; }
        
        public DbSet<Appointment> Appointment { get; set; }
        
        public DbSet<Domain.Timetables.Timetable> Timetable { get; set; }
        
        public DbSet<AssignedStaff> AssignedStaff { get; set; }
        
        public DbSet<RoomCoordinate> RoomCoordinate { get; set; }
        
        public DbSet<TimeSlot> TimeSlot { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PatientEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration());
            
            modelBuilder.ApplyConfiguration(new OperationRequestEntityConfiguration());

            modelBuilder.ApplyConfiguration(new OperationTypeEntityConfiguration());

            modelBuilder.ApplyConfiguration(new SystemUsersTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PatientLogTypeConfiguration());

            modelBuilder.ApplyConfiguration(new RequiredStaffEntityConfiguration());
            
            modelBuilder.ApplyConfiguration(new StaffLogTypeConfiguration());

            modelBuilder.ApplyConfiguration(new SurgeryRoomTypeConfiguration());
            
            modelBuilder.ApplyConfiguration(new MaintenanceSlotTypeConfiguration());

            modelBuilder.ApplyConfiguration(new EquipmentAssigmentTypeConfiguration());

            modelBuilder.ApplyConfiguration(new AppointmentTypeConfiguration());

            modelBuilder.ApplyConfiguration(new TimetableEntityConfiguration());

            modelBuilder.ApplyConfiguration(new TimeSlotTypeConfiguration());

            modelBuilder.ApplyConfiguration(new AssignedStaffConfiguration());

            modelBuilder.ApplyConfiguration(new SpecializationEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new RoomCoordinateEntityConfiguration());
            
            // Call this once in the end
            base.OnModelCreating(modelBuilder);
        }
    }
}