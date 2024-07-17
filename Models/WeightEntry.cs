
    public class WeightEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Weight { get; set; }
        public int UserId { get; set; } // Foreign key to link to the UserModel
        public int GoalId { get; set; } // Foreign key to link to the Goal

        // Navigation property for the relationship
        public Goal Goal { get; set; }
    }
