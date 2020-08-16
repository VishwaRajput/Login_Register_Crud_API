using System;
using System.ComponentModel.DataAnnotations;

namespace assignmentAPI.Model
{
    public class StudentsGridModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Semester { get; set; }
        public int Marks { get; set; }
    }
}
