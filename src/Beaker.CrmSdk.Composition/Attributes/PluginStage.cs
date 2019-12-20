namespace Beaker.CrmSdk.Composition.Attributes
{
	/// <summary>
	///     Stage a plugin can execute during
	/// </summary>
	public enum PluginStage
	{
		/// <summary>
		///		Execute before any work is being done on the database level
		/// </summary>
		/// <remarks>
		///		Security Note: This stage is done BEFORE any permissions are validated. 
		///		Thus the user is authenticated, but might not be authorized.
		/// </remarks>

		PreEvent = 10,
		/// <summary>
		///		Execute inside database transaction, but before the main action
		/// </summary>
		/// <remarks>
		///		Any changes made to the target entity are persisted
		/// </remarks>
		PreOperation = 20,

		/// <summary>
		///		Execute inside database transaction (if not async), and after the main action
		/// </summary>
		/// <remarks>
		///		Any changes made to the target entity are not persisted.
		///		This is the default stage applied, when no stage attribute is set on the plugin step.
		/// </remarks>
		PostOperation = 40,
	}
}