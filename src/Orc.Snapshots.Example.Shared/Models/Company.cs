// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Company.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Models
{
    using Catel.Data;

    public class Company : ModelBase
    {
        public Company()
        {
            Name = string.Empty;
        }

        public string Name { get; set; }
    }
}