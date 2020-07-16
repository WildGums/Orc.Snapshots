// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Models
{
    using Catel.Data;

    public class Person : ModelBase
    {
        public Person()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}