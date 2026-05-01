namespace ChessApplication.DTO
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DtoTypeAttribute : Attribute
    {
        public DtoType Type { get; }

        public DtoTypeAttribute(DtoType type)
        {
            Type = type;
        }
    }
}
