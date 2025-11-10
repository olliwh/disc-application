namespace backend_disc.Dtos.BaseDtos
{
    /// <summary>
    /// Represents the base class for Data Transfer Objects (DTOs) with a unique identifier.
    /// </summary>
    /// <remarks>All Base Dto have an Id</remarks>
    public abstract class BaseDto
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Represents a base interface for Data Transfer Objects (DTOs) used in create operations.
    /// </summary>
    /// <remarks>If interface will need code at some point make it an abstract class instead</remarks>
    public interface ICreateDtoBase
    {
    }

    /// <summary>
    /// Represents a base interface for Data Transfer Objects (DTOs) used in create operations.
    /// </summary>
    /// <remarks>If interface will need code at some point make it an abstract class instead</remarks>
    public interface IUpdateDtoBase
    {
    }
}
