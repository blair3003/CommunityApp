using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Moq;

namespace CommunityApp.Tests.UnitTests
{
    public class PaymentServiceTests : IDisposable
    {
        private readonly Mock<IPaymentRepository> _mockRepository;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _mockRepository = new Mock<IPaymentRepository>();
            _paymentService = new PaymentService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllPaymentsAsync_ReturnsAllPayments()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new() { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) },
                new() { Id = 2, LeaseId = 2, Amount = 150.00m, PaymentDate = new DateTime(2023, 1, 1) },
                new() { Id = 3, LeaseId = 3, Amount = 200.00m, PaymentDate = new DateTime(2023, 1, 1) }
            };

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(payments);

            // Act
            var result = await _paymentService.GetAllPaymentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, p => p.Amount == 100.00m);
            Assert.Contains(result, p => p.Amount == 150.00m);
            Assert.Contains(result, p => p.Amount == 200.00m);

            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ReturnsPayment()
        {
            // Arrange
            var expectedPayment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedPayment);

            // Act
            var result = await _paymentService.GetPaymentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100.00m, result.Amount);

            _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ReturnsNull_WhenPaymentDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Payment?)null);

            // Act
            var result = await _paymentService.GetPaymentByIdAsync(999);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task AddPaymentAsync_CreatesPayment()
        {
            // Arrange
            var newPayment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            _mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment p) => p);

            // Act
            var result = await _paymentService.AddPaymentAsync(newPayment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(100.00m, result.Amount);

            _mockRepository.Verify(repo => repo.AddAsync(It.Is<Payment>(h => h == newPayment)), Times.Once);
        }

        [Fact]
        public async Task UpdatePaymentAsync_ModifiesPayment()
        {
            // Arrange
            var updatedPayment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            _mockRepository
                .Setup(repo => repo.UpdateAsync(1, It.IsAny<Payment>()))
                .ReturnsAsync(updatedPayment);

            // Act
            var result = await _paymentService.UpdatePaymentAsync(1, updatedPayment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100.00m, result.Amount);

            _mockRepository.Verify(repo => repo.UpdateAsync(1, updatedPayment), Times.Once);
        }

        [Fact]
        public async Task UpdatePaymentAsync_ReturnsNull_WhenIdMismatch()
        {
            // Arrange
            var payment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            _mockRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Payment>()))
                .ReturnsAsync((Payment?)null);

            // Act
            var result = await _paymentService.UpdatePaymentAsync(999, payment);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(repo => repo.UpdateAsync(999, It.IsAny<Payment>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePaymentAsync_ReturnsNull_WhenPaymentDoesNotExist()
        {
            // Arrange
            var payment = new Payment { Id = 999, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            _mockRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Payment>()))
                .ReturnsAsync((Payment?)null);

            // Act
            var result = await _paymentService.UpdatePaymentAsync(999, payment);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(repo => repo.UpdateAsync(999, It.IsAny<Payment>()), Times.Once);
        }

        [Fact]
        public async Task DeletePaymentAsync_RemovesPayment()
        {
            // Arrange
            var deletedPayment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            _mockRepository
                .Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(deletedPayment);

            // Act
            var result = await _paymentService.DeletePaymentAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100.00m, result.Amount);

            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeletePaymentAsync_ReturnsNull_WhenPaymentDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((Payment?)null);

            // Act
            var result = await _paymentService.DeletePaymentAsync(999);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(repo => repo.DeleteAsync(999), Times.Once);
        }

        public void Dispose()
        {
            _mockRepository.Reset();
        }
    }
}
