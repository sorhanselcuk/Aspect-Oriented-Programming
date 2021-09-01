# Aspect-Oriented-Programming
Aspect Oriented Programming For C# .Net Core

1- How to create Aspect ?  

	-> For create aspect, first create a concrete class and this created class attend from the Aspect class.

-> Example 
	
	public class LogAspect:Aspect
	{
		//This codes
	}

2- Methods

	public class Aspect : Attribute, IAspect 
	{
		public virtual void OnAfterVoid(object arg);

        	public virtual object OnAfter(object arg);

        	public virtual void OnBeforeVoid(object[] args);

        	public virtual object OnBefore(object[] args);
	}

	void OnAfterVoid(object arg) -> After the method equipped with Aspect is running.
					Its parameter is the return value of the function equipped with Aspect.

	
	object OnAfter(object arg); -> The return value replaces the return value of the function with Aspect.


	void OnBeforeVoid(object[] args) -> Before the method equipped with Aspect is running.
					    Its parameters are the input parameters of the corresponding function.

	
	object OnBefore(object[] args) -> The function equipped with Aspect is not executed. 
					  The return value of the corresponding function is the return value from here.
3- Properties

	public class Aspect
	{
		public readonly AspectContext AspectContext;
	}
	public class AspectContext
	{
		public string MethodName { get; set; }
	        public object[] Arguments { get; set; }
	}	
	
	Class containing the information of the function equipped with Aspect
		

4- How to use ?
	
	Objects must be run through AspectProxy in order for Aspects to be activated.
	The class to initialize must have a constructor without parameters.

	public static IT AspectProxy<T>.As<IT>(); 			-> T must be class and attend from IT. IT must be interface
	public static IT AspectProxy<T>.As<IT>(object[] args); 	 	-> Function inputs for parameterized constructor classes.

	-> Example

	public class ProductManager : IProductService
	{
		public ProductManager()	
		{

		}
		public ProductManager(object arg)
		{
			//Definations
		}

		[LogAspect]
		public void Add(object arg)
		{
			//Commands
		}
	}
	
	class Program
    	{
        	static void Main(string[] args)
        	{
			IProductService instance = AspectProxy<ProductManager>.As<IProductService>(); // Parameterless Constructor

			object[] parameters = new object[]
			{
				//arguments
			}
			IProductService instanceWithParameters = AspectProxy<ProductManager>.As<IProductService>(parameters);
		}
	}
