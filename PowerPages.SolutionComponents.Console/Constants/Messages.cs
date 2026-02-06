namespace PowerPages.SolutionComponents.Console.Constants
{
    public static class Messages
    {
        public const string AppTitle = "Power Pages Solution Component Adder";
        public const string LineSeparator = "------------------------------------";
        public const string PromptEnvironmentUrl = "Enter your Power Platform Environment URL (e.g., https://org.crm.dynamics.com): ";
        public const string ErrorEnvironmentUrlRequired = "Environment URL is required.";
        public const string ConnectingToDataverse = "Connecting to Dataverse...";
        public const string ConnectedSuccessfully = "Connected successfully.";
        public const string PromptSolutionName = "Enter the Solution Unique Name: ";
        public const string ErrorSolutionNameRequired = "Solution Name is required.";
        public const string PromptSiteGuid = "Enter the Power Pages Site GUID: ";
        public const string ErrorInvalidSiteGuid = "Invalid Site GUID.";
        public const string PromptExcludeSiteSettings = "Do you want to exclude specific site settings? (y/n): ";
        public const string DonePressKey = "Done. Press any key to exit.";
        
        public const string SolutionNotFound = "Solution '{0}' not found.";
        public const string SiteNotFound = "Power Page Site with GUID {0} not found.";
        
        public const string ErrorConnectionFailed = "Failed to connect to Dataverse: {0}";
        public const string ErrorGeneral = "An error occurred: {0}";

        public const string AddingEnhancedComponents = "Adding Power Page components...";
        public const string ExcludingSiteSettings = "Excluding site settings as requested.";
        public const string SkippingSiteSetting = "Skipping site setting: {0}";
        public const string AddingPowerPageSiteRecord = "Adding Power Page Site record...";
        public const string PowerPageSiteLanguageDisplay = "Power Page Site Language";
        public const string PowerPageComponentDisplay = "Power Page Component";
        public const string EnhancedAdditionCompleted = "Power Page components addition completed.";
        
        public const string FoundRelatedEntities = "Found {0} {1}(s).";
        public const string ErrorRelatedEntities = "Error retrieving or adding {0}s: {1}";
        
        public const string AddingComponentWithType = "Adding {0} (ID: {1})";
        public const string RetrievedComponentType = "Retrieved component type {0} for {1}";
        public const string SkippingUnknownType = "Skipping {0}: Unknown component type.";
        public const string ErrorAddingToSolution = "Error adding {0} to solution: {1}";
        public const string ErrorAddingToSolutionWithType = "Error adding {0} to solution: {1} (Type: {2})";
        public const string AlreadyExists = "already exists";
    }
}
