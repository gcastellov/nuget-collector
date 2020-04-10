namespace NugetCollector.Domain
{
    public class Stats
    {
        public int ProjectsCount { get; set; }
        public int AllReferenceCount { get; set; }
        public int UniqueReferenceCount { get; set; }
        public int DifferentReferenceCount { get; set; }
    }
}