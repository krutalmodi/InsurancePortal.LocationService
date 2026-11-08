namespace InsurancePortal.LocationService.Application.Abstractions
{
    public interface ICommandHandler<in TCommand, TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
