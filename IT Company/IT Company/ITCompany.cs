using IT_Company;
using System.Xml;
using System.Runtime.Serialization;
class ITCompany
{
    private DataContractSerializer serializer;
    private List<ProjectTeam> teams;
    private DateTime today;

    //Returns this month total work days
    int GetMonthWorkDays() 
    {
        int aux = 0;
        foreach (ProjectTeam team in teams) { aux += team.MonthWorkDays(today); }
        return aux;
    }
    //Returns all team remaining work days
    int GetRemainingWorkDays()
    {
        int aux = 0;
        foreach (ProjectTeam team in teams) { aux += team.RemainingWorkDays(today); }
        return aux;
    }
    //Returns number of programmers
    int GetProgrammerCount() 
    {
        int aux = 0;
        foreach (ProjectTeam team in teams) { aux += team.TeamCount(); }
        return aux;
    }
    //Prints the full company report
    public void PrintReport()
    {
        Console.Clear();
        int programmers = GetProgrammerCount();
        Console.WriteLine("IT COMPANY - Report:\n");
        Console.WriteLine($"IT Company is actually composed of {teams.Count} Project Teams and {programmers} Programmers.\n");
        Console.WriteLine($"This month {GetMonthWorkDays()} days has been consummed by {programmers} programmers and {GetRemainingWorkDays()} days still in charge.\n");
        Console.WriteLine("PROJECT TEAM DETAILS:\n");
        foreach (ProjectTeam team in teams) { team.TeamReport(today); }
        Console.WriteLine("Press Enter to go to Main Menu");
        Console.ReadLine();
        MainMenu();
    }

    //Displays save company menu option
    public void SaveCompanyOption() 
    {
        Console.Clear();
        string fileName = AskForString("Type output file name (without extension): ");
        Console.WriteLine(SaveCompany(fileName));
        Console.WriteLine("Press Enter to go to Main Menu");
        Console.ReadLine();
        MainMenu();
    }

