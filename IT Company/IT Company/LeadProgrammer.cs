using System.Runtime.Serialization;

namespace IT_Company
{
    /*LeadProgrammer it's an extension to the PA minimum requirements that gives Employee
     * interface an additional derived class. 
     * A lead programmer it's similar to ProgrammerInCharge, but a lead doesn't have a "workEnd" 
     * field, so neither has a finite work duration. A lead will work since their hiring day until 
     * they are fired. Each team can only have one lead, but doesn't necesarilly need one.
     */
    [DataContract]
    public class LeadProgrammer : IEmployee
    {
        [DataMember]
        private string[] name;
        [DataMember]
        private float salary;
        [DataMember]
        private string activity;
        [DataMember]
        private string teamName;
        [DataMember]
        private DateTime workStart;
        public string[] Name { get { return name; } }
        public float Salary
        {
            get { return salary; }
            set { salary = value; }
        }
        public string Activity { get { return activity; } }

        public string TeamName { get { return teamName; } }

        //Constructor for XML serialization
        private LeadProgrammer() { }

        public LeadProgrammer(string[] wName, float wSalary, string team, DateTime wStart)
        {
            name = wName;
            salary = wSalary;
            teamName = team;
            activity = "Lead";
            workStart = wStart;
        }

        //Returns the number of the current month days the worker has been working
        public int MonthWorkDays(DateTime today)
        {
            //if work hasn't started yet
            if (today < workStart) return 0;

            DateTime month1st = new DateTime(today.Year, today.Month, 1);
            //if work started on this month, 1st day of the month is work start
            if (today.Year == workStart.Year && today.Month == workStart.Month) month1st = workStart;
            //Worked current month and still working
            return (today - month1st).Days;
        }

        //Returns the sum of the salaries paid to the worker at a specific date
        public float TotalCost(DateTime today, TeamType team)
        {
            //Applies the team salary multiplier
            float realSalary = (salary / 2) * (int)team;
            //if work hasn't started yet
            if (today < workStart) return 0f;
            //if work is on going
            else
            {
                return (today - workStart).Days * realSalary;
            }
        }

        //Prints the employee's own report
        public void WriteReport(DateTime today, TeamType team)
        {
            Console.WriteLine($"- {Name[0]} {Name[1]}, lead of {teamName} from {workStart.ToShortDateString()}" +
                $", this month: {MonthWorkDays(today)} (total cost = {TotalCost(today, team)}$)");
        }
    }
}
