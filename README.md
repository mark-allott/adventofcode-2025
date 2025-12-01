# Advent of Code 2025 Challenge

## Requirements
All requirements are taken from the two-part challenges issued daily on the [Advent of Code](https://adventofcode.com/) website. This specific repo will contain the  code used to solve the problems provided in the 2025 challenge. As per the limits of the 2025 challenge, there are only 12 days of challenges provided.

## Additional personal constraints:
1. All solutions and data shall reside in a common "root" folder
1. Input data for each daily challenge shall be located in the `data` folder of the "root"
1. Data for each daily challenge shall be supplied in the format day*nn*-input.txt
1. Code written shall be entirely my own and no use of AI assistants will be permitted (eg. no use of GitHub Copilot etc.)
1. Solutions shall be placed in language-specific sub-folders: e.g. `csharp`, `javascript`, etc., similar to the following layout:
```console
+ root
 \
  + data
  |\
  | +  day01-input.txt
  | +  day02-input.txt
  | +  etc.
  + csharp
  |\
  | + (C# solution files)
  + javascript
```

## C# Solution constraints:
For code written using C#, the following _additional_ constraints are to be applied:

1. Code **shall** use the current LTS version of dotnet (*net10.0*)
1. A **single application** shall be used to solve all daily challenges (using a similar format to the 2024 solutions)
1. The application shall hold all code for each of the daily challenges in separate classes
1. Running the application:
    - Shall determine, during startup, the list of _registered_ daily solutions contained in the application
    - without parameters: shall execute the most recent solution (both parts)
    - with `all` as a parameter: shall execute each solution in ascending day order
    - with a numeric parameter (or parameters): shall execute the solution(s) to the days specified
