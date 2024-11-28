namespace StudentsManagment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Management management = new Management();
            showMenu();
            while (start(management)) ;

        }
        //This method is one part of program and is called again and again until user wants to quit.
        private static bool start(Management management)
        {
            Console.Write("Choose operation : ");
            String operation = Console.ReadLine();
            if (operation == null || operation.Length != 1 || !Char.IsDigit(operation[0]))
            {
                Console.WriteLine("Enter valid operation!");
                return true;
            }
            return answerOperation(operation[0], management);
        }

        //This method decides what to do according to user's choice.
        private static bool answerOperation(char operation, Management management)
        {
            Student st = null; 
            switch (operation)
            {
                case '1':
                     st = new Student(getValidName(), getValidRollNumber(), getValidGrade());
                    if (management.addStudent(st)) Console.WriteLine("Student added successfully!");
                    else Console.WriteLine("Student with this Rollnumber already exist");
                    break;
                case '2':
                    var students = management.getAllStudents();
                    if (students.Count==0) Console.WriteLine("There are no students.");
                    foreach (Student s in students) Console.WriteLine($"Name : {s.Name}, Grade : {s.Grade}, RollNumber : {s.RollNumber} ");
                    break;
                case '3':
                    int rollNum = getValidRollNumber();
                     st = management.getStudentByNumber(rollNum);
                    if (st == null) Console.WriteLine("Student with this roll number does not exist!");
                    else Console.WriteLine($"Name : {st.Name}, Grade : {st.Grade}, RollNumber : {st.RollNumber} ");
                    break;
                case '4':
                    rollNum = getValidRollNumber();
                    char grade = getValidGrade();
                    bool ischanged = management.changeGrade(rollNum, grade);
                    if (!ischanged) Console.WriteLine("Student with this roll number does not exist!");
                    else Console.WriteLine("Grade changed successfully!");
                    break;
                case '5':
                    return false;
            }
            return true;
        }
        //This method prints the menu.
        private static void showMenu() {
            Console.WriteLine("Welcome to the students management system.");
            Console.WriteLine("1. Add student");
            Console.WriteLine("2. Show all students");
            Console.WriteLine("3. Find student");
            Console.WriteLine("4. Change grade");
            Console.WriteLine("5. Quit");
        }

        // This method is responsible for reading valid name from user
        private static string getValidName()
        {
            string name;
            while (true)
            {
                Console.Write("Enter new Student's name : ");
                name = Console.ReadLine();
                if (name != null && name.Length>=4) return name;
                Console.WriteLine("please enter valid name!, name should contains at least 4 letter");
            }

        }

        // This method is responsible for reading valid Roll number from user
        private static int getValidRollNumber()
        {
            int n;
            while (true)
            {
                Console.Write("please enter student's RollNumber : ");
                if (int.TryParse(Console.ReadLine(), out n)) return n;
                Console.WriteLine("Please, enter valid RollNUmber!");
            }



        }


        // This method is responsible for reading valid Grade from user
        private static char getValidGrade()
        {
            string grades = "ABCDEF";
            char g;
            while (true)
            {
                Console.Write("please enter student's grade : ");
                if (char.TryParse(Console.ReadLine(), out g) && grades.Contains(Char.ToUpper(g))) return Char.ToUpper(g);
                Console.WriteLine("Please, enter valid grade!");
            }
        }

        class Management
        {

            private List<Student> students;


            public Management()
            {
                students = new List<Student>();
            }
            public bool addStudent(Student st)
            {
                if(st == null) return false;
                if (students != null && students.Any(x => x.RollNumber == st.RollNumber)) return false;
                students.Add(st);
                return true;
            }

            public List<Student> getAllStudents()
            {
                return students;
            }

            public Student getStudentByNumber(int number)
            {
                var st = students.Where(x => x.RollNumber == number).FirstOrDefault();
                return st;
            }

            public bool changeGrade(int number, char grade)
            {
                var st = getStudentByNumber(number);
                if (st == null) return false;
                st.Grade = grade;

                return true;
            }


        }



        class Student
        {
            public int RollNumber { get; set; }
            public string Name { get; set; }

            public Char Grade { get; set; }

            Student() { }
            public Student(string name, int rollNumber, Char grade)
            {
                RollNumber = rollNumber;
                Name = name;
                Grade = grade;
            }

        }
    }
}
