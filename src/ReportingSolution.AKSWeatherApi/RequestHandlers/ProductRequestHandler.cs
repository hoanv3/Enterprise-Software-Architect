using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingSolution.AKSWeatherApi.Common.ApiDescriptions;
using ReportingSolution.Data.NoSqlModel;
using ServiceComposer.AspNetCore;

namespace ReportingSolution.AKSWeatherApi.RequestHandlers
{
    public class ProductRequestHandler : ICompositionRequestsHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProductRequestHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet("api/products/{productId}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ApiParameterDescription(Name = "productId", IsRequired = true, Type = typeof(string), Source = "Path")]
        public async Task Handle(HttpRequest request)
        {
            var vm = request.GetComposedResponseModel();
            Guard.Against.Null(request.HttpContext);
            var productId = request.HttpContext.GetRouteValue("productId");
            Guard.Against.Null(productId);
            var convertedProductId = productId.ToString();
            Guard.Against.NullOrWhiteSpace(convertedProductId, nameof(convertedProductId));

            var scope = _serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<CosmosDbContext>();

            var product = await context
                .Set<Product>()
                .FirstOrDefaultAsync(x => x.Id == convertedProductId && x.Section == "Computer",
                request.HttpContext.RequestAborted);
            vm.Product = product;
        }
    }
}
