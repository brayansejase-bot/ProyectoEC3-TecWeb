using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using CourtReservation_Core.Entities;
using System.Globalization;
using CourtReservation_Infraestructure.Dto_s;

namespace CourtReservation_Infraestructure.Validators
{
    public class ReservaDtoValidator : AbstractValidator<ReservaDto>

    {
        public ReservaDtoValidator()
        {
            // Regla para Id: Debe ser mayor a 0 
            RuleFor(x => x.Id)
                .GreaterThan(0).When(x => x.Id != 0) 
                .WithMessage("El ID debe ser mayor a 0.");
            // Regla para UsuarioId: Debe ser mayor a 0
            RuleFor(x => x.UsuarioId)
                .GreaterThan(0)
                .WithMessage("El ID del usuario es requerido y debe ser mayor a 0.");
            // Regla para CanchaId: Debe ser mayor a 0
            RuleFor(x => x.CanchaId).GreaterThan(0)
            .WithMessage("El ID de la cancha es requerido y debe ser mayor a 0.");
        // Regla para FechaInicio: Debe ser en el futuro
            RuleFor(x => x.FechaInicio)
            .GreaterThan(DateTime.Now)
            .WithMessage("La fecha de inicio debe ser en el futuro.");
        // Regla para FechaFin: Debe ser después de FechaInicio
        RuleFor(x => x.FechaFin)
            .GreaterThan(x => x.FechaInicio)
            .WithMessage("La fecha de fin debe ser después de la fecha de inicio.");
        // Regla para Estado: Debe ser un valor válido y no vacío
        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("El estado es requerido.")
            .Must(BeAValidState)
            .WithMessage("El estado debe ser uno de los siguientes: Reservada, Cancelada, Completada o Pagada.");
        // Regla para Notas: Máximo 500 caracteres
        RuleFor(x => x.Notas)
            .MaximumLength(500)
            .WithMessage("Las notas no pueden exceder 500 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Notas)); // Solo valida si Notas no es nulo o vacío
    }
        private bool BeAValidState(string state)
        {
            if (string.IsNullOrEmpty(state)) return false;
            var validStates = new[] { "Reservada", "Cancelada", "Completada", "Pagada" };
            return validStates.Contains(state, StringComparer.OrdinalIgnoreCase); 
        }
    }
}
