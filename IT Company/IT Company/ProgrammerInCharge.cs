using System.Runtime.Serialization;
namespace IT_Company
{
    [DataContract]
    public class ProgrammerInCharge : IEmployee
    {
        [DataMember]
        private string[] name;
        [DataMember]
        private float salary;
        [DataMember]
        private string activity;
        [DataMember]
        private DateTime workStart;
        [DataMember]
        private DateTime workEnd;
        public string[] Name { get { return name; } }
        public float Salary { 
            get { return salary; }
            set { salary = value; }
        }
        public string Activity { get { return activity; } }

        //Constructor for XML serialization
        private ProgrammerInCharge() { }

        public ProgrammerInCharge(string[] wName, float wSalary, string wActivity, DateTime wStart, DateTime wEnd)
        {
            name = wName;
            salary = wSalary;
            activity = wActivity;
            workStart = wStart;
            workEnd = wEnd;
        }

        //Returns the total number of working days
        public int Workdays() { return (workEnd - workStart).Days; }

        //Makes the work duration a day longer
        public void AddWorkDay() { workEnd = workEnd.AddDays(1); }

        //Returs worker remaining work days
        public int RemainingWorkDays(DateTime today)
        {
            //if work hasn't started yet
            if (today < workStart) return Workdays();
            //if work ended already
            else if (today > workEnd) return 0;
            //if work is on going
            else
            {
                return (workEnd-today).Days;
            }
        }

        //Returns the number of the current month days the worker has been working
        public int MonthWorkDays (DateTime today)
        {
            //if work hasn't started yet
            if (today < workStart) return 0;

            DateTime month1st = new DateTime(today.Year, today.Month, 1);
            //if work started on this month, 1st day of the month is work start
            if (today.Year == workStart.Year && today.Month == workStart.Month) month1st = workStart;
            //if work ended already
            if (today > workEnd)
            {
                //work ended in a previous month (or year)
                if (today.Year != workEnd.Year || today.Month != workEnd.Month) return 0;
                //work ended this month
                else return (workEnd - month1st).Days;
            }
            //Worked current month and still working
            else return (today - month1st).Days;
        }

        //Returns the sum of the salaries paid to the worker at a specific date
        public float TotalCost(DateTime today, TeamType team) 
        {
            //Applies the team salary multiplier
            float realSalary = (salary / 2) * (int)team;
            //if work hasn't started yet
            if (today < workStart) return 0f;
            //if work ended already
            else if (today > workEnd){
                return Workdays() * realSalary;
            }
            //if work is on going
            else { 
                return (today - workStart).Days * realSalary;
            }
        }

        //Prints the employee's own report
        public void WriteReport(DateTime today, TeamType team)
        {
            Console.WriteLine($"- {Name[0]} {Name[1]}, in charge of {Activity} from {workStart.ToShortDateString()} to {workEnd.ToShortDateString()} " +
                $"(duration: {Workdays()}), this month: {MonthWorkDays(today)} (total cost = {TotalCost(today, team)}$)");
        }
    }
}