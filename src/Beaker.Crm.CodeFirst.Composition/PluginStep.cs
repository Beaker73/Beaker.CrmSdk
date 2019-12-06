namespace Beaker.Crm.CodeFirst.Composition
{
	/// <summary>
	/// Base class for a plugin step that handles the target <typeparamref name="TEntity"/>
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public abstract class PluginStep<TEntity>
	{
		/// <summary>
		/// Called to execute the plugin step
		/// </summary>
		/// <param name="target">The target entity to be handled</param>
		protected abstract void Execute(TEntity target);
	}
}
