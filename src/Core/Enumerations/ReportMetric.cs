using System;
using Yay.Enumerations;

namespace Core.Enumerations
{
	[Serializable]
	public class ReportMetric : Enumeration<ReportMetric>
	{
		public static readonly ReportMetric Average = new ReportMetric(1, "Average Duration", "ROUND(AVG(DurationInSeconds / (60*60)),2)");
		public static readonly ReportMetric Min = new ReportMetric(2, "Minimum Duration", "ROUND(MIX(DurationInSeconds / (60*60)),2)");
		public static readonly ReportMetric Max = new ReportMetric(3, "Maximum Duration", "ROUND(MAX(DurationInSeconds / (60*60)),2)");
		public static readonly ReportMetric Sum = new ReportMetric(4, "Sum Duration", "ROUND(SUM(DurationInSeconds / (60*60)),2)");
		public static readonly ReportMetric Count = new ReportMetric(5, "Transition Counts", "CAST(SUM(CASE WHEN DurationInSeconds IS NOT NULL THEN 1 ELSE 0 END) AS FLOAT)");

		public string sql { get; set; }

		public ReportMetric(int value, string displayName, string sql)
			: base(value, displayName)
		{
			this.sql = sql;
		}
	}
}