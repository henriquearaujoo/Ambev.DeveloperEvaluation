using System.Net.Http.Json;
using System.Net;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale;
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

    [Fact]
    public async Task Post_CreateSale_Should_Return_Created()
    {
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

        var response = await _client.PostAsJsonAsync("/api/sales", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
        Assert.NotEqual(Guid.Empty, content.Data.Id);
    }

    [Fact]
    public async Task Get_Sale_Should_Return_NotFound_When_Id_Does_Not_Exist()
    {
        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/sales/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Sale_Should_Return_BadRequest_When_Id_Invalid()
    {
        var response = await _client.DeleteAsync("/api/sales/00000000-0000-0000-0000-000000000000");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
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

        // 2. Get
        var getResponse = await _client.GetAsync($"/api/sales/{created.Data.Id}");
        getResponse.EnsureSuccessStatusCode();
        var sale = await getResponse.Content.ReadFromJsonAsync<ApiResponseWithData<ApiResponseWithData<GetSaleResponse>>>();
        Assert.Equal("Full Flow Test", sale.Data.Data.Customer);

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

        var updateResponse = await _client.PutAsJsonAsync($"/api/sales/{created.Data.Id}", updateRequest);
        updateResponse.EnsureSuccessStatusCode();
        var updated = await updateResponse.Content.ReadFromJsonAsync<ApiResponseWithData<ApiResponseWithData<UpdateSaleResponse>>>();
        Assert.Equal("Updated Customer", updated.Data.Data.Customer);

        // 4. Cancel
        var cancelResponse = await _client.PutAsync($"/api/sales/cancel/{created.Data.Id}", null);
        cancelResponse.EnsureSuccessStatusCode();

        // 5. Delete
        var deleteResponse = await _client.DeleteAsync($"/api/sales/{created.Data.Id}");
        deleteResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Update_Sale_Should_Return_NotFound_When_Id_Not_Exist()
    {
        var updateRequest = new UpdateSaleRequest
        {
            Customer = "Ghost",
            Branch = "Nowhere",
            Items = new List<UpdateSaleItemDtoRequest>
            {
                new() { Product = "Ghost", Quantity = 1, UnitPrice = 100.0m }
            }
        };

        var response = await _client.PutAsJsonAsync($"/api/sales/{Guid.NewGuid()}", updateRequest);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Cancel_Sale_Should_Return_BadRequest_When_Id_Invalid()
    {
        var response = await _client.PutAsync("/api/sales/cancel/00000000-0000-0000-0000-000000000000", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_Sale_Should_Return_BadRequest_When_Id_Invalid()
    {
        var response = await _client.GetAsync("/api/sales/00000000-0000-0000-0000-000000000000");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}