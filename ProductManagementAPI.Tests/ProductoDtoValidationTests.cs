using System.ComponentModel.DataAnnotations;
using ProductManagementAPI.DTOs;

namespace ProductManagementAPI.Tests;

public class ProductoDtoValidationTests
{
    private List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }

    #region Codigo Validation Tests

    [Fact]
    public void Codigo_Vacio_RetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "",
            Nombre = "Producto Test",
            Precio = 100m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.Contains(results, r => r.MemberNames.Contains("Codigo"));
    }

    [Fact]
    public void Codigo_Null_RetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = null!,
            Nombre = "Producto Test",
            Precio = 100m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.Contains(results, r => r.MemberNames.Contains("Codigo"));
    }

    [Fact]
    public void Codigo_Valido_NoRetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "PROD001",
            Nombre = "Producto Test",
            Precio = 100m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.DoesNotContain(results, r => r.MemberNames.Contains("Codigo"));
    }

    #endregion

    #region Nombre Validation Tests

    [Fact]
    public void Nombre_Vacio_RetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "PROD001",
            Nombre = "",
            Precio = 100m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.Contains(results, r => r.MemberNames.Contains("Nombre"));
    }

    [Fact]
    public void Nombre_Null_RetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "PROD001",
            Nombre = null!,
            Precio = 100m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.Contains(results, r => r.MemberNames.Contains("Nombre"));
    }

    [Fact]
    public void Nombre_Valido_NoRetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "PROD001",
            Nombre = "Producto Test",
            Precio = 100m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.DoesNotContain(results, r => r.MemberNames.Contains("Nombre"));
    }

    #endregion

    #region Precio Validation Tests

    [Fact]
    public void Precio_Cero_RetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "PROD001",
            Nombre = "Producto Test",
            Precio = 0m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.Contains(results, r => r.MemberNames.Contains("Precio"));
    }

    [Fact]
    public void Precio_Negativo_RetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "PROD001",
            Nombre = "Producto Test",
            Precio = -10m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.Contains(results, r => r.MemberNames.Contains("Precio"));
    }

    [Fact]
    public void Precio_MayorACero_NoRetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "PROD001",
            Nombre = "Producto Test",
            Precio = 0.01m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.DoesNotContain(results, r => r.MemberNames.Contains("Precio"));
    }

    [Fact]
    public void Precio_ValorAlto_NoRetornaErrorValidacion()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "PROD001",
            Nombre = "Producto Test",
            Precio = 999999.99m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.DoesNotContain(results, r => r.MemberNames.Contains("Precio"));
    }

    #endregion

    #region Complete Validation Tests

    [Fact]
    public void DtoCompleto_Valido_NoRetornaErrores()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "PROD001",
            Nombre = "Producto Test",
            Precio = 100m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void DtoCompleto_TodosLosCamposInvalidos_RetornaMultiplesErrores()
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = "",
            Nombre = "",
            Precio = 0m
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.True(results.Count >= 3);
    }

    [Theory]
    [InlineData("A", "Producto", 1)]
    [InlineData("CODIGO123", "Nombre del Producto", 999.99)]
    [InlineData("X", "Y", 0.01)]
    public void DtoCompleto_ValoresVariados_SonValidos(string codigo, string nombre, decimal precio)
    {
        // Arrange
        var dto = new ProductoDto
        {
            Codigo = codigo,
            Nombre = nombre,
            Precio = precio
        };

        // Act
        var results = ValidateModel(dto);

        // Assert
        Assert.Empty(results);
    }

    #endregion
}
