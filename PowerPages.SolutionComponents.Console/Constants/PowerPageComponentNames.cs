namespace PowerPages.SolutionComponents.Console.Constants
{
    public static class PowerPageComponentNames
    {
        public const string PublishingState = "Publishing State";
        public const string WebPage = "Web Page";
        public const string WebFile = "Web File";
        public const string WebLinkSet = "Web Link Set";
        public const string WebLink = "Web Link";
        public const string PageTemplate = "Page Template";
        public const string ContentSnippet = "Content Snippet";
        public const string WebTemplate = "Web Template";
        public const string SiteSetting = "Site Setting";
        public const string WebPageAccessControlRule = "Web Page Access Control Rule";
        public const string WebRole = "Web Role";
        public const string WebsiteAccess = "Website Access";
        public const string SiteMarker = "Site Marker";
        public const string BasicForm = "Basic Form";
        public const string BasicFormMetadata = "Basic Form Metadata";
        public const string List = "List";
        public const string TablePermission = "Table Permission";
        public const string AdvancedForm = "Advanced Form";
        public const string AdvancedFormStep = "Advanced Form Step";
        public const string AdvancedFormMetadata = "Advanced Form Metadata";
        public const string PollPlacement = "Poll Placement";
        public const string AdPlacement = "Ad Placement";
        public const string BotConsumer = "Bot Consumer";
        public const string ColumnPermissionProfile = "Column Permission Profile";
        public const string ColumnPermission = "Column Permission";
        public const string Redirect = "Redirect";
        public const string PublishingStateTransitionRule = "Publishing State Transition Rule";
        public const string Shortcut = "Shortcut";
        public const string CloudFlow = "Cloud Flow";
        public const string UxComponent = "UX Component";
        public const string ServerLogic = "Server Logic";
        public const string Unknown = "Unknown";

        public static string GetTypeName(int typeValue)
        {
            return typeValue switch
            {
                PowerPageComponentTypeValues.PublishingState => PublishingState,
                PowerPageComponentTypeValues.WebPage => WebPage,
                PowerPageComponentTypeValues.WebFile => WebFile,
                PowerPageComponentTypeValues.WebLinkSet => WebLinkSet,
                PowerPageComponentTypeValues.WebLink => WebLink,
                PowerPageComponentTypeValues.PageTemplate => PageTemplate,
                PowerPageComponentTypeValues.ContentSnippet => ContentSnippet,
                PowerPageComponentTypeValues.WebTemplate => WebTemplate,
                PowerPageComponentTypeValues.SiteSetting => SiteSetting,
                PowerPageComponentTypeValues.WebPageAccessControlRule => WebPageAccessControlRule,
                PowerPageComponentTypeValues.WebRole => WebRole,
                PowerPageComponentTypeValues.WebsiteAccess => WebsiteAccess,
                PowerPageComponentTypeValues.SiteMarker => SiteMarker,
                PowerPageComponentTypeValues.BasicForm => BasicForm,
                PowerPageComponentTypeValues.BasicFormMetadata => BasicFormMetadata,
                PowerPageComponentTypeValues.List => List,
                PowerPageComponentTypeValues.TablePermission => TablePermission,
                PowerPageComponentTypeValues.AdvancedForm => AdvancedForm,
                PowerPageComponentTypeValues.AdvancedFormStep => AdvancedFormStep,
                PowerPageComponentTypeValues.AdvancedFormMetadata => AdvancedFormMetadata,
                PowerPageComponentTypeValues.PollPlacement => PollPlacement,
                PowerPageComponentTypeValues.AdPlacement => AdPlacement,
                PowerPageComponentTypeValues.BotConsumer => BotConsumer,
                PowerPageComponentTypeValues.ColumnPermissionProfile => ColumnPermissionProfile,
                PowerPageComponentTypeValues.ColumnPermission => ColumnPermission,
                PowerPageComponentTypeValues.Redirect => Redirect,
                PowerPageComponentTypeValues.PublishingStateTransitionRule => PublishingStateTransitionRule,
                PowerPageComponentTypeValues.Shortcut => Shortcut,
                PowerPageComponentTypeValues.CloudFlow => CloudFlow,
                PowerPageComponentTypeValues.UxComponent => UxComponent,
                PowerPageComponentTypeValues.ServerLogic => ServerLogic,
                _ => Unknown
            };
        }
    }
}