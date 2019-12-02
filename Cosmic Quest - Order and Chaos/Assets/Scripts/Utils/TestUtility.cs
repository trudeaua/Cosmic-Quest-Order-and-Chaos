using System;
using System.Linq;

public class TestUtility
{
    public static readonly bool IsRunningInTest = 
        AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.ToLowerInvariant().StartsWith("nunit.framework"));
}