namespace AssetInfoService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapGet("/assetInfo", (HttpContext httpContext) =>
            {
                
                var assetInfo = Enumerable
                                .Range(1, 5)
                                .Aggregate(new List<Vector3DTO>(), (list, item) =>
                                {
                                    var assetInfo = AssetInfo.Create2DAssetInfo(item, item + 10, item, item + 10);
                                    list.AddRange(Vector3DTO.FromAssetInfoPos(assetInfo));
                                    return list;
                                }).ToArray();
                return assetInfo;
            });

            app.Run();
        }
    }
}