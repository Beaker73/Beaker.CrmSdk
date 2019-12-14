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

	MethodReference GetConstructor(TypeReference type, params TypeReference[] parameterTypes)
	{
		var mType = ModuleDefinition.ImportReference(type);

		// ensure the parameter types are imported
		TypeDefinition[] importedParameterTypes = parameterTypes.Select(pt => ModuleDefinition.ImportReference(pt).Resolve()).ToArray();

		var query = mType.Resolve().Methods
			.Where(m => m.IsConstructor)
			.Where(m => m.Parameters.Count == parameterTypes.Length);
		for (int i = 0; i < parameterTypes.Length; i++)
		{
			int ci = i;
			query = query.Where(m => m.Parameters[ci].ParameterType.Resolve() == importedParameterTypes[ci]);
		}

		// get the constructor and import it
		MethodDefinition method = query.Single();
		return ModuleDefinition.ImportReference(method);
	}

	MethodReference GetConstructor<T1>(TypeReference type)
		=> GetConstructor(type, ModuleDefinition.ImportReference(typeof(T1)));

	MethodReference GetMethod(GenericInstanceType genericType, string methodName)
	{
		// get the method and import it
		TypeDefinition genericDefinition = genericType.Resolve();
		MethodDefinition method = genericDefinition.Methods.Single(m => m.Name == methodName);
		MethodReference methodReference = ModuleDefinition.ImportReference(method);

		// do not forget to set declaring type to the generic type !!!
		// during import it is lost...
		methodReference.DeclaringType = genericType;

		// and again, import the method so it can be used.
		return ModuleDefinition.ImportReference(methodReference);
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
	MethodReference GetMethod<TReturn, TParam1, TParam2>(GenericInstanceType genericType, string methodName)
		=> GetMethod(genericType, methodName, ModuleDefinition.ImportReference(typeof(TReturn)), ModuleDefinition.ImportReference(typeof(TParam1)), ModuleDefinition.ImportReference(typeof(TParam2)));

	bool IsSameType(GenericInstanceType genericType, TypeReference param, TypeReference expectedParam)
	{
		if (param.IsGenericParameter && param is GenericParameter gp)
			return genericType.GenericArguments[gp.Position].Resolve() == expectedParam;
		if (param is ByReferenceType br && br.ElementType.ContainsGenericParameter)
			return IsSameType(genericType, br.ElementType, expectedParam);

		return param.Resolve() == expectedParam;
	}

	void AddAttributeCore(ICustomAttributeProvider attributeProvider, string attrName, TypeReference[] parameterTypes, object[] arguments)
	{
		// resolve attribute type name name
		var typeDef = FindType(attrName);
		var typeRef = ModuleDefinition.ImportReference(typeDef);

		var exists = attributeProvider.CustomAttributes.SingleOrDefault(c => c.AttributeType.Resolve() == typeRef.Resolve());
		if (!(exists is null))
			attributeProvider.CustomAttributes.Remove(exists);


		// create attribute via constructor
		var ctor = GetConstructor(typeRef, parameterTypes);
		var ca = new CustomAttribute(ctor);

		if (!(parameterTypes is null))
		{
			for (int i = 0; i < parameterTypes.Length; i++)
				ca.ConstructorArguments.Add(new CustomAttributeArgument(ModuleDefinition.ImportReference(parameterTypes[i]), arguments[i]));
		}

		attributeProvider.CustomAttributes.Add(ca);
	}

	void AddAttribute(ICustomAttributeProvider attributeProvider, string attrName)
		=> AddAttributeCore(attributeProvider, attrName, new TypeReference[0], new object[0]);
	void AddAttribute<T1>(ICustomAttributeProvider attributeProvider, string attrName, T1 arg1)
		=> AddAttributeCore(attributeProvider, attrName, new[] { ImportType<T1>() }, new object[] { arg1 });


	bool IsNullableType(TypeReference possibleNullableType, out TypeReference baseType)
	{
		if(possibleNullableType.IsGenericInstance 
			&& possibleNullableType is GenericInstanceType git 
			&& git.GenericArguments.Count == 1 
			&& git.ElementType.FullName == "System.Nullable`1")
		{
			baseType = git.GenericArguments[0].GetElementType();
			return true;
		}

		baseType = null;
		return false;
	}
}
