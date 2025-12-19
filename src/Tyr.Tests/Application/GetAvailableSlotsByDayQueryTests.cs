using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tyr.Infrastructure.Persistence;
using Tyr.Domain.ServiceAggregate;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Application.BusinessHours.Queries;
using Tyr.Domain.Interfaces;
using Xunit;

namespace Tyr.Tests.Application
{
    public class GetAvailableSlotsByDayQueryTests : IDisposable
    {
        private readonly AppDbContext _db;
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IRepository<Service> _serviceRepo;
        private readonly IRepository<BusinessHour> _bhRepo;

        public GetAvailableSlotsByDayQueryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _db = new AppDbContext(options);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            _appointmentRepo = new EfRepository<Appointment>(_db);
            _serviceRepo = new EfRepository<Service>(_db);
            _bhRepo = new EfRepository<BusinessHour>(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        [Fact]
        public async Task BusinessClosed_ReturnsError()
        {
            // Arrange: pick a day with no business hours
            var targetDay = DateTime.UtcNow.Date.AddDays(1).DayOfWeek;

            var handler = new GetAvailableSlotsByDayQueryHandler(
                (IReadRepository<BusinessHour>)_bhRepo,
                (IReadRepository<Appointment>)_appointmentRepo,
                (IReadRepository<Service>)_serviceRepo);

            // Act
            var result = await handler.Handle(new GetAvailableSlotsByDayQuery(targetDay, null), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Business is closed on that day.", result.Errors.First());
        }

        [Fact]
        public async Task ServiceNotFound_ReturnsError()
        {
            // Arrange
            var targetDate = DateTime.UtcNow.Date.AddDays(1);
            var dayOfWeek = targetDate.DayOfWeek;
            var bh = new BusinessHour(dayOfWeek, TimeSpan.FromHours(8), TimeSpan.FromHours(18));
            await _bhRepo.AddAsync(bh, CancellationToken.None);
            await _bhRepo.SaveChangesAsync(CancellationToken.None);

            var handler = new GetAvailableSlotsByDayQueryHandler(
                (IReadRepository<BusinessHour>)_bhRepo,
                (IReadRepository<Appointment>)_appointmentRepo,
                (IReadRepository<Service>)_serviceRepo);

            // Act
            var result = await handler.Handle(new GetAvailableSlotsByDayQuery(dayOfWeek, Guid.NewGuid()), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Service not found.", result.Errors.First());
        }

        [Fact]
        public async Task AvailableSlots_ReturnsSlots_WhenNoAppointments()
        {
            // Arrange
            var targetDate = DateTime.UtcNow.Date.AddDays(1);
            var dayOfWeek = targetDate.DayOfWeek;

            var service = new Service("Haircut", 50m, 30);
            await _serviceRepo.AddAsync(service, CancellationToken.None);

            var bh = new BusinessHour(dayOfWeek, TimeSpan.FromHours(8), TimeSpan.FromHours(10));
            await _bhRepo.AddAsync(bh, CancellationToken.None);

            await _serviceRepo.SaveChangesAsync(CancellationToken.None);
            await _bhRepo.SaveChangesAsync(CancellationToken.None);

            var handler = new GetAvailableSlotsByDayQueryHandler(
                (IReadRepository<BusinessHour>)_bhRepo,
                (IReadRepository<Appointment>)_appointmentRepo,
                (IReadRepository<Service>)_serviceRepo);

            // Act
            var result = await handler.Handle(new GetAvailableSlotsByDayQuery(dayOfWeek, service.Id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var slots = result.Value!;
            Assert.NotEmpty(slots);
            // first slot should be business start
            Assert.Contains(TimeSpan.FromHours(8), slots);
        }

        [Fact]
        public async Task ServiceDurationGreaterThan30_IsCappedTo30()
        {
            // Arrange
            var targetDate = DateTime.UtcNow.Date.AddDays(1);
            var dayOfWeek = targetDate.DayOfWeek;

            var service = new Service("LongService", 100m, 90); // 90 minutes
            await _serviceRepo.AddAsync(service, CancellationToken.None);

            var bh = new BusinessHour(dayOfWeek, TimeSpan.FromHours(8), TimeSpan.FromHours(10));
            await _bhRepo.AddAsync(bh, CancellationToken.None);

            await _serviceRepo.SaveChangesAsync(CancellationToken.None);
            await _bhRepo.SaveChangesAsync(CancellationToken.None);

            var handler = new GetAvailableSlotsByDayQueryHandler(
                (IReadRepository<BusinessHour>)_bhRepo,
                (IReadRepository<Appointment>)_appointmentRepo,
                (IReadRepository<Service>)_serviceRepo);

            // Act
            var result = await handler.Handle(new GetAvailableSlotsByDayQuery(dayOfWeek, service.Id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var slots = result.Value!;
            // with business hours 8-10 and slot length capped to 30, there should be slots at 8:00 and 8:15 and 8:30 and 8:45 etc until 9:30
            Assert.Contains(TimeSpan.FromHours(8), slots);
            Assert.Contains(TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(15)), slots);
        }
    }
}
