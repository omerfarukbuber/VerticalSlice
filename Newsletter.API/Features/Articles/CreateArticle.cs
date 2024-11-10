using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Newsletter.API.Contracts;
using Newsletter.API.Database;
using Newsletter.API.Entities;
using Newsletter.API.Shared;

namespace Newsletter.API.Features.Articles;

public static class CreateArticle
{
    public class Command : IRequest<Result<Guid>>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Title).NotEmpty().WithMessage("Tittle cannot be empty");
            RuleFor(c => c.Content).NotEmpty().WithMessage("Content cannot be empty");
        }
    }


    internal sealed class Handler(ApplicationDbContext dbContext, IValidator<Command> validator) : IRequestHandler<Command, Result<Guid>>
    {
        private readonly IValidator<Command> _validator  = validator;
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<Guid>.Failure(new Error("CreateArticle.Validation", validationResult.Errors.ToString()));
            }

            var article = new Article
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                Tags = request.Tags,
                CreatedOnUtc = DateTime.UtcNow,
            };

            await _dbContext.Articles.AddAsync(article, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(article.Id);
        }
    }
}

public class CreateArticleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/articles", async (CreateArticleRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateArticle.Command>();

            var result = await sender.Send(command);

            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.Ok(result.Value);
        });
    }
}