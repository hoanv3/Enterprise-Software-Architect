using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReportingSolution.AKSWeatherApi.Common.ApiDescriptions;
using ReportingSolution.AKSWeatherApi.Common.CustomerFilters;
using ReportingSolution.Data;
using ReportingSolution.gRPC.SDK;
using ReportingSolution.Infrastructure;
using ServiceComposer.AspNetCore;

namespace ReportingSolution.AKSWeatherApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddData(builder.Configuration);
      builder.Services.AddInfrastructure();
      builder.Services.AddGrpcSdk(builder.Configuration);
      builder.Services.AddViewModelComposition();

      builder.Services.AddCors(opt =>
      {
        opt.AddPolicy("AllowFrontendApp",
                  policy =>
                  {
                policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
              });
      });

      builder.Services.AddSwaggerGen(c =>
      {
        c.OperationFilter<ETagHeaderOperationFilter>();
      });
      builder.Services.AddControllers();
      builder.Services.AddRouting();
      builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApiDescriptionProvider, ServiceComposerApiDescriptionProvider>());

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        // do something that is specific to dev environment here.
      }
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ATS v1");
      });

      app.UseCors();
      app.UseHttpsRedirection();
      app.UseAuthentication();

      app.UseRouting();

      // Replace UseEndpoints with top-level route registrations
      app.MapCompositionHandlers();
      app.MapControllers();

      app.Run();
    }
  }
}
