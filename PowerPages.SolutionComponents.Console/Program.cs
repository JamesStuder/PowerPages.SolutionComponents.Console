using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using PowerPages.SolutionComponents.Console.Constants;

namespace PowerPages.SolutionComponents.Console
{
    internal class Program
    {
        private static readonly Dictionary<string, int> _typeCache = new ();

        public static async Task Main(string[] args)
        {
            bool excludeSiteSettings = false;
            System.Console.WriteLine(Messages.AppTitle);
            System.Console.WriteLine(Messages.LineSeparator);
            
            System.Console.Write(Messages.PromptEnvironmentUrl);
            string? envUrl = System.Console.ReadLine();

            if (string.IsNullOrWhiteSpace(envUrl))
            {
                System.Console.WriteLine(Messages.ErrorEnvironmentUrlRequired);
                return;
            }

            System.Console.WriteLine(Messages.ConnectingToDataverse);
            string connectionString = string.Format(Config.ConnectionStringTemplate, envUrl);
            
            using ServiceClient serviceClient = new (connectionString);

            if (!serviceClient.IsReady)
            {
                System.Console.WriteLine(Messages.ErrorConnectionFailed, serviceClient.LastError);
                return;
            }

            System.Console.WriteLine(Messages.ConnectedSuccessfully);

            System.Console.Write(Messages.PromptSolutionName);
            string? solutionName = System.Console.ReadLine();
            if (string.IsNullOrWhiteSpace(solutionName))
            {
                System.Console.WriteLine(Messages.ErrorSolutionNameRequired);
                return;
            }

            System.Console.Write(Messages.PromptSiteGuid);
            string? siteGuidStr = System.Console.ReadLine();
            if (!Guid.TryParse(siteGuidStr, out Guid siteId))
            {
                System.Console.WriteLine(Messages.ErrorInvalidSiteGuid);
                return;
            }

            System.Console.Write(Messages.PromptExcludeSiteSettings);
            string? excludeSettingsInput = System.Console.ReadLine();
            excludeSiteSettings = excludeSettingsInput?.Trim().ToLower() == "y";

            try
            {
                await AddSiteComponentsToSolution(serviceClient, solutionName, siteId, excludeSiteSettings);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(Messages.ErrorGeneral, ex.Message);
            }

            System.Console.WriteLine(Messages.DonePressKey);
            System.Console.ReadKey();
        }

        /// <summary>
        /// Adds components of a specified Power Pages site to a solution within Dataverse.
        /// </summary>
        /// <param name="service">An instance of the <see cref="ServiceClient"/> used to interact with the Dataverse environment.</param>
        /// <param name="solutionName">The unique name of the solution to which the components of the site should be added.</param>
        /// <param name="siteId">The unique identifier (GUID) of the Power Pages site whose components are to be added to the solution.</param>
        /// <returns>A task that represents the asynchronous operation of adding the site components to the solution.</returns>
        private static async Task AddSiteComponentsToSolution(ServiceClient service, string solutionName, Guid siteId, bool excludeSiteSettings)
        {
            using OrganizationServiceContext context = new (service);
            
            Entity? solution = context.CreateQuery(Tables.Solution).FirstOrDefault(s => s.GetAttributeValue<string>(Fields.UniqueName) == solutionName);

            if (solution == null)
            {
                System.Console.WriteLine(Messages.SolutionNotFound, solutionName);
                return;
            }
            
            try
            {
                await service.RetrieveAsync(Tables.PowerPageSite, siteId, new ColumnSet(Fields.PowerPageSiteId));
            }
            catch
            {
                System.Console.WriteLine(Messages.SiteNotFound, siteId);
                return;
            }

            await AddEnhancedComponents(service, solutionName, siteId, excludeSiteSettings);
        }