    //Serializes company into XML
    public string SaveCompany(string fileName) 
    {
        Console.Write("Type destination directory path: ");
        string path = Console.ReadLine() +"/"+ fileName + ".xml";
        var settings = new XmlWriterSettings() { Indent = true };
        try{
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                serializer.WriteObject(writer, teams);
            }
        }
        catch {
            Console.WriteLine("Incorrect directory. Try again");
            return SaveCompany(fileName);
        }
        return "Company successfully saved in " + path;
    }

    //Deserializes company from XML
    public void LoadCompnay(string filepath)
    {
        FileStream stream = new FileStream(filepath, FileMode.Open);
        XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
        teams = (List<ProjectTeam>)serializer.ReadObject(reader);
        reader.Close();
        stream.Close();
    }

    //Runs the company management app. Displays the initial menu
    public void Run() 
    {
        Console.Clear();
        Console.WriteLine("Wellcome to IT Company's Management tool!");
        Console.WriteLine("Please enter the file path you wish to load (Enter a incorrect one to create a new one):");
        string? input = Console.ReadLine();
        Console.Clear();
        try {
            LoadCompnay(input);
        }
        catch{
            Console.WriteLine("Incorrect path. Do you wish to create a new company file? (yes/no)");
            input= Console.ReadLine();
            switch (input)
            {
                case "yes":
                    CreateNewCompany();
                    break;
                default:
                    Run();
                    break;
            }
        }
    }
    //Prints and manages the main menu
    public void MainMenu() 
    {
        Console.Clear();
        Console.WriteLine($"Today is: {today.ToShortDateString()}");
        Console.WriteLine("Select an option");
        Console.WriteLine("Update company (update)");
        Console.WriteLine("Save company (save)");
        Console.WriteLine("Company report (report)");
        Console.WriteLine("Exit program (exit)");
        Console.Write("Your option: ");

        string? input = Console.ReadLine();
        switch (input)
        {
            case "update":
                EditMenu();
                break;
            case "save":
                SaveCompanyOption();
                break;
            case "report":
                PrintReport();
                break;
            case "exit":
                Environment.Exit(0);
                break;
            default:
                MainMenu();
                break;
        }
    }

    //Prints and manages the edit menu
    public void EditMenu() 
    {
        Console.Clear();
        Console.WriteLine("Select an option");
        Console.WriteLine("Create new Team (cteam)");
        Console.WriteLine("Hire a team lead (clead)");
        Console.WriteLine("Hire new programmer (cworker)");
        Console.WriteLine("Edit team information (eteam)");
        Console.WriteLine("Edit employee salary (esalary)");
        Console.WriteLine("Fire employee (fire)");
        Console.WriteLine("Increase work duration of all programmers (iworker)");
        Console.WriteLine("Advance time one day (nextday)");
        Console.WriteLine("Change current date (date)");
        Console.WriteLine("Back to main menu (menu)");
        Console.Write("Your option: ");

        string? input = Console.ReadLine();
        switch (input)
        {
            case "cteam":
                AddNewTeam();
                break;
            case "clead":
                AddNewLead();
                break;
            case "cworker":
                AddNewWorker();
                break;
            case "eteam":
                ChangeTeamInfo();
                break;
            case "esalary":
                EditEmployeeSalary();
                break;
            case "fire":
                RemoveEmployee();
                break;
            case "iworker":
                IncreaseWorkDuration();
                break;
            case "nextday":
                PassDay();
                break;
            case "date":
                {
                    Console.Clear();
                    ChangeDate();
                    break;
                }
            default:
                MainMenu();
                break;
        }
    }

    //Ask the user a new date
    public void ChangeDate() 
    {
        try{
            DateTime date = new DateTime(AskForIntRange("Enter date year (From 2020 to 2099): ", 2100, 2019), AskForIntRange("Enter month number: ", 12, 0), AskForIntRange("Enter day number: ", 31, 0));
            today = date;
            Console.WriteLine($"New date: {today.ToShortDateString()}");
            Console.WriteLine("Press Enter to return to Main Menu");
            Console.ReadLine();
            MainMenu();
        }
        catch{
            Console.Clear();
            Console.WriteLine("Invaled date. Please try again");
            ChangeDate();
        }
    }
    
    //set current date to tomorrow
    public void PassDay() {
        today = today.AddDays(1);
        Console.WriteLine($"New date: {today.ToShortDateString()}");
        Console.WriteLine("Press Enter to return to Main Menu");
        Console.ReadLine();
        MainMenu();
    }

    //Go through all employees and add a day to their workEnd
    public void IncreaseWorkDuration()
    {
        Console.Clear();
        foreach (ProjectTeam team in teams) { team.AddWorkDay(); }
        Console.WriteLine("All workers will work a day longer");
        Console.WriteLine("Press Enter to return to Main Menu");
        Console.ReadLine();
        MainMenu();
    }

    //Ask the user for a employee to change their salary
    public void EditEmployeeSalary()
    {
        int tID = TeamSelection();
        int wID = EmployeeSelection(tID);
        float newSalary = AskForFloat("Enter employee's new salary per day: ");
        teams[tID].ChangeSalary(wID, newSalary);
        Console.WriteLine($"Team {teams[tID].TeamName} member {wID + 1} salary changed to {newSalary}$ per day.");
        Console.WriteLine("Press Enter to return to Main Menu");
        Console.ReadLine();
        MainMenu();
    }
    //Ask the user for a employee to remove
    public void RemoveEmployee() 
    {
        int tID = TeamSelection();
        int wID = EmployeeSelection(tID);
        teams[tID].WorkerDelete(wID);
        Console.WriteLine($"Team {teams[tID].TeamName} member {wID+1} successfully removed.");
        Console.WriteLine("Press Enter to return to Main Menu");
        Console.ReadLine();
        MainMenu();
    }

    //Ask for for input to change team information
    public void ChangeTeamInfo() 
    {
        int sel = TeamSelection();
        string? name; TeamType type;
        AskForTeamInfo(sel+1, out name, out type);
        teams[sel].EditTeamInfo(name, type);
        Console.WriteLine($"Team {teams[sel].TeamName} sucessfully edited.");
        Console.WriteLine("Press Enter to return to Main Menu");
        Console.ReadLine();
        MainMenu();
    }

    //Add a new lead to a team
    public void AddNewLead()
    {
        int sel = TeamSelection();
        //Checks if there is a lead in the selected team
        if (!teams[sel].HasLead())
        {
            LeadProgrammer p = CreateNewLead(sel + 1, teams[sel].TeamCount() + 1);
            teams[sel].AddLead(p);
            Console.Clear();
            Console.WriteLine($"{p.Name[1]} {p.Name[0]} is now lead of {teams[sel].TeamName}.");
            Console.WriteLine("Press Enter to return to Main Menu");
            Console.ReadLine();
            MainMenu();
        }
        else
        {
            Console.WriteLine($"Team {teams[sel].TeamName} already has a lead. Press Enter to go back");
            Console.ReadLine();
            EditMenu();
        }
    }

    //Add a new programmer to a team
    public void AddNewWorker() 
    {
        int sel = TeamSelection();
        ProgrammerInCharge p = CreateNewProgrammer(sel+1, teams[sel].TeamCount() + 1);
        teams[sel].AddProgrammer(p);
        Console.Clear();
        Console.WriteLine($"{p.Name[1]} {p.Name[0]} successfully added to team {teams[sel].TeamName}.");
        Console.WriteLine("Press Enter to return to Main Menu");
        Console.ReadLine();
        MainMenu();
    }

    //Create and adds a new team
    public void AddNewTeam() 
    {
        teams.Add(CreateNewTeam(teams.Count+1));
        Console.Clear();
        Console.WriteLine($"Team {teams[teams.Count-1].TeamName} successfully created.");
        Console.WriteLine("Press Enter to return to Main Menu");
        Console.ReadLine();
        MainMenu();
    }

    //Ask the user for a team employee index
    public int EmployeeSelection(int teamIndex)
    {
        teams[teamIndex].EmployeeSelectionDisplay();
        return AskForIntRange("Enter a valid employee number: ", teams[teamIndex].TeamCount(), 0) - 1;
    }

    //Ask the uer for a team index
    public int TeamSelection() 
    {
        Console.Clear();
        Console.WriteLine("Please, select a team number");
        for(int i = 0; i<teams.Count; i++) Console.WriteLine($"{i+1} - Team {teams[i].TeamName}");
        return AskForIntRange("Enter a valid team number: ", teams.Count, 0)-1;
    }
    //Collects user input to create new company
    public void CreateNewCompany() 
    {
        //Creates 2 teams for the company
        teams.Add(CreateNewTeam(1));
        teams.Add(CreateNewTeam(2));
    }

    //Ask for a team basic information
    public void AskForTeamInfo(int index,out string name, out TeamType type) 
    {
        Console.Clear();
        Console.WriteLine($"Team {index}:");
        //Asks for team name
        name = AskForString("Enter team name: ");
        //Ask for the team type
        Console.WriteLine("Enter team type. (1 - Full Salary team | 2 or default - Half salary team)");
        string? aux = Console.ReadLine();
        switch (aux)
        {
            case "1":
                type = TeamType.FullSalary;
                Console.WriteLine("Selected: Full Salary. Press enter to continue.");
                break;
            default:
                type = TeamType.HalfSalary;
                Console.WriteLine("Selected: Half Salary. Press enter to continue.");
                break;
        }
        Console.ReadLine();
    }
    //Collects user input to create new team
    public ProjectTeam CreateNewTeam(int index) 
    {
        string? name; TeamType type;
        AskForTeamInfo(index, out name, out type);
        ProjectTeam p = new ProjectTeam(name, type);
        //Creates 2 programmers for the team
        p.AddProgrammer(CreateNewProgrammer(index, 1));
        p.AddProgrammer(CreateNewProgrammer(index, 2));
        return p;
    }
    //Collects user input to create a new lead
    public LeadProgrammer CreateNewLead(int teamIndex, int index)
    {
        Console.Clear();
        Console.WriteLine($"Team {teamIndex} | New Lead");
        string[] name = new string[2];
        //Asks for name
        name[0] = AskForString("Enter new employee's last name: ");
        name[1] = AskForString("Enter new employee's first name: ");
        //Ask for salary
        float salary = AskForFloat("Enter employee's salary per day: ");
        //Creates the worker
        LeadProgrammer p = new LeadProgrammer(name, salary, teams[teamIndex-1].TeamName, today);
        Console.WriteLine($"New lead created. Press enter to continue.");
        Console.ReadLine();
        return p;
    }
    //Collects user input to create a new programmer
    public ProgrammerInCharge CreateNewProgrammer(int teamIndex,int index)
    {
        Console.Clear();
        Console.WriteLine($"Team {teamIndex} | Employee {index}");
        string[] name = new string[2];
        //Asks for name
        name[0] = AskForString("Enter new employee's last name: ");
        name[1] = AskForString("Enter new employee's first name: ");
        //Ask for salary
        float salary = AskForFloat("Enter employee's salary per day: ");
        //Ask for activity
        string activity = AskForString("Enter employee's activity: ");
        //Ask for amount of working days
        int jobDuration = AskForInt("Enter the amount of days the employee will be working: ");
        DateTime workEnd = today.AddDays(jobDuration);
        //Creates the worker
        ProgrammerInCharge p = new ProgrammerInCharge(name, salary, activity, today, workEnd);
        Console.WriteLine($"Worker will be working from {today.ToShortDateString()} to {workEnd.ToShortDateString()}. Press enter to continue.");
        Console.ReadLine();
        return p;
    }
    //Ask for a string to be inputted until it's not a null
    public string AskForString(string phrase)
    {
        Console.Write(phrase);
        string? s = Console.ReadLine();
        while (s == "")
        {
            Console.WriteLine("Null name. Please try again");
            Console.Write(phrase);
            s = Console.ReadLine();
        }
        return s;
    }

    //Ask for a float to be inputted until it gets a correct one
    public float AskForFloat(string phrase) 
    {
        float number;
        Console.Write(phrase);
        try{
            number = float.Parse(Console.ReadLine());
        }
        catch{
            Console.WriteLine("Incorrect number. Please try again.");
            number = AskForFloat(phrase);
        }
        return number;
    }

    //Ask for a int to be inputted until it gets a correct one
    public int AskForInt(string phrase)
    {
        int number;
        Console.Write(phrase);
        try{
            number = int.Parse(Console.ReadLine());
        }
        catch{
            Console.WriteLine("Incorrect number. Please try again.");
            number = AskForInt(phrase);
        }
        return number;
    }
    //Ask for an int inside a specified range
    public int AskForIntRange(string phrase, int maxValue, int minValue)
    {
        int aux = int.MaxValue;
        while (aux > maxValue || aux <= minValue) aux = AskForInt(phrase);
        return aux;
    }

    public ITCompany() 
    {
        teams = new List<ProjectTeam>();
        today = DateTime.Today;

        //Initialise the serializer to serialize a list of interfaces
        serializer = new DataContractSerializer(typeof(List<ProjectTeam>), new List<Type> {typeof(List<IEmployee>),typeof(ProgrammerInCharge), typeof(LeadProgrammer) });
    }

    static void Main(string[] args)
    {   
        ITCompany tCompany = new ITCompany();
        tCompany.Run();
        tCompany.MainMenu();
    }

}