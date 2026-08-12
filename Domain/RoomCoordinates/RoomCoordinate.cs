using dddnet8.Domain.Shared;
using dddnet8.Domain.Patients.V.O;
using System;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.RoomCoordinates.Domain
{
    /// <summary>
    /// Represents the coordinates and other properties of a room.
    /// </summary>
    public class RoomCoordinate : Entity<Guid>, IAggregateRoot
    {
        /// <summary>
        /// Gets or sets the room number (unique identifier for the room).
        /// </summary>
        public RoomNumber RoomNumber { get; private set; }

        /// <summary>
        /// Gets the position (coordinate) of the room.
        /// </summary>
        public Coordinate Position { get; private set; }

        /// <summary>
        /// Gets the dimensions (width and length) of the room.
        /// </summary>
        public Dimensions Size { get; private set; }

        /// <summary>
        /// Gets the direction of the door of the room.
        /// </summary>
        public DoorDirection DoorDirection { get; private set; }

        /// <summary>
        /// Gets the creation date of the room coordinates.
        /// </summary>
        public DateTime CreatedOn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomCoordinate"/> class.
        /// </summary>
        /// <param name="roomNumber">The unique identifier (number) for the room.</param>
        /// <param name="position">The position (coordinate) of the room.</param>
        /// <param name="size">The dimensions (width and length) of the room.</param>
        /// <param name="doorDirection">The direction of the door of the room.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public RoomCoordinate(RoomNumber roomNumber, Coordinate position, Dimensions size, DoorDirection doorDirection)
            : base(Guid.NewGuid())
        {
            RoomNumber = roomNumber ?? throw new ArgumentNullException(nameof(roomNumber), "Room number cannot be null.");
            Position = position ?? throw new ArgumentNullException(nameof(position), "Position cannot be null.");
            Size = size ?? throw new ArgumentNullException(nameof(size), "Size cannot be null.");
            DoorDirection = doorDirection ?? throw new ArgumentNullException(nameof(doorDirection), "Door direction cannot be null.");
            CreatedOn = DateTime.UtcNow;
        }

        protected RoomCoordinate() : base(Guid.NewGuid()) { }

        /// <summary>
        /// Updates the room number.
        /// </summary>
        /// <param name="roomNumber">The new room number for the room.</param>
        public void UpdateRoomNumber(RoomNumber roomNumber)
        {
            RoomNumber = roomNumber ?? throw new ArgumentNullException(nameof(roomNumber), "Room number cannot be null.");
        }

        /// <summary>
        /// Updates the position (coordinate) of the room.
        /// </summary>
        /// <param name="position">The new position for the room.</param>
        public void UpdatePosition(Coordinate position)
        {
            Position = position ?? throw new ArgumentNullException(nameof(position), "Position cannot be null.");
        }

        /// <summary>
        /// Updates the dimensions (width and length) of the room.
        /// </summary>
        /// <param name="size">The new dimensions for the room.</param>
        public void UpdateSize(Dimensions size)
        {
            Size = size ?? throw new ArgumentNullException(nameof(size), "Size cannot be null.");
        }

        /// <summary>
        /// Updates the direction of the door of the room.
        /// </summary>
        /// <param name="doorDirection">The new direction for the room's door.</param>
        public void UpdateDoorDirection(DoorDirection doorDirection)
        {
            DoorDirection = doorDirection ?? throw new ArgumentNullException(nameof(doorDirection), "Door direction cannot be null.");
        }

        /// <summary>
        /// Returns a string representation of the room coordinates.
        /// </summary>
        /// <returns>A string representation of the room coordinates.</returns>
        public override string ToString()
        {
            return $"{RoomNumber} - {Position} - {Size} - {DoorDirection}";
        }
    }
}
