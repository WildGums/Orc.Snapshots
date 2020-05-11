// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Project.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Models
{
    public class Project
    {
        public Project()
        {
            Person = new Person();
            Company = new Company();
        }

        public Person Person { get; private set; }

        public Company Company { get; private set; }
    }
}