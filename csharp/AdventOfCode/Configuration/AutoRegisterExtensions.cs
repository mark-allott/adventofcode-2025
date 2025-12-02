using System.Reflection;
using AdventOfCode.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode.Configuration
{
	public static class AutoRegisterExtensions
	{
		/// <summary>
		/// Extension method to search the <paramref name="assemblies"/> for classes which implement the <see cref="IAutoRegister"/>
		/// interface and then register the interface and corresponding classes with DI
		/// </summary>
		/// <param name="services">The current set of services to be added to</param>
		/// <param name="assemblies">The assemblies to search</param>
		/// <returns>The updated set of services</returns>
		/// <exception cref="ArgumentException"></exception>
		public static IServiceCollection AddAutoRegistrationDailyChallenges(this IServiceCollection services, params Assembly[] assemblies)
		{
			if (assemblies.Length == 0)
				throw new ArgumentException("No assemblies selected", nameof(assemblies));

			//	Define the interface(s) to detect for auto-registration
			var interfaceTypes = new[] { typeof(IAutoRegister) };

			// Locate challenges with class inheritance of the interfaceTypes which are not abstract classes,
			// then locate all classes whose interfaces that are not in the interfaceTypes list and present
			// for auto-registration
			var autoRegistrations = assemblies
				.SelectMany(s => s.ExportedTypes)
				.Where(q => q is { IsClass: true, IsAbstract: false, BaseType: not null })
				.Select(s => new
				{
					ExportedType = s,
					ImplementedTypes = s.GetInterfaces()
						.Where(q => interfaceTypes.Contains(q))
						.ToList()
				})
				.Where(q => q.ImplementedTypes.Count > 0)
				.Select(s => new
				{
					s.ExportedType,
					Interfaces = s.ExportedType.GetInterfaces()
						.Where(q => !interfaceTypes.Contains(q))
						.ToList()
				})
				.Where(q => q.Interfaces.Count > 0)
				.ToList();

			// For the list of auto-registrations found, add each class as a transient service
			autoRegistrations.ForEach(c =>
			{
				services.AddTransient(c.ExportedType);
				c.Interfaces.ForEach(i =>
				{
					services.AddTransient(i, c.ExportedType);
				});
			});
			return services;
		}
	}
}