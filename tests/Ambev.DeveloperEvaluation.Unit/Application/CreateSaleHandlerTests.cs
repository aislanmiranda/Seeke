using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
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
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleHandler(_saleRepository,_customerRepository,
            _productRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid Sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid Sale data When creating Sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var customerResponse = new Customer(Guid.NewGuid(), "Cliente1");
        var productsResponse = new List<Product> { new (Guid.NewGuid(), "Produto1") };

        var saleItems = new List<SaleItem> {
            new SaleItem(Guid.NewGuid(), "Produto1", quantity: 1, unitPrice: 100.00M)
        };

        var sale = new Sale(
            customerResponse.Id,
            customerResponse.CustomerName!,Guid.NewGuid(),"Sao Paulo");

        sale.AddItems(saleItems);

        var createSaleResult = new CreateSaleResult() {
            Id = Guid.NewGuid(),
            SaleNumber = Guid.NewGuid().ToString()
        };

        _customerRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(customerResponse);

        _productRepository.GetByIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(productsResponse);

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(createSaleResult);

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var createSaleHandlerResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleHandlerResult.Should().NotBeNull();
        createSaleHandlerResult.Id.Should().Be(createSaleHandlerResult.Id);
        createSaleHandlerResult.SaleNumber.Should().Be(createSaleHandlerResult.SaleNumber);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid Sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid Sale data When creating Sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
