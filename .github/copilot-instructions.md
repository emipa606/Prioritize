# GitHub Copilot Instructions for RimWorld Modding Project

## Mod Overview and Purpose

This mod, titled "Prioritize," enhances the construction and management systems within RimWorld by providing players with advanced priority customization tools. It allows for more granular control over task prioritizations, enabling optimized resource allocation and efficiency enhancements. The mod leverages both C# and XML to introduce new mechanics and integrate seamlessly with RimWorld's existing codebase.

## Key Features and Systems

- **Priority Designators**: Introduces new designators for setting task priorities, including `Designator_Priority_Cell`, `Designator_Priority_Thing`, and `Designator_PrioritySettings`.
- **Priority Adjustments**: Via the `Dialog_SelectPriority`, players can adjust priorities for different objects and tasks, providing flexibility and control.
- **Priority Management**: Through classes like `PriorityMapData` and `PSaveData`, the mod tracks and manages priority settings across the game, maintaining state even through saves and reloads.
- **Construction and Implementation**: Includes various enhancements to construction tasks, with classes like `Workgiver_UniversalConstruct` and methods to handle failure and success of construction (`Frame_FailConstruction`).
- **Play Settings and Utility Functions**: Incorporates new player settings managed via `PlaySettings_DoPlaySettingsGlobalControls` and utility methods in `PriorityUtils`.

## Coding Patterns and Conventions

- **Class and Naming Conventions**: Uses PascalCase for class and method names, adhering to standard C# conventions.
- **Inheritance and Interfaces**: Implements object-oriented principles such as inheritance (e.g., `Designator_Priority_Cell` inherits from `Designator`) for expandable and modular design.
- **Static Helpers**: Uses static classes like `PriorityUtils` for shared functionality, reducing code duplication and promoting reuse.
  
## XML Integration

- **XML Defs and Patch Operations**: The XML configurations, though not parsed here, likely define custom Defs for new entities and patch existing ones to integrate the mod's features.
- **In-Game Descriptions and UI Text**: Leveraging XML for localizations and text definitions ensures adaptability and extendibility for different languages.
- **Copilot Integration**: Use XML comments for summarizing and documenting complex XML structures and provide Copilot with additional context to generate better suggestions.

## Harmony Patching

- **Patch Setup**: Utilizes Harmony to apply patches, which allows modification of the game's methods while minimizing direct alteration of the original codebase.
- **Safe Injection**: Ensures stability and compatibility by applying patches to methods such as `Thing_Destroy` and various `GenClosest_` methods.
- **Method Patching**: Specific patching scenarios are handled in dedicated classes, ensuring that the game's functionality can be extended or altered in a controlled manner.

## Suggestions for Copilot

- **Code Completion**: When generating code related to priority settings, leverage Copilot's ability to suggest method implementations based on existing patterns within the class `PriorityGameComponent`.
- **Error-Checking and Debugging**: Suggest adding error-handling within critical methods, especially those modifying game data structures, to enhance reliability.
- **UI Enhancements**: For UI-related tasks, like those in `Dialog_SelectPriority`, Copilot can assist in generating user-friendly, intuitive interfaces.
- **XML Description**: Encourage using Copilot for generating initial XML snippets with placeholders for Defs, making future customization easier.
- **Method Stubs**: Utilize Copilot to suggest method stubs in context with naming conventions and class inheritance beyond the listed classes for future expansion.

By adhering to these structured instructions, contributors can maintain and expand the mod effectively, ensuring high compatibility and performance within the game framework.
