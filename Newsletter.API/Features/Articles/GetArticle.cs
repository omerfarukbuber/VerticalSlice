using Carter;
using Mapster;
using MediatR;
using Newsletter.API.Contracts;
using Newsletter.API.Database;
using Newsletter.API.Entities;
using Newsletter.API.Shared;

namespace Newsletter.API.Features.Articles;

public static class GetArticle
{
    public class Query : IRequest<Result<ArticleResponse>>
    {
        public Guid Id { get; set; }    
    }

    internal sealed class Handler(ApplicationDbContext context) : IRequestHandler<Query, Result<ArticleResponse>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<ArticleResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var response = await _context.Articles.FindAsync(request.Id, cancellationToken);
            return Result<ArticleResponse>.Success(response?.Adapt<ArticleResponse>() ?? new ArticleResponse());
        }
    }
}

public class GetArticleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/articles/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetArticle.Query() { Id = id });
            return (result.IsFailure)
                ? Results.BadRequest(result.Error)
                : Results.Ok(result);
            
        });
    }
}