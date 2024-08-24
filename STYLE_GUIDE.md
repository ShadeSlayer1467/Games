# GameEngine Code Style Guide
## General Principles
- **Readability over Brevity**: Write code that is easy to read and understand over code that is overly concise.
- **Consistency:** Follow existing patterns and practices in the codebase.

## Code Complexity
- **Simplicity is Key:** If you find yourself thinking "this is super genius" while writing a line of code, it may be too complex.  
Opt for simplicityby breaking it down into multiple, more readable lines. 
This ensures that your code is maintainable and understandable by others who may not share the same context (or you tomorrow/next week when coming back to it).

# Formatting
- **Creator:** We encourage you to put YOUR name attatched to any file you create (github profile name if prefered)
- **Indentation:** Use 4 spaces for indentation.
- **Braces:** Open braces should be on the same line as the statement.
- **Line Length:** Keep lines shorter where possible.
- **Spacing:**
   - Include a space after commas and semicolons.
   - Include a space around operators (e.g., =, +, -, <, >, etc.).
     
## Naming Conventions
- **Classes & Interfaces:** Use PascalCase. Example: `GameEngine`, `ConsoleGame`.
- **Methods:** Use PascalCase. Example: `SelectGame`, `GenerateFood`.
- **Variables and Fields:** Use camelCase for private fields and local variables. Example: `gameTypes`, `selectGameMenuBuilder`.
- **Constants:** Use ALL_CAPS with underscores. Example: `SELECT_GAME_MENU`.
- **Properties:** Use PascalCase. Example: `ForegroundColor`.
  
## Commenting
- **Inline Comments:** Use inline comments sparingly code should be self explanatory
- If commenting it should be "why" something is done, not "what" is done.
  
## Exception Handling
- Always check for nulls and bounds where applicable before using variables.
- Provide clear error messages that are helpful for debugging.

## Code Structure
- **Methods:** Methods can be long or short just try to keep it focused on a single task.
- **Classes:** Strive for small to medium-sized classes. When a class takes on too many responsibilities, it's advisable to divide it into smaller, more focused classes. Look into the Single Responsibility Principle for guidance.

## Version Control
- **Commits:** Make small, incremental commits with clear, descriptive messages.
