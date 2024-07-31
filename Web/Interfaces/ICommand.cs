namespace Web.Interfaces;

public interface ICommand<TEntity>
{
    void Execute(TEntity entity);
}