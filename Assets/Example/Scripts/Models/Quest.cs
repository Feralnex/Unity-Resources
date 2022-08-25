using Newtonsoft.Json;
using System;

class Quest : ICloneable
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    [JsonConstructor]
    public Quest(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    public object Clone() => new Quest(Name, Description);
}