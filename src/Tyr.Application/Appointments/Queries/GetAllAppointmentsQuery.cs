using Ardalis.Result;
using MediatR;
using Tyr.Application.Appointments.Dtos;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.Appointments.Extensions;
using Tyr.Domain.AppointmentAggregate.Specifications;

namespace Tyr.Application.Appointments.Queries
{
    public record GetAllAppointmentsQuery : IRequest<Result<IReadOnlyList<AppointmentOutputDto>>>;

    public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, Result<IReadOnlyList<AppointmentOutputDto>>>
    {
        private readonly IReadRepository<Appointment> _repository;

        public GetAllAppointmentsQueryHandler(IReadRepository<Appointment> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IReadOnlyList<AppointmentOutputDto>>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var spec = new AppointmentsWithDetailsSpec();
            var list = await _repository.ListAsync(spec, cancellationToken);
            var dtos = list.Select(a => a.ToDto()).ToList();
            return Result.Success<IReadOnlyList<AppointmentOutputDto>>(dtos.AsReadOnly());
        }
    }
}
