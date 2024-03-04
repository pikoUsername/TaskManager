﻿using TaskManager.Database.Models;

namespace TaskManager.Schemas
{
    public class CreateGroupScheme
    {
        public string Name { get; set; }
        public Guid TeamId  { get; set; }
        public GroupRoles Role {  get; set; }
    }
}
