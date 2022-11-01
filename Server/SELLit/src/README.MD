# ASP.NET Core WEB API Template

- A ready-to-use template for ASP.NET Core WEB API with repositories, services, models mapping, DI.
- **Check Home Controller before using**

## Authors

- [Sinan Abdulgafurov](https://github.com/Sinkarq)

## Code snippets from
 
- [Nikolay Kostov](https://github.com/NikolayIT)
  - https://github.com/NikolayIT/ASP.NET-Core-Template
- [Ivaylo Kenov](https://github.com/ivaylokenov)
  - https://www.youtube.com/watch?v=TwsB9dhDbEU&list=PLLGdqRi7N09Yiq6Qo8lHAasz4ZpaqQdau

## Package Installation

You can install this template using [NuGet](https://www.nuget.org/packages/WEB-API-TEMPLATE/):

```powershell
dotnet new -i WEB-API-TEMPLATE
```

```powershell
dotnet new aspnet-core-web-api -n SELLitName
```

## Project Overview

![image](https://raw.githubusercontent.com/Sinkarq/Ideas/main/Screenshot_2.png?token=GHSAT0AAAAAABRAVTP47FKHVHIZHOBYNRZMYRKMBWQ)

### Common

**AspNetCoreTemplate.Common** contains common things for the project solution. For example:

- [GlobalConstants.cs](https://github.com/Sinkarq/WEB-API-Template/blob/master/SELLit.Common/GlobalConstants.cs).

### Data

This solution folder contains three subfolders:

- WEB-API-Template.Data.Common
- WEB-API-Template.Data.Models
- WEB-API-Template.Data

#### WEB-API-Template.Data.Common

[WEB-API-Template.Data.Common.Models](https://github.com/Sinkarq/WEB-API-Template/tree/master/Data/SELLit.Data.Common/Models) provides abstract generics classes and interfaces, which holds information about our entities. For example when the object is Created, Modified, Deleted or IsDeleted. It contains a property for the primary key as well.

[WEB-API-Template.Data.Common.Repositories](https://github.com/Sinkarq/WEB-API-Template/tree/master/Data/SELLit.Data.Common/Repositories) provides two interfaces IDeletableEntityRepository and IRepository, which are part of the **repository pattern**.

#### AspNetCoreTemplate.Data.Models

[WEB-API-Template.Data.Models](https://github.com/Sinkarq/WEB-API-Template/tree/master/Data/SELLit.Data.Models) contains models related to the database including the user class.

#### WEB-API-Template.Data

[WEB-API-Template.Data](https://github.com/Sinkarq/WEB-API-Template/tree/master/Data/SELLit.Data) contains DbContext, Migrations and Configuraitons for the EF Core. There is Seeding and Repository functionality as well.

### Services

This solution folder contains four subfolders:

- WEB-API-Template.Services.Data
- WEB-API-Template.Services.Mapping
- WEB-API-Template.Services

#### WEB-API-Template.Services.Data

[WEB-API-Template.Services.Data](https://github.com/Sinkarq/WEB-API-Template/tree/master/Services/SELLit.Services.Data) wil contains service layer logic.

#### WEB-API-Template.Services.Mapping

[WEB-API-Template.Services.Mapping](https://github.com/Sinkarq/WEB-API-Template/tree/master/Services/SELLit.Services.Mapping) provides simplified functionlity for auto mapping. For example:

```csharp
using Blog.Data.Models;
using Blog.Services.Mapping;

public class TagViewModel : IMapFrom<Tag>
{
    public int Id { get; set; }

    public string Name { get; set; }
}
```

Or if you have something specific:

```csharp
using System;

using AutoMapper;
using Blog.Data.Models;
using Blog.Services.Mapping;

public class IndexPostViewModel : IMapFrom<Post>, IHaveCustomMappings
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string ImageUrl { get; set; }

    public DateTime CreatedOn { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Post, IndexPostViewModel>()
            .ForMember(
                source => source.Author,
                destination => destination.MapFrom(member => member.ApplicationUser.UserName));
    }
}

```

#### WEB-API-Template.Services

[WEB-API-Template.Services](https://github.com/Sinkarq/WEB-API-Template/tree/master/Services/SELLit.Services)

### Web

This solution folder contains three subfolders:

- WEB-API-Template.Web.Infrastructure
- WEB-API-Template.Web.ViewModels
- WEB-API-Template.Web

#### WEB-API-Template.Web.Infrastructure

[WEB-API-Template.Web.Infrastructure](https://github.com/Sinkarq/WEB-API-Template/tree/master/Web/SELLit.Infrastructure) contains functionality like Middlewares and Filters.

#### WEB-API-Template.Web.ViewModels

[WEB-API-Template.Web.ViewModels](https://github.com/Sinkarq/WEB-API-Template/tree/master/Web/SELLit.ViewModels) contains objects, which will be mapped from/to our entities and used in the front-end/back-end.

#### AspNetCoreTemplate.Web

[WEB-API-Template.Web](https://github.com/Sinkarq/WEB-API-Template/tree/master/Web/SELLit) self explanatory.

## Support

If you are having problems, please let us know by [raising a new issue](https://github.com/Sinkarq/WEB-API-Template/issues).

## Example Projects (MVC) but self explanatory

- <https://github.com/NikolayIT/PressCenters.com>
- <https://github.com/NikolayIT/nikolay.it>

## License

This project is licensed with the [MIT license](LICENSE).
