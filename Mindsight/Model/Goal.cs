using System;
using SQLite;
namespace MindSight
{
    [Table("goal")]
    public class Goal
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string TargetTitle{ get; set; }

        [MaxLength(250)]
        public string TargetContent { get; set; }

        [MaxLength(250)]
        public string Color { get; set; }

        [MaxLength(250)]
        public string DaysOfTheWeek { get; set; }

        [MaxLength(250)]
        public string IconImage { get; set; }

        public int AccumulateDays { get; set; }

        [MaxLength(25)]
        public string TodayIsChecked { get; set; }

        [MaxLength(25)]
        public string LastUpdatedDate { get; set; }

        public static explicit operator Goal(StackLayout v)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Goal otherGoal = (Goal)obj;
            return Id == otherGoal.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}

