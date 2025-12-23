# ProductManagementAPI

API REST para gestión de productos con autenticación JWT, desarrollada en .NET 8.

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Arquitectura del proyecto

El proyecto sigue una arquitectura de tres capas:

```
ProductManagementAPI/
├── Controllers/          # Capa API - Controladores REST
│   ├── ProductosController.cs
│   └── AuthController.cs
├── Services/             # Capa Aplicación - Lógica de negocio
│   ├── ProductoService.cs
│   ├── AuthService.cs
│   └── JwtService.cs
├── Models/               # Capa Dominio - Entidades
│   ├── Producto.cs
│   └── User.cs
├── DTOs/                 # Data Transfer Objects
│   ├── ProductoDto.cs
│   ├── LoginRequestDto.cs
│   └── LoginResponseDto.cs
├── Data/                 # Contexto de base de datos
│   ├── AppDbContext.cs
│   └── DbInitializer.cs
└── Program.cs            # Configuración de la aplicación
```

## Ejecutar el proyecto

1. Restaurar dependencias:

```bash
dotnet restore
```

2. Ejecutar la aplicación:

```bash
dotnet run
```

La API estará disponible en:
- HTTP: http://localhost:5112
- HTTPS: https://localhost:7224

3. Acceder a Swagger UI para probar los endpoints:

```
http://localhost:5112/swagger
```

## Endpoints disponibles

### Autenticación

| Método | Ruta | Descripción | Autenticación |
|--------|------|-------------|---------------|
| POST | `/api/users/login` | Iniciar sesión | No |
| GET | `/api/users/me` | Obtener usuario actual | JWT |

### Productos

| Método | Ruta | Descripción | Autenticación |
|--------|------|-------------|---------------|
| GET | `/api/productos` | Listar todos los productos | JWT |
| POST | `/api/productos` | Agregar un nuevo producto | JWT |

## Modelo de Producto

```json
{
  "codigo": "string (requerido)",
  "nombre": "string (requerido)",
  "precio": "decimal (requerido, mayor a 0)"
}
```

### Validaciones implementadas

- `codigo`: Requerido, no puede repetirse
- `nombre`: Requerido
- `precio`: Requerido, debe ser mayor a 0

## Ejemplo de uso

### 1. Obtener token JWT

```bash
curl -X POST http://localhost:5112/api/users/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin123"}'
```

Respuesta:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### 2. Agregar un producto

```bash
curl -X POST http://localhost:5112/api/productos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <TOKEN>" \
  -d '{"codigo": "PROD001", "nombre": "Laptop HP", "precio": 999.99}'
```

### 3. Listar productos

```bash
curl http://localhost:5112/api/productos \
  -H "Authorization: Bearer <TOKEN>"
```

## Ejecutar pruebas automatizadas

El proyecto incluye un proyecto de pruebas unitarias con xUnit.

### Ejecutar todas las pruebas

```bash
dotnet test
```

### Ejecutar pruebas con detalle

```bash
dotnet test --verbosity normal
```

### Ejecutar pruebas con cobertura de código

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Estructura de pruebas

```
ProductManagementAPI.Tests/
├── ProductManagementAPI.Tests.csproj
├── GlobalUsings.cs
└── UnitTest1.cs
```

Para agregar nuevas pruebas, cree clases de prueba en el proyecto `ProductManagementAPI.Tests` usando el atributo `[Fact]` de xUnit:

```csharp
using ProductManagementAPI.Services;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Tests;

public class ProductoServiceTests
{
    [Fact]
    public void Agregar_ProductoNuevo_RetornaExito()
    {
        // Arrange
        var service = new ProductoService();
        var producto = new Producto
        {
            Codigo = "TEST001",
            Nombre = "Producto Test",
            Precio = 100
        };

        // Act
        var (exito, mensaje) = service.Agregar(producto);

        // Assert
        Assert.True(exito);
    }

    [Fact]
    public void Agregar_ProductoDuplicado_RetornaError()
    {
        // Arrange
        var service = new ProductoService();
        var producto = new Producto
        {
            Codigo = "TEST001",
            Nombre = "Producto Test",
            Precio = 100
        };
        service.Agregar(producto);

        // Act
        var (exito, mensaje) = service.Agregar(producto);

        // Assert
        Assert.False(exito);
        Assert.Equal("Ya existe un producto con ese codigo", mensaje);
    }
}
```

## Almacenamiento

- **Productos**: Se almacenan en memoria usando un servicio Singleton. Los datos se pierden al reiniciar la aplicación.
- **Usuarios**: Se almacenan en SQLite. La base de datos `users.db` se crea automáticamente al iniciar la aplicación.

## Tecnologías utilizadas

- .NET 8
- Entity Framework Core (SQLite)
- JWT Bearer Authentication
- Swagger/OpenAPI
- xUnit (pruebas unitarias)

## Características implementadas

- [x] Endpoint POST /api/productos para guardar productos
- [x] Endpoint GET /api/productos para listar productos
- [x] Almacenamiento en memoria (Singleton)
- [x] Validación de código único de producto
- [x] Validaciones de modelo con Data Annotations
- [x] Arquitectura de tres capas (API, Aplicación, Dominio)
- [x] Protección de endpoints con JWT
