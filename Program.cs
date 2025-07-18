using System;
using System.Data.SQLite;
using System.Collections.Generic;
namespace Studentnamepace
{
    class Program
    {
        static void Main(string[]args)
        {
            //List
            List<string> menulist = new List<string> {". Add Student",". Show Student",". Edit Student",". Delete Student",". Exit" };
            // เชื่อมData base
            Console.WriteLine("Connecting to sql.......");
            string connect = "Data Source = mysql3.db;version=3";
            using (SQLiteConnection conn = new SQLiteConnection(connect))
            {
                conn.Open();
                Console.WriteLine("Connect to mysql3 Success");
                
                // สร้างTable
                string createTable = @"CREATE TABLE IF NOT EXISTS student(
                                     name TEXT,
                                     age INTEGER
                                     );";
                SQLiteCommand command = new SQLiteCommand(createTable, conn);
                command.ExecuteNonQuery();
                Console.WriteLine("mysql Create Table Success");
                //Menu
                string checkbackmenu = "";
                do
                {
                    for (int i = 0; i < menulist.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}{menulist[i]}");
                    }
                    Console.Write("Choose Menu : ");
                    string ChooseMenu = Console.ReadLine();
                    if (ChooseMenu == "1" || ChooseMenu.ToUpper() == "ADD STUDENT")
                    {
                        // เพิ่มstudent
                        string addstudentdo = "";
                        do
                        {
                            Console.Write("Enter name : ");
                            string name = Console.ReadLine();
                            Console.Write("Enter Age : ");
                            if (!int.TryParse(Console.ReadLine(), out int age)) {
                                Console.WriteLine("Enter Only Number");
                                continue;
                            }
                           
                            string addstudent = "INSERT INTO student (name,age) VALUES(@name,@age)";
                            SQLiteCommand Addstudent = new SQLiteCommand(addstudent, conn);
                            Addstudent.Parameters.AddWithValue("@name", name);
                            Addstudent.Parameters.AddWithValue("@age", age);
                            Addstudent.ExecuteNonQuery();
                            Console.WriteLine("Add student Success");
                            Console.Write("Do you want to add more? (y/n) : ");
                            addstudentdo = Console.ReadLine();
                        } while (addstudentdo.ToUpper().Trim() == "Y");
                    }
                    //แสดงstudent
                    else if (ChooseMenu == "2" || ChooseMenu.ToUpper() == "SHOW STUDENT")
                    { int i = 0;
                        Console.WriteLine("----------Student List----------");
                        string showstudent = "SELECT * FROM student";
                        SQLiteCommand showcommand = new SQLiteCommand(showstudent, conn);
                        SQLiteDataReader read = showcommand.ExecuteReader();
                        while (read.Read())
                        {
                            string stname = read["name"].ToString();
                            int strage = Convert.ToInt32(read["age"]);
                            Console.WriteLine($"Name : {stname} | Age : {strage}");
                            i += 1;
                        }
                        read.Close();

                        if (i < 1)
                        { Console.WriteLine("-------No Student In Here-------"); }

                    }
                    //แก้ไขstudent
                    else if (ChooseMenu == "3" || ChooseMenu.ToUpper() == "EDIT STUDENT")
                    {
                        string showstudent = "SELECT * FROM student";
                        SQLiteCommand showcommand = new SQLiteCommand(showstudent, conn);
                        SQLiteDataReader read = showcommand.ExecuteReader();
                        while (read.Read())
                        {
                            string stname = read["name"].ToString();
                            int strage = Convert.ToInt32(read["age"]);
                            Console.WriteLine($"Name : {stname} | Age : {strage}");
                        }
                        read.Close();
                        Console.Write("Enter Name Student to Edit : ");
                        string oldeditname = Console.ReadLine();
                        Console.Write("Enter New Student Name : ");
                        string newname = Console.ReadLine();
                        Console.Write("Enter New Student Age : ");
                        if (!int.TryParse(Console.ReadLine(), out int newage))
                        {
                            Console.WriteLine("Enter Only Number");
                            continue;
                        }
                       
                        string editstudent = "UPDATE student SET name = @newname , age = @newage WHERE name = @oldname";
                        SQLiteCommand editcommand = new SQLiteCommand(editstudent, conn);
                        editcommand.Parameters.AddWithValue("@newname", newname);
                        editcommand.Parameters.AddWithValue("@newage", newage);
                        editcommand.Parameters.AddWithValue("@oldname", oldeditname);
                        int editnamecheck = editcommand.ExecuteNonQuery();
                        if (editnamecheck > 0)
                        {
                            Console.WriteLine("Edit Success");
                        }
                        else
                        {
                            Console.WriteLine("No Found Student");
                        }
                    }
                    else if (ChooseMenu == "4" || ChooseMenu.ToUpper() == "DELETE STUDENT")
                    {
                        // ลบstudent

                        string deleteindo = "";
                        //แสดงstudent
                        string deleteshowall = "SELECT * FROM student";
                        SQLiteCommand deleteshowallcommand = new SQLiteCommand(deleteshowall, conn);
                        SQLiteDataReader Readshowdelete = deleteshowallcommand.ExecuteReader();
                        while (Readshowdelete.Read())
                        {
                            string stnamedel = Readshowdelete["name"].ToString();
                            int inagedel = Convert.ToInt32(Readshowdelete["age"]);
                            Console.WriteLine($"Name : {stnamedel} | Age : {inagedel}");
                        }
                        Readshowdelete.Close();
                        do
                        {
                            Console.Write("Select student to Delete : ");
                            string deletename = Console.ReadLine();
                            string delete = "DELETE FROM student WHERE name = @name";
                            SQLiteCommand deletecommand = new SQLiteCommand(delete, conn);
                            deletecommand.Parameters.AddWithValue("@name", deletename);
                            int checkdelete = deletecommand.ExecuteNonQuery();
                            if (checkdelete > 0)
                            {
                                Console.WriteLine("Delete Success");
                            }
                            else
                            {
                                Console.WriteLine("No student found");
                            }
                            Console.WriteLine("Do you want to delete more ? (y/n) : ");
                            deleteindo = Console.ReadLine();
                        } while (deleteindo.ToUpper().Trim() == "Y");
                    }
                    else if (ChooseMenu == "5" || ChooseMenu == "EXIT") {
                        Console.WriteLine("----------EXIT----------");
                        break;
                    }
                    else
                    {
                        return;
                    }
                    Console.Write("Back to Menu? (y/n) : ");
                    checkbackmenu = Console.ReadLine();
                } while (checkbackmenu.ToUpper().Trim() == "Y");
            }
        }
    }
}