using System;
using System.Collections.Generic;

public class Goal
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TargetWeight { get; set; }
    public int UserId { get; set; } // Foreign key to link to the UserModel

    // Navigation property for the relationship
    public ICollection<WeightEntry> WeightEntries { get; set; }
}