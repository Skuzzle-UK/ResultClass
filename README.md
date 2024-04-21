# Result Class
My take on a simple Result class for C#. Inspired by several that have come before mine.

### Usage:
To return a Result from a method:
```
// Simple method example
public Result AMethod()
{
    try
    {
        // ... Some logic happens here
        return Result.Ok();
    }
    catch (Exception ex)
    {
        return Result.Fail(ex, "Failed with error message: {ErrorMessage}, ex.Message);
    }
}

// Return value with Result example (assumed Colours class exists)
public Result<List<Colour>> GetColours()
{
    try
    {
        var colours = new List<Colours>();
        // ... Try some logic to get colours from database or similar
        
        return Result.Ok(colours);
    }
    catch (Exception ex)
    {
        return Result.Fail<List<Colour>>(ex, "Failed with error message: {ErrorMessage}, ex.Message");
    }
}
```

Get the nuget at:
https://www.nuget.org/packages/Skuzzle.DotNetCore.ResultClass/