using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.Timetable;

namespace dddnet8.Domain.Staffs
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly ITimetableRepository _timetableRepository;

        public TimeSlotService(ITimeSlotRepository timeSlotRepository, ITimetableRepository timetableRepository)
        {
            _timeSlotRepository = timeSlotRepository;
            _timetableRepository = timetableRepository;
        }

        // Método para buscar todos os TimeSlots
        public async Task<List<TimeSlot>> GetAllTimeSlots()
        {
            return await _timeSlotRepository.GetAllAsync();
        }

        public async Task<bool> CheckIfStaffTimetableIsAvailable(DateTime appointmentDate, string surgeryStartTime,
        EstimatedDuration estimatedDuration, string teamLicenseNumber){
           
           Console.WriteLine("AQUII ---------->" + teamLicenseNumber);
           
    try
    { 
        var staffTimetableForDate = await _timetableRepository.GetTimetableByDateAndLicenseNumber(new LicenseNumber(teamLicenseNumber), DateOnly.FromDateTime(appointmentDate));

        if (staffTimetableForDate == null) {throw new Exception($"No staff timetable found for {teamLicenseNumber} on {appointmentDate.ToShortDateString()}. The staff might be on vacation.");}

        double surgeryStartTimeSpan = TimeSpan.Parse(surgeryStartTime).TotalMinutes;
        
        double surgeryEndTimeSpan = surgeryStartTimeSpan + estimatedDuration.GetTotalMinutesEstimatedDuration();

        double staffStartTimeSpan = staffTimetableForDate.TimeShift.Entrance.TotalMinutes;

        double staffEndTimeSpan = staffTimetableForDate.TimeShift.Exit.TotalMinutes;

        if (surgeryStartTimeSpan >= staffStartTimeSpan && surgeryStartTimeSpan < staffEndTimeSpan && surgeryEndTimeSpan <= staffEndTimeSpan)
        { return true;
        }
        return false;
    }
    catch (Exception ex) {throw new Exception("Erro ao verificar o horario do horário do staff", ex);}
}


        public async Task<bool> CheckIfStaffIsAvailable2(DateTime dateTime, string licenseNumber)
        {
            var result = await _timetableRepository.GetTimetableByDateAndLicenseNumber(new LicenseNumber(licenseNumber),
                DateOnly.FromDateTime(dateTime));

            if (result == null)
            {return false;
            }
            return true;
        }

        public async Task<bool> CheckIfStaffTimeSlotIsAvailable(DateTime appointmentDate, string surgeryStartTime, EstimatedDuration estimatedDuration, string teamLicenseNumber)
{
    try
    {
        // Obtém todos os slots de horário do staff para a data e licença fornecida
        var staffTimeSlotForAppointmentDate = await _timeSlotRepository.GetTimeSlotByLicenseNumberAndDate(DateOnly.FromDateTime(appointmentDate), new LicenseNumber(teamLicenseNumber));

        // Se não existir nenhum slot para o staff na data, isso significa que o staff está disponível
        if (staffTimeSlotForAppointmentDate == null || !staffTimeSlotForAppointmentDate.Any())
        {
            return true;
        }

        
        
        double surgeryStartTimeSpan = TimeSpan.Parse(surgeryStartTime).TotalMinutes;
        double surgeryEndTimeSpan = surgeryStartTimeSpan + estimatedDuration.GetTotalMinutesEstimatedDuration();

        
        foreach (var timeSlot in staffTimeSlotForAppointmentDate){
            double staffStartTimeSpan = timeSlot.TimeShift.Entrance.TotalMinutes;
            double staffEndTimeSpan = timeSlot.TimeShift.Exit.TotalMinutes;

            if ((surgeryStartTimeSpan < staffEndTimeSpan && surgeryEndTimeSpan > staffStartTimeSpan) ||
                (surgeryStartTimeSpan >= staffStartTimeSpan && surgeryStartTimeSpan < staffEndTimeSpan)){
                return false;
            }
        }
        return true;
    }
    catch (Exception ex) {throw new Exception("Erro ao verificar a disponibilidade do horário do staff", ex);}
}

        public async Task SaveTimeSlot(TimeSlot timeSlot)
        {
            await _timeSlotRepository.AddTimeSlot(timeSlot);
        }

        public async Task<List<TimeSlot>> GetStaffAllTimeSlots(LicenseNumber sLicenseNumber)
        {
            return await _timeSlotRepository.GetStaffAllTimeSlots(sLicenseNumber);
        }

        public async Task<List<TimeSlot>> GetTimeSlotByLicenseNumberAndDate(DateOnly date, LicenseNumber licenseNumber)
        {
            return await _timeSlotRepository.GetTimeSlotByLicenseNumberAndDate(date, licenseNumber);
        }
    }
}
