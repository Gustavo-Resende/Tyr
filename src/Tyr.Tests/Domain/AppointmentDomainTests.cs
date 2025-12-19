using System;
using Tyr.Domain.ServiceAggregate;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.AppointmentAggregate;
using Xunit;

namespace Tyr.Tests
{
    public class AppointmentDomainTests
    {
        [Fact]
        public void CreatingAppointment_FromServiceAndCustomer_SetsEndTimeAndPendingStatus()
        {
            // Arrange
            var service = new Service("Haircut", 50m, 30, "Basic haircut");
            var customer = new Customer("Gustavo Resende", "+5573999999999", "gustavo@example.com");
            var start = DateTime.UtcNow;

            // Act
            var appointment = new Appointment(customer.Id, service.Id, start);
            appointment.CalculateEndDateTime(service.DurationInMinutes);
             
            // Assert
            Assert.Equal(customer.Id, appointment.CustomerId);
            Assert.Equal(service.Id, appointment.ServiceId);
            Assert.Equal(start, appointment.StartDateTime);
            Assert.Equal(start.AddMinutes(service.DurationInMinutes), appointment.EndDateTime);
            Assert.Equal(AppointmentStatus.Pending, appointment.Status);
            Assert.NotEqual(default(DateTime), appointment.CreatedAt);
        }
    }
}
