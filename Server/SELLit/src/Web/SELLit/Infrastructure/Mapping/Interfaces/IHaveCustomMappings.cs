using AutoMapper;

namespace SELLit.Server.Infrastructure.Mapping.Interfaces;

public interface IHaveCustomMappings
{
    void CreateMappings(IProfileExpression configuration);
}