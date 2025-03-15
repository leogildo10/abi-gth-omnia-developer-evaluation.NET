namespace Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response
{
    public class CreateSaleResponseDto
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
    }
}
