namespace ShopFront.WebApi.ChainOfResposibility.TokenValidation;

public interface IValidatorChain
{
    IValidatorChain SetNext(IValidatorChain handler);
        
    Task Validate(HttpContext context, RequestDelegate next);
}