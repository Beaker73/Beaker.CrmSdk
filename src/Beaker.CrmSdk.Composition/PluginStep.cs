using Microsoft.Xrm.Sdk;

namespace Beaker.CrmSdk.Composition
{
	/// <summary>
	///		Base class for a plugin step that handles the target <typeparamref name="TEntity"/>
	/// </summary>
	/// <typeparam name="TEntity">The type of the target entity this plugin step opperates on</typeparam>
	public abstract class PluginStep<TEntity>
		: IPluginStep
	{
		void IPluginStep.Execute(IPluginExecutionContext context)
		{
			if (!context.InputParameters.TryGetValue("Target", out object target) || !(target is Entity))
				throw new InvalidPluginExecutionException(OperationStatus.Failed, "Plugin executed without target entity, while plugin expects one.");
			if (!(target is TEntity targetEntity))
				throw new InvalidPluginExecutionException(OperationStatus.Failed, "Plugin executed with target entity of an unexpected type.");

			Execute(targetEntity);
		}

		/// <summary>
		/// The pre image entity
		/// </summary>
		protected TEntity PreImage { get; }

		/// <summary>
		/// The post image entity
		/// </summary>
		protected TEntity PostImage { get; }

		/// <summary>
		/// Called to execute the plugin step
		/// </summary>
		/// <param name="target">The target entity to be handled</param>
		protected abstract void Execute(TEntity target);
	}
}
