using System;
using Tyr.Domain.ServiceAggregate;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.AppointmentAggregate;
using Xunit;

namespace Tyr.Tests.Domain
{
    public class DomainEntityTests
    {
        [Fact]
        public void Service_InvalidDuration_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Service("Haircut", 50m, 0));
        }

        [Fact]
        public void Service_InvalidPrice_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Service("Haircut", 0m, 30));
        }

        [Fact]
        public void Customer_InvalidName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Customer(null!, "+123456789"));
        }

        [Fact]
        public void Customer_InvalidPhone_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Customer("Gustavo Resende", null!));
        }

        [Fact]
        public void Appointment_UpdateStatus_SetsStatusAndUpdatedAt()
        {
            // Arrange
            var service = new Service("Haircut", 50m, 30);
            var customer = new Customer("Gustavo Resende", "+5511999999999");
            var start = DateTime.UtcNow;
            var appointment = new Appointment(customer.Id, service.Id, start);

            // Act
            appointment.UpdateStatus(AppointmentStatus.Confirmed);

            // Assert
            Assert.Equal(AppointmentStatus.Confirmed, appointment.Status);
            Assert.True(appointment.UpdatedAt.HasValue);
            Assert.True(appointment.UpdatedAt.Value > appointment.CreatedAt);
        }

        [Fact]
        public void Appointment_UpdateStart_UpdatesStartAndUpdatedAt()
        {
            // Arrange
            var service = new Service("Haircut", 50m, 30);
            var customer = new Customer("Gustavo Resende", "+5511999999999");
            var start = DateTime.UtcNow;
            var appointment = new Appointment(customer.Id, service.Id, start);

            var newStart = start.AddHours(1);
    
            // Act
            appointment.UpdateStart(newStart);

            // Assert
            Assert.Equal(newStart, appointment.StartDateTime);
            Assert.True(appointment.UpdatedAt.HasValue);
            Assert.True(appointment.UpdatedAt.Value > appointment.CreatedAt);
        }
    }
}
