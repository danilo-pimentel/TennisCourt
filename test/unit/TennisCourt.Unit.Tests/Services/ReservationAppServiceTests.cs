using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourt.Application.AutoMapper;
using TennisCourt.Application.DTO;
using TennisCourt.Application.Services;
using TennisCourt.Domain.Enums;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Models;
using TennisCourt.Infra.Data.Context;
using TennisCourt.Infra.Data.Repositories;
using Xunit;
using Xunit.Sdk;

namespace TennisCourt.Unit.Tests.Services
{

    public class ReservationAppServiceTests
    {

        [Fact]
        public void GetAllReservations()
        {
            // Arrange
            var reservations = new List<Domain.Models.Reservation> {
                new Domain.Models.Reservation() {
                    Id = Guid.Parse("098D908F-32A1-4D10-88CB-E15A712CF5E3"), Name = "Test1", Phone = "61 99999-9999", Value = 50, RefundValue = 0,
                    ReservationStatus = Domain.Enums.ReservationStatusEnum.READY_TO_PLAY, Date = DateTime.Now
                },
                new Domain.Models.Reservation() {
                    Id = Guid.Parse("0E667271-8F5C-4F3B-9C3B-AA98E159B4CD"), Name = "Test2", Phone = "61 88888-8888", Value = 100, RefundValue = 0,
                    ReservationStatus = Domain.Enums.ReservationStatusEnum.READY_TO_PLAY, Date = DateTime.Now
                }
            }.AsQueryable();

            var dbSetMock = new Mock<DbSet<Domain.Models.Reservation>>();
            dbSetMock.As<IQueryable<Domain.Models.Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Domain.Models.Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Domain.Models.Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Domain.Models.Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.AsQueryable().GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Domain.Models.Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = sut.GetAllReservations().Result;

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(Guid.Parse("098D908F-32A1-4D10-88CB-E15A712CF5E3"), result.ToList()[0].Id);
            Assert.Equal(Guid.Parse("0E667271-8F5C-4F3B-9C3B-AA98E159B4CD"), result.ToList()[1].Id);
        }

        [Fact]
        public async void GetReservation()
        {

            // Arrange
            var reservations = GetReservationList();

            var testObject = reservations[1];

            var dbSetMock = new Mock<DbSet<Domain.Models.Reservation>>();
            dbSetMock.As<IQueryable<Domain.Models.Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Domain.Models.Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Domain.Models.Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Domain.Models.Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());
            
            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Domain.Models.Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.GetReservation(new ReservationDetailDTO() { Id = testObject.Id });

            // Assert
            Assert.Equal(result.Id, testObject.Id);
            Assert.Equal(result.Name, testObject.Name);
            Assert.Equal(result.Phone, testObject.Phone);
            Assert.Equal(result.Value, testObject.Value);
            Assert.Equal(result.RefundValue, testObject.RefundValue);
            Assert.Equal(result.ReservationStatus, testObject.ReservationStatus.ToString());
            Assert.Equal(result.Date, testObject.Date);

        }

