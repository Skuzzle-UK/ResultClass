# Result Class
My take on a simple Result class for C#. Inspired by several that have come before mine.

### Usage:
To return a Result from a method:
```
public Result AMethod(bool IsSuccessful)
{
  if(IsSuccessful)
  {
    return Result.Ok();
  }

  return Result.Fail("Some error message with a {Param}, "parameter");
}
```
