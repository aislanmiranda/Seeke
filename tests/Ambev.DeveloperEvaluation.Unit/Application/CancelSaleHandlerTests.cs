using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleHandler"/> class.
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CancelSaleHandler(_saleRepository,_customerRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid Sale cancel request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid Sale data When cancel Sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CancelSaleHandlerTestData.GenerateValidCommand();
        var customerResponse = new Customer(Guid.NewGuid(), "Cliente1");
        

        var saleItems = new List<SaleItem> {
            new SaleItem(Guid.NewGuid(), "Produto1", quantity: 1, unitPrice: 100.00M)
        };

        var sale = new Sale(
            customerResponse.Id,
            customerResponse.CustomerName!, branchId: Guid.NewGuid(), branchName: "Sao Paulo");

        sale.AddItems(saleItems);

        sale.CancelSale();

        var CancelSaleResult = new CancelSaleResult() {            
            SaleNumber = sale.SaleNumber,
            CustomerName = sale.CustomerName,
            IsCanceled = sale.IsCanceled
        };

        _customerRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(customerResponse);

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<CancelSaleResult>(sale).Returns(CancelSaleResult);

        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var CancelSaleHandlerResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        CancelSaleHandlerResult.Should().NotBeNull();
        CancelSaleHandlerResult.SaleNumber.Should().Be(CancelSaleResult.SaleNumber);
        CancelSaleHandlerResult.IsCanceled.Should().Be(CancelSaleResult.IsCanceled);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid Sale cancel request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid Sale data When cancel Sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CancelSaleCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
