<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

void Main()
{

	var Liste1 = new List<GanttDataTask>();
	Liste1.Add(new GanttDataTask() { Id = 1, Name = "Test1Name", Tag = "Orginal" ,StartDatum=DateTime.Today,EndDatum =DateTime.Today.AddHours(23.9999d) });
	Liste1.Add(new GanttDataTask() { Id = 2, Name = "Test2Name", Tag = "Orginal1" });

	Liste1.Dump("Liste 1");

	var Liste2 = new List<GanttDataTask>(){
		new GanttDataTask() { Id = 1, Name = "Test1Name", Tag = "Orginal1" },
		new GanttDataTask() { Id = 2, Name = "Test2Name", Tag = "Orginal2" } }.Dump("Liste 2");	
	
	
	var dupes = Liste1.GroupBy(x => new { x.Name, x.Id })
				   .Where(x => x.Skip(1).Any()).Dump();

	var hasDupes = Liste1.GroupBy(x => new { x.Name, x.Id })
					   .Where(x => x.Skip(1).Any()).Any().Dump();




	var v1 = new GanttDataTask {Id = 1, Name = "Test1Name", Tag = "Orginal" };

	var original = new List<GanttDataTask>();
	original.Add(v1);

	var testclone = original.Clone();

	var v2 = new GanttDataTask {Id = 2, Name = "Test2Name", Tag = "Orginal" };
	testclone.Add(v2);

	original.Dump();
	testclone.Dump();

	var auswertung = original.Except(testclone);
	auswertung.Dump();
}







public class GanttDataTask : ICloneable, IEqualityComparer, IComparable<GanttDataTask>
{
	public int Id { get; set; }
	public int ParentId { get; set; }
	public string Name { get; set; }
	public object Tag { get; set; }
	public DateTime? StartDatum { get; set; }
	public DateTime? EndDatum { get; set; }
	public double Progress { get; set; }
	
	public object Clone()
	{
		return this.MemberwiseClone();
	}

	public int CompareTo([AllowNull] GanttDataTask other)
	{
		if(this.Name != other.Name)
			return 1;
			else
			return 0;
		
	}

	public new bool Equals(object x, object y)
	{
		throw new NotImplementedException();
	}
	public int GetHashCode(object obj)
	{
		throw new NotImplementedException();
	}
}




// Extension methods must be defined in a static class.
public static class CloneExtension
{
	public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
	{
		return listToClone.Select(item => (T)item.Clone()).ToList();
	}
}

