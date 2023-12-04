using BarberShop.Controllers;
using BarberShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace ShopTests;

public class ClientControllerTests
{
    [Fact]
    public void List_ReturnsViewWithClients()
    {
        // Arrange
        var dbContext = GetDbContextWithClients();
        var controller = new ClientController(dbContext);

        // Act
        var result = controller.List(search: null) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as List<Client>;
        Assert.NotNull(model);
        Assert.Equal(10, model.Count); 
    }

    [Fact]
    public void AddClient_InvalidModel_ReturnsView()
    {
        // Arrange
        var dbContext = GetDbContextWithClients();
        var controller = new ClientController(dbContext);
        controller.ModelState.AddModelError("Name", "Name is required"); 

        // Act
        var result = controller.AddClient(new Client()) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Client>(result.Model);
    }

    [Fact]
    public void AddClient_ValidModel_RedirectsToListAction()
    {
        // Arrange
        var dbContext = GetDbContextWithClients();
        var controller = new ClientController(dbContext);
        var clientToAdd = new Client { Name = "New Client", Pronouns = "They/Them", Number = "789" };

        // Act
        var result = controller.AddClient(clientToAdd) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("List", result.ActionName);
    }

    [Fact]
    public void EditClient_ExistingClient_ReturnsViewWithClient()
    {
        // Arrange
        var dbContext = GetDbContextWithClients();
        var controller = new ClientController(dbContext);
        var clientId = 1; 

        // Act
        var result = controller.EditClient(clientId) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as Client;
        Assert.NotNull(model);
        Assert.Equal(clientId, model.ClientId);
    }

    [Fact]
    public void EditClient_NonExistingClient_ReturnsNotFound()
    {
        // Arrange
        var dbContext = GetDbContextWithClients();
        var controller = new ClientController(dbContext);
        var nonExistingClientId = 999; 

        // Act
        var result = controller.EditClient(nonExistingClientId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteClient_ExistingClient_RedirectsToListAction()
    {
        // Arrange
        var dbContext = GetDbContextWithClients();
        var controller = new ClientController(dbContext);
        var clientId = 1;

        // Act
        var result = controller.DeleteClient(clientId) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("List", result.ActionName);
    }

    [Fact]
    public void DeleteClient_NonExistingClient_ReturnsNotFound()
    {
        // Arrange
        var dbContext = GetDbContextWithClients();
        var controller = new ClientController(dbContext);
        var nonExistingClientId = 999; 

        // Act
        var result = controller.DeleteClient(nonExistingClientId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private AppDbContext GetDbContextWithClients()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}") 
            .Options;

        var dbContext = new AppDbContext(options);

        dbContext.Clients.AddRange(new[]
        {
        new Client { ClientId = 1, Name = "Client1", Pronouns = "They/Them", Number = "123" },
        new Client { ClientId = 2, Name = "Client2", Pronouns = "She/Her", Number = "456" }
    });

        // Ensure unique keys for test data
        for (int i = 3; i <= 10; i++)
        {
            dbContext.Clients.Add(new Client
            {
                ClientId = i,
                Name = $"Client{i}",
                Pronouns = $"Pronouns{i}",
                Number = $"{i * 111}"
            });
        }

        dbContext.SaveChanges();

        return dbContext;
    }


}
