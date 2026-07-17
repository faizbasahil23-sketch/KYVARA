namespace Kyvara.Builder.Engine;

public sealed class CsprojGenerator
{
    public string Generate(
        string projectName,
        string sdk = "Microsoft.NET.Sdk")
    {
        return $$"""
<Project Sdk="{{sdk}}">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>

    <Nullable>enable</Nullable>

    <ImplicitUsings>enable</ImplicitUsings>

    <RootNamespace>{{projectName}}</RootNamespace>

    <AssemblyName>{{projectName}}</AssemblyName>

    <LangVersion>preview</LangVersion>

    <TreatWarningsAsErrors>false</TreatWarningsAsErrors>

  </PropertyGroup>

</Project>
""";
    }
}
