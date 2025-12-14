namespace CrudAuthenAuthortruyenthong.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "có lỗi xảy ra");


                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        Success = false,
                        Message = "Có lỗi xảy ra",
                        Error = ex.Message
                    };

                    await context.Response.WriteAsJsonAsync(response);
                }
            }
        }
    }
}
