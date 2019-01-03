using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Interfaces
{
    public interface IResourceRepository:ICrudRepository<Resource>,IDatabaseRepository
    {}
}