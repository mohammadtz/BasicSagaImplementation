namespace BasicSagaImplementation.Exceptions;

public class SagaException : Exception
{
    public SagaException(string orderProcessingFailed, Exception exception) : base(orderProcessingFailed, exception)
    {

    }
}