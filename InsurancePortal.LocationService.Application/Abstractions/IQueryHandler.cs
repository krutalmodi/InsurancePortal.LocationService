namespace InsurancePortal.LocationService.Application.Abstractions
{
    public interface IQueryHandler<in TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
