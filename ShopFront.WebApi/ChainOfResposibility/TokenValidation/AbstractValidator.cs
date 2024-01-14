namespace ShopFront.WebApi.ChainOfResposibility.TokenValidation;

public abstract class AbstractValidator : IValidatorChain
{
    protected IValidatorChain _nextValiator;

    public IValidatorChain SetNext(IValidatorChain valiator)
    {
        _nextValiator = valiator;

        return valiator;
    }
    
    public virtual Task Validate(HttpContext context, RequestDelegate next)
    {
        if (_nextValiator != null)
        {
            return _nextValiator.Validate(context, next);
        }
        
        return null;
        
    }
}