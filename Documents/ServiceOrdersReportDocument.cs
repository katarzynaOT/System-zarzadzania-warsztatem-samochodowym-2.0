using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using WorkshopManager.Models;

namespace WorkshopManager.Documents
{
    public class ServiceOrdersReportDocument
    {
        private readonly List<ServiceOrder> _orders;

        public ServiceOrdersReportDocument(List<ServiceOrder> orders)
        {
            _orders = orders ?? new List<ServiceOrder>();
        }

        public byte[] GeneratePdf()
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Raport zleceń serwisowych")
                        .FontSize(20).Bold();

                    page.Content().Column(column =>
                    {
                        foreach (var order in _orders)
                        {
                            column.Item().Text(text =>
                            {
                                text.Span("Customer: ");
                                text.Span($"{order.Car?.Customer?.Name} {order.Car?.Customer?.LastName}").Italic();
                            });

                            column.Item().Text(text =>
                            {
                                text.Span("Car: ");
                                text.Span(order.Car?.Name ?? "-").Italic();
                            });

                            column.Item().Text(text =>
                            {
                                text.Span("Description: ");
                                text.Span(order.Description ?? "-").Italic();
                            });

                            column.Item().Text(text =>
                            {
                                text.Span("Completed Date: ");
                                text.Span(order.CompletedDate ?? "-").Italic();
                            });

                            column.Item().PaddingVertical(10).LineHorizontal(0.5f);
                        }

                        if (_orders.Count == 0)
                        {
                            column.Item().Text(":(").Italic();
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Generated: ");
                            x.Span(DateTime.Now.ToString("g")).SemiBold();
                        });
                });
            });

            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }
    }
}
