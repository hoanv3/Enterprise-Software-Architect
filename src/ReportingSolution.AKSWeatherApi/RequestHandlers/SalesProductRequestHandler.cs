using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using ReportingSolution.AKSWeatherApi.Common.ApiDescriptions;
using ReportingSolution.Data.NoSqlModel;
using ServiceComposer.AspNetCore;

namespace ReportingSolution.AKSWeatherApi.RequestHandlers
{
    public class SalesProductRequestHandler : ICompositionRequestsHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SalesProductRequestHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet("api/products/{productId}")]
        public async Task Handle(HttpRequest request)
        {
            var vm = request.GetComposedResponseModel();
            Guard.Against.Null(request.HttpContext);
            var productId = request.HttpContext.GetRouteValue("productId");
            Guard.Against.Null(productId);
            var convertedProductId = productId.ToString();
            Guard.Against.NullOrWhiteSpace(convertedProductId, nameof(convertedProductId));
            vm.ProductId = convertedProductId;

            var scope = _serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<CosmosDbContext>();

            var salesInfo = await context
                .Set<SalesInfo>()
                .FirstOrDefaultAsync(x => x.ProductId == convertedProductId && x.Section == "computer-sales",
                request.HttpContext.RequestAborted);
            vm.SalesInfo = salesInfo;
        }
    }
}