        /// <summary>
        /// Adds enhanced components of a specified Power Pages site to a solution within Dataverse.
        /// </summary>
        /// <param name="service">An instance of the <see cref="ServiceClient"/> used to interact with the Dataverse environment.</param>
        /// <param name="solutionName">The unique name of the solution to which the enhanced components of the site should be added.</param>
        /// <param name="siteId">The unique identifier (GUID) of the Power Pages site whose enhanced components are to be added to the solution.</param>
        /// <returns>A task that represents the asynchronous operation of adding the enhanced components to the solution.</returns>
        private static async Task AddEnhancedComponents(ServiceClient service, string solutionName, Guid siteId, bool excludeSiteSettings)
        {
            System.Console.WriteLine(Messages.AddingEnhancedComponents);
            if (excludeSiteSettings)
            {
                System.Console.WriteLine(Messages.ExcludingSiteSettings);
            }

            System.Console.WriteLine(Messages.AddingPowerPageSiteRecord);
            await AddToSolution(service, solutionName, siteId, Tables.PowerPageSite);
            await AddRelatedLanguages(service, solutionName, siteId);
            await AddRelatedComponents(service, solutionName, siteId, excludeSiteSettings);

            System.Console.WriteLine(Messages.EnhancedAdditionCompleted);
        }

