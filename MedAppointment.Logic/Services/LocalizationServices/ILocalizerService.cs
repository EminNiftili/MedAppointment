using MedAppointment.DataTransferObjects.LocalizationDtos;

namespace MedAppointment.Logics.Services.LocalizationServices
{
    public interface ILocalizerService
    {
        Task<Result<long>> AddResourceAsync(IEnumerable<LocalizationDto> localization);
    }
}
