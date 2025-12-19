using Microsoft.EntityFrameworkCore;
using Tyr.Application.Appointments.Commands;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServiceAggregate;
using Tyr.Infrastructure.Persistence;

namespace Tyr.Tests.Application
{
    public class CreateAppointmentFlowTests : IDisposable
    {
        private readonly AppDbContext _db;
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<Service> _serviceRepo;
        private readonly IRepository<BusinessHour> _bhRepo;

        public CreateAppointmentFlowTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TyrTestDb")
                .Options;

            _db = new AppDbContext(options);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            _appointmentRepo = new EfRepository<Appointment>(_db);
            _customerRepo = new EfRepository<Customer>(_db);
            _serviceRepo = new EfRepository<Service>(_db);
            _bhRepo = new EfRepository<BusinessHour>(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        [Fact]
        public async Task FullFlow_CreateAppointmentForHaircut_Succeeds()
        {
            // Arrange
            var service = new Service("Haircut", 50m, 30);
            await _serviceRepo.AddAsync(service, CancellationToken.None);

            var customer = new Customer("Gustavo Resende", "+5511999999999", "gustavo@example.com");
            await _customerRepo.AddAsync(customer, CancellationToken.None);

            var start = DateTime.UtcNow.Date.AddDays(1).AddHours(9); // next day 9am (assume weekday)
            var bh = new BusinessHour(start.DayOfWeek, TimeSpan.FromHours(8), TimeSpan.FromHours(18));
            await _bhRepo.AddAsync(bh, CancellationToken.None);

            await _serviceRepo.SaveChangesAsync(CancellationToken.None);
            await _customerRepo.SaveChangesAsync(CancellationToken.None);
            await _bhRepo.SaveChangesAsync(CancellationToken.None);

            var handler = new CreateAppointmentCommandHandler(
                (IRepository<Appointment>)_appointmentRepo,
                (IReadRepository<Service>)_serviceRepo,
                (IReadRepository<Customer>)_customerRepo,
                (IReadRepository<BusinessHour>)_bhRepo);

            var command = new CreateAppointmentCommand(start, customer.Id, service.Id, "Haircut please");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var dto = result.Value!;

            Assert.Equal(command.StartDateTime, dto.StartDateTime);
            Assert.Equal(command.Notes, dto.Notes);
            Assert.Equal(start.AddMinutes(service.DurationInMinutes), dto.EndDateTime);
            Assert.Equal(AppointmentStatus.Pending.ToString(), dto.Status);
        }

        [Fact]
        public async Task CreateAppointment_OutsideBusinessHours_Fails()
        {
            // Arrange
            var service = new Service("Haircut", 50m, 30);
            await _serviceRepo.AddAsync(service, CancellationToken.None);

            var customer = new Customer("Gustavo Resende", "+5511999999999", "gustavo@example.com");
            await _customerRepo.AddAsync(customer, CancellationToken.None);

            var start = DateTime.UtcNow.Date.AddDays(1).AddHours(9); // 9am but business starts 10am
            var bh = new BusinessHour(start.DayOfWeek, TimeSpan.FromHours(10), TimeSpan.FromHours(12));
            await _bhRepo.AddAsync(bh, CancellationToken.None);

            await _serviceRepo.SaveChangesAsync(CancellationToken.None);
            await _customerRepo.SaveChangesAsync(CancellationToken.None);
            await _bhRepo.SaveChangesAsync(CancellationToken.None);

            var handler = new CreateAppointmentCommandHandler(
                (IRepository<Appointment>)_appointmentRepo,
                (IReadRepository<Service>)_serviceRepo,
                (IReadRepository<Customer>)_customerRepo,
                (IReadRepository<BusinessHour>)_bhRepo);

            var command = new CreateAppointmentCommand(start, customer.Id, service.Id, null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Start time outside business hours.", result.Errors.First());
        }

        [Fact]
        public async Task CreateAppointment_Conflict_Fails()
        {
            // Arrange
            var service = new Service("Haircut", 50m, 60);
            await _serviceRepo.AddAsync(service, CancellationToken.None);

            var customer = new Customer("Gustavo Resende", "+5511999999999", "gustavo@example.com");
            await _customerRepo.AddAsync(customer, CancellationToken.None);

            var bh = new BusinessHour(DateTime.UtcNow.Date.AddDays(1).DayOfWeek, TimeSpan.FromHours(8), TimeSpan.FromHours(18));
            await _bhRepo.AddAsync(bh, CancellationToken.None);

            await _serviceRepo.SaveChangesAsync(CancellationToken.None);
            await _customerRepo.SaveChangesAsync(CancellationToken.None);
            await _bhRepo.SaveChangesAsync(CancellationToken.None);

            var start = DateTime.UtcNow.Date.AddDays(1).AddHours(9);

            // create existing appointment that will conflict
            var existing = new Appointment(customer.Id, service.Id, start);
            existing.CalculateEndDateTime(service.DurationInMinutes);
            await _appointmentRepo.AddAsync(existing, CancellationToken.None);
            await _appointmentRepo.SaveChangesAsync(CancellationToken.None);

            var handler = new CreateAppointmentCommandHandler(
                (IRepository<Appointment>)_appointmentRepo,
                (IReadRepository<Service>)_serviceRepo,
                (IReadRepository<Customer>)_customerRepo,
                (IReadRepository<BusinessHour>)_bhRepo);

            var command = new CreateAppointmentCommand(start, customer.Id, service.Id, null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Time slot is not available.", result.Errors.First());
        }
    }
}
