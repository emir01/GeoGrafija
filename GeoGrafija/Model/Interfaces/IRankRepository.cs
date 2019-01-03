using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Interfaces
{
    interface IRankRepository: ICrudRepository<Rank>, IDatabaseRepository
    {
    }
}
