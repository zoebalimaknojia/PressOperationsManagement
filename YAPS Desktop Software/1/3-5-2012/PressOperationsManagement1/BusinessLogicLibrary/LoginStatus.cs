using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLibrary
{
    public class LoginStatus
    {
        bool firstLogin;
        bool validUser;
        Employee employee;
        public LoginStatus(bool firstLogin, bool validUser, Employee employee)
        {
            this.firstLogin = firstLogin;
            this.validUser = validUser;
            this.employee = employee;
        }

        public bool FirstLogin
        {
            get
            {
                return firstLogin;
            }
            set
            {
                firstLogin = value;
            }
        }
        public bool ValidUser
        {
            get
            {
                return validUser;
            }
            set
            {
                validUser = value;
            }
        }

        public Employee Employee
        {
            get
            {
                return employee;
            }
            set
            {
                employee = value;
            }
        }
    }
}
