﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Interfaces
{
    public interface IResourceTypeRepository:ICrudRepository<ResourceType>, IDatabaseRepository
    {}
}
