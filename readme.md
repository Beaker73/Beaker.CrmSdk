# Beaker.CrmSdk

``` CSharp
/**
** This SDK is still a Work in Progress and not yet ready to be used in a project.
**/
```

This SDK aims to work together with the default Microsft.CrmSdk and add some sorely missed missing features. Every one of these features can be used seperatly without the need to include the other packages. The main features provided by this SDK are:

- Code First for CRM
- Composition via MEF (works in sandbox)
- Automatic CRM Solution generation

## Code First

Code first aims to allow you to write a bare class using pure C# and then generate the code that is normally generated via the CrmSvcUtil or other means. This is done during compile time, using code weaving via a [Fody](https://github.com/Fody/Fody) nuget package.

Add the `Beaker.CrmSdk.CodeFirst.Fody` nuget package, and you'll be able to write the following code:

``` CSharp
[Entity]
public sealed class Something: Entity
{
	[Required]
	[StringLength(50)]
	public string Name { get; set; }

	[Range(0, 50)]
	public int LengthOfName {get; set;} = 0;
}
```

Setting this `Name` property will set the underlying `my_name` attribute in the `Attributes` collection of the underlying `Entity` base class. Apply required attributes, like AttributeLogicalName. Add validation of the attributes on setting the value.

You can mix your own CodeFirst entities with generate early bound entities as needed. While CodeFirst can be used as-is, it will be realy code-first, in combination with automatic solution generation.

## Composition

Composition aims to add dependency injection and a lot of handy base classes that help with implementing plug-in steps quickly without almost no effort.
Creating a step for the previous Something entity could be done like this.

``` CSharp
[Step("Create", "Update")]
public sealed CountCharactersStep: StepBase<Something>
{
	protected override Execute(Something target) {
		target.LengthOfName = target.Name?.Length ?? 0;
	}
}
```

This will create a class that implements a step on the Something Entity during Create and Update.
Using DI the `CompositionPlugin` base class will find this step automaticly when the correct Create or Update message arrives for the target `Something`.

If you need any of the default interfaces like `IOrganizationService` or `IPluginExecutionContext` you can simply inject them via the constructor and use them as normal.

## CRM Solution generation

While the Microsoft CrmSdk provides us with the solution packager tool, setting up or modifing the XML for the Solution is a very complex and error prone task. The `Beaker.CrmSdk.Solution` package aims to make this as automatic as possible. 

Link a C# Project with a plugin and steps and it will be added as a plugin into the solution. The metadata attributes used to register the steps are also use to automaticly build the whole plugin registration information needed. All projects and packages referenced by the plugin will be merged via ILMerge into a single assembly.

Link a C# Project with code first entities and the solution will add the entities and register them into CRM, result in a real code first experience. Again the metadata attributes applied on the C# roperties is passed on to the CRM Solution to configure the entities in CRM.

## Unit Testing

While unit testing plugins is already possible using projects like [FakeXrmEasy](https://github.com/jordimontana82/fake-xrm-easy) and in compination with Beaker.CrmSdk.Composition and dependeny injection. Some things require you to test them during a real Sandbox Environment. Therefore whe supply you a SandboxBuilder to run the plugin test inside of.

We also supply serveral other handy builders to setup all the interface the plugin needs to work and be fully tested. Like the `PluginExecutionContextBuilder`, `ServiceProviderBuilder` and the `WorkflowActivityBuilder`

``` CSharp
ISandbox sandbox = new SandboxBuilder()
	.ForPlugin<MyPlugin>()
	.Build();

sandbox.Execute(serviceProvider);
```