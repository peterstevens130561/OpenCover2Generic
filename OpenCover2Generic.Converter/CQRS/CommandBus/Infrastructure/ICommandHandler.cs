namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Infrastructure
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// executes a POST operation
        /// </summary>
        /// <param name="command"></param>
        void Execute(TCommand command);
    }
}