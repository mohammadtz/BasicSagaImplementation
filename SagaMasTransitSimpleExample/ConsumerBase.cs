using MassTransit;

namespace SagaMasTransitSimpleExample;

public abstract class ConsumerBase<T> : IConsumer<T> where T : class
{
    public async Task Consume(ConsumeContext<T> context)
    {
        try
        {
            await ConsumeInternal(context);
        }
        catch (Exception e)
        {
            await context.Publish<Fault<T>>(context);
            
            // global exception handling
            throw;
        }
    }

    protected abstract Task ConsumeInternal(ConsumeContext<T> context);
}