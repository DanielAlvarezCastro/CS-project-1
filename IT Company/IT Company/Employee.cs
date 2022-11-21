using System.Runtime.Serialization;

namespace IT_Company
{
    public interface IEmployee
    {
        //Request employee's lastname and firstname
        string[] Name { get; }
        //Resquest and set employee's salary per day
        float Salary { get; set; }
        //Request employee's activity
        public string Activity { get; }
        //Returns the number of the current month days the worker has been working
        public int MonthWorkDays(DateTime today);
        //Returns the sum of the salaries paid to the worker at a specific date
        public float TotalCost(DateTime today, TeamType team);
        //Prints the employee's own report
        public void WriteReport(DateTime today, TeamType team);
    }
}
