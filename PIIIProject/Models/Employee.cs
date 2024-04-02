using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace PIIIProject.Models
{
    //Inherits from the User class:
    internal class Employee : User
    {
       
        private int _employeeId;
        //Uses the base constructor to fill in the datamembers
        public Employee(string firstName, string lastName, string username, string password, int employeeId)
            :base(firstName, lastName, username, password) 
        {
            EmployeeId = employeeId;
           
        }

        public Employee(string firstName, string lastName, string password)
            :base(firstName, lastName, password) 
        {
            EmployeeId = GetEmployeeId();
        }


        public int EmployeeId
        { 
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        private int GetEmployeeId()
        {
            string filePath = "TestFiles/Users.txt";
            int lastEmployeeId = 0;

            foreach (string line in File.ReadLines(filePath))
            {
                string[] parts = line.Split(',');
                if (parts.Length > 0 && parts[0] == "2")
                {
                    if (int.TryParse(parts.Last(), out int employeeId))
                    {
                        lastEmployeeId = Math.Max(lastEmployeeId, employeeId);
                    }
                }
            }

            return lastEmployeeId+1;
        }
       
        public override bool PasswordAuthentication(string enteredPassword)
        {
            return base.PasswordAuthentication(enteredPassword);
        }
    }
}
