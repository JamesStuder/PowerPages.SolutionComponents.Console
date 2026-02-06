# PowerPages.SolutionComponents.Console

A specialized console application designed to streamline the process of adding Power Pages site components to a Microsoft Dataverse solution.

## Overview

When working with Power Pages, specifically the "Enhanced Data Model," manually adding all related components of a site to a solution can be tedious and error-prone. This tool automates the discovery and addition of:
- Power Page Site records
- Power Page Site Languages
- Power Page Components (Web Pages, Content Snippets, Web Templates, etc.)
- Related entities required for the site to function correctly.

## Prerequisites

- **.NET 8.0 SDK** or later.
- Access to a **Power Platform Environment**.
- **System Administrator** or equivalent permissions in the target environment to modify solutions.
- The **Unique Name** of the solution where you want to add the components.
- The **GUID** of the Power Pages site (can be found in the Power Pages Management app or via Power Platform CLI).

## How to Use

1. **Clone the repository** and navigate to the project folder.
2. **Build the project**:
   ```powershell
   dotnet build
   ```
3. **Run the application**:
   ```powershell
   dotnet run --project PowerPages.SolutionComponents.Console
   ```
4. **Follow the interactive prompts**:
   - **Environment URL**: Enter your Dataverse environment URL (e.g., `https://org.crm4.dynamics.com`).
   - **Solution Unique Name**: Enter the internal name of your solution (not the display name).
   - **Power Pages Site GUID**: Provide the ID of the site you wish to include.
   - **Exclude Site Settings**: Option to skip specific site settings based on prefixes (e.g., `Authentication/*`) (useful when moving between environments with different identity providers).

 ## Authentication

The application uses the `Microsoft.PowerPlatform.Dataverse.Client` and will prompt you for authentication via a browser window (Interactive Login) upon first run or if a session has expired.

## Key Features

- **Automated Component Discovery**: Recursively finds and adds all site-related components.
- **Enhanced Data Model Support**: Specifically designed for the latest Power Pages data architecture.
- **Selective Inclusion**: Allows excluding specific site settings to prevent accidental overwriting of environment-specific configuration. To customize which settings are excluded, you can modify the `ExcludedSiteSettingPrefixes` list in `PowerPages.SolutionComponents.Console\Constants\ExcludedSettings.cs`.

## Important Considerations

- **Table Inclusion**: This tool automatically adds all related tables to the solution. If your deployment strategy requires only the Power Pages Site, Components, and Languages to be included in the solution, you will need to manually remove the table definitions from the solution after the tool has finished running.