        /// <summary>
        /// Adds the related language components of a specified Power Pages site to a solution in Dataverse.
        /// </summary>
        /// <param name="service">An instance of the <see cref="ServiceClient"/> used to access and interact with the Dataverse environment.</param>
        /// <param name="solutionName">The unique name of the solution to which the related language components should be added.</param>
        /// <param name="siteId">The unique identifier (GUID) of the Power Pages site whose related language components are to be added to the solution.</param>
        /// <returns>A task that represents the asynchronous operation of adding the related language components to the solution.</returns>
        private static async Task AddRelatedLanguages(ServiceClient service, string solutionName, Guid siteId)
        {
            try
            {
                using OrganizationServiceContext context = new (service);
                List<Entity> languages = context.CreateQuery(Tables.PowerPageSiteLanguage)
                    .Where(l => l.GetAttributeValue<EntityReference>(Fields.PowerPageSiteId) != null && l.GetAttributeValue<EntityReference>(Fields.PowerPageSiteId).Id == siteId)
                    .ToList();

                System.Console.WriteLine(Messages.FoundRelatedEntities, languages.Count, Messages.PowerPageSiteLanguageDisplay);

                foreach (Entity lang in languages)
                {
                    await AddToSolution(service, solutionName, lang.Id, Tables.PowerPageSiteLanguage);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(Messages.ErrorRelatedEntities, Messages.PowerPageSiteLanguageDisplay, ex.Message);
            }
        }

        /// <summary>
        /// Adds related Power Pages components associated with a specified Power Pages site to a solution within Dataverse.
        /// </summary>
        /// <param name="service">An instance of the <see cref="ServiceClient"/> used to interact with the Dataverse environment.</param>
        /// <param name="solutionName">The unique name of the solution to which the related Power Pages components should be added.</param>
        /// <param name="siteId">The unique identifier (GUID) of the Power Pages site whose related components are to be added to the solution.</param>
        /// <returns>A task that represents the asynchronous operation of adding the related Power Pages components to the solution.</returns>
        private static async Task AddRelatedComponents(ServiceClient service, string solutionName, Guid siteId, bool excludeSiteSettings)
        {
            try
            {
                using OrganizationServiceContext context = new (service);
                List<Entity> components = context.CreateQuery(Tables.PowerPageComponent)
                    .Where(c => c.GetAttributeValue<EntityReference>(Fields.PowerPageSiteId) != null && c.GetAttributeValue<EntityReference>(Fields.PowerPageSiteId).Id == siteId)
                    .ToList();

                int addedCount = 0;
                foreach (Entity comp in components)
                {
                    if (excludeSiteSettings)
                    {
                        OptionSetValue? typeValueOptionSet = comp.GetAttributeValue<OptionSetValue>(Fields.PowerPageComponentType);
                        if (typeValueOptionSet != null && typeValueOptionSet.Value == PowerPageComponentTypeValues.SiteSetting)
                        {
                            string? name = comp.GetAttributeValue<string>(Fields.Name);
                            if (!string.IsNullOrEmpty(name) && ExcludedSettings.ExcludedSiteSettingPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                            {
                                System.Console.WriteLine(Messages.SkippingSiteSetting, name);
                                continue;
                            }
                        }
                    }

                    await AddToSolution(service, solutionName, comp.Id, Tables.PowerPageComponent, comp);
                    addedCount++;
                }

                System.Console.WriteLine(Messages.FoundRelatedEntities, addedCount, Messages.PowerPageComponentDisplay);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(Messages.ErrorRelatedEntities, Messages.PowerPageComponentDisplay, ex.Message);
            }
        }

        /// <summary>
        /// Adds a specific component to a solution in Dataverse with optional record details.
        /// </summary>
        /// <param name="service">An instance of the <see cref="ServiceClient"/> used to interact with the Dataverse environment.</param>
        /// <param name="solutionName">The unique name of the solution to which the component should be added.</param>
        /// <param name="componentId">The unique identifier (GUID) of the component to be added to the solution.</param>
        /// <param name="entityLogicalName">The logical name of the entity to which the component belongs.</param>
        /// <param name="record">An optional <see cref="Entity"/> object containing additional details about the record being added. Can be null if not required.</param>
        /// <returns>A task that represents the asynchronous operation of adding the component to the solution.</returns>
        private static async Task AddToSolution(ServiceClient service, string solutionName, Guid componentId, string entityLogicalName, Entity? record = null)
        {
            int componentType = -1;
            try
            {
                componentType = await GetComponentType(service, entityLogicalName, record);
                if (componentType == -1)
                {
                    System.Console.WriteLine(Messages.SkippingUnknownType, entityLogicalName);
                    return;
                }

                AddSolutionComponentRequest request = new ()
                {
                    ComponentId = componentId,
                    ComponentType = componentType, 
                    SolutionUniqueName = solutionName,
                    AddRequiredComponents = false,
                    DoNotIncludeSubcomponents = true 
                };

                try
                {
                    await service.ExecuteAsync(request);
                }
                catch (System.ServiceModel.FaultException ex) when (ex.Message.Contains("DoNotIncludeSubcomponents"))
                {
                    request.DoNotIncludeSubcomponents = false;
                    await service.ExecuteAsync(request);
                }
            }
            catch (System.ServiceModel.FaultException ex)
            {
                if (!ex.Message.Contains(Messages.AlreadyExists) && ex.HResult != ErrorCodes.ComponentAlreadyExists)
                {
                    System.Console.WriteLine(Messages.ErrorAddingToSolutionWithType, entityLogicalName, ex.Message, componentType);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(Messages.ErrorAddingToSolution, entityLogicalName, ex.Message);
            }
        }

        /// <summary>
        /// Determines the component type for a given entity in the Dataverse environment.
        /// </summary>
        /// <param name="service">An instance of the <see cref="ServiceClient"/> used to interact with the Dataverse environment.</param>
        /// <param name="entityLogicalName">The logical name of the entity whose component type is being retrieved.</param>
        /// <param name="record">An optional instance of the <see cref="Entity"/> representing the entity record to retrieve specific component type information if applicable.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the integer value representing the component type, or -1 if the type cannot be determined.</returns>
        private static async Task<int> GetComponentType(ServiceClient service, string entityLogicalName, Entity? record = null)
        {
            if (entityLogicalName == Tables.PowerPageComponent && record != null)
            {
                OptionSetValue? typeValueOptionSet = record.GetAttributeValue<OptionSetValue>(Fields.PowerPageComponentType);
                if (typeValueOptionSet != null)
                {
                    int typeValue = typeValueOptionSet.Value;
                    System.Console.WriteLine(Messages.AddingComponentWithType, PowerPageComponentNames.GetTypeName(typeValue), record.Id);
                }
            }

            if (_typeCache.TryGetValue(entityLogicalName, out int cachedType))
            {
                return cachedType;
            }
            
            RetrieveEntityRequest request = new ()
            {
                LogicalName = entityLogicalName,
                EntityFilters = EntityFilters.Entity
            };
            RetrieveEntityResponse response = (RetrieveEntityResponse)await service.ExecuteAsync(request);
            int type = response.EntityMetadata.ObjectTypeCode ?? -1;
            
            if (type != -1)
            {
                System.Console.WriteLine(Messages.RetrievedComponentType, type, entityLogicalName);
                _typeCache[entityLogicalName] = type;
                return type;
            }

            int fallbackType = entityLogicalName switch
            {
                Tables.PowerPageSite => ComponentTypes.PowerPageSite,
                Tables.PowerPageComponent => ComponentTypes.PowerPageComponent,
                Tables.PowerPageSiteLanguage => ComponentTypes.PowerPageSiteLanguage,
                _ => -1 
            };
            
            return fallbackType;
        }
    }
}