using System.Runtime.Serialization;
namespace IT_Company
{
    public enum TeamType :  int {FullSalary = 2, HalfSalary = 1}
    [DataContract]
    public class ProjectTeam
    {
        [DataMember]
        private string teamName;
        [DataMember]
        private List<IEmployee> teamMembers;
        [DataMember]
        private TeamType type;
        public TeamType Type { get { return type; } }
        public string TeamName { get { return teamName; } }

        //Constructor for XML serialization
        private ProjectTeam() { }

        public ProjectTeam(string name, TeamType teamType) {
            teamMembers = new List<IEmployee>();
            teamName = name;
            type = teamType;
        }

        //Returns number of team members
        public int TeamCount() { return teamMembers.Count; }
        //Returns total number of month's work days of all members
        public int MonthWorkDays(DateTime today) {
            int aux = 0;
            foreach (ProgrammerInCharge member in teamMembers.OfType<ProgrammerInCharge>()) { aux += member.MonthWorkDays(today); };
            return aux;
        }
        //Returns remaining number of work days of all programmers
        public int RemainingWorkDays(DateTime today)
        {
            int aux = 0;
            foreach (ProgrammerInCharge member in teamMembers.OfType<ProgrammerInCharge>()) { aux += member.RemainingWorkDays(today); };
            return aux;
        }
        //Adds a new programmer to the end of the list
        public void AddProgrammer(IEmployee member) { teamMembers.Add(member); }
        //Adds a new lead to the top of the list
        public void AddLead(IEmployee member) { teamMembers.Insert(0, member); }
        //Ask if there's a lead in the team
        public bool HasLead() { return teamMembers[0].GetType() == typeof(LeadProgrammer); }

        //Prints this team report
        public void TeamReport(DateTime today) {
            Console.WriteLine($"Project team - {teamName}:");
            //Calls the report method for each of the team members
            foreach (IEmployee member in teamMembers) { member.WriteReport(today, Type); };
        }

        //Changes team info
        public void EditTeamInfo(string newName, TeamType newType)
        {
            teamName = newName;
            type = newType;
        }

        //Increase all workers work duration a day
        public void AddWorkDay()
        {
            foreach (ProgrammerInCharge member in teamMembers.OfType<ProgrammerInCharge>()) { member.AddWorkDay(); };
        }

        //Ask the user for a team employee index
        public void EmployeeSelectionDisplay()
        {
            Console.Clear();
            Console.WriteLine("Please, select a employee number");
            for (int i = 0; i < teamMembers.Count; i++) Console.WriteLine($"{i + 1} - {teamMembers[i].Name[1]} {teamMembers[i].Name[0]} - {teamMembers[i].Activity}");
        }

        //Changes an employee's salary
        public void ChangeSalary(int indx, float newSalary) { teamMembers[indx].Salary = newSalary; }

        //Deletes an employee
        public void WorkerDelete(int indx) { teamMembers.RemoveAt(indx); }
    }
}
