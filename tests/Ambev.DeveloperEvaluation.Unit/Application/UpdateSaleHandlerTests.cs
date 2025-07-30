using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
/// Contains unit tests for the <see cref="UpdateSaleHandler"/> class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateSaleHandler(_saleRepository,_customerRepository,
            _productRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid Sale updating request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid Sale data When updating Sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var customerResponse = new Customer(Guid.NewGuid(), "Cliente1");
        var productsResponse = new List<Product> { new (Guid.NewGuid(), "Produto1") };

        var saleItems = new List<SaleItem> {
            new SaleItem(Guid.NewGuid(), "Produto1", quantity: 1, unitPrice: 100.00M)
        };

        var sale = new Sale(
            customerResponse.Id,
            customerResponse.CustomerName!, branchId: Guid.NewGuid(), branchName: "Sao Paulo");

        sale.UpdateItems(saleItems);

        var UpdateSaleResult = new UpdateSaleResult() {
            Id = Guid.NewGuid(),
            SaleNumber = Guid.NewGuid().ToString()
        };

        _customerRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(customerResponse);

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns(sale);

        _productRepository.GetByIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
           .Returns(productsResponse);

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<UpdateSaleResult>(sale).Returns(UpdateSaleResult);

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var UpdateSaleHandlerResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        UpdateSaleHandlerResult.Should().NotBeNull();
        UpdateSaleHandlerResult.Id.Should().Be(UpdateSaleHandlerResult.Id);
        UpdateSaleHandlerResult.SaleNumber.Should().Be(UpdateSaleHandlerResult.SaleNumber);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid Sale updating request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid Sale data When updating Sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new UpdateSaleCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
