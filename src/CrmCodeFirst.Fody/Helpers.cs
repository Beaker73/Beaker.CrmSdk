using Mono.Cecil;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

partial class ModuleWeaver
{
	GenericInstanceType MakeGenericType<T1>(string typeName)
		=> MakeGenericType(typeName, ModuleDefinition.ImportReference(typeof(T1)));
	GenericInstanceType MakeGenericType<T1, T2>(string typeName)
		=> MakeGenericType(typeName, ModuleDefinition.ImportReference(typeof(T1)), ModuleDefinition.ImportReference(typeof(T2)));

	GenericInstanceType MakeGenericType(string typeName, params TypeReference[] genericArguments)
	{
		// find the generic type name
		int argCount = genericArguments.Length;
		TypeDefinition typeReference = FindType(typeName + "`" + argCount.ToString(CultureInfo.InvariantCulture));
		// import it and resolve it
		TypeDefinition typeDefinition = ModuleDefinition.ImportReference(typeReference).Resolve();

		// create a generic version of it
		GenericInstanceType genericType = new GenericInstanceType(typeDefinition);
		foreach (TypeReference genericArg in genericArguments)
			genericType.GenericArguments.Add(genericArg);

		return genericType;
	}

	MethodReference GetMethod(GenericInstanceType genericType, string methodName, TypeReference returnType, params TypeReference[] parameterTypes)
	{
		// ensure the return and parameter types are imported
		TypeDefinition retType = ModuleDefinition.ImportReference(returnType).Resolve();
		TypeDefinition[] importedParameterTypes = parameterTypes.Select(pt => ModuleDefinition.ImportReference(pt).Resolve()).ToArray();

		// build the query to find the method
		TypeDefinition genericDefinition = genericType.Resolve();
		IEnumerable<MethodDefinition> query = genericDefinition.Methods
			.Where(m => m.Name == methodName)
			.Where(m => IsSameType(genericType, m.ReturnType, retType))
			.Where(m => m.Parameters.Count == parameterTypes.Length);
		for (int i = 0; i < parameterTypes.Length; i++)
		{
			int ci = i;
			query = query.Where(m => IsSameType(genericType, m.Parameters[ci].ParameterType, importedParameterTypes[ci]));
		}

		// get the method and import it
		MethodDefinition method = query.Single();
		MethodReference methodReference = ModuleDefinition.ImportReference(method);

		// do not forget to set declaring type to the generic type !!!
		// during import it is lost...
		methodReference.DeclaringType = genericType;

		// and again, import the method so it can be used.
		return ModuleDefinition.ImportReference(methodReference);
	}

	MethodReference GetMethod<TReturn>(GenericInstanceType genericType, string methodName)
		=> GetMethod(genericType, methodName, ModuleDefinition.ImportReference(typeof(TReturn)));

	MethodReference GetMethod<TReturn, TParam1>(GenericInstanceType genericType, string methodName)
	=> GetMethod(genericType, methodName, ModuleDefinition.ImportReference(typeof(TReturn)), ModuleDefinition.ImportReference(typeof(TParam1)));

	bool IsSameType(GenericInstanceType genericType, TypeReference param, TypeReference expectedParam)
	{
		if (param.IsGenericParameter && param is GenericParameter gp)
			return genericType.GenericArguments[gp.Position].Resolve() == expectedParam;
		return param.Resolve() == expectedParam;
	}

	TypeReference Import<T>() => ModuleDefinition.ImportReference(typeof(T));
}