        [Fact]
        public async void ProcessReservation()
        {

            // Arrange
            var reservations = GetReservationList();

            var reservation = new ReservationCreateDTO()
            {
                Name = "Test3",
                Phone = "61 5555-5555",
                Value = 70,
                Date = DateTime.Parse("2022/11/28 12:00")
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.ProcessReservation(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, reservation.Name);
            Assert.Equal(result.Phone, reservation.Phone);
            Assert.Equal(result.Value, reservation.Value);
            Assert.Equal(result.ReservationStatus, ReservationStatusEnum.READY_TO_PLAY.ToString());
            Assert.Equal(result.Date, reservation.Date);
            Assert.Equal(0, result.RefundValue);
        }

        [Fact]
        public async void ProcessReservationConflictedBefore()
        {

            // Arrange
            var reservations = GetReservationList();

            var reservation = new ReservationCreateDTO()
            {
                Name = "Test3",
                Phone = "61 5555-5555",
                Value = 70,
                Date = DateTime.Parse("2022/11/28 09:30")
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.ProcessReservation(reservation);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void ProcessReservationConflictedAfter()
        {

            // Arrange
            var reservations = GetReservationList();

            var reservation = new ReservationCreateDTO()
            {
                Name = "Test3",
                Phone = "61 5555-5555",
                Value = 70,
                Date = DateTime.Parse("2022/11/28 11:30")
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.ProcessReservation(reservation);

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public async void ProcessReservationConflictedExact()
        {

            // Arrange
            var reservations = GetReservationList();

            var reservation = new ReservationCreateDTO()
            {
                Name = "Test3",
                Phone = "61 5555-5555",
                Value = 70,
                Date = DateTime.Parse("2022/11/28 10:00")
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.ProcessReservation(reservation);

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public async void CancelReservation()
        {

            // Arrange
            var reservations = GetReservationList();

            var testObject = new ReservationCancelDTO()
            {
                Id = reservations[1].Id,
                RefundValue = 15
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.CancelReservation(testObject);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, testObject.Id);
            Assert.Equal(result.ReservationStatus, ReservationStatusEnum.CANCELED.ToString());
            Assert.Equal(result.RefundValue, testObject.RefundValue);

        }

        [Fact]
        public async void CancelReservationNotFound()
        {

            // Arrange
            var reservations = GetReservationList();

            var testObject = new ReservationCancelDTO()
            {
                Id = Guid.NewGuid(),
                RefundValue = 25
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.CancelReservation(testObject);

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public async void CancelReservationAlreadyCanceled()
        {

            // Arrange
            var reservations = GetReservationList();

            var testObject = new ReservationCancelDTO()
            {
                Id = reservations[2].Id,
                RefundValue = 25
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.CancelReservation(testObject);

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public async void RescheduleReservation()
        {

            // Arrange
            var reservations = GetReservationList();

            var reservation = new ReservationRescheduleDTO()
            {
                Id = reservations[0].Id,
                NewDate = DateTime.Parse("2022/11/28 09:30")
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.RescheduleReservation(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, reservation.Id);
            Assert.Equal(result.Date, reservation.NewDate);
        }

        [Fact]
        public async void RescheduleReservationConflictBefore()
        {

            // Arrange
            var reservations = GetReservationList();

            var reservation = new ReservationRescheduleDTO()
            {
                Id = reservations[0].Id,
                NewDate = DateTime.Parse("2022/11/28 10:30")
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.RescheduleReservation(reservation);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void RescheduleReservationConflictAfter()
        {

            // Arrange
            var reservations = GetReservationList();

            var reservation = new ReservationRescheduleDTO()
            {
                Id = reservations[0].Id,
                NewDate = DateTime.Parse("2022/11/28 11:30")
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.RescheduleReservation(reservation);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void RescheduleReservationConflictExact()
        {

            // Arrange
            var reservations = GetReservationList();

            var reservation = new ReservationRescheduleDTO()
            {
                Id = reservations[0].Id,
                NewDate = DateTime.Parse("2022/11/28 11:00")
            };

            var dbSetMock = new Mock<DbSet<Reservation>>();
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Provider).Returns(reservations.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.Expression).Returns(reservations.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.ElementType).Returns(reservations.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Reservation>>().Setup(x => x.GetEnumerator()).Returns(reservations.GetEnumerator());

            var context = new Mock<TennisCourtContext>();
            context.Setup(x => x.Set<Reservation>()).Returns(dbSetMock.Object);

            var configurationProvider = AutoMapperConfig.RegisterMappings();
            IMapper mapper = configurationProvider.CreateMapper();

            var repository = new ReservationRepository(context.Object);

            // Act
            var sut = new ReservationAppService(repository, mapper);
            var result = await sut.RescheduleReservation(reservation);

            // Assert
            Assert.Null(result);
        }

        private List<Reservation> GetReservationList()
        {
            return new List<Domain.Models.Reservation> {
                new Domain.Models.Reservation()
                {
                    Id = Guid.Parse("098D908F-32A1-4D10-88CB-E15A712CF5E3"),
                    Name = "Test1",
                    Phone = "61 99999-9999",
                    Value = 50,
                    RefundValue = 0,
                    ReservationStatus = Domain.Enums.ReservationStatusEnum.READY_TO_PLAY,
                    Date = DateTime.Parse("2022/11/28 10:00")
                },
                new Domain.Models.Reservation()
                {
                    Id = Guid.Parse("0E667271-8F5C-4F3B-9C3B-AA98E159B4CD"),
                    Name = "Test2",
                    Phone = "61 88888-8888",
                    Value = 100,
                    RefundValue = 0,
                    ReservationStatus = Domain.Enums.ReservationStatusEnum.READY_TO_PLAY,
                    Date = DateTime.Parse("2022/11/28 11:00")
                },
                new Domain.Models.Reservation()
                {
                    Id = Guid.Parse("2D634364-6D5A-4D19-BCFD-7240570D7F00"),
                    Name = "Test3",
                    Phone = "61 33333-3333",
                    Value = 30,
                    RefundValue = 0,
                    ReservationStatus = Domain.Enums.ReservationStatusEnum.CANCELED,
                    Date = DateTime.Parse("2022/11/28 19:00")
                }
            };
        }

    }
}
