using System;
using dddnet8.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace SurgicalManagement.Domain.Common
{
    [Owned]
    public class DeletionStatus : ValueObject
    {
        public bool IsToDelete { get; }
        public DateTime? DeletionDate { get; }

        protected DeletionStatus() {}
        // Construtor privado para garantir a imutabilidade
        private DeletionStatus(bool isToDelete, DateTime? deletionDate)
        {
            IsToDelete = isToDelete;
            DeletionDate = deletionDate;
        }

        // Método de fábrica para criar um novo DeletionStatus
        public static DeletionStatus Create(bool isToDelete, DateTime? deletionDate = null)
        {
            return new DeletionStatus(isToDelete, deletionDate);
        }
        
        public static DeletionStatus FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Create(false, null); // Retornar um estado padrão se a string for nula ou vazia
            }

            var parts = value.Split(';');
            if (parts.Length < 1 || !bool.TryParse(parts[0], out var isToDelete))
            {
                throw new FormatException("A string não está no formato esperado.");
            }

            DateTime? deletionDate = null;
            if (parts.Length > 1 && DateTime.TryParse(parts[1], out var parsedDate))
            {
                deletionDate = parsedDate;
            }

            return Create(isToDelete, deletionDate);
        }
        
        public override string ToString()
        {
            return $"{IsToDelete};{DeletionDate?.ToString("o")}";
        }
        public bool CanDelete()
        {
            return IsToDelete && DeletionDate.HasValue && (DateTime.UtcNow - DeletionDate.Value).TotalSeconds >= 30;
        }

        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return IsToDelete;
            yield return DeletionDate;
        }
    }
}