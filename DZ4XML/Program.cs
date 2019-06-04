using System;
using Models.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DZ4XML
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Student> ImportStudents = new List<Student>();
            ImportStudents = Program.ImportStudents(ImportStudents);
        }

        private static void ImportCourses(Student importStudent, IEnumerable<XElement> courseElements)
        {
            foreach (var courseElement in courseElements)
            {
                Course course = new Course();
                course.Name = courseElement.Element("Name").Value;
                course.Id = int.Parse(courseElement.Element("Id").Value);
                course.StartDate = DateTime.Parse(courseElement.Element("StartDate").Value);
                course.EndDate = DateTime.Parse(courseElement.Element("EndDate").Value);
                course.PassCredits = int.Parse(courseElement.Element("PassCredits").Value);
                ImportHomeTasks(course, courseElement.Elements().SelectMany(p => p.Elements("HomeTask")));
                ImportLecturers(course, courseElement.Elements().SelectMany(p => p.Elements("Lecturer")));
                importStudent.Courses.Add(course);
            }
        }

        private static void ImportLecturers(Course course, IEnumerable<XElement> lecturerElements)
        {
            foreach(var lecturerElement in lecturerElements)
            {
                Lecturer lecturer = new Lecturer();
                lecturer.Id = int.Parse(lecturerElement.Element("Id").Value);
                lecturer.Name = lecturerElement.Element("Name").Value;
                lecturer.BirthDate = DateTime.Parse(lecturerElement.Element("BirthDate").Value);
                course.Lecturers.Add(lecturer);
            }
        }

        private static void ImportHomeTasks(Course importCourse, IEnumerable<XElement> homeTaskElements)
        {
            foreach (var homeTaskElement in homeTaskElements)
            {
                HomeTask homeTask = new HomeTask();
                homeTask.Id = int.Parse(homeTaskElement.Element("Id").Value);
                homeTask.Date = DateTime.Parse(homeTaskElement.Element("Date").Value);
                homeTask.Title = homeTaskElement.Element("Title").Value;
                homeTask.Description = homeTaskElement.Element("Description").Value;
                homeTask.Number = int.Parse(homeTaskElement.Element("Number").Value);
                ImportHomeTaskAssesments(homeTask, homeTaskElement.Elements().SelectMany(p => p.Elements("HomeTaskAssessment")));
                importCourse.HomeTasks.Add(homeTask);
            }
        }

        private static void ImportHomeTaskAssesments(HomeTask homeTask, IEnumerable<XElement> homeTaskAssessmentElements)
        {
            foreach(var homeTaskAssessmentElement in homeTaskAssessmentElements)
            {
                HomeTaskAssessment homeTaskAssessment = new HomeTaskAssessment();
                homeTaskAssessment.Id = int.Parse(homeTaskAssessmentElement.Element("Id").Value);
                homeTaskAssessment.IsComplete = bool.Parse(homeTaskAssessmentElement.Element("IsComplete").Value);
                homeTaskAssessment.Date = DateTime.Parse(homeTaskAssessmentElement.Element("Date").Value);
                homeTask.HomeTaskAssessments.Add(homeTaskAssessment);
            }
        }

        private static List<Student> ImportStudents(List<Student> importStudents)
        {
            XDocument document = XDocument.Load("StudyManager.Xml");
            var studentsElement = document.Elements().First();
            List<Student> students = new List<Student>();
            foreach (var student in studentsElement.Elements())
            {
                Student importStudent = new Student();
                importStudent.Name = student.Attribute("Name").Value;
                importStudent.Id = int.Parse(student.Element("Id").Value);
                importStudent.PhoneNumber = student.Element("PhoneNumber").Value;
                importStudent.Email = student.Element("Email").Value;
                importStudent.GitHubLink = student.Element("GitHubLink").Value;
                ImportCourses(importStudent, student.Elements().SelectMany(p => p.Elements("Course")));
                students.Add(importStudent);
            }
            return students;
        }
    }
}
