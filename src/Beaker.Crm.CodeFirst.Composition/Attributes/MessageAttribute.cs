using System;
using System.Composition;

namespace Beaker.Crm.CodeFirst.Composition.Attributes
{
	/// <summary>
	/// Exports the class as a step that handles the provided messages
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class MessageAttribute
		: ExportAttribute
	{
		/// <summary>
		/// Initializes a new message attribute to signal that this class is a plugin step that handles the provided <paramref name="messages"/>
		/// </summary>
		/// <param name="messages">The messages this step will handle</param>
		public MessageAttribute(params string[] messages)
		{
			Messages = messages;
		}

		/// <summary>
		/// The message the step should process
		/// </summary>
		public string[] Messages { get; }
	}
}
