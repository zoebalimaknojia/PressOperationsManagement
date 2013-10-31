using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLibrary
{
    class DatabaseConnectionException:Exception
    {
        public DatabaseConnectionException() : base("Could Not Connect to Database")
        {
            
        }
        public DatabaseConnectionException(String message) : base(message)
        {

        }
    }
}
