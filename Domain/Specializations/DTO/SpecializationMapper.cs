namespace dddnet8.Domain.Specializations.DTO
{
    /// <summary>
    /// Provides mapping methods between Specialization entities and DTOs.
    /// </summary>
    public static class SpecializationMapper
    {
        /// <summary>
        /// Maps a Specialization entity to a SpecializationDto.
        /// </summary>
        /// <param name="specialization">The specialization entity to map.</param>
        /// <returns>A mapped SpecializationDto.</returns>
        public static SpecializationDto ToDto(Specialization specialization)
        {
            if (specialization == null)
            {
                throw new ArgumentNullException(nameof(specialization), "Specialization cannot be null.");
            }

            return new SpecializationDto(specialization.Name.Value, specialization.Description.Value, specialization.Code.Code);
        }
    }
}

