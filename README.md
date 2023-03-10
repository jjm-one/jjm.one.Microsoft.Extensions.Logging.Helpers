# jjm.one.Microsoft.Extensions.Logging.Helpers

A collection of helper functions for the [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging) logging tool.

## Status

|                       |                       |
|----------------------:|-----------------------|
| Nuget Package Version | [![Nuget Version](https://img.shields.io/nuget/v/jjm.one.Microsoft.Extensions.Logging.Helpers?style=flat-square)](https://www.nuget.org/packages/jjm.one.Microsoft.Extensions.Logging.Helpers/) |

## Table of contents

- [jjm.one.Microsoft.Extensions.Logging.Helpers](#jjmonemicrosoftextensionslogginghelpers)
  - [Status](#status)
  - [Table of contents](#table-of-contents)
  - [Usage](#usage)
    - [Use function logging](#use-function-logging)
    - [Output of function logging](#output-of-function-logging)

## Usage

### Use function logging

```csharp
class MyClass {

    // ...

    void MyFancyFunction() {

        // log the function call
        logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Debug);

        try {
            
            //...
        }
        catch (Exception exc) {

            // Log the exception
            logger.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), "My custom exception message!", LogLevel.Error);
        }
    }

    // ...
}
```

### Output of function logging

```text
Function called: MyClass -> MyFancyFunction
Exception thrown in: MyClass -> MyFancyFunction
My custom exception message!
```
