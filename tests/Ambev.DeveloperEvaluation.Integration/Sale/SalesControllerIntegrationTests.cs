using System.Net.Http.Json;
using System.Net;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sale;

public class SalesControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SalesControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "POST CreateSale should return Created with Sale ID")]
    public async Task Post_CreateSale_Should_Return_Created()
    {
        // Arrange
        var request = new CreateSaleRequest
        {
            Customer = "Integration Test",
            Branch = "Test Branch",
            Items = new List<CreateSaleItemDtoRequest>
            {
                new() { Product = "Item 1", Quantity = 5, UnitPrice = 10.0m },
                new() { Product = "Item 1", Quantity = 5, UnitPrice = 10.0m }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/sales", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        content.Should().NotBeNull();
        content!.Success.Should().BeTrue();
        content.Data.Id.Should().NotBe(Guid.Empty);
    }

    [Fact(DisplayName = "GET Sale should return NotFound when Sale does not exist")]
    public async Task Get_Sale_Should_Return_NotFound_When_Id_Does_Not_Exist()
    {
        // Act
        var id = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/sales/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "DELETE Sale should return BadRequest when ID is invalid")]
    public async Task Delete_Sale_Should_Return_BadRequest_When_Id_Invalid()
    {
        // Act
        var response = await _client.DeleteAsync("/api/sales/00000000-0000-0000-0000-000000000000");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Full Sale lifecycle (create, get, update, cancel, delete) should succeed")]
    public async Task Full_Sale_Lifecycle_Should_Succeed()
    {
        // 1. Create
        var createRequest = new CreateSaleRequest
        {
            Customer = "Full Flow Test",
            Branch = "Main Branch",
            Items = new List<CreateSaleItemDtoRequest>
            {
                new() { Product = "Item A", Quantity = 4, UnitPrice = 25.0m }
            }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/sales", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        created.Should().NotBeNull();
        created!.Data.Id.Should().NotBe(Guid.Empty);

        var saleId = created.Data.Id;

        // 2. Get
        var getResponse = await _client.GetAsync($"/api/sales/{saleId}");
        getResponse.EnsureSuccessStatusCode();
        var sale = await getResponse.Content.ReadFromJsonAsync<ApiResponseWithData<GetSaleResponse>>();
        sale.Should().NotBeNull();
        sale!.Data.Customer.Should().Be("Full Flow Test");

        // 3. Update
        var updateRequest = new UpdateSaleRequest
        {
            Customer = "Updated Customer",
            Branch = "Updated Branch",
            Items = new List<UpdateSaleItemDtoRequest>
            {
                new() { Product = "Item A", Quantity = 8, UnitPrice = 20.0m }
            }
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/sales/{saleId}", updateRequest);
        updateResponse.EnsureSuccessStatusCode();
        var updated = await updateResponse.Content.ReadFromJsonAsync<ApiResponseWithData<UpdateSaleResponse>>();
        updated.Should().NotBeNull();
        updated!.Data.Customer.Should().Be("Updated Customer");

        // 4. Cancel
        var cancelResponse = await _client.PutAsync($"/api/sales/cancel/{saleId}", null);
        cancelResponse.EnsureSuccessStatusCode();

        // 5. Delete
        var deleteResponse = await _client.DeleteAsync($"/api/sales/{saleId}");
        deleteResponse.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "PUT UpdateSale should return NotFound when Sale does not exist")]
    public async Task Update_Sale_Should_Return_NotFound_When_Id_Not_Exist()
    {
        // Arrange
        var updateRequest = new UpdateSaleRequest
        {
            Customer = "Ghost",
            Branch = "Nowhere",
            Items = new List<UpdateSaleItemDtoRequest>
            {
                new() { Product = "Ghost", Quantity = 1, UnitPrice = 100.0m }
            }
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/sales/{Guid.NewGuid()}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "PUT CancelSale should return BadRequest when ID is invalid")]
    public async Task Cancel_Sale_Should_Return_BadRequest_When_Id_Invalid()
    {
        // Act
        var response = await _client.PutAsync("/api/sales/cancel/00000000-0000-0000-0000-000000000000", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "GET Sale should return BadRequest when ID is invalid")]
    public async Task Get_Sale_Should_Return_BadRequest_When_Id_Invalid()
    {
        // Act
        var response = await _client.GetAsync("/api/sales/00000000-0000-0000-0000-000000000000");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